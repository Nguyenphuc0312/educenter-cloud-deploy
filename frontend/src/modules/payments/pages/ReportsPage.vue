<template>
  <div class="reports-page">
    <!-- Header -->
    <div class="page-header mb-6">
      <div>
        <h2 class="page-title">Báo Cáo & Thống Kê</h2>
        <p class="page-subtitle">Theo dõi doanh thu, phân tích tài chính</p>
      </div>
      <a-segmented
        v-model:value="activeTab"
        :options="[
          { label: 'Tổng Quan', value: 'overview' },
          { label: 'Doanh Thu', value: 'revenue' },
          { label: 'Theo Khóa', value: 'course' },
        ]"
        size="large"
      />
    </div>

    <!-- Tab: Overview -->
    <div v-if="activeTab === 'overview'">
      <!-- KPI Summary -->
      <a-row :gutter="20" class="mb-6">
        <a-col v-for="(kpi, index) in overviewKpis" :key="kpi.label" :xs="24" :sm="12" :lg="3">
          <div class="kpi-card" :style="{ animationDelay: index * 0.1 + 's' }">
            <div class="kpi-glow" :style="{ background: kpi.gradient }"></div>
            <div class="kpi-body">
              <div class="kpi-info">
                <span class="kpi-label">{{ kpi.label }}</span>
                <strong class="kpi-value" :style="{ color: kpi.color }">{{ kpi.value }}</strong>
              </div>
              <div class="kpi-icon" :style="{ background: kpi.bg }">
                <component :is="kpi.icon" :style="{ color: kpi.color }" />
              </div>
            </div>
          </div>
        </a-col>
      </a-row>

      <!-- Revenue Chart + Category -->
      <a-row :gutter="20" class="mb-6">
        <a-col :xs="24" :lg="16">
          <a-card class="chart-card" :bordered="false">
            <div class="chart-header">
              <div>
                <div class="chart-title">Doanh Thu 6 Tháng</div>
                <div class="chart-sub">So sánh thực tế vs mục tiêu</div>
              </div>
              <a-select v-model:value="selectedYear" style="width:120px">
                <a-select-option v-for="y in yearOptions" :key="y" :value="y">{{ y }}</a-select-option>
              </a-select>
            </div>
            <div class="chart-container">
              <canvas ref="revenueChartRef"></canvas>
            </div>
          </a-card>
        </a-col>
        <a-col :xs="24" :lg="8">
          <a-card class="chart-card h-100" :bordered="false">
            <div class="chart-title mb-1">Phân Bổ Theo Loại</div>
            <div class="chart-sub mb-4">Doanh thu theo khóa học</div>
            <div class="chart-container-doughnut">
              <canvas ref="categoryChartRef"></canvas>
            </div>
            <div class="chart-legend mt-4">
              <div v-for="(item, i) in categoryLegend" :key="i" class="legend-item">
                <div class="legend-color" :style="{ background: item.color }"></div>
                <span>{{ item.label }}</span>
                <span class="legend-percent">{{ item.percent }}%</span>
              </div>
            </div>
          </a-card>
        </a-col>
      </a-row>

      <!-- Recent Activity -->
      <a-row :gutter="20">
        <a-col :xs="24" :md="12">
          <a-card :bordered="false" class="activity-card">
            <div class="card-title">Thanh Toán Gần Đây</div>
            <div v-for="item in recentPayments" :key="item.id" class="recent-item">
              <div class="recent-row">
                <div class="recent-left">
                  <a-avatar :size="36" style="background:#2e7d32; flex-shrink:0">
                    <CheckCircleOutlined style="color:white" />
                  </a-avatar>
                  <div>
                    <div class="recent-name">{{ item.studentName }}</div>
                    <div class="recent-sub">{{ item.courseName }}</div>
                  </div>
                </div>
                <div class="recent-right">
                  <div class="recent-amount text-success">{{ fmt(item.paidAmount) }}</div>
                  <div class="recent-date">{{ formatDate(item.paymentDate) }}</div>
                </div>
              </div>
            </div>
            <div v-if="recentPayments.length === 0" class="empty-state">
              <InboxOutlined style="font-size:32px; color:#ccc" />
              <p>Chưa có thanh toán nào</p>
            </div>
          </a-card>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-card :bordered="false" class="activity-card">
            <div class="card-title">Công Nợ Nổi Bật</div>
            <div v-for="item in topDebts" :key="item.id" class="recent-item">
              <div class="recent-row">
                <div class="recent-left">
                  <a-avatar :size="36" style="background:#ff4d4f">
                    {{ getInitials(item.studentName) }}
                  </a-avatar>
                  <div>
                    <div class="recent-name">{{ item.studentName }}</div>
                    <a-tag :color="item.status === 'Overdue' ? 'error' : 'warning'" size="small">
                      {{ item.status === 'Overdue' ? 'Quá hạn' : 'Cảnh báo' }}
                    </a-tag>
                  </div>
                </div>
                <div class="recent-right">
                  <div class="recent-amount text-danger">{{ fmt(item.remainingAmount) }}</div>
                  <div class="recent-date">{{ item.daysOverdue }} ngày quá hạn</div>
                </div>
              </div>
            </div>
            <div v-if="topDebts.length === 0" class="empty-state">
              <InboxOutlined style="font-size:32px; color:#ccc" />
              <p>Không có công nợ nào</p>
            </div>
          </a-card>
        </a-col>
      </a-row>
    </div>

    <!-- Tab: Revenue -->
    <div v-if="activeTab === 'revenue'">
      <a-row :gutter="20" class="mb-6">
        <a-col :xs="24" :sm="8">
          <div class="stat-card">
            <div class="stat-info">
              <div class="stat-label">Tổng Doanh Thu</div>
              <div class="stat-value text-success">{{ fmt(revenueTotal) }}</div>
            </div>
            <div class="stat-icon" style="background:rgba(46,125,50,0.1)">
              <DollarOutlined style="color:#2e7d32; font-size:22px" />
            </div>
          </div>
        </a-col>
        <a-col :xs="24" :sm="8">
          <div class="stat-card">
            <div class="stat-info">
              <div class="stat-label">Số Giao Dịch</div>
              <div class="stat-value text-primary">{{ revenueByMonth.length }}</div>
            </div>
            <div class="stat-icon" style="background:rgba(24,144,255,0.1)">
              <FileTextOutlined style="color:#1890ff; font-size:22px" />
            </div>
          </div>
        </a-col>
        <a-col :xs="24" :sm="8">
          <div class="stat-card">
            <div class="stat-info">
              <div class="stat-label">Trung Bình/Tháng</div>
              <div class="stat-value text-violet">{{ fmt(revenueTotal / Math.max(revenueByMonth.length, 1)) }}</div>
            </div>
            <div class="stat-icon" style="background:rgba(114,46,209,0.1)">
              <LineChartOutlined style="color:#722ed1; font-size:22px" />
            </div>
          </div>
        </a-col>
      </a-row>

      <a-card class="mb-6" :bordered="false">
        <div class="toolbar-row">
          <a-select v-model:value="revenueYear" style="width:150px">
            <a-select-option v-for="y in yearOptions" :key="y" :value="y">{{ y }}</a-select-option>
          </a-select>
          <div class="toolbar-spacer" />
          <a-button style="color:#52c41a; border-color:#52c41a">
            <template #icon><FileExcelOutlined /></template>
            Xuất Excel
          </a-button>
          <a-button style="color:#ff4d4f; border-color:#ff4d4f">
            <template #icon><FilePdfOutlined /></template>
            Xuất PDF
          </a-button>
        </div>
      </a-card>

      <a-card class="mb-6" :bordered="false">
        <div class="chart-title mb-4">Doanh Thu Theo Tháng - {{ revenueYear }}</div>
        <div class="chart-container" style="height:280px">
          <canvas ref="revenueMonthChartRef"></canvas>
        </div>
      </a-card>

      <a-card :bordered="false">
        <a-table
          :columns="revenueColumns"
          :data-source="revenueByMonth"
          :pagination="{ pageSize: 12 }"
          row-key="MonthLabel"
        >
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'Revenue'">
              <span class="text-success" style="font-weight:600">{{ fmt(record.Revenue || 0) }}</span>
            </template>
          </template>
        </a-table>
      </a-card>
    </div>

    <!-- Tab: By Course -->
    <div v-if="activeTab === 'course'">
      <a-row :gutter="20" class="mb-6">
        <a-col v-for="kpi in courseKpis" :key="kpi.label" :xs="24" :sm="8">
          <div class="kpi-card">
            <div class="kpi-glow" :style="{ background: kpi.gradient }"></div>
            <div class="kpi-body">
              <div class="kpi-info">
                <span class="kpi-label">{{ kpi.label }}</span>
                <strong class="kpi-value" :style="{ color: kpi.color }">{{ kpi.value }}</strong>
              </div>
              <div class="kpi-icon" :style="{ background: kpi.bg }">
                <component :is="kpi.icon" :style="{ color: kpi.color }" />
              </div>
            </div>
          </div>
        </a-col>
      </a-row>

      <a-card :bordered="false">
        <div class="toolbar-row mb-4">
          <div class="chart-title">Doanh Thu Theo Khóa Học</div>
          <a-radio-group v-model:value="courseViewMode" button-style="solid">
            <a-radio-button value="table">
              <TableOutlined /> Bảng
            </a-radio-button>
            <a-radio-button value="chart">
              <BarChartOutlined /> Biểu đồ
            </a-radio-button>
          </a-radio-group>
        </div>

        <div v-if="courseViewMode === 'table'">
          <a-table
            :columns="courseColumns"
            :data-source="courseData"
            :pagination="{ pageSize: 10 }"
            row-key="courseName"
          >
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'totalAmount'">
                <span style="font-weight:600">{{ fmt(record.totalAmount) }}</span>
              </template>
              <template v-else-if="column.key === 'paidAmount'">
                <span class="text-success" style="font-weight:600">{{ fmt(record.paidAmount) }}</span>
              </template>
              <template v-else-if="column.key === 'unpaidAmount'">
                <span class="text-danger">{{ fmt(record.unpaidAmount) }}</span>
              </template>
              <template v-else-if="column.key === 'progress'">
                <div style="display:flex; align-items:center; gap:8px">
                  <a-progress
                    :percent="getPaymentProgress(record)"
                    :stroke-color="getPaymentProgressColor(record)"
                    size="small"
                    style="width:100px"
                  />
                  <span style="font-size:12px; color:#888">{{ getPaymentProgress(record) }}%</span>
                </div>
              </template>
            </template>
          </a-table>
        </div>

        <div v-else style="height:400px">
          <canvas ref="courseChartRef"></canvas>
        </div>
      </a-card>
    </div>

    <!-- Loading -->
    <a-spin v-if="loading" size="large" style="position:fixed; top:50%; left:50%; transform:translate(-50%,-50%)" />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch, nextTick } from 'vue'
import {
  CheckCircleOutlined,
  DollarOutlined,
  FileTextOutlined,
  LineChartOutlined,
  FileExcelOutlined,
  FilePdfOutlined,
  TableOutlined,
  BarChartOutlined,
  InboxOutlined,
  RiseOutlined,
  TeamOutlined,
  AlertOutlined,
  BookOutlined,
  TrophyOutlined,
} from '@ant-design/icons-vue'
import { Chart, registerables } from 'chart.js'
import { usePaymentStore } from '@/stores/payment'
import { getRevenueReport, getReportByCourse } from '@/modules/payments/api/paymentApi'

Chart.register(...registerables)

const payment = usePaymentStore()
const activeTab = ref('overview')
const loading = computed(() => payment.loading)
const selectedYear = ref(2026)
const revenueYear = ref(2026)
const courseViewMode = ref('table')
const yearOptions = [2024, 2025, 2026]

const revenueChartRef = ref(null)
const categoryChartRef = ref(null)
const revenueMonthChartRef = ref(null)
const courseChartRef = ref(null)

const revenueByMonth = ref([])
const courseData = ref([])
const revenueTotal = computed(() => revenueByMonth.value.reduce((s, m) => s + (m.Revenue || 0), 0))
const dashboardData = computed(() => payment.dashboard)
const recentPayments = computed(() => payment.invoices.filter(i => i.status === 'Paid').slice(0, 5))
const topDebts = computed(() => [...payment.debts].sort((a, b) => b.remainingAmount - a.remainingAmount).slice(0, 5))

const fmt = v => new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', maximumFractionDigits: 0 }).format(v || 0)

const formatDate = (date) => {
  if (!date) return '—'
  return new Date(date).toLocaleDateString('vi-VN')
}

const getInitials = (name) => {
  if (!name) return '?'
  return name.split(' ').map(n => n[0]).join('').substring(0, 2).toUpperCase()
}

const getPaymentProgress = (item) => {
  if (!item.totalAmount) return 0
  return Math.round((item.paidAmount / item.totalAmount) * 100)
}

const getPaymentProgressColor = (item) => {
  const p = getPaymentProgress(item)
  if (p >= 80) return '#52c41a'
  if (p >= 50) return '#faad14'
  return '#ff4d4f'
}

const revenueColumns = [
  { title: 'Tháng', key: 'MonthLabel', dataIndex: 'MonthLabel' },
  { title: 'Doanh Thu', key: 'Revenue', dataIndex: 'Revenue', align: 'right' },
]

const courseColumns = [
  { title: 'Khóa học', key: 'courseName', dataIndex: 'courseName' },
  { title: 'Tổng tiền', key: 'totalAmount', dataIndex: 'totalAmount', align: 'right' },
  { title: 'Đã TT', key: 'paidAmount', dataIndex: 'paidAmount', align: 'right' },
  { title: 'Còn nợ', key: 'unpaidAmount', dataIndex: 'unpaidAmount', align: 'right' },
  { title: 'Học viên', key: 'studentCount', dataIndex: 'studentCount', align: 'center', width: 100 },
  { title: 'Tiến độ', key: 'progress', align: 'center', width: 160 },
]

const categoryLegend = [
  { label: 'Tiếng Anh', percent: 45, color: '#2e7d32' },
  { label: 'Lập trình', percent: 25, color: '#1890ff' },
  { label: 'Marketing', percent: 12, color: '#faad14' },
  { label: 'Kế toán', percent: 10, color: '#722ed1' },
  { label: 'Thiết kế', percent: 8, color: '#13c2c2' },
]

const overviewKpis = computed(() => {
  const d = dashboardData.value
  return [
    { label: 'Doanh Thu Tháng', value: fmt(d?.totalRevenue || 0), icon: RiseOutlined, color: '#2e7d32', bg: 'rgba(46,125,50,0.1)', gradient: 'linear-gradient(135deg, rgba(46,125,50,0.3), transparent)' },
    { label: 'Học Viên', value: d?.totalStudents || 0, icon: TeamOutlined, color: '#1890ff', bg: 'rgba(24,144,255,0.1)', gradient: 'linear-gradient(135deg, rgba(24,144,255,0.3), transparent)' },
    { label: 'Tổng Công Nợ', value: fmt(d?.totalDebt || 0), icon: AlertOutlined, color: '#ff4d4f', bg: 'rgba(255,77,79,0.1)', gradient: 'linear-gradient(135deg, rgba(255,77,79,0.3), transparent)' },
    { label: 'Khóa Học Active', value: d?.activeCourses || 0, icon: BookOutlined, color: '#722ed1', bg: 'rgba(114,46,209,0.1)', gradient: 'linear-gradient(135deg, rgba(114,46,209,0.3), transparent)' },
  ]
})

const courseKpis = computed(() => [
  { label: 'Tổng Khóa Học', value: courseData.value.length, icon: BookOutlined, color: '#2e7d32', bg: 'rgba(46,125,50,0.1)', gradient: 'linear-gradient(135deg, rgba(46,125,50,0.3), transparent)' },
  { label: 'Tổng Doanh Thu', value: fmt(courseData.value.reduce((s, c) => s + (c.paidAmount || 0), 0)), icon: DollarOutlined, color: '#1890ff', bg: 'rgba(24,144,255,0.1)', gradient: 'linear-gradient(135deg, rgba(24,144,255,0.3), transparent)' },
  { label: 'Tổng Học Viên', value: courseData.value.reduce((s, c) => s + (c.studentCount || 0), 0), icon: TeamOutlined, color: '#faad14', bg: 'rgba(250,173,20,0.1)', gradient: 'linear-gradient(135deg, rgba(250,173,20,0.3), transparent)' },
])

let revenueChart = null
let categoryChart = null
let revenueMonthChart = null
let courseChart = null

function destroyChart(chart) {
  if (chart) { chart.destroy(); chart = null }
}

async function initOverviewCharts() {
  await nextTick()
  destroyChart(revenueChart)
  destroyChart(categoryChart)

  if (revenueChartRef.value) {
    revenueChart = new Chart(revenueChartRef.value, {
      type: 'bar',
      data: {
        labels: ['T12/2025', 'T1/2026', 'T2/2026', 'T3/2026', 'T4/2026', 'T5/2026'],
        datasets: [
          { label: 'Thực tế', data: [185, 245, 198, 312, 278, 295], backgroundColor: 'rgba(46, 125, 50, 0.8)', borderRadius: 8 },
          { label: 'Mục tiêu', data: [180, 220, 200, 280, 300, 290], backgroundColor: 'rgba(24, 144, 255, 0.5)', borderRadius: 8 }
        ]
      },
      options: {
        responsive: true, maintainAspectRatio: false,
        plugins: { legend: { position: 'top' }, tooltip: { callbacks: { label: ctx => `${ctx.dataset.label}: ${ctx.raw} triệu` } } },
        scales: { y: { beginAtZero: true, ticks: { callback: v => v + 'M' } } }
      }
    })
  }

  if (categoryChartRef.value) {
    categoryChart = new Chart(categoryChartRef.value, {
      type: 'doughnut',
      data: {
        labels: categoryLegend.map(l => l.label),
        datasets: [{ data: categoryLegend.map(l => l.percent), backgroundColor: categoryLegend.map(l => l.color), borderWidth: 0, hoverOffset: 8 }]
      },
      options: { responsive: true, maintainAspectRatio: false, cutout: '65%', plugins: { legend: { display: false } } }
    })
  }
}

async function initRevenueMonthChart() {
  await nextTick()
  destroyChart(revenueMonthChart)
  if (revenueMonthChartRef.value && revenueByMonth.value.length > 0) {
    revenueMonthChart = new Chart(revenueMonthChartRef.value, {
      type: 'line',
      data: {
        labels: revenueByMonth.value.map(m => m.MonthLabel),
        datasets: [{ label: 'Doanh thu', data: revenueByMonth.value.map(m => (m.Revenue || 0) / 1000000), borderColor: '#2e7d32', backgroundColor: 'rgba(46, 125, 50, 0.1)', fill: true, tension: 0.4, pointRadius: 5, pointBackgroundColor: '#2e7d32' }]
      },
      options: { responsive: true, maintainAspectRatio: false, plugins: { legend: { display: false } }, scales: { y: { ticks: { callback: v => v + 'M' } } } }
    })
  }
}

async function initCourseChart() {
  await nextTick()
  destroyChart(courseChart)
  if (courseChartRef.value && courseData.value.length > 0) {
    courseChart = new Chart(courseChartRef.value, {
      type: 'bar',
      data: {
        labels: courseData.value.map(c => c.courseName?.substring(0, 15) + (c.courseName?.length > 15 ? '...' : '')),
        datasets: [
          { label: 'Tổng', data: courseData.value.map(c => (c.totalAmount || 0) / 1000000), backgroundColor: 'rgba(24, 144, 255, 0.7)', borderRadius: 6 },
          { label: 'Đã TT', data: courseData.value.map(c => (c.paidAmount || 0) / 1000000), backgroundColor: 'rgba(46, 125, 50, 0.7)', borderRadius: 6 }
        ]
      },
      options: { responsive: true, maintainAspectRatio: false, indexAxis: 'y', plugins: { legend: { position: 'top' } }, scales: { x: { ticks: { callback: v => v + 'M' } } } }
    })
  }
}

async function loadData() {
  try {
    await Promise.all([payment.fetchDashboard(), payment.fetchInvoices(), payment.fetchDebts()])
    try {
      const revData = await getRevenueReport({ year: revenueYear.value })
      if (revData?.byMonth) revenueByMonth.value = revData.byMonth
    } catch (e) { console.warn('Revenue report error:', e) }
    try {
      const courseRes = await getReportByCourse()
      if (Array.isArray(courseRes)) courseData.value = courseRes
    } catch (e) { console.warn('Course report error:', e) }
    await initOverviewCharts()
  } catch (e) {
    console.error('Load data error:', e)
  }
}

watch(activeTab, async (tab) => {
  await nextTick()
  if (tab === 'revenue') await initRevenueMonthChart()
  else if (tab === 'course') await initCourseChart()
})

onMounted(loadData)
</script>

<style scoped>
.reports-page {
  padding: 0;
  animation: fadeIn 0.5s ease;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(16px); }
  to { opacity: 1; transform: translateY(0); }
}

.mb-6 { margin-bottom: 24px; }
.mb-4 { margin-bottom: 16px; }
.mt-4 { margin-top: 16px; }
.mb-1 { margin-bottom: 4px; }
.h-100 { height: 100%; }

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  flex-wrap: wrap;
  gap: 16px;
}

.page-title {
  font-size: 24px;
  font-weight: 700;
  color: #1a1a2e;
  margin: 0 0 4px;
}

.page-subtitle {
  font-size: 13px;
  color: #888;
  margin: 0;
}

/* KPI */
.kpi-card {
  position: relative;
  overflow: hidden;
  background: white;
  border-radius: 16px;
  padding: 20px;
  border: 1px solid rgba(0,0,0,0.05);
  box-shadow: 0 2px 12px rgba(0,0,0,0.04);
  transition: all 0.3s ease;
  height: 100%;
  animation: fadeIn 0.5s ease both;
}

.kpi-card:hover { transform: translateY(-3px); box-shadow: 0 8px 30px rgba(0,0,0,0.08); }

.kpi-glow {
  position: absolute;
  top: -40%; right: -40%;
  width: 120%; height: 120%;
  opacity: 0.5;
}

.kpi-body {
  position: relative; z-index: 1;
  display: flex; justify-content: space-between; align-items: center;
}

.kpi-label {
  display: block;
  font-size: 11px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  color: #888;
  font-weight: 600;
  margin-bottom: 6px;
}

.kpi-value {
  display: block;
  font-size: 24px;
  font-weight: 800;
  line-height: 1.2;
}

.kpi-icon {
  width: 48px; height: 48px;
  border-radius: 14px;
  display: flex; align-items: center; justify-content: center;
  font-size: 22px; flex-shrink: 0;
}

/* Chart Card */
.chart-card {
  border-radius: 16px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.04);
  border: 1px solid rgba(0,0,0,0.05);
  padding: 20px;
}

.chart-header {
  display: flex; justify-content: space-between; align-items: flex-start;
  margin-bottom: 20px; flex-wrap: wrap; gap: 8px;
}

.chart-title {
  font-size: 15px; font-weight: 700; color: #1a1a2e;
}

.chart-sub {
  font-size: 12px; color: #888; margin-top: 2px;
}

.chart-container { height: 260px; position: relative; }
.chart-container-doughnut { height: 180px; position: relative; }

.chart-legend { display: flex; flex-direction: column; gap: 8px; }
.legend-item { display: flex; align-items: center; gap: 8px; font-size: 13px; }
.legend-color { width: 12px; height: 12px; border-radius: 3px; }
.legend-percent { margin-left: auto; font-weight: 700; color: #1a1a2e; }

/* Activity Card */
.activity-card { border-radius: 16px; box-shadow: 0 2px 12px rgba(0,0,0,0.04); border: 1px solid rgba(0,0,0,0.05); padding: 20px; }
.card-title { font-size: 15px; font-weight: 700; color: #1a1a2e; margin-bottom: 16px; }

.recent-item { padding: 4px 0; }
.recent-row { display: flex; justify-content: space-between; align-items: center; gap: 8px; }
.recent-left { display: flex; align-items: center; gap: 10px; }
.recent-name { font-size: 13px; font-weight: 600; color: #1a1a2e; }
.recent-sub { font-size: 11px; color: #888; margin-top: 2px; }
.recent-right { text-align: right; }
.recent-amount { font-size: 14px; font-weight: 700; }
.recent-date { font-size: 11px; color: #888; margin-top: 2px; }

.empty-state { text-align: center; padding: 24px 0; color: #ccc; }
.empty-state p { margin: 8px 0 0; font-size: 13px; }

/* Stat Card */
.stat-card {
  background: white;
  border-radius: 16px;
  padding: 20px;
  display: flex; align-items: center; justify-content: space-between;
  border: 1px solid rgba(0,0,0,0.05);
  box-shadow: 0 2px 12px rgba(0,0,0,0.04);
}

.stat-label { font-size: 12px; color: #888; font-weight: 600; text-transform: uppercase; letter-spacing: 0.5px; margin-bottom: 6px; }
.stat-value { font-size: 22px; font-weight: 800; line-height: 1.2; }
.stat-icon { width: 48px; height: 48px; border-radius: 14px; display: flex; align-items: center; justify-content: center; flex-shrink: 0; }

/* Toolbar */
.toolbar-row { display: flex; align-items: center; gap: 12px; flex-wrap: wrap; }
.toolbar-spacer { flex: 1; }

/* Text helpers */
.text-success { color: #52c41a; }
.text-danger { color: #ff4d4f; }
.text-primary { color: #1890ff; }
.text-violet { color: #722ed1; }
</style>
