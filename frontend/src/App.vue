<script setup>
import { computed, h, ref } from 'vue'
import {
  BarChartOutlined,
  BookOutlined,
  CalendarOutlined,
  CheckSquareOutlined,
  CreditCardOutlined,
  BellOutlined,
  SearchOutlined,
  HomeOutlined,
  TrophyOutlined,
  UserOutlined,
} from '@ant-design/icons-vue'
import HomeView from './views/HomeView.vue'
import CoursesPage from './modules/courses/pages/CoursesPage.vue'
import SchedulesPage from './modules/courses/pages/SchedulesPage.vue'
import StudentsPage from './modules/students/pages/StudentsPage.vue'
import EnrollmentsPage from './modules/students/pages/EnrollmentsPage.vue'
import AttendancesPage from './modules/students/pages/AttendancesPage.vue'
import ResultsPage from './modules/students/pages/ResultsPage.vue'
import PaymentsPage from './modules/payments/pages/PaymentsPage.vue'
import ReportsPage from './modules/payments/pages/ReportsPage.vue'
import PaymentServiceShell from './PaymentServiceShell.vue'

const showPaymentService = ref(false)

const menuItems = [
  { key: 'home', label: 'Home', group: 'System' },
  { key: 'courses', label: 'Courses', group: 'Course & Schedule' },
  { key: 'schedules', label: 'Schedules', group: 'Course & Schedule' },
  { key: 'students', label: 'Students', group: 'Student, Attendance & Results' },
  { key: 'enrollments', label: 'Enrollments', group: 'Student, Attendance & Results' },
  { key: 'attendances', label: 'Attendances', group: 'Student, Attendance & Results' },
  { key: 'results', label: 'Results', group: 'Student, Attendance & Results' },
  { key: 'payments', label: 'Payments', group: 'Payment & Report' },
  { key: 'reports', label: 'Reports', group: 'Payment & Report' },
  { key: 'payment-admin', label: 'Payment Admin', group: 'Payment Service' },
]

const pages = {
  home: HomeView,
  courses: CoursesPage,
  schedules: SchedulesPage,
  students: StudentsPage,
  enrollments: EnrollmentsPage,
  attendances: AttendancesPage,
  results: ResultsPage,
  payments: PaymentsPage,
  reports: ReportsPage,
}

const pageMeta = {
  home: {
    group: 'System',
    description: 'Overview of the shared frontend shell and LAN demo entry point.',
  },
  courses: {
    group: 'Course & Schedule',
    description: 'Course and timetable management owned by Team 1.',
  },
  schedules: {
    group: 'Course & Schedule',
    description: 'Session planning and room scheduling for Team 1.',
  },
  students: {
    group: 'Student, Attendance & Results',
    description: 'Student profiles, enrollments, attendance, and results for Team 2.',
  },
  enrollments: {
    group: 'Student, Attendance & Results',
    description: 'Enrollment records and linked student data for Team 2.',
  },
  attendances: {
    group: 'Student, Attendance & Results',
    description: 'Attendance records per schedule for Team 2.',
  },
  results: {
    group: 'Student, Attendance & Results',
    description: 'Result management and score tracking for Team 2.',
  },
  payments: {
    group: 'Payment & Report',
    description: 'Payments, debts, and finance workflows owned by Team 3.',
  },
  reports: {
    group: 'Payment & Report',
    description: 'Reporting and dashboard data owned by Team 3.',
  },
}

const activePage = ref('home')
const activeMenuItem = computed(() => menuItems.find((item) => item.key === activePage.value))
const activePageMeta = computed(() => pageMeta[activePage.value] ?? pageMeta.home)
const ActiveComponent = computed(() => pages[activePage.value] ?? HomeView)
</script>

<template>
  <!-- PaymentServiceShell renders full screen, no Ant Design wrapper -->
  <PaymentServiceShell 
    v-if="showPaymentService" 
    @exit="showPaymentService = false" 
  />
  
  <!-- Main App Shell (hidden when in Payment Service) -->
  <a-layout v-else class="app-shell">
    <a-layout-sider
      class="app-sidebar"
      width="260"
      breakpoint="lg"
      collapsed-width="0"
    >
      <div class="brand">
        <div class="brand-mark">EC</div>
        <div class="brand-copy">
          <strong>EduCenter</strong>
          <span>Microservices dashboard</span>
        </div>
      </div>

      <a-menu
        class="sidebar-menu"
        mode="inline"
        :selected-keys="[activePage]"
        @click="({ key }) => {
          if (key === 'payment-admin') {
            showPaymentService = true
          } else {
            activePage = key
          }
        }"
      >
        <a-menu-item-group key="system" title="System">
          <a-menu-item key="home" :icon="h(HomeOutlined)">Home</a-menu-item>
        </a-menu-item-group>

        <a-menu-item-group key="course" title="Course &amp; Schedule">
          <a-menu-item key="courses" :icon="h(BookOutlined)">Courses</a-menu-item>
          <a-menu-item key="schedules" :icon="h(CalendarOutlined)">Schedules</a-menu-item>
        </a-menu-item-group>

        <a-menu-item-group key="student" title="Student &amp; Attendance">
          <a-menu-item key="students" :icon="h(UserOutlined)">Students</a-menu-item>
          <a-menu-item key="enrollments" :icon="h(BookOutlined)">Enrollments</a-menu-item>
          <a-menu-item key="attendances" :icon="h(CheckSquareOutlined)">Attendances</a-menu-item>
          <a-menu-item key="results" :icon="h(TrophyOutlined)">Results</a-menu-item>
        </a-menu-item-group>

        <a-menu-item-group key="payment" title="Payment & Report Service">
          <a-menu-item key="payments">Payments</a-menu-item>
          <a-menu-item key="reports">Reports</a-menu-item>
          <a-menu-item key="payment-admin" style="color:#2E7D32;font-weight:700">
            <span style="display:flex;align-items:center;gap:6px">
              <span>⚡</span> Payment Admin
            </span>
          </a-menu-item>
        </a-menu-item-group>
      </a-menu>

      <div class="sidebar-foot">
        <span>LAN mode</span>
        <strong>Direct service calls</strong>
        <small>5001 · 5002 · 5003</small>
      </div>
    </a-layout-sider>

    <a-layout class="main-layout">
      <a-layout-header class="app-header">
        <div class="header-copy">
          <span class="header-label">{{ activePageMeta.group }}</span>
          <h1>{{ activeMenuItem?.label }}</h1>
          <p>{{ activePageMeta.description }}</p>
        </div>
        <div class="header-meta">
          <a-input
            class="header-search"
            :prefix="h(SearchOutlined)"
            placeholder="Tìm nhanh..."
            allow-clear
          />
          <a-badge dot>
            <a-button class="header-icon-button" type="text" :icon="h(BellOutlined)" />
          </a-badge>
          <div class="header-avatar">NP</div>
        </div>
      </a-layout-header>

      <a-layout-content class="app-content">
        <component :is="ActiveComponent" />
      </a-layout-content>
    </a-layout>
  </a-layout>
</template>

<style>
html, body, #app {
  height: 100%;
  overflow: hidden;
}

.app-shell {
  height: 100vh;
  overflow: hidden;
}

.app-shell .ant-layout-sider {
  position: fixed;
  left: 0;
  top: 0;
  height: 100vh;
  overflow-y: auto;
  overflow-x: hidden;
  z-index: 100;
  scrollbar-width: thin;
  scrollbar-color: rgba(0,0,0,0.15) transparent;
}

.app-shell .ant-layout-sider::-webkit-scrollbar {
  width: 4px;
}
.app-shell .ant-layout-sider::-webkit-scrollbar-thumb {
  background: rgba(0,0,0,0.15);
  border-radius: 2px;
}

.main-layout {
  margin-left: 260px;
  height: 100vh;
  overflow: hidden;
}

.app-header {
  position: sticky;
  top: 0;
  z-index: 50;
  background: white !important;
  padding: 20px 32px !important;
  height: auto !important;
  line-height: normal !important;
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 16px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.08);
}

.header-copy .header-label {
  font-size: 11px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  color: #888;
  font-weight: 600;
}

.header-copy h1 {
  margin: 0;
  font-size: 20px;
  font-weight: 700;
  color: #1a1a2e;
}

.header-copy p {
  margin: 0;
  font-size: 12px;
  color: #888;
}

.header-meta {
  display: flex;
  align-items: center;
  gap: 12px;
}

.header-search {
  width: 220px;
  border-radius: 8px;
}

.header-icon-button {
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.header-avatar {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: #2e7d32;
  color: white;
  font-size: 13px;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}

.app-content {
  height: calc(100vh - 80px);
  overflow-y: auto;
  padding: 0;
  background: #f5f5f5;
  scrollbar-width: thin;
  scrollbar-color: rgba(0,0,0,0.12) transparent;
}

.app-content::-webkit-scrollbar {
  width: 6px;
}
.app-content::-webkit-scrollbar-thumb {
  background: rgba(0,0,0,0.12);
  border-radius: 3px;
}

.brand {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 20px 20px 16px;
  border-bottom: 1px solid rgba(255,255,255,0.1);
}

.brand-mark {
  width: 40px;
  height: 40px;
  background: linear-gradient(135deg, #2e7d32, #43a047);
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 16px;
  font-weight: 800;
  color: white;
  flex-shrink: 0;
  box-shadow: 0 4px 12px rgba(46,125,50,0.3);
}

.brand-copy {
  display: flex;
  flex-direction: column;
}

.brand-copy strong {
  font-size: 15px;
  font-weight: 700;
  color: #1a1a2e;
}

.brand-copy span {
  font-size: 10px;
  color: #888;
}

.sidebar-menu {
  border-right: none !important;
}

.sidebar-foot {
  padding: 16px 20px;
  border-top: 1px solid rgba(0,0,0,0.06);
  font-size: 10px;
  color: #aaa;
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.sidebar-foot span:first-child {
  font-weight: 700;
  color: #52c41a;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.sidebar-foot strong {
  font-size: 12px;
  color: #333;
}

.sidebar-foot small {
  font-family: monospace;
  color: #888;
}
</style>
