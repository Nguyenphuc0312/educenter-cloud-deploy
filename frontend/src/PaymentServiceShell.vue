<template>
  <v-app class="payment-app">
    <v-navigation-drawer 
      v-model="drawer" 
      :rail="rail" 
      permanent 
      color="dark" 
      width="280"
      class="payment-drawer"
    >
      <!-- Brand Header -->
      <div class="brand-container pa-4">
        <div class="brand-content">
          <v-avatar size="48" color="success" rounded="lg" class="brand-avatar">
            <v-icon color="white" size="28">mdi-cash-multiple</v-icon>
          </v-avatar>
          <div v-if="!rail" class="brand-text">
            <div class="text-subtitle-1 font-weight-bold text-white">EduCenter</div>
            <div class="text-caption brand-subtitle">Payment & Report Service</div>
            <v-chip size="x-small" color="success" variant="flat" class="mt-2">
              <v-icon start size="10">mdi-shield-check</v-icon>
              Nhóm 3
            </v-chip>
          </div>
        </div>
      </div>
      
      <v-divider class="border-opacity-10 mb-3" />

      <!-- Navigation Items -->
      <v-list density="compact" nav bg-color="transparent" color="white" class="px-2">
        <v-list-item
          v-for="item in navItems"
          :key="item.key"
          :prepend-icon="item.icon"
          :title="item.label"
          :value="item.key"
          :active="activePage === item.key"
          active-color="success"
          rounded="lg"
          class="nav-item mb-1"
          @click="activePage = item.key"
        >
          <template #prepend>
            <v-icon :color="activePage === item.key ? 'success' : 'grey'" class="nav-icon">
              {{ item.icon }}
            </v-icon>
          </template>
        </v-list-item>
      </v-list>

      <!-- Divider -->
      <v-divider class="border-opacity-10 my-3 mx-4" />

      <!-- User Section -->
      <div v-if="auth.isAuthenticated && !rail" class="user-section px-4">
        <div class="d-flex align-center">
          <v-avatar size="36" color="success" variant="tonal" class="mr-3">
            <v-icon size="20">mdi-account</v-icon>
          </v-avatar>
          <div class="user-info">
            <div class="text-caption text-white font-weight-medium">
              {{ auth.role === 'admin' ? 'Admin User' : 'Học Viên' }}
            </div>
            <div class="text-caption" style="color:#9ca3af">admin@educenter.vn</div>
          </div>
        </div>
      </div>

      <!-- Logout -->
      <template #append>
        <v-divider class="border-opacity-10" />
        <v-list density="compact" nav bg-color="transparent" color="white" class="px-2 pb-3">
          <v-list-item
            prepend-icon="mdi-exit-to-app"
            title="Quay lại Menu"
            rounded="lg"
            class="nav-item logout-item"
            @click="$emit('exit')"
          />
        </v-list>
      </template>
    </v-navigation-drawer>

    <!-- App Bar -->
    <v-app-bar color="white" elevation="2" height="64">
      <v-btn icon variant="text" @click="rail = !rail" class="ml-2">
        <v-icon>mdi-menu</v-icon>
      </v-btn>
      
      <v-app-bar-title class="text-subtitle-1 font-weight-bold text-primary">
        <v-icon class="mr-2" color="success">{{ currentIcon }}</v-icon>
        {{ currentTitle }}
      </v-app-bar-title>
      
      <v-spacer />
      
      <!-- Header Actions -->
      <v-btn icon variant="text" class="mr-1">
        <v-badge color="error" content="3" overlap>
          <v-icon>mdi-bell</v-icon>
        </v-badge>
      </v-btn>
      
      <v-chip v-if="auth.isAuthenticated" size="small" variant="tonal" color="success" class="mr-3">
        <v-icon start size="14">{{ auth.isAdmin ? 'mdi-shield-account' : 'mdi-school' }}</v-icon>
        {{ auth.role === 'admin' ? 'Admin' : 'Học viên' }}
      </v-chip>
      
      <v-btn v-if="auth.isAuthenticated" icon variant="text" @click="handleLogout" class="mr-2">
        <v-icon>mdi-logout</v-icon>
      </v-btn>
    </v-app-bar>

    <!-- Main Content -->
    <v-main class="main-content">
      <v-container fluid class="pa-6">
        <!-- Login -->
        <LoginView v-if="!auth.isAuthenticated" />

        <!-- Admin Views -->
        <AdminDashboardView v-else-if="activePage === 'dashboard' && auth.isAdmin" />
        <FinancialDashboardView v-else-if="activePage === 'finance' && auth.isAdmin" />
        <DebtManagerView v-else-if="activePage === 'debt' && auth.isAdmin" />
        <TeacherPayrollView v-else-if="activePage === 'payroll' && auth.isAdmin" />
        <UserManagementView v-else-if="activePage === 'users' && auth.isAdmin" />
        <SystemSettingsView v-else-if="activePage === 'settings' && auth.isAdmin" />
        <AuditLogView v-else-if="activePage === 'audit' && auth.isAdmin" />

        <!-- Student Views -->
        <StudentBillingView v-else-if="activePage === 'billing' && auth.isStudent" />
        <PaymentCheckoutView v-else-if="activePage === 'checkout' && auth.isStudent" />

        <!-- Profile / Notifications (both roles) -->
        <UserProfileView v-else-if="activePage === 'profile'" />
        <NotificationCenterView v-else-if="activePage === 'notifications'" />
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useAuthStore } from '@/stores/auth'

import LoginView from './views/payment/Login.vue'
import AdminDashboardView from './views/payment/AdminDashboard.vue'
import FinancialDashboardView from './views/payment/FinancialDashboard.vue'
import DebtManagerView from './views/payment/DebtManager.vue'
import TeacherPayrollView from './views/payment/TeacherPayroll.vue'
import UserManagementView from './views/payment/UserManagement.vue'
import SystemSettingsView from './views/payment/SystemSettings.vue'
import AuditLogView from './views/payment/AuditLog.vue'
import StudentBillingView from './views/payment/StudentBilling.vue'
import PaymentCheckoutView from './views/payment/PaymentCheckout.vue'
import UserProfileView from './views/payment/UserProfile.vue'
import NotificationCenterView from './views/payment/NotificationCenter.vue'

defineEmits(['exit'])

const auth = useAuthStore()
const drawer = ref(true)
const rail = ref(false)
const activePage = ref('dashboard')

const navItems = computed(() => {
  if (!auth.isAuthenticated) return []
  if (auth.isAdmin) return [
    { key: 'dashboard', label: 'Tổng Quan', icon: 'mdi-view-dashboard' },
    { key: 'finance', label: 'Tài Chính & BI', icon: 'mdi-chart-bar' },
    { key: 'debt', label: 'Công Nợ & Trả Góp', icon: 'mdi-alert-circle' },
    { key: 'payroll', label: 'Bảng Lương GV', icon: 'mdi-cash-multiple' },
    { key: 'users', label: 'Quản Lý Người Dùng', icon: 'mdi-account-group' },
    { key: 'settings', label: 'Cài Đặt', icon: 'mdi-cog' },
    { key: 'audit', label: 'Audit Log', icon: 'mdi-clipboard-list' },
    { key: 'profile', label: 'Hồ Sơ', icon: 'mdi-account' },
    { key: 'notifications', label: 'Thông Báo', icon: 'mdi-bell' },
  ]
  return [
    { key: 'billing', label: 'Học Phí Của Tôi', icon: 'mdi-cash-multiple' },
    { key: 'checkout', label: 'Thanh Toán', icon: 'mdi-credit-card' },
    { key: 'profile', label: 'Hồ Sơ', icon: 'mdi-account' },
    { key: 'notifications', label: 'Thông Báo', icon: 'mdi-bell' },
  ]
})

const labels = {
  dashboard: 'Tổng Quan', 
  finance: 'Tài Chính & BI', 
  debt: 'Công Nợ & Trả Góp',
  payroll: 'Bảng Lương GV', 
  users: 'Quản Lý Người Dùng', 
  settings: 'Cài Đặt',
  audit: 'Audit Log', 
  profile: 'Hồ Sơ', 
  notifications: 'Thông Báo',
  billing: 'Học Phí Của Tôi', 
  checkout: 'Thanh Toán',
}

const icons = {
  dashboard: 'mdi-view-dashboard',
  finance: 'mdi-chart-bar',
  debt: 'mdi-alert-circle',
  payroll: 'mdi-cash-multiple',
  users: 'mdi-account-group',
  settings: 'mdi-cog',
  audit: 'mdi-clipboard-list',
  profile: 'mdi-account',
  notifications: 'mdi-bell',
  billing: 'mdi-cash-multiple',
  checkout: 'mdi-credit-card',
}

const currentTitle = computed(() => {
  if (!auth.isAuthenticated) return 'Đăng Nhập'
  return labels[activePage.value] || 'Payment Service'
})

const currentIcon = computed(() => {
  return icons[activePage.value] || 'mdi-cash-multiple'
})

function handleLogout() {
  auth.logout()
  activePage.value = 'dashboard'
}
</script>

<style scoped>
/* App wrapper */
.payment-app {
  overflow: hidden;
}

/* Navigation Drawer - ensure it's above everything */
.payment-drawer {
  z-index: 1000 !important;
}

/* Brand Container */
.brand-container {
  background: linear-gradient(135deg, rgba(46,125,50,0.2), rgba(46,125,50,0.05));
}

.brand-content {
  display: flex;
  align-items: center;
  gap: 12px;
}

.brand-avatar {
  animation: pulse-glow 3s ease-in-out infinite;
}

@keyframes pulse-glow {
  0%, 100% { box-shadow: 0 0 0 0 rgba(76, 175, 80, 0.4); }
  50% { box-shadow: 0 0 0 10px rgba(76, 175, 80, 0); }
}

.brand-subtitle {
  color: #9ca3af !important;
  font-size: 11px;
}

/* Navigation Items */
.nav-item {
  transition: all 0.2s ease;
}

.nav-item:hover {
  background: rgba(255, 255, 255, 0.05) !important;
}

.nav-item :deep(.v-list-item__prepend) {
  margin-right: 12px;
}

.nav-icon {
  transition: all 0.2s ease;
}

/* Logout Item */
.logout-item {
  color: #f87171 !important;
}

.logout-item:hover {
  background: rgba(248, 113, 113, 0.1) !important;
}

/* User Section */
.user-section {
  padding: 12px;
  background: rgba(255, 255, 255, 0.03);
  border-radius: 12px;
  margin: 0 8px;
}

/* Main Content */
.main-content {
  background: #f5f5f5;
}
</style>
