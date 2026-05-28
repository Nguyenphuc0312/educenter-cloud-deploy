<template>
  <div class="login-page">
    <!-- Background Effects -->
    <div class="bg-gradient"></div>
    <div class="bg-blur-circle circle-1"></div>
    <div class="bg-blur-circle circle-2"></div>
    
    <v-card class="login-card" rounded="xl">
      <div class="text-center pt-6 pb-3">
        <!-- Animated Logo -->
        <div class="logo-container mb-3">
          <v-avatar size="64" color="success" variant="tonal" class="logo-avatar">
            <v-icon size="32" color="success">mdi-cash-multiple</v-icon>
          </v-avatar>
          <div class="logo-ring"></div>
        </div>
        <h2 class="text-h5 font-weight-bold text-success">EduCenter</h2>
        <p class="text-body-2 mt-1" style="color:#64748b">Payment & Report Service</p>
        <v-chip size="x-small" color="success" variant="flat" class="mt-2">
          <v-icon start size="12">mdi-shield-check</v-icon>
          Nhóm 3
        </v-chip>
      </div>
      <v-divider />
      <v-card-text class="pa-5 pt-4">
        <v-form @submit.prevent="handleLogin">
          <v-text-field 
            v-model="email" 
            label="Email" 
            type="email" 
            prepend-inner-icon="mdi-email-outline" 
            :error-messages="errors.email" 
            class="mb-3 custom-input"
            variant="outlined"
            density="compact"
          />
          <v-text-field 
            v-model="password" 
            label="Mật khẩu" 
            :type="showPass ? 'text' : 'password'" 
            prepend-inner-icon="mdi-lock-outline" 
            :append-inner-icon="showPass ? 'mdi-eye-off' : 'mdi-eye'" 
            @click:append-inner="showPass = !showPass" 
            :error-messages="errors.password" 
            class="mb-2 custom-input"
            variant="outlined"
            density="compact"
          />
          <div v-if="loginError" class="error-banner mb-3">
            <v-icon size="16" color="error">mdi-alert-circle</v-icon>
            <span>{{ loginError }}</span>
          </div>
          <v-btn 
            type="submit" 
            color="success" 
            variant="flat" 
            block 
            size="medium" 
            :loading="loading" 
            class="mt-2 login-btn"
          >
            <v-icon start size="18">mdi-login</v-icon>
            Đăng Nhập
          </v-btn>
        </v-form>
        <v-divider class="my-3" />
        <p class="text-caption text-center mb-2" style="color:#64748b">Tài khoản demo:</p>
        <div class="demo-accounts">
          <v-chip 
            v-for="acc in demoAccounts" 
            :key="acc.email" 
            size="small" 
            variant="tonal" 
            color="success"
            class="mr-2 mb-2 demo-chip"
            @click="fillDemo(acc)"
          >
            <v-icon start size="14">{{ acc.icon }}</v-icon>
            {{ acc.label }}
          </v-chip>
        </div>
      </v-card-text>
      <div class="login-footer pa-3 text-center">
        <span>© 2026 EduCenter - Nhóm 3</span>
      </div>
    </v-card>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const email = ref('')
const password = ref('')
const showPass = ref(false)
const loading = ref(false)
const loginError = ref('')
const errors = reactive({ email: '', password: '' })

const demoAccounts = [
  { label: 'Admin', email: 'admin@educenter.vn', password: 'admin123', icon: 'mdi-shield-account' },
  { label: 'Student', email: 'student@educenter.vn', password: 'student123', icon: 'mdi-school' },
]

function fillDemo(acc) {
  email.value = acc.email
  password.value = acc.password
}

async function handleLogin() {
  errors.email = ''
  errors.password = ''
  loginError.value = ''

  if (!email.value) { errors.email = 'Email không được để trống'; return }
  if (!password.value) { errors.password = 'Mật khẩu không được để trống'; return }

  loading.value = true
  try {
    await auth.login(email.value, password.value)
  } catch (e) {
    loginError.value = e.message || 'Đăng nhập thất bại'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
/* Background Effects */
.login-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #1a1a2e 0%, #16213e 50%, #0f3460 100%);
  position: relative;
  overflow: hidden;
  padding: 16px;
}

.bg-gradient {
  position: absolute;
  inset: 0;
  background: 
    radial-gradient(circle at 20% 80%, rgba(46, 125, 50, 0.15) 0%, transparent 50%),
    radial-gradient(circle at 80% 20%, rgba(46, 125, 50, 0.1) 0%, transparent 50%);
}

.bg-blur-circle {
  position: absolute;
  border-radius: 50%;
  filter: blur(80px);
  opacity: 0.4;
  animation: float 8s ease-in-out infinite;
}

.circle-1 {
  width: 400px;
  height: 400px;
  background: rgba(46, 125, 50, 0.3);
  top: -100px;
  left: -100px;
  animation-delay: 0s;
}

.circle-2 {
  width: 300px;
  height: 300px;
  background: rgba(76, 175, 80, 0.2);
  bottom: -50px;
  right: -50px;
  animation-delay: -4s;
}

@keyframes float {
  0%, 100% { transform: translateY(0) scale(1); }
  50% { transform: translateY(-30px) scale(1.05); }
}

/* Card Styles */
.login-card { 
  width: 100%;
  max-width: 380px;
  box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.5) !important;
  backdrop-filter: blur(20px);
  background: rgba(255, 255, 255, 0.95) !important;
  animation: slideUp 0.6s ease-out;
  position: relative;
  z-index: 10;
}

@keyframes slideUp {
  from { opacity: 0; transform: translateY(30px); }
  to { opacity: 1; transform: translateY(0); }
}

/* Logo Animation */
.logo-container {
  position: relative;
  display: inline-block;
}

.logo-avatar {
  animation: pulse 2s ease-in-out infinite;
}

@keyframes pulse {
  0%, 100% { transform: scale(1); box-shadow: 0 0 0 0 rgba(46, 125, 50, 0.4); }
  50% { transform: scale(1.05); box-shadow: 0 0 0 12px rgba(46, 125, 50, 0); }
}

.logo-ring {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 80px;
  height: 80px;
  border: 2px solid rgba(46, 125, 50, 0.3);
  border-radius: 50%;
  animation: ring-pulse 2s ease-out infinite;
}

@keyframes ring-pulse {
  0% { transform: translate(-50%, -50%) scale(0.8); opacity: 1; }
  100% { transform: translate(-50%, -50%) scale(1.3); opacity: 0; }
}

/* Input Styles */
.custom-input :deep(.v-field) {
  border-radius: 10px;
  transition: all 0.3s ease;
}

.custom-input :deep(.v-field--focused) {
  box-shadow: 0 0 0 3px rgba(46, 125, 50, 0.2);
}

/* Error Banner */
.error-banner {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px 12px;
  background: rgba(244, 67, 54, 0.1);
  border: 1px solid rgba(244, 67, 54, 0.3);
  border-radius: 8px;
  color: #D32F2F;
  font-size: 12px;
  animation: shake 0.5s ease-in-out;
}

@keyframes shake {
  0%, 100% { transform: translateX(0); }
  25% { transform: translateX(-5px); }
  75% { transform: translateX(5px); }
}

/* Login Button */
.login-btn {
  border-radius: 10px;
  text-transform: none;
  font-weight: 600;
  letter-spacing: 0.5px;
  transition: all 0.3s ease;
}

.login-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(46, 125, 50, 0.4);
}

/* Demo Accounts */
.demo-accounts { display: flex; flex-wrap: wrap; justify-content: center; }

.demo-chip {
  transition: all 0.3s ease;
  cursor: pointer;
}

.demo-chip:hover {
  transform: scale(1.08);
  box-shadow: 0 4px 12px rgba(46, 125, 50, 0.3);
}

/* Footer */
.login-footer {
  background: rgba(0, 0, 0, 0.02);
  border-top: 1px solid rgba(0, 0, 0, 0.05);
  color: rgba(0, 0, 0, 0.4);
  font-size: 11px;
}
</style>
