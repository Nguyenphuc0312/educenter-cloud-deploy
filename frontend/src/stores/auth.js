import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { login as apiLogin } from '@/modules/payments/api/paymentApi'

export const useAuthStore = defineStore('auth', () => {
  const token = ref(localStorage.getItem('educenter_token') || '')
  const role = ref(localStorage.getItem('educenter_role') || 'admin')
  const user = ref(null)

  const isAuthenticated = computed(() => !!token.value)
  const isAdmin = computed(() => role.value === 'admin')
  const isStudent = computed(() => role.value === 'student')

  async function login(email, password) {
    const data = await apiLogin(email, password)
    if (!data?.token) throw new Error('Invalid credentials')

    token.value = data.token
    role.value = data.role || 'admin'
    user.value = data.user

    localStorage.setItem('educenter_token', data.token)
    localStorage.setItem('educenter_role', data.role || 'admin')

    return data
  }

  function logout() {
    token.value = ''
    user.value = null
    role.value = 'admin'
    localStorage.removeItem('educenter_token')
    localStorage.removeItem('educenter_role')
  }

  return { token, role, user, isAuthenticated, isAdmin, isStudent, login, logout }
})
