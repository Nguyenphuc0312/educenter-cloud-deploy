<template>
  <div class="admin-dashboard">
    <!-- Header -->
    <div class="dashboard-header mb-6">
      <div>
        <h2 class="text-h4 font-weight-bold">Tổng Quan Hệ Thống</h2>
        <p class="text-body-2" style="color:#64748b">Chào mừng đến với Payment & Report Service - Nhóm 3</p>
      </div>
      <div class="header-actions">
        <v-chip color="success" variant="flat" size="small">
          <v-icon start size="14">mdi-circle</v-icon>
          Live
        </v-chip>
        <span class="text-caption" style="color:#64748b">{{ currentDate }}</span>
      </div>
    </div>

    <!-- KPI Cards -->
    <v-row class="mb-6">
      <v-col cols="12" sm="6" lg="3" v-for="(kpi, index) in kpis" :key="kpi.label">
        <v-card 
          class="kpi-card" 
          :style="{ animationDelay: index * 0.1 + 's' }"
          rounded="xl"
        >
          <div class="kpi-glow" :style="{ background: kpi.gradient }"></div>
          <div class="d-flex align-center justify-space-between pa-4">
            <div class="kpi-content">
              <p class="text-caption mb-1 kpi-label">{{ kpi.label }}</p>
              <h3 class="text-h5 font-weight-bold kpi-value" :style="{ color: kpi.color }">{{ kpi.value }}</h3>
              <div class="kpi-trend mt-2">
                <v-icon 
                  :color="kpi.trend > 0 ? 'success' : 'error'" 
                  size="16"
                >
                  {{ kpi.trend > 0 ? 'mdi-trending-up' : 'mdi-trending-down' }}
                </v-icon>
                <span 
                  class="text-caption ml-1" 
                  :style="{ color: kpi.trend > 0 ? '#388E3C' : '#D32F2F' }"
                >
                  {{ Math.abs(kpi.trend) }}% so với tháng trước
                </span>
              </div>
            </div>
            <v-avatar 
              size="56" 
              :color="kpi.bgColor" 
              variant="tonal" 
              rounded="xl"
              class="kpi-icon"
            >
              <v-icon :color="kpi.color" size="28">{{ kpi.icon }}</v-icon>
            </v-avatar>
          </div>
        </v-card>
      </v-col>
    </v-row>

    <!-- Charts Row -->
    <v-row class="mb-6">
      <v-col cols="12" lg="8">
        <v-card class="chart-card pa-5" rounded="xl">
          <div class="d-flex align-center mb-4">
            <div>
              <span class="text-subtitle-1 font-weight-bold">Doanh Thu 6 Tháng</span>
              <p class="text-caption" style="color:#64748b">Biểu đồ doanh thu học phí</p>
            </div>
            <v-spacer />
            <v-btn-toggle v-model="chartPeriod" mandatory density="compact" rounded="lg" color="success">
              <v-btn value="6m" size="x-small">6 tháng</v-btn>
              <v-btn value="12m" size="x-small">12 tháng</v-btn>
            </v-btn-toggle>
          </div>
          <div class="chart-container">
            <canvas ref="revenueChart"></canvas>
          </div>
        </v-card>
      </v-col>
      <v-col cols="12" lg="4">
        <v-card class="chart-card pa-5 h-100" rounded="xl">
          <span class="text-subtitle-1 font-weight-bold d-block mb-1">Doanh Thu Theo Loại</span>
          <p class="text-caption mb-4" style="color:#64748b">Phân bổ theo khóa học</p>
          <div class="chart-container-doughnut">
            <canvas ref="categoryChart"></canvas>
          </div>
          <div class="chart-legend mt-4">
            <div v-for="(item, i) in legendItems" :key="i" class="legend-item">
              <div class="legend-color" :style="{ background: item.color }"></div>
              <span class="text-caption">{{ item.label }}</span>
              <span class="text-caption font-weight-bold ml-auto">{{ item.value }}%</span>
            </div>
          </div>
        </v-card>
      </v-col>
    </v-row>

    <!-- Recent Payments Table -->
    <v-row>
      <v-col cols="12">
        <v-card class="table-card" rounded="xl">
          <v-card-title class="d-flex align-center pa-5">
            <div>
              <span class="text-subtitle-1 font-weight-bold">Hóa Đơn Gần Đây</span>
              <p class="text-caption" style="color:#64748b">5 hóa đơn mới nhất</p>
            </div>
            <v-spacer />
            <v-btn color="success" variant="tonal" size="small">
              <v-icon start size="18">mdi-plus</v-icon>
              Thêm Hóa Đơn
            </v-btn>
          </v-card-title>
          <v-divider />
          <v-data-table 
            :headers="headers" 
            :items="recentPayments" 
            :items-per-page="5" 
            density="comfortable"
            class="custom-table"
          >
            <template #item.status="{ item }">
              <v-chip 
                :color="getStatusColor(item.status)" 
                variant="flat" 
                size="small" 
                label
                class="status-chip"
              >
                <v-icon start size="14">{{ getStatusIcon(item.status) }}</v-icon>
                {{ getStatusText(item.status) }}
              </v-chip>
            </template>
            <template #item.paidAmount="{ item }">
              <span class="font-weight-bold text-success">{{ fmt(item.paidAmount) }}</span>
            </template>
            <template #item.amount="{ item }">
              <span>{{ fmt(item.amount) }}</span>
            </template>
            <template #item.actions="{ item }">
              <v-btn icon variant="text" size="small" color="primary">
                <v-icon size="18">mdi-eye</v-icon>
              </v-btn>
              <v-btn icon variant="text" size="small" color="success">
                <v-icon size="18">mdi-pencil</v-icon>
              </v-btn>
            </template>
          </v-data-table>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { Chart, registerables } from 'chart.js'
import { usePaymentStore } from '@/stores/payment'

Chart.register(...registerables)

const payment = usePaymentStore()
const revenueChart = ref(null)
const categoryChart = ref(null)
const chartPeriod = ref('6m')

const currentDate = computed(() => {
  return new Date().toLocaleDateString('vi-VN', { 
    weekday: 'long', 
    year: 'numeric', 
    month: 'long', 
    day: 'numeric' 
  })
})

const fmt = v => new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', maximumFractionDigits: 0 }).format(v || 0)

const recentPayments = computed(() => payment.invoices.slice(0, 5))

const kpis = computed(() => [
  { 
    label: 'Doanh Thu Tháng 5', 
    value: fmt(295000000), 
    icon: 'mdi-trending-up', 
    color: '#2E7D32', 
    bgColor: 'success',
    gradient: 'linear-gradient(135deg, rgba(46,125,50,0.3), rgba(76,175,80,0.1))',
    trend: 6.9 
  },
  { 
    label: 'Học Viên Đang Học', 
    value: '20', 
    icon: 'mdi-account-group', 
    color: '#1976D2', 
    bgColor: 'primary',
    gradient: 'linear-gradient(135deg, rgba(25,118,210,0.3), rgba(100,181,246,0.1))',
    trend: 12.5 
  },
  { 
    label: 'Tổng Công Nợ', 
    value: fmt(payment.totalDebt || 45000000), 
    icon: 'mdi-alert-circle-outline', 
    color: '#E65100', 
    bgColor: 'warning',
    gradient: 'linear-gradient(135deg, rgba(230,81,0,0.3), rgba(255,183,77,0.1))',
    trend: -2.1 
  },
  { 
    label: 'Hóa Đơn Đã Thanh Toán', 
    value: payment.invoices.length || 12, 
    icon: 'mdi-receipt-text', 
    color: '#7B1FA2', 
    bgColor: 'secondary',
    gradient: 'linear-gradient(135deg, rgba(123,31,162,0.3), rgba(186,104,200,0.1))',
    trend: 3.1 
  },
])

const legendItems = [
  { label: 'Tiếng Anh', value: 45, color: '#2E7D32' },
  { label: 'Lập trình', value: 25, color: '#1976D2' },
  { label: 'Marketing', value: 12, color: '#E65100' },
  { label: 'Kế toán', value: 10, color: '#7B1FA2' },
  { label: 'Thiết kế', value: 8, color: '#00838F' },
]

const headers = [
  { title: 'Mã HV', key: 'studentId' },
  { title: 'Khóa học', key: 'courseName' },
  { title: 'Tổng tiền', key: 'amount', align: 'end' },
  { title: 'Đã TT', key: 'paidAmount', align: 'end' },
  { title: 'Trạng thái', key: 'status' },
  { title: '', key: 'actions', sortable: false, align: 'end' },
]

function getStatusColor(s) { return { Paid: 'success', Partial: 'warning', Unpaid: 'error' }[s] || 'secondary' }
function getStatusIcon(s) { return { Paid: 'mdi-check-circle', Partial: 'mdi-clock-outline', Unpaid: 'mdi-alert-circle' }[s] || 'mdi-help-circle' }
function getStatusText(s) { return { Paid: 'Đã thanh toán', Partial: 'Một phần', Unpaid: 'Chưa TT' }[s] || s }

onMounted(async () => {
  await payment.fetchInvoices()

  new Chart(revenueChart.value, {
    type: 'bar',
    data: {
      labels: ['T12/2025', 'T1/2026', 'T2/2026', 'T3/2026', 'T4/2026', 'T5/2026'],
      datasets: [{
        label: 'Doanh thu (triệu)',
        data: [185, 245, 198, 312, 278, 295],
        backgroundColor: (ctx) => {
          const gradient = ctx.chart.ctx.createLinearGradient(0, 0, 0, 300)
          gradient.addColorStop(0, 'rgba(46, 125, 50, 0.9)')
          gradient.addColorStop(1, 'rgba(76, 175, 80, 0.6)')
          return gradient
        },
        borderColor: '#2E7D32',
        borderWidth: 2,
        borderRadius: 8,
        borderSkipped: false,
      }]
    },
    options: { 
      responsive: true, 
      maintainAspectRatio: false,
      plugins: { 
        legend: { display: false } ,
        tooltip: {
          backgroundColor: 'rgba(0,0,0,0.8)',
          padding: 12,
          cornerRadius: 8,
        }
      }, 
      scales: { 
        y: { 
          beginAtZero: true, 
          grid: { color: 'rgba(0,0,0,0.05)' },
          ticks: { callback: v => v + 'M' } 
        },
        x: {
          grid: { display: false }
        }
      } 
    }
  })

  new Chart(categoryChart.value, {
    type: 'doughnut',
    data: {
      labels: ['Tiếng Anh', 'Lập trình', 'Marketing', 'Kế toán', 'Thiết kế'],
      datasets: [{ 
        data: [45, 25, 12, 10, 8], 
        backgroundColor: ['#2E7D32', '#1976D2', '#E65100', '#7B1FA2', '#00838F'],
        borderWidth: 0,
        hoverOffset: 10,
      }]
    },
    options: { 
      responsive: true, 
      maintainAspectRatio: false,
      cutout: '65%',
      plugins: { 
        legend: { display: false },
        tooltip: {
          backgroundColor: 'rgba(0,0,0,0.8)',
          padding: 12,
          cornerRadius: 8,
        }
      } 
    }
  })
})
</script>

<style scoped>
.admin-dashboard {
  animation: fadeIn 0.5s ease-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}

/* Header */
.dashboard-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.header-actions {
  display: flex;
  align-items: center;
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

.kpi-value {
  font-size: 1.6rem !important;
  font-weight: 700 !important;
}

.kpi-icon {
  transition: all 0.3s ease;
}

.kpi-card:hover .kpi-icon {
  transform: scale(1.1) rotate(5deg);
}

/* Chart Cards */
.chart-card {
  background: white;
  border: 1px solid rgba(0,0,0,0.05);
  transition: all 0.3s ease;
}

.chart-card:hover {
  box-shadow: 0 10px 30px rgba(0,0,0,0.08) !important;
}

.chart-container {
  height: 280px;
  position: relative;
}

.chart-container-doughnut {
  height: 180px;
  position: relative;
}

/* Legend */
.chart-legend {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.legend-item {
  display: flex;
  align-items: center;
  gap: 8px;
}

.legend-color {
  width: 12px;
  height: 12px;
  border-radius: 3px;
}

/* Table */
.table-card {
  border: 1px solid rgba(0,0,0,0.05);
  overflow: hidden;
}

.status-chip {
  font-weight: 500;
}

.custom-table :deep(th) {
  background: #f8f9fa !important;
  font-weight: 600 !important;
  color: #2E7D32 !important;
}

.custom-table :deep(tr:hover) {
  background: rgba(46, 125, 50, 0.04) !important;
}
</style>
