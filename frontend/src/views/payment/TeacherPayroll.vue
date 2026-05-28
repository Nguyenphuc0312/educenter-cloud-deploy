<template>
  <div class="teacher-payroll">
    <!-- Header -->
    <div class="page-header mb-6">
      <div>
        <h2 class="text-h4 font-weight-bold">Bảng Lương Giáo Viên</h2>
        <p class="text-body-2" style="color:#64748b">Quản lý và chi trả lương cho giáo viên</p>
      </div>
      <div class="header-actions">
        <v-btn-toggle v-model="monthFilter" mandatory density="compact" rounded="lg" color="success">
          <v-btn v-for="m in months" :key="m" :value="m" size="small">{{ m }}</v-btn>
        </v-btn-toggle>
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
              <div class="kpi-trend mt-1" v-if="kpi.trend !== undefined">
                <v-icon :color="kpi.trend > 0 ? 'success' : 'error'" size="14">
                  {{ kpi.trend > 0 ? 'mdi-arrow-up' : 'mdi-arrow-down' }}
                </v-icon>
                <span class="text-caption ml-1" :style="{ color: kpi.trend > 0 ? '#388E3C' : '#D32F2F' }">
                  {{ Math.abs(kpi.trend) }}%
                </span>
              </div>
            </div>
            <v-avatar size="52" :color="kpi.bgColor" variant="tonal" rounded="xl">
              <v-icon :color="kpi.color" size="26">{{ kpi.icon }}</v-icon>
            </v-avatar>
          </div>
        </v-card>
      </v-col>
    </v-row>

    <!-- Actions Bar -->
    <v-card class="mb-6" rounded="xl">
      <div class="d-flex flex-wrap align-center justify-space-between pa-4 gap-3">
        <div class="d-flex align-center gap-3">
          <v-chip color="success" variant="flat" size="small">
            <v-icon start size="14">mdi-calendar-check</v-icon>
            Tháng {{ monthFilter }}
          </v-chip>
          <span class="text-body-2" style="color:#64748b">
            {{ payrollData.length }} giáo viên
          </span>
        </div>
        <div class="d-flex align-center gap-2">
          <v-btn color="success" variant="tonal" prepend-icon="mdi-file-excel" size="small">
            <v-icon start size="18">mdi-microsoft-excel</v-icon>
            Xuất Excel
          </v-btn>
          <v-btn color="error" variant="tonal" prepend-icon="mdi-file-pdf-box" size="small">
            <v-icon start size="18">mdi-file-pdf-box</v-icon>
            Xuất PDF
          </v-btn>
        </div>
      </div>
    </v-card>

    <!-- Payroll Table -->
    <v-card class="table-card" rounded="xl">
      <v-data-table 
        :headers="headers" 
        :items="payrollData" 
        :items-per-page="10" 
        hover
        class="custom-table"
      >
        <template #item.teacherName="{ item }">
          <div class="d-flex align-center py-2">
            <v-avatar size="44" color="success" variant="tonal" class="mr-3">
              <span class="text-subtitle-1 font-weight-bold">{{ item.teacherName?.[0] }}</span>
            </v-avatar>
            <div>
              <span class="font-weight-medium text-body-1">{{ item.teacherName }}</span>
              <br>
              <span class="text-caption" style="color:#64748b">
                <v-icon size="12" class="mr-1">mdi-star</v-icon>
                {{ item.specialty }}
              </span>
            </div>
          </div>
        </template>
        
        <template #item.totalClasses="{ item }">
          <div class="text-center">
            <span class="text-body-2 font-weight-medium">{{ item.totalClasses }}</span>
            <br>
            <span class="text-caption" style="color:#64748b">lớp</span>
          </div>
        </template>
        
        <template #item.totalSessions="{ item }">
          <v-chip size="small" color="info" variant="tonal" label>
            {{ item.totalSessions }} buổi
          </v-chip>
        </template>
        
        <template #item.baseSalary="{ item }">
          <span class="font-weight-medium">{{ fmt(item.baseSalary) }}</span>
        </template>
        
        <template #item.bonus="{ item }">
          <span class="text-success font-weight-medium">+{{ fmt(item.bonus) }}</span>
        </template>
        
        <template #item.deduction="{ item }">
          <span class="text-error">-{{ fmt(item.deduction) }}</span>
        </template>
        
        <template #item.netSalary="{ item }">
          <v-chip color="success" variant="flat" size="small" label class="font-weight-bold text-body-2">
            {{ fmt(item.netSalary) }}
          </v-chip>
        </template>
        
        <template #item.status="{ item }">
          <v-chip 
            :color="item.status === 'Paid' ? 'success' : 'warning'" 
            variant="flat" 
            size="small" 
            label
            class="font-weight-medium"
          >
            <v-icon start size="14">{{ item.status === 'Paid' ? 'mdi-check-circle' : 'mdi-clock-outline' }}</v-icon>
            {{ item.status === 'Paid' ? 'Đã trả' : 'Chưa trả' }}
          </v-chip>
        </template>
        
        <template #item.actions="{ item }">
          <v-btn 
            v-if="item.status !== 'Paid'" 
            size="small" 
            variant="flat" 
            color="success"
            class="action-btn"
            @click="markPaid(item)"
          >
            <v-icon start size="16">mdi-cash</v-icon>
            Trả lương
          </v-btn>
          <v-btn 
            v-else
            size="small" 
            variant="text" 
            color="success"
            disabled
          >
            <v-icon start size="16">mdi-check</v-icon>
            Đã xong
          </v-btn>
        </template>
      </v-data-table>

      <!-- Summary Footer -->
      <v-divider />
      <div class="d-flex align-center justify-space-between pa-5 summary-footer">
        <div>
          <span class="text-subtitle-1 font-weight-bold">Tổng chi lương tháng {{ monthFilter }}:</span>
        </div>
        <div class="d-flex align-center gap-3">
          <span class="text-caption" style="color:#64748b">
            {{ payrollData.filter(p => p.status === 'Paid').length }}/{{ payrollData.length }} đã thanh toán
          </span>
          <v-chip color="success" variant="flat" size="large" label class="text-h6 font-weight-bold">
            {{ fmt(totalPayroll) }}
          </v-chip>
        </div>
      </div>
    </v-card>

    <!-- Snackbar -->
    <v-snackbar v-model="snack" color="success" rounded="lg" location="top" timeout="3000">
      <v-icon class="mr-2">mdi-check-circle</v-icon>
      {{ snackText }}
    </v-snackbar>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { usePaymentStore } from '@/stores/payment'

const payment = usePaymentStore()
const monthFilter = ref('5/2026')
const months = ['3/2026', '4/2026', '5/2026']
const snack = ref(false)
const snackText = ref('')

const fmt = v => new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', maximumFractionDigits: 0 }).format(v || 0)

const headers = [
  { title: 'Giáo viên', key: 'teacherName', minWidth: '220px' },
  { title: 'Số lớp', key: 'totalClasses', align: 'center', width: '80px' },
  { title: 'Số buổi', key: 'totalSessions', align: 'center', width: '100px' },
  { title: 'Lương cơ bản', key: 'baseSalary', align: 'end', minWidth: '130px' },
  { title: 'Thưởng', key: 'bonus', align: 'end', minWidth: '110px' },
  { title: 'Khấu trừ', key: 'deduction', align: 'end', minWidth: '110px' },
  { title: 'Thực lĩnh', key: 'netSalary', align: 'end', minWidth: '140px' },
  { title: 'Trạng thái', key: 'status', align: 'center', width: '120px' },
  { title: 'Thao tác', key: 'actions', align: 'center', width: '130px', sortable: false },
]

const payrollData = computed(() => payment.payrolls)
const totalPayroll = computed(() => payrollData.value.reduce((s, p) => s + (p.netSalary || 0), 0))

const kpis = computed(() => [
  { 
    label: 'Tổng chi lương', 
    value: fmt(totalPayroll.value), 
    icon: 'mdi-cash-multiple', 
    color: '#2E7D32', 
    bgColor: 'success',
    gradient: 'linear-gradient(135deg, rgba(46,125,50,0.3), transparent)',
  },
  { 
    label: 'GV đang dạy', 
    value: payrollData.value.filter(p => p.totalClasses > 0).length, 
    icon: 'mdi-human-male-board', 
    color: '#1976D2', 
    bgColor: 'primary',
    gradient: 'linear-gradient(135deg, rgba(25,118,210,0.3), transparent)',
  },
  { 
    label: 'Tổng buổi dạy', 
    value: payrollData.value.reduce((s, p) => s + (p.totalSessions || 0), 0), 
    icon: 'mdi-clock-outline', 
    color: '#00838F', 
    bgColor: 'info',
    gradient: 'linear-gradient(135deg, rgba(0,131,143,0.3), transparent)',
    suffix: ' buổi'
  },
  { 
    label: 'Đã thanh toán', 
    value: `${payrollData.value.filter(p => p.status === 'Paid').length}/${payrollData.value.length}`, 
    icon: 'mdi-check-circle', 
    color: '#7B1FA2', 
    bgColor: 'secondary',
    gradient: 'linear-gradient(135deg, rgba(123,31,162,0.3), transparent)',
  },
])

async function markPaid(item) {
  await payment.markPayrollPaid(item.id)
  snackText.value = `Đã trả lương cho ${item.teacherName}`
  snack.value = true
}

onMounted(() => payment.fetchPayrolls())
</script>

<style scoped>
.teacher-payroll {
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

.action-btn {
  text-transform: none;
  font-weight: 500;
}

.summary-footer {
  background: linear-gradient(135deg, rgba(46,125,50,0.03), rgba(46,125,50,0.08));
}
</style>
