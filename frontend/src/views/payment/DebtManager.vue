<template>
  <div class="debt-manager">
    <!-- Header -->
    <div class="page-header mb-6">
      <div>
        <h2 class="text-h4 font-weight-bold">Quản Lý Công Nợ</h2>
        <p class="text-body-2" style="color:#64748b">Theo dõi và xử lý công nợ học phí</p>
      </div>
      <div class="header-actions">
        <v-btn color="warning" variant="tonal" prepend-icon="mdi-email-fast" size="small" @click="sendReminders" :loading="loading">
          <v-icon start size="18">mdi-email-multiple</v-icon>
          Gửi nhắc nợ hàng loạt
        </v-btn>
      </div>
    </div>

    <!-- KPI Cards -->
    <v-row class="mb-6">
      <v-col v-for="(kpi, index) in kpis" :key="kpi.label" cols="12" sm="6" lg="3">
        <v-card class="kpi-card" :style="{ animationDelay: index * 0.1 + 's' }" rounded="xl">
          <div class="kpi-glow" :style="{ background: kpi.gradient }"></div>
          <div class="d-flex align-center justify-space-between pa-4">
            <div>
              <p class="text-caption mb-1 kpi-label">{{ kpi.label }}</p>
              <h3 class="text-h5 font-weight-bold" :style="{ color: kpi.color }">{{ kpi.value }}</h3>
            </div>
            <v-avatar size="50" :color="kpi.bgColor" variant="tonal" rounded="xl">
              <v-icon :color="kpi.color" size="24">{{ kpi.icon }}</v-icon>
            </v-avatar>
          </div>
        </v-card>
      </v-col>
    </v-row>

    <!-- Filter & Search -->
    <v-card class="mb-6" rounded="xl">
      <div class="d-flex flex-wrap align-center pa-5 gap-4">
        <v-text-field 
          v-model="search" 
          prepend-inner-icon="mdi-magnify" 
          placeholder="Tìm học viên, khóa học..." 
          hide-details 
          density="compact" 
          style="max-width:280px"
          clearable 
          rounded="lg"
          variant="outlined"
          color="success"
        />
        <v-select 
          v-model="statusFilter" 
          :items="statusOptions" 
          label="Trạng thái" 
          density="compact" 
          hide-details 
          style="max-width:160px"
          variant="outlined"
          rounded="lg"
          color="success"
        />
        <v-spacer />
        <div class="total-debt">
          <span class="text-caption" style="color:#64748b">Tổng công nợ:</span>
          <span class="text-h6 font-weight-bold text-error ml-2">{{ fmt(totalDebt) }}</span>
        </div>
      </div>
    </v-card>

    <!-- Debt Table -->
    <v-card class="table-card" rounded="xl">
      <v-data-table 
        :headers="headers" 
        :items="filteredDebts" 
        :items-per-page="10" 
        hover
        class="custom-table"
      >
        <template #item.studentName="{ item }">
          <div class="d-flex align-center">
            <v-avatar size="36" :color="getAvatarColor(item.daysOverdue)" variant="tonal" class="mr-3">
              <span class="text-subtitle-2 font-weight-bold">{{ item.studentName?.[0] }}</span>
            </v-avatar>
            <div>
              <span class="font-weight-medium text-body-2">{{ item.studentName }}</span>
              <br>
              <span class="text-caption" style="color:#64748b">ID: {{ item.studentId }}</span>
            </div>
          </div>
        </template>
        
        <template #item.courseName="{ item }">
          <div>
            <span class="text-body-2">{{ item.courseName }}</span>
          </div>
        </template>
        
        <template #item.totalFee="{ item }">
          <span class="text-body-2">{{ fmt(item.totalFee) }}</span>
        </template>
        
        <template #item.remainingAmount="{ item }">
          <span class="font-weight-bold text-error text-body-2">{{ fmt(item.remainingAmount) }}</span>
        </template>
        
        <template #item.daysOverdue="{ item }">
          <v-chip 
            :color="getDaysColor(item.daysOverdue)" 
            variant="tonal" 
            size="small" 
            label
            class="font-weight-medium"
          >
            <v-icon start size="14">{{ getDaysIcon(item.daysOverdue) }}</v-icon>
            {{ item.daysOverdue }} ngày
          </v-chip>
        </template>
        
        <template #item.progress="{ item }">
          <div class="d-flex align-center gap-2">
            <v-progress-linear 
              :model-value="getProgress(item)" 
              :color="getProgressColor(item)" 
              height="6" 
              rounded
              style="max-width: 80px"
            />
            <span class="text-caption">{{ getProgress(item) }}%</span>
          </div>
        </template>
        
        <template #item.status="{ item }">
          <v-chip 
            :color="item.status === 'Overdue' ? 'error' : 'warning'" 
            variant="flat" 
            size="small" 
            label
            class="font-weight-medium"
          >
            {{ getStatusText(item.status) }}
          </v-chip>
        </template>
        
        <template #item.actions="{ item }">
          <v-btn 
            size="small" 
            variant="tonal" 
            color="success" 
            class="mr-2 action-btn"
            @click="openInstallment(item)"
          >
            <v-icon start size="16">mdi-cash-clock</v-icon>
            Trả góp
          </v-btn>
          <v-btn 
            size="small" 
            variant="tonal" 
            color="warning"
            class="action-btn"
            @click="sendReminder(item)"
          >
            <v-icon start size="16">mdi-email</v-icon>
            Nhắc nợ
          </v-btn>
        </template>
      </v-data-table>
    </v-card>

    <!-- Installment Dialog -->
    <v-dialog v-model="showInstallment" max-width="520" persistent>
      <v-card rounded="xl" class="installment-dialog">
        <v-card-title class="pa-5 pb-3 d-flex align-center">
          <v-avatar color="success" variant="tonal" size="44" class="mr-3">
            <v-icon color="success">mdi-cash-clock</v-icon>
          </v-avatar>
          <div>
            <span class="text-h6 font-weight-bold">Thiết Lập Trả Góp</span>
            <br>
            <span class="text-caption" style="color:#64748b">{{ selectedDebt?.studentName }}</span>
          </div>
          <v-spacer />
          <v-btn icon="mdi-close" variant="text" size="small" @click="showInstallment = false" />
        </v-card-title>
        <v-divider />
        <v-card-text class="pa-5">
          <v-alert type="info" variant="tonal" density="compact" class="mb-4">
            <div class="d-flex justify-space-between">
              <span>Số tiền còn nợ:</span>
              <strong class="text-error">{{ fmt(selectedDebt?.remainingAmount || 0) }}</strong>
            </div>
          </v-alert>
          
          <v-select 
            v-model="installmentCount" 
            :items="[2, 3, 4, 6]" 
            label="Số đợt thanh toán" 
            density="compact"
            class="mb-4"
            variant="outlined"
            rounded="lg"
            color="success"
          >
            <template #prepend-inner>
              <v-icon>mdi-calendar-clock</v-icon>
            </template>
          </v-select>
          
          <div v-if="installmentPlan.length" class="installment-preview">
            <p class="text-subtitle-2 font-weight-bold mb-3">Lịch trả góp dự kiến:</p>
            <v-table density="compact" class="installment-table">
              <thead>
                <tr>
                  <th class="text-left">Đợt</th>
                  <th class="text-left">Số tiền</th>
                  <th class="text-left">Ngày đến hạn</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in installmentPlan" :key="item.no">
                  <td>
                    <v-chip size="x-small" color="success" variant="flat">{{ item.no }}</v-chip>
                  </td>
                  <td class="font-weight-bold text-success">{{ fmt(item.amount) }}</td>
                  <td>{{ item.dueDate }}</td>
                </tr>
              </tbody>
            </v-table>
          </div>
        </v-card-text>
        <v-divider />
        <v-card-actions class="pa-5 pt-3">
          <v-spacer />
          <v-btn variant="outlined" rounded="lg" @click="showInstallment = false">
            Hủy bỏ
          </v-btn>
          <v-btn color="success" variant="flat" rounded="lg" prepend-icon="mdi-check" @click="confirmInstallment">
            Xác nhận
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

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
const statusFilter = ref('All')
const loading = ref(false)
const showInstallment = ref(false)
const selectedDebt = ref(null)
const installmentCount = ref(3)
const snack = ref(false)
const snackText = ref('')
const snackColor = ref('success')
const snackIcon = ref('mdi-check-circle')

const statusOptions = ['All', 'Overdue', 'Warning']

const instHeaders = [
  { title: 'Đợt', key: 'no', width: 60 },
  { title: 'Số tiền', key: 'amount' },
  { title: 'Hạn', key: 'dueDate' },
]

const headers = [
  { title: 'Học viên', key: 'studentName', minWidth: '200px' },
  { title: 'Khóa học', key: 'courseName', minWidth: '150px' },
  { title: 'Tổng phí', key: 'totalFee', align: 'end' },
  { title: 'Còn nợ', key: 'remainingAmount', align: 'end' },
  { title: 'Tiến độ', key: 'progress', align: 'center', width: '130px' },
  { title: 'Quá hạn', key: 'daysOverdue', align: 'center', width: '120px' },
  { title: 'Trạng thái', key: 'status', align: 'center', width: '110px' },
  { title: 'Thao tác', key: 'actions', width: '220px', sortable: false },
]

const fmt = v => new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', maximumFractionDigits: 0 }).format(v || 0)

const filteredDebts = computed(() => {
  let list = payment.debts
  if (search.value) {
    const q = search.value.toLowerCase()
    list = list.filter(d => d.studentName?.toLowerCase().includes(q) || d.courseName?.toLowerCase().includes(q))
  }
  if (statusFilter.value !== 'All') list = list.filter(d => d.status === statusFilter.value)
  return list
})

const totalDebt = computed(() => payment.debts.reduce((s, d) => s + (d.remainingAmount || 0), 0))

const kpis = computed(() => [
  { 
    label: 'Tổng công nợ', 
    value: fmt(totalDebt.value), 
    icon: 'mdi-alert-circle', 
    color: '#D32F2F', 
    bgColor: 'error',
    gradient: 'linear-gradient(135deg, rgba(211,47,47,0.3), transparent)',
  },
  { 
    label: 'Học viên nợ', 
    value: payment.debts.length, 
    icon: 'mdi-account-alert', 
    color: '#E65100', 
    bgColor: 'warning',
    gradient: 'linear-gradient(135deg, rgba(230,81,0,0.3), transparent)',
  },
  { 
    label: 'Quá hạn > 30 ngày', 
    value: payment.debts.filter(d => d.daysOverdue > 30).length, 
    icon: 'mdi-timer-alert', 
    color: '#C62828', 
    bgColor: 'error',
    gradient: 'linear-gradient(135deg, rgba(198,40,40,0.3), transparent)',
  },
  { 
    label: 'Cảnh báo sắp tới', 
    value: payment.debts.filter(d => d.status === 'Warning').length, 
    icon: 'mdi-alert', 
    color: '#FF8F00', 
    bgColor: 'warning',
    gradient: 'linear-gradient(135deg, rgba(255,143,0,0.3), transparent)',
  },
])

const installmentPlan = computed(() => {
  if (!selectedDebt.value) return []
  const amt = Math.ceil(selectedDebt.value.remainingAmount / installmentCount.value)
  return Array.from({ length: installmentCount.value }, (_, i) => ({
    no: i + 1,
    amount: i < installmentCount.value - 1 ? amt : selectedDebt.value.remainingAmount - amt * (installmentCount.value - 1),
    dueDate: new Date(Date.now() + (i + 1) * 30 * 86400000).toLocaleDateString('vi-VN'),
  }))
})

function getAvatarColor(days) {
  if (days > 30) return 'error'
  if (days > 0) return 'warning'
  return 'success'
}

function getDaysColor(days) {
  if (days > 30) return 'error'
  if (days > 0) return 'warning'
  return 'success'
}

function getDaysIcon(days) {
  if (days > 30) return 'mdi-alert-circle'
  if (days > 0) return 'mdi-clock-alert'
  return 'mdi-check-circle'
}

function getProgress(item) {
  if (!item.totalFee) return 0
  return Math.round(((item.totalFee - item.remainingAmount) / item.totalFee) * 100)
}

function getProgressColor(item) {
  const p = getProgress(item)
  if (p >= 80) return 'success'
  if (p >= 50) return 'warning'
  return 'error'
}

function getStatusText(s) {
  return { Overdue: 'Quá hạn', Warning: 'Cảnh báo' }[s] || s
}

function openInstallment(item) { selectedDebt.value = item; showInstallment.value = true }

function confirmInstallment() {
  showInstallment.value = false
  snackText.value = `Đã thiết lập trả góp ${installmentCount.value} đợt cho ${selectedDebt.value.studentName}`
  snackColor.value = 'success'
  snackIcon.value = 'mdi-check-circle'
  snack.value = true
}

function sendReminder(item) {
  snackText.value = `Đã gửi email nhắc nợ tới ${item.studentName}`
  snackColor.value = 'warning'
  snackIcon.value = 'mdi-email-check'
  snack.value = true
}

async function sendReminders() {
  loading.value = true
  await new Promise(r => setTimeout(r, 1000))
  snackText.value = `Đã gửi nhắc nợ tới ${payment.debts.length} học viên!`
  snackColor.value = 'warning'
  snackIcon.value = 'mdi-email-multiple'
  snack.value = true
  loading.value = false
}

onMounted(() => payment.fetchDebts())
</script>

<style scoped>
.debt-manager {
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

.total-debt {
  display: flex;
  align-items: center;
  padding: 8px 16px;
  background: rgba(211, 47, 47, 0.05);
  border-radius: 8px;
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
  white-space: nowrap;
}

.custom-table :deep(tr:hover) {
  background: rgba(46, 125, 50, 0.04) !important;
}

.action-btn {
  text-transform: none;
  font-weight: 500;
}

/* Installment Dialog */
.installment-dialog {
  overflow: hidden;
}

.installment-preview {
  background: #f8f9fa;
  padding: 16px;
  border-radius: 12px;
}

.installment-table {
  background: transparent !important;
}
</style>
