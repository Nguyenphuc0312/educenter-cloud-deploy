<template>
  <v-card max-width="480">
    <v-card-title class="text-center pa-6 pb-4">
      <v-avatar size="80" color="success" variant="tonal" class="mb-3">
        <span class="text-h4 font-weight-bold">{{ initials }}</span>
      </v-avatar>
      <h3 class="text-h6 font-weight-bold">{{ user?.fullName || user?.email }}</h3>
      <v-chip :color="roleColor" variant="tonal" size="small" class="mt-2" label>{{ user?.role }}</v-chip>
    </v-card-title>
    <v-divider />
    <v-card-text class="pa-6">
      <v-list density="compact">
        <v-list-item prepend-icon="mdi-email-outline" title="Email" :subtitle="user?.email || '—'" />
        <v-list-item prepend-icon="mdi-phone-outline" title="Số điện thoại" :subtitle="user?.phone || '—'" />
        <v-list-item prepend-icon="mdi-shield-account-outline" title="Vai trò" :subtitle="user?.role || '—'" />
        <v-list-item prepend-icon="mdi-account-check-outline" title="Trạng thái" subtitle="Active" />
      </v-list>
    </v-card-text>
    <v-card-actions class="pa-6 pt-0">
      <v-btn variant="text" @click="auth.logout()" color="error">Đăng xuất</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script setup>
import { computed } from 'vue'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const user = computed(() => auth.user)
const initials = computed(() => {
  const name = user.value?.fullName || user.value?.email || 'U'
  const parts = name.split(' ')
  return parts.length >= 2 ? (parts[0][0] + parts[parts.length - 1][0]).toUpperCase() : name[0].toUpperCase()
})
const roleColor = computed(() => ({ admin: 'success', teacher: 'info', student: 'warning' })[user.value?.role] || 'secondary')
</script>
