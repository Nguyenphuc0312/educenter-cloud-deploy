<script setup>
import {
  BarChartOutlined,
  BookOutlined,
  CalendarOutlined,
  CheckSquareOutlined,
  CreditCardOutlined,
  HomeOutlined,
  TrophyOutlined,
  UserOutlined,
  RightOutlined,
  SafetyCertificateOutlined,
  CloudServerOutlined,
  ApiOutlined,
} from '@ant-design/icons-vue'

const serviceCards = [
  {
    title: 'Course Service',
    machine: 'Machine 1',
    state: 'Live',
    color: '#1890ff',
    bg: '#e6f7ff',
    icon: BookOutlined,
  },
  {
    title: 'Student Service',
    machine: 'Machine 2',
    state: 'Live',
    color: '#52c41a',
    bg: '#f6ffed',
    icon: UserOutlined,
  },
  {
    title: 'Payment Service',
    machine: 'Machine 3',
    state: 'Live',
    color: '#faad14',
    bg: '#fffbe6',
    icon: CreditCardOutlined,
  },
  {
    title: 'Vue Frontend',
    machine: 'Machine 4',
    state: 'Live',
    color: '#722ed1',
    bg: '#f9f0ff',
    icon: HomeOutlined,
  },
]

const teamCards = [
  {
    code: '01',
    title: 'Course & Schedule',
    description: 'Course, class, and schedule management for Team 1.',
    icon: CalendarOutlined,
    chips: ['Courses', 'Classes', 'Schedules'],
    color: '#1890ff',
    bg: '#e6f7ff',
    border: '#91d5ff',
  },
  {
    code: '02',
    title: 'Student & Attendance',
    description: 'Profiles, enrollments, attendance, and results for Team 2.',
    icon: CheckSquareOutlined,
    chips: ['Students', 'Enrollments', 'Attendances', 'Results'],
    color: '#13c2c2',
    bg: '#e6fffb',
    border: '#87e8de',
  },
  {
    code: '03',
    title: 'Payment & Report',
    description: 'Payments, debts, and reporting for Team 3.',
    icon: CreditCardOutlined,
    chips: ['Payments', 'Debts', 'Reports'],
    color: '#722ed1',
    bg: '#f9f0ff',
    border: '#d3adf7',
  },
  {
    code: '04',
    title: 'Shared Frontend',
    description: 'Single Vue shell for LAN demo and team handoff.',
    icon: BarChartOutlined,
    chips: ['Vue 3', 'Vuetify', 'Direct API'],
    color: '#fa541c',
    bg: '#fff2e8',
    border: '#ffbb96',
  },
]

const summaryCards = [
  {
    label: 'Services online',
    value: '3',
    note: 'Course, Student, Payment',
    icon: CloudServerOutlined,
    color: '#1890ff',
    bg: '#e6f7ff',
  },
  {
    label: 'CRUD modules',
    value: '9',
    note: 'Students, enrollments, attendances',
    icon: SafetyCertificateOutlined,
    color: '#52c41a',
    bg: '#f6ffed',
  },
  {
    label: 'LAN endpoints',
    value: '3',
    note: 'Direct calls to each API',
    icon: ApiOutlined,
    color: '#faad14',
    bg: '#fffbe6',
  },
  {
    label: 'Front-end shell',
    value: '1',
    note: 'Shared Vue workspace for demo',
    icon: TrophyOutlined,
    color: '#722ed1',
    bg: '#f9f0ff',
  },
]

const workflowSteps = [
  {
    step: '01',
    title: 'Open the shell',
    text: 'Use the shared landing page as the entry point for the demo.',
    color: '#1890ff',
  },
  {
    step: '02',
    title: 'Test each module',
    text: 'Move between the three team areas without changing the front-end shell.',
    color: '#13c2c2',
  },
  {
    step: '03',
    title: 'Check the APIs',
    text: 'Swagger and LAN endpoints stay available for direct verification.',
    color: '#722ed1',
  },
]
</script>

<template>
  <main class="home-page">
    <!-- Hero Section -->
    <section class="home-hero">
      <div class="hero-bg-shapes">
        <div class="shape shape-1"></div>
        <div class="shape shape-2"></div>
        <div class="shape shape-3"></div>
      </div>
      <div class="hero-content">
        <div class="hero-badge">
          <span class="badge-dot"></span>
          LAN Demo Workspace · 2026
        </div>

        <h1 class="hero-title">
          EduCenter <span class="hero-title-accent">Platform</span>
        </h1>

        <p class="hero-desc">
          A shared workspace for three microservice teams. Each module connects directly to its own API
          on the LAN — Course Service (port 5001), Student Service (port 5002), and Payment Service (port 5003).
        </p>

        <div class="hero-pills">
          <span class="pill pill-blue">Direct API</span>
          <span class="pill pill-green">Swagger Ready</span>
          <span class="pill pill-violet">Vue 3 + Vuetify</span>
        </div>
      </div>

      <!-- Service Status Panel -->
      <div class="hero-panel">
        <div class="panel-header">
          <div class="panel-title">
            <span class="panel-dot"></span>
            Runtime Status
          </div>
          <a-tag color="success">All Systems Live</a-tag>
        </div>

        <div class="service-grid">
          <div
            v-for="service in serviceCards"
            :key="service.title"
            class="service-card"
            :style="{ '--svc-color': service.color, '--svc-bg': service.bg }"
          >
            <div class="service-icon-wrap">
              <component :is="service.icon" :style="{ color: service.color }" />
            </div>
            <div class="service-info">
              <strong>{{ service.title }}</strong>
              <small>{{ service.machine }}</small>
            </div>
            <a-tag :color="service.state === 'Live' ? 'success' : 'default'" class="service-tag">
              {{ service.state }}
            </a-tag>
          </div>
        </div>

        <div class="panel-footer">
          <span>Last checked: just now</span>
          <RightOutlined style="font-size:10px" />
        </div>
      </div>
    </section>

    <!-- Summary KPIs -->
    <section class="summary-section">
      <div
        v-for="item in summaryCards"
        :key="item.label"
        class="summary-card"
        :style="{ '--sum-color': item.color, '--sum-bg': item.bg }"
      >
        <div class="summary-icon-wrap">
          <component :is="item.icon" :style="{ color: item.color }" />
        </div>
        <div class="summary-body">
          <span class="summary-label">{{ item.label }}</span>
          <strong class="summary-value">{{ item.value }}</strong>
          <small class="summary-note">{{ item.note }}</small>
        </div>
      </div>
    </section>

    <!-- Team Modules -->
    <section class="modules-section">
      <div class="section-header">
        <h2 class="section-title">Microservice Modules</h2>
        <p class="section-sub">Each team owns their domain — direct API calls, no middleman</p>
      </div>

      <div class="modules-grid">
        <div
          v-for="team in teamCards"
          :key="team.code"
          class="module-card"
          :style="{
            '--mod-color': team.color,
            '--mod-bg': team.bg,
            '--mod-border': team.border,
          }"
        >
          <div class="module-card-head">
            <div class="module-icon-wrap">
              <component :is="team.icon" :style="{ color: team.color }" />
            </div>
            <span class="module-code">{{ team.code }}</span>
          </div>

          <h3 class="module-title">{{ team.title }}</h3>
          <p class="module-desc">{{ team.description }}</p>

          <div class="module-chips">
            <span v-for="chip in team.chips" :key="chip" class="module-chip">
              {{ chip }}
            </span>
          </div>

          <div class="module-card-foot">
            <a-tag :color="team.color" bordered>
              Team {{ team.code }} →
            </a-tag>
          </div>
        </div>
      </div>
    </section>

    <!-- Workflow -->
    <section class="workflow-section">
      <div class="section-header">
        <h2 class="section-title">Demo Workflow</h2>
        <p class="section-sub">3 simple steps to navigate the full platform</p>
      </div>

      <div class="workflow-track">
        <div
          v-for="(step, index) in workflowSteps"
          :key="step.step"
          class="workflow-step"
          :style="{ '--wf-color': step.color }"
        >
          <div class="step-connector" v-if="index > 0"></div>
          <div class="step-badge">{{ step.step }}</div>
          <div class="step-body">
            <strong>{{ step.title }}</strong>
            <p>{{ step.text }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Port Quick Reference -->
    <section class="ports-section">
      <div class="port-card">
        <div class="port-header">
          <span>Quick Reference — LAN Ports</span>
        </div>
        <div class="port-list">
          <div class="port-item">
            <span class="port-num">5001</span>
            <span class="port-name">Course Service API</span>
            <a-tag color="blue" size="small">GET/POST/PUT/DELETE</a-tag>
          </div>
          <div class="port-item">
            <span class="port-num">5002</span>
            <span class="port-name">Student Service API</span>
            <a-tag color="cyan" size="small">GET/POST/PUT/DELETE</a-tag>
          </div>
          <div class="port-item">
            <span class="port-num">5003</span>
            <span class="port-name">Payment Service API</span>
            <a-tag color="gold" size="small">GET/POST/PUT/DELETE</a-tag>
          </div>
        </div>
      </div>
    </section>
  </main>
</template>

<style scoped>
.home-page {
  padding: 0;
  animation: fadeInUp 0.5s ease;
}

@keyframes fadeInUp {
  from { opacity: 0; transform: translateY(16px); }
  to { opacity: 1; transform: translateY(0); }
}

/* ── Hero ── */
.home-hero {
  position: relative;
  display: grid;
  grid-template-columns: 1fr 420px;
  gap: 40px;
  padding: 48px 48px 40px;
  overflow: hidden;
  background: linear-gradient(135deg, #f0f5ee 0%, #e8f5e9 40%, #f1f8e9 100%);
}

.hero-bg-shapes {
  position: absolute;
  inset: 0;
  pointer-events: none;
  overflow: hidden;
}

.shape {
  position: absolute;
  border-radius: 50%;
  opacity: 0.12;
}

.shape-1 {
  width: 400px;
  height: 400px;
  background: #2e7d32;
  top: -100px;
  right: 200px;
  animation: float 8s ease-in-out infinite;
}

.shape-2 {
  width: 200px;
  height: 200px;
  background: #1976d2;
  bottom: -50px;
  left: 100px;
  animation: float 6s ease-in-out infinite reverse;
}

.shape-3 {
  width: 120px;
  height: 120px;
  background: #7b1fa2;
  top: 60%;
  left: 50%;
  animation: float 10s ease-in-out infinite;
}

@keyframes float {
  0%, 100% { transform: translate(0, 0) scale(1); }
  33% { transform: translate(15px, -20px) scale(1.05); }
  66% { transform: translate(-10px, 10px) scale(0.95); }
}

.hero-content {
  position: relative;
  z-index: 1;
}

.hero-badge {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  font-size: 12px;
  font-weight: 600;
  color: #2e7d32;
  background: rgba(46, 125, 50, 0.1);
  border: 1px solid rgba(46, 125, 50, 0.25);
  padding: 5px 12px;
  border-radius: 20px;
  margin-bottom: 20px;
  letter-spacing: 0.3px;
  text-transform: uppercase;
}

.badge-dot {
  width: 7px;
  height: 7px;
  background: #2e7d32;
  border-radius: 50%;
  animation: pulse 2s infinite;
}

@keyframes pulse {
  0%, 100% { opacity: 1; transform: scale(1); }
  50% { opacity: 0.5; transform: scale(0.85); }
}

.hero-title {
  font-size: 52px;
  font-weight: 800;
  color: #1a1a2e;
  line-height: 1.1;
  margin: 0 0 16px;
  letter-spacing: -1px;
}

.hero-title-accent {
  color: #2e7d32;
  position: relative;
}

.hero-title-accent::after {
  content: '';
  position: absolute;
  bottom: 2px;
  left: 0;
  width: 100%;
  height: 6px;
  background: rgba(46, 125, 50, 0.25);
  border-radius: 3px;
}

.hero-desc {
  font-size: 15px;
  color: #555;
  line-height: 1.7;
  margin: 0 0 24px;
  max-width: 480px;
}

.hero-pills {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
}

.pill {
  font-size: 12px;
  font-weight: 600;
  padding: 4px 14px;
  border-radius: 20px;
  letter-spacing: 0.3px;
}

.pill-blue { background: #e6f4ff; color: #1890ff; border: 1px solid #91d5ff; }
.pill-green { background: #f6ffed; color: #52c41a; border: 1px solid #b7eb8f; }
.pill-violet { background: #f9f0ff; color: #722ed1; border: 1px solid #d3adf7; }

/* ── Service Panel ── */
.hero-panel {
  position: relative;
  z-index: 1;
  background: white;
  border-radius: 20px;
  padding: 24px;
  box-shadow: 0 8px 40px rgba(0, 0, 0, 0.08);
  border: 1px solid rgba(0, 0, 0, 0.05);
  height: fit-content;
}

.panel-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 20px;
}

.panel-title {
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 700;
  font-size: 14px;
  color: #1a1a2e;
}

.panel-dot {
  width: 8px;
  height: 8px;
  background: #52c41a;
  border-radius: 50%;
  animation: pulse 2s infinite;
}

.service-grid {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.service-card {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 12px 14px;
  background: var(--svc-bg);
  border-radius: 12px;
  border: 1px solid rgba(0,0,0,0.04);
  transition: all 0.25s ease;
}

.service-card:hover {
  transform: translateX(4px);
  box-shadow: 0 4px 16px rgba(0,0,0,0.06);
}

.service-icon-wrap {
  width: 36px;
  height: 36px;
  background: white;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
  font-size: 16px;
  flex-shrink: 0;
}

.service-info {
  flex: 1;
}

.service-info strong {
  display: block;
  font-size: 13px;
  font-weight: 700;
  color: #1a1a2e;
}

.service-info small {
  font-size: 11px;
  color: #888;
}

.service-tag {
  font-size: 11px;
  border-radius: 8px;
}

.panel-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 16px;
  padding-top: 14px;
  border-top: 1px dashed #e0e0e0;
  font-size: 11px;
  color: #aaa;
}

/* ── Summary KPIs ── */
.summary-section {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
  padding: 0 48px 36px;
}

.summary-card {
  background: var(--sum-bg);
  border: 1px solid rgba(0,0,0,0.05);
  border-radius: 16px;
  padding: 20px;
  display: flex;
  align-items: center;
  gap: 14px;
  transition: all 0.3s ease;
}

.summary-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 8px 24px rgba(0,0,0,0.06);
}

.summary-icon-wrap {
  width: 48px;
  height: 48px;
  background: white;
  border-radius: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 22px;
  box-shadow: 0 2px 10px rgba(0,0,0,0.06);
  flex-shrink: 0;
}

.summary-body {
  display: flex;
  flex-direction: column;
}

.summary-label {
  font-size: 11px;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  color: #888;
  font-weight: 600;
}

.summary-value {
  font-size: 28px;
  font-weight: 800;
  color: var(--sum-color);
  line-height: 1.2;
}

.summary-note {
  font-size: 11px;
  color: #999;
  margin-top: 2px;
}

/* ── Modules ── */
.modules-section {
  padding: 20px 48px 36px;
}

.section-header {
  margin-bottom: 24px;
}

.section-title {
  font-size: 22px;
  font-weight: 800;
  color: #1a1a2e;
  margin: 0 0 4px;
}

.section-sub {
  font-size: 13px;
  color: #888;
  margin: 0;
}

.modules-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
}

.module-card {
  background: var(--mod-bg);
  border: 1.5px solid var(--mod-border);
  border-radius: 20px;
  padding: 24px 20px 20px;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.module-card::before {
  content: '';
  position: absolute;
  top: -30px;
  right: -30px;
  width: 80px;
  height: 80px;
  background: var(--mod-color);
  border-radius: 50%;
  opacity: 0.06;
}

.module-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 12px 36px rgba(0,0,0,0.1);
}

.module-card-head {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 14px;
}

.module-icon-wrap {
  width: 42px;
  height: 42px;
  background: white;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 20px;
  box-shadow: 0 2px 10px rgba(0,0,0,0.08);
}

.module-code {
  font-size: 11px;
  font-weight: 800;
  color: var(--mod-color);
  background: white;
  border: 1.5px solid var(--mod-border);
  padding: 3px 10px;
  border-radius: 20px;
}

.module-title {
  font-size: 15px;
  font-weight: 800;
  color: #1a1a2e;
  margin: 0 0 6px;
}

.module-desc {
  font-size: 12px;
  color: #666;
  line-height: 1.6;
  margin: 0 0 14px;
}

.module-chips {
  display: flex;
  flex-wrap: wrap;
  gap: 5px;
  margin-bottom: 16px;
}

.module-chip {
  font-size: 11px;
  font-weight: 600;
  padding: 3px 10px;
  background: white;
  border: 1px solid var(--mod-border);
  border-radius: 20px;
  color: var(--mod-color);
}

.module-card-foot {
  margin-top: auto;
}

/* ── Workflow ── */
.workflow-section {
  padding: 20px 48px 36px;
}

.workflow-track {
  display: flex;
  gap: 0;
  position: relative;
}

.workflow-step {
  flex: 1;
  position: relative;
  padding: 24px;
  background: white;
  border-radius: 16px;
  border: 1.5px solid #f0f0f0;
  margin-right: 16px;
  transition: all 0.3s ease;
}

.workflow-step:last-child {
  margin-right: 0;
}

.workflow-step:hover {
  border-color: var(--wf-color);
  box-shadow: 0 6px 24px rgba(0,0,0,0.06);
  transform: translateY(-2px);
}

.step-connector {
  position: absolute;
  top: 50%;
  right: -20px;
  width: 24px;
  height: 2px;
  background: linear-gradient(to right, var(--wf-color), #ddd);
  z-index: 1;
}

.step-badge {
  width: 40px;
  height: 40px;
  border-radius: 12px;
  background: var(--wf-color);
  color: white;
  font-size: 14px;
  font-weight: 800;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 12px;
}

.step-body strong {
  display: block;
  font-size: 14px;
  font-weight: 700;
  color: #1a1a2e;
  margin-bottom: 4px;
}

.step-body p {
  font-size: 12px;
  color: #777;
  line-height: 1.6;
  margin: 0;
}

/* ── Ports ── */
.ports-section {
  padding: 0 48px 48px;
}

.port-card {
  background: white;
  border-radius: 16px;
  border: 1.5px solid #f0f0f0;
  overflow: hidden;
}

.port-header {
  padding: 16px 24px;
  background: #fafafa;
  border-bottom: 1px solid #f0f0f0;
  font-weight: 700;
  font-size: 13px;
  color: #555;
}

.port-list {
  display: flex;
  flex-direction: column;
}

.port-item {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 14px 24px;
  border-bottom: 1px solid #f5f5f5;
  transition: background 0.2s;
}

.port-item:last-child {
  border-bottom: none;
}

.port-item:hover {
  background: #fafafa;
}

.port-num {
  font-size: 14px;
  font-weight: 800;
  color: #2e7d32;
  background: #e8f5e9;
  padding: 4px 12px;
  border-radius: 8px;
  font-family: monospace;
  min-width: 56px;
  text-align: center;
}

.port-name {
  flex: 1;
  font-size: 13px;
  font-weight: 600;
  color: #333;
}
</style>
