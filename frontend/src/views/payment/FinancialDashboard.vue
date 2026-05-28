<template>
  <div class="financial-dashboard">
    <!-- Header -->
    <div class="dashboard-header mb-6">
      <div>
        <h2 class="text-h4 font-weight-bold">Phân Tích Tài Chính</h2>
        <p class="text-body-2" style="color:#64748b">Báo cáo và biểu đồ chi tiết về tài chính</p>
      </div>
      <div class="header-actions">
        <v-btn-toggle v-model="chartPeriod" mandatory density="compact" rounded="lg" color="success">
          <v-btn value="6m" size="x-small">6 tháng</v-btn>
          <v-btn value="12m" size="x-small">12 tháng</v-btn>
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
              <div class="kpi-trend mt-2 d-flex align-center">
                <v-icon :color="kpi.trend > 0 ? 'success' : 'error'" size="16">
                  {{ kpi.trend > 0 ? 'mdi-arrow-up-bold' : 'mdi-arrow-down-bold' }}
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

    <!-- Main Charts -->
    <v-row class="mb-6">
      <v-col cols="12" md="8">
        <v-card class="chart-card pa-5" rounded="xl">
          <div class="d-flex align-center mb-4">
            <div>
              <span class="text-subtitle-1 font-weight-bold">Doanh Thu vs Mục Tiêu</span>
              <p class="text-caption" style="color:#64748b">So sánh thực tế với kế hoạch</p>
            </div>
            <v-spacer />
            <v-chip size="small" color="success" variant="tonal">
              <v-icon start size="14">mdi-check-circle</v-icon>
              Vượt mục tiêu
            </v-chip>
          </div>
          <div class="chart-container">
            <canvas ref="revenueChart"></canvas>
          </div>
        </v-card>
      </v-col>
      <v-col cols="12" md="4">
        <v-card class="chart-card pa-5 h-100" rounded="xl">
          <span class="text-subtitle-1 font-weight-bold d-block mb-1">Doanh Thu Theo Lĩnh Vực</span>
          <p class="text-caption mb-3" style="color:#64748b">Phân bổ theo ngành</p>
          <div class="chart-container-pie">
            <canvas ref="categoryChart"></canvas>
          </div>
        </v-card>
      </v-col>
    </v-row>

    <!-- Secondary Charts -->
    <v-row>
      <v-col cols="12" md="6">
        <v-card class="chart-card pa-5" rounded="xl">
          <div class="d-flex align-center mb-3">
            <div>
              <span class="text-subtitle-1 font-weight-bold">Chi Phí Lương vs Doanh Thu</span>
              <p class="text-caption" style="color:#64748b">Tỷ lệ chi phí nhân sự</p>
            </div>
            <v-spacer />
            <v-chip size="x-small" color="warning" variant="flat">28.8%</v-chip>
          </div>
          <div class="chart-container-bar">
            <canvas ref="costChart"></canvas>
          </div>
        </v-card>
      </v-col>
      <v-col cols="12" md="6">
        <v-card class="chart-card pa-5" rounded="xl">
          <div class="d-flex align-center mb-3">
            <div>
              <span class="text-subtitle-1 font-weight-bold">Biên Lợi Nhuận</span>
              <p class="text-caption" style="color:#64748b">Tỷ suất lợi nhuận theo tháng</p>
            </div>
            <v-spacer />
            <div class="profit-badge">
              <v-icon size="14" color="success">mdi-trending-up</v-icon>
              <span class="text-success font-weight-bold ml-1">Trung bình 58%</span>
            </div>
          </div>
          <div class="chart-container-line">
            <canvas ref="profitChart"></canvas>
          </div>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { Chart, registerables } from 'chart.js'

Chart.register(...registerables)

const revenueChart = ref(null)
const categoryChart = ref(null)
const costChart = ref(null)
const profitChart = ref(null)
const chartPeriod = ref('6m')

const fmt = v => new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', maximumFractionDigits: 0 }).format(v * 1000000)

const kpis = [
  { 
    label: 'Doanh Thu Tháng 5', 
    value: fmt(295), 
    icon: 'mdi-trending-up', 
    color: '#2E7D32', 
    bgColor: 'success',
    gradient: 'linear-gradient(135deg, rgba(46,125,50,0.3), transparent)',
    trend: 6.9 
  },
  { 
    label: 'Chi Phí Lương', 
    value: fmt(85), 
    icon: 'mdi-cash', 
    color: '#E65100', 
    bgColor: 'warning',
    gradient: 'linear-gradient(135deg, rgba(230,81,0,0.3), transparent)',
    trend: -3.1 
  },
  { 
    label: 'Biên Lợi Nhuận', 
    value: '58%', 
    icon: 'mdi-chart-line', 
    color: '#7B1FA2', 
    bgColor: 'secondary',
    gradient: 'linear-gradient(135deg, rgba(123,31,162,0.3), transparent)',
    trend: 0.8 
  },
  { 
    label: 'Chi Phí Vận Hành', 
    value: fmt(32), 
    icon: 'mdi-cog', 
    color: '#00838F', 
    bgColor: 'info',
    gradient: 'linear-gradient(135deg, rgba(0,131,143,0.3), transparent)',
    trend: 15 
  },
]

onMounted(() => {
  const labels = ['T12/2025', 'T1/2026', 'T2/2026', 'T3/2026', 'T4/2026', 'T5/2026']
  const actual = [185, 245, 198, 312, 278, 295]
  const target = [180, 220, 200, 280, 300, 290]
  const payroll = [65, 72, 68, 80, 78, 85]
  const operating = [20, 22, 18, 25, 28, 32]
  const margin = [54, 59, 56, 66, 62, 58]

  // Revenue Chart with Target Line
  new Chart(revenueChart.value, {
    type: 'bar',
    data: {
      labels,
      datasets: [
        { 
          label: 'Mục tiêu', 
          data: target, 
          type: 'line',
          borderColor: '#4CAF50',
          borderWidth: 2,
          borderDash: [6, 4],
          pointBackgroundColor: '#4CAF50',
          pointRadius: 4,
          fill: false,
          tension: 0.3,
        },
        { 
          label: 'Thực tế', 
          data: actual, 
          backgroundColor: (ctx) => {
            const gradient = ctx.chart.ctx.createLinearGradient(0, 0, 0, 300)
            gradient.addColorStop(0, 'rgba(46, 125, 50, 0.9)')
            gradient.addColorStop(1, 'rgba(76, 175, 80, 0.5)')
            return gradient
          },
          borderRadius: 6,
          barThickness: 32,
        },
      ]
    },
    options: { 
      responsive: true, 
      maintainAspectRatio: false,
      plugins: { 
        legend: { 
          position: 'bottom',
          labels: { usePointStyle: true, padding: 20 }
        } 
      }, 
      scales: { 
        y: { 
          beginAtZero: true, 
          grid: { color: 'rgba(0,0,0,0.05)' },
          ticks: { callback: v => v + 'M' } 
        },
        x: { grid: { display: false } }
      } 
    }
  })

  // Category Doughnut
  new Chart(categoryChart.value, {
    type: 'doughnut',
    data: {
      labels: ['Tiếng Anh', 'Lập trình', 'Marketing', 'Kế toán', 'Thiết kế'],
      datasets: [{ 
        data: [45, 25, 12, 10, 8], 
        backgroundColor: ['#2E7D32', '#1976D2', '#E65100', '#7B1FA2', '#00838F'],
        borderWidth: 0,
        hoverOffset: 8,
      }]
    },
    options: { 
      responsive: true, 
      maintainAspectRatio: false,
      cutout: '60%',
      plugins: { legend: { display: false } } 
    }
  })

  // Cost Comparison
  new Chart(costChart.value, {
    type: 'bar',
    data: {
      labels,
      datasets: [
        { 
          label: 'Doanh thu', 
          data: actual, 
          backgroundColor: 'rgba(46, 125, 50, 0.7)', 
          borderRadius: 4,
        },
        { 
          label: 'Lương GV', 
          data: payroll, 
          backgroundColor: 'rgba(230, 81, 0, 0.7)', 
          borderRadius: 4,
        },
        { 
          label: 'Vận hành', 
          data: operating, 
          backgroundColor: 'rgba(0, 131, 143, 0.7)', 
          borderRadius: 4,
        },
      ]
    },
    options: { 
      responsive: true, 
      maintainAspectRatio: false,
      plugins: { 
        legend: { 
          position: 'bottom',
          labels: { usePointStyle: true, padding: 15 }
        } 
      }, 
      scales: { 
        y: { 
          beginAtZero: true, 
          grid: { color: 'rgba(0,0,0,0.05)' },
          ticks: { callback: v => v + 'M' } 
        },
        x: { grid: { display: false } }
      } 
    }
  })

  // Profit Margin Line
  new Chart(profitChart.value, {
    type: 'line',
    data: {
      labels,
      datasets: [{ 
        label: 'Biên LN %', 
        data: margin, 
        borderColor: '#7B1FA2',
        backgroundColor: 'rgba(123, 31, 162, 0.1)',
        fill: true, 
        tension: 0.4, 
        pointRadius: 6,
        pointBackgroundColor: '#7B1FA2',
        pointBorderColor: '#fff',
        pointBorderWidth: 2,
      }]
    },
    options: { 
      responsive: true, 
      maintainAspectRatio: false,
      plugins: { legend: { display: false } }, 
      scales: { 
        y: { 
          min: 40, 
          max: 85, 
          grid: { color: 'rgba(0,0,0,0.05)' },
          ticks: { callback: v => v + '%' } 
        },
        x: { grid: { display: false } }
      } 
    }
  })
})
</script>

<style scoped>
.financial-dashboard {
  animation: fadeIn 0.5s ease-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}

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

.chart-card {
  border: 1px solid rgba(0,0,0,0.05);
  transition: all 0.3s ease;
}

.chart-card:hover {
  box-shadow: 0 10px 30px rgba(0,0,0,0.08) !important;
}

.chart-container { height: 300px; position: relative; }
.chart-container-pie { height: 220px; position: relative; }
.chart-container-bar { height: 250px; position: relative; }
.chart-container-line { height: 250px; position: relative; }

.profit-badge {
  display: flex;
  align-items: center;
  padding: 4px 12px;
  background: rgba(76, 175, 80, 0.1);
  border-radius: 20px;
}
</style>
