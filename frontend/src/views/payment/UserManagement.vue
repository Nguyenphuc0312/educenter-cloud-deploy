<template>
  <div class="user-management">
    <!-- Header -->
    <div class="page-header mb-6">
      <div>
        <h2 class="text-h4 font-weight-bold">Quản Lý Người Dùng</h2>
        <p class="text-body-2" style="color:#64748b">Quản lý tài khoản admin, giáo viên và học viên</p>
      </div>
      <div class="header-actions">
        <v-btn color="success" variant="flat" prepend-icon="mdi-plus" @click="showCreate = true">
          <v-icon start>mdi-account-plus</v-icon>
          Thêm người dùng
        </v-btn>
      </div>
    </div>

    <!-- KPI Cards -->
    <v-row class="mb-6">
      <v-col v-for="(kpi, index) in kpis" :key="kpi.label" cols="12" sm="4">
        <v-card class="kpi-card" :style="{ animationDelay: index * 0.1 + 's' }" rounded="xl">
          <div class="kpi-glow" :style="{ background: kpi.gradient }"></div>
          <div class="d-flex align-center justify-space-between pa-4">
            <div>
              <p class="text-caption mb-1 kpi-label">{{ kpi.label }}</p>
              <h3 class="text-h4 font-weight-bold" :style="{ color: kpi.color }">{{ kpi.value }}</h3>
            </div>
            <v-avatar size="54" :color="kpi.bgColor" variant="tonal" rounded="xl">
              <v-icon :color="kpi.color" size="28">{{ kpi.icon }}</v-icon>
            </v-avatar>
          </div>
        </v-card>
      </v-col>
    </v-row>

    <!-- Filter Bar -->
    <v-card class="mb-6" rounded="xl">
      <div class="d-flex flex-wrap align-center pa-4 gap-4">
        <v-text-field 
          v-model="search" 
          prepend-inner-icon="mdi-magnify" 
          placeholder="Tìm theo tên, email..." 
          hide-details 
          density="compact" 
          style="max-width:300px"
          clearable 
          rounded="lg"
          variant="outlined"
          color="success"
        />
        <v-select 
          v-model="roleFilter" 
          :items="roleOptions" 
          label="Vai trò" 
          density="compact" 
          hide-details 
          style="max-width:160px"
          variant="outlined"
          rounded="lg"
          color="success"
        />
        <v-spacer />
        <v-chip color="success" variant="tonal" size="small">
          <v-icon start size="14">mdi-account-group</v-icon>
          {{ filteredUsers.length }} người dùng
        </v-chip>
      </div>
    </v-card>

    <!-- Users Table -->
    <v-card class="table-card" rounded="xl">
      <v-data-table 
        :headers="headers" 
        :items="filteredUsers" 
        :items-per-page="10" 
        hover
        class="custom-table"
      >
        <template #item.fullName="{ item }">
          <div class="d-flex align-center py-2">
            <v-avatar size="44" :color="getRoleColor(item.role)" variant="tonal" class="mr-3">
              <span class="text-subtitle-1 font-weight-bold">{{ item.fullName?.[0] }}</span>
            </v-avatar>
            <div>
              <span class="font-weight-medium text-body-1">{{ item.fullName }}</span>
              <br>
              <span class="text-caption" style="color:#64748b">
                <v-icon size="12" class="mr-1">mdi-email-outline</v-icon>
                {{ item.email }}
              </span>
            </div>
          </div>
        </template>
        
        <template #item.role="{ item }">
          <v-chip 
            :color="getRoleColor(item.role)" 
            variant="tonal" 
            size="small" 
            label
            class="role-chip"
          >
            <v-icon start size="14">{{ getRoleIcon(item.role) }}</v-icon>
            {{ getRoleText(item.role) }}
          </v-chip>
        </template>
        
        <template #item.phone="{ item }">
          <span class="text-body-2">
            <v-icon size="14" class="mr-1">mdi-phone</v-icon>
            {{ item.phone || 'Chưa cập nhật' }}
          </span>
        </template>
        
        <template #item.status="{ item }">
          <v-chip 
            :color="item.status === 'Active' ? 'success' : 'error'" 
            variant="flat" 
            size="small" 
            label
            class="font-weight-medium"
          >
            <v-icon start size="14">{{ item.status === 'Active' ? 'mdi-check-circle' : 'mdi-close-circle' }}</v-icon>
            {{ item.status === 'Active' ? 'Hoạt động' : 'Khóa' }}
          </v-chip>
        </template>
        
        <template #item.lastLogin="{ item }">
          <span class="text-caption" style="color:#64748b">
            {{ item.lastLogin || 'Chưa đăng nhập' }}
          </span>
        </template>
        
        <template #item.actions="{ item }">
          <v-btn 
            size="small" 
            variant="tonal" 
            color="success" 
            class="mr-2 action-btn"
            @click="editUser(item)"
          >
            <v-icon start size="16">mdi-pencil</v-icon>
            Sửa
          </v-btn>
          <v-btn 
            size="small" 
            variant="tonal" 
            color="error"
            class="action-btn"
            @click="deleteUser(item)"
          >
            <v-icon start size="16">mdi-delete</v-icon>
            Xóa
          </v-btn>
        </template>
      </v-data-table>
    </v-card>

    <!-- Snackbar -->
    <v-snackbar v-model="snack" :color="snackColor" rounded="lg" location="top" timeout="3000">
      <v-icon class="mr-2">{{ snackIcon }}</v-icon>
      {{ snackText }}
    </v-snackbar>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { usePaymentStore } from '@/stores/payment'

const payment = usePaymentStore()
const search = ref('')
const roleFilter = ref('Tất cả')
const showCreate = ref(false)
const snack = ref(false)
const snackText = ref('')
const snackColor = ref('success')
const snackIcon = ref('mdi-check-circle')

const roleOptions = ['Tất cả', 'admin', 'teacher', 'student']

const headers = [
  { title: 'Người dùng', key: 'fullName', minWidth: '250px' },
  { title: 'Email', key: 'email', minWidth: '200px' },
  { title: 'Vai trò', key: 'role', align: 'center', width: '130px' },
  { title: 'SĐT', key: 'phone', width: '150px' },
  { title: 'Trạng thái', key: 'status', align: 'center', width: '120px' },
  { title: 'Đăng nhập cuối', key: 'lastLogin', width: '140px' },
  { title: 'Thao tác', key: 'actions', align: 'center', width: '200px', sortable: false },
]

const filteredUsers = computed(() => {
  let list = payment.users
  if (search.value) {
    const q = search.value.toLowerCase()
    list = list.filter(u => u.fullName?.toLowerCase().includes(q) || u.email?.toLowerCase().includes(q))
  }
  if (roleFilter.value !== 'Tất cả') list = list.filter(u => u.role === roleFilter.value)
  return list
})

const kpis = computed(() => [
  { 
    label: 'Tổng người dùng', 
    value: payment.users.length, 
    icon: 'mdi-account-group', 
    color: '#2E7D32', 
    bgColor: 'success',
    gradient: 'linear-gradient(135deg, rgba(46,125,50,0.3), transparent)',
  },
  { 
    label: 'Giáo viên', 
    value: payment.users.filter(u => u.role === 'teacher').length, 
    icon: 'mdi-human-male-board', 
    color: '#1976D2', 
    bgColor: 'primary',
    gradient: 'linear-gradient(135deg, rgba(25,118,210,0.3), transparent)',
  },
  { 
    label: 'Học viên', 
    value: payment.users.filter(u => u.role === 'student').length, 
    icon: 'mdi-school', 
    color: '#E65100', 
    bgColor: 'warning',
    gradient: 'linear-gradient(135deg, rgba(230,81,0,0.3), transparent)',
  },
])

function getRoleColor(r) { 
  return { admin: 'success', teacher: 'primary', student: 'warning' }[r] || 'secondary' 
}

function getRoleIcon(r) {
  return { admin: 'mdi-shield-account', teacher: 'mdi-human-male-board', student: 'mdi-school' }[r] || 'mdi-account'
}

function getRoleText(r) {
  return { admin: 'Admin', teacher: 'Giáo viên', student: 'Học viên' }[r] || r
}

function editUser(u) { 
  snackText.value = `Đang sửa thông tin của ${u.fullName}`
  snackColor.value = 'info'
  snackIcon.value = 'mdi-pencil'
  snack.value = true 
}

function deleteUser(u) { 
  snackText.value = `Đã xóa người dùng ${u.fullName}`
  snackColor.value = 'error'
  snackIcon.value = 'mdi-delete'
  snack.value = true 
}

onMounted(() => payment.fetchUsers())
</script>

<style scoped>
.user-management {
  animation: fadeIn 0.5s ease-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.header-actions {
  display: flex;
  gap: 12px;
}

/* KPI Cards */
.kpi-card {
  position: relative;
  overflow: hidden;
  transition: all 0.3s ease;
  animation: slideUp 0.5s ease-out backwards;
  border: 1px solid rgba(0,0,0,0.05);
}

.kpi-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 20px 40px rgba(0,0,0,0.1) !important;
}

@keyframes slideUp {
  from { opacity: 0; transform: translateY(30px); }
  to { opacity: 1; transform: translateY(0); }
}

.kpi-glow {
  position: absolute;
  top: -50%;
  right: -50%;
  width: 100%;
  height: 100%;
  opacity: 0.5;
}

.kpi-label {
  color: #64748b !important;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  font-size: 11px !important;
}

/* Table */
.table-card {
  border: 1px solid rgba(0,0,0,0.05);
  overflow: hidden;
}

.custom-table :deep(th) {
  background: #f8f9fa !important;
  font-weight: 600 !important;
  color: #2E7D32 !important;
}

.custom-table :deep(tr:hover) {
  background: rgba(46, 125, 50, 0.04) !important;
}

.role-chip {
  text-transform: capitalize;
}

.action-btn {
  text-transform: none;
  font-weight: 500;
}
</style>
