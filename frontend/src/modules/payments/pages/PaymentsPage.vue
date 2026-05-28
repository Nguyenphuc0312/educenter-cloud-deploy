<template>
  <div class="payments-page">
    <!-- Header -->
    <div class="page-header mb-6">
      <div>
        <h2 class="page-title">Quản Lý Thanh Toán</h2>
        <p class="page-subtitle">Theo dõi và xử lý hóa đơn học phí</p>
      </div>
      <a-button type="primary" size="large" @click="openCreateDialog">
        <template #icon><PlusOutlined /></template>
        Tạo Hóa Đơn
      </a-button>
    </div>

    <!-- KPI Cards -->
    <a-row :gutter="20" class="mb-6">
      <a-col v-for="(kpi, index) in kpis" :key="kpi.label" :xs="24" :sm="12" :lg="6">
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

    <!-- Filter & Search -->
    <div class="filter-card mb-6">
      <div class="filter-row">
        <a-input-search
          v-model:value="search"
          placeholder="Tìm học viên, khóa học..."
          style="width: 280px"
          allow-clear
          @search="search"
        >
          <template #prefix><SearchOutlined /></template>
        </a-input-search>

        <a-select
          v-model:value="statusFilter"
          style="width: 160px"
          placeholder="Trạng thái"
        >
          <a-select-option value="Tất cả">Tất cả</a-select-option>
          <a-select-option value="Paid">Đã thanh toán</a-select-option>
          <a-select-option value="Partial">Một phần</a-select-option>
          <a-select-option value="Unpaid">Chưa thanh toán</a-select-option>
        </a-select>

        <div class="filter-stats">
          <a-tag color="success">
            <CheckCircleOutlined /> {{ paidCount }} Đã TT
          </a-tag>
          <a-tag color="error">
            <CloseCircleOutlined /> {{ unpaidCount }} Chưa TT
          </a-tag>
        </div>
      </div>
    </div>

    <!-- Payments Table -->
    <a-card class="table-card" :bordered="false">
      <a-table
        :columns="columns"
        :data-source="filteredPayments"
        :loading="loading"
        :pagination="{ pageSize: 10, showSizeChanger: true, showTotal: (total) => `Tổng ${total} hóa đơn` }"
        row-key="id"
        :scroll="{ x: 1100 }"
      >
        <template #bodyCell="{ column, record }">
          <!-- Học viên -->
          <template v-if="column.key === 'studentName'">
            <div class="student-cell">
              <a-avatar :size="36" style="background:#2e7d32; flex-shrink:0">
                {{ getInitials(record.studentName) }}
              </a-avatar>
              <div>
                <div class="student-name">{{ record.studentName }}</div>
                <div class="student-id">ID: {{ record.studentId }}</div>
              </div>
            </div>
          </template>

          <!-- Khóa học -->
          <template v-else-if="column.key === 'courseName'">
            <div>
              <div>{{ record.courseName || '—' }}</div>
              <div class="text-secondary">Khóa #{{ record.courseId }}</div>
            </div>
          </template>

          <!-- Tổng tiền -->
          <template v-else-if="column.key === 'amount'">
            <span class="amount-text">{{ fmt(record.amount) }}</span>
          </template>

          <!-- Đã TT -->
          <template v-else-if="column.key === 'paidAmount'">
            <span class="text-success">{{ fmt(record.paidAmount || 0) }}</span>
          </template>

          <!-- Còn lại -->
          <template v-else-if="column.key === 'remaining'">
            <span class="text-danger">{{ fmt(record.amount - (record.paidAmount || 0)) }}</span>
          </template>

          <!-- Ngày TT -->
          <template v-else-if="column.key === 'paymentDate'">
            {{ formatDate(record.paymentDate) }}
          </template>

          <!-- Hạn -->
          <template v-else-if="column.key === 'dueDate'">
            <a-tag :color="getDueDateColor(record.dueDate, record.status)">
              <template #icon><component :is="getDueDateIcon(record.dueDate, record.status)" /></template>
              {{ formatDate(record.dueDate) }}
            </a-tag>
          </template>

          <!-- Trạng thái -->
          <template v-else-if="column.key === 'status'">
            <a-tag :color="getStatusColor(record.status)" class="status-tag">
              <template #icon><component :is="getStatusIcon(record.status)" /></template>
              {{ getStatusText(record.status) }}
            </a-tag>
          </template>

          <!-- Thao tác -->
          <template v-else-if="column.key === 'actions'">
            <div class="action-buttons">
              <a-tooltip title="Thanh toán" v-if="record.status !== 'Paid'">
                <a-button
                  size="small"
                  style="color:#2e7d32; border-color:#2e7d32"
                  @click="openPayDialog(record)"
                >
                  <template #icon><DollarOutlined /></template>
                </a-button>
              </a-tooltip>
              <a-tooltip title="Sửa">
                <a-button
                  size="small"
                  style="color:#1890ff; border-color:#1890ff"
                  @click="openEditDialog(record)"
                >
                  <template #icon><EditOutlined /></template>
                </a-button>
              </a-tooltip>
              <a-popconfirm
                title="Xóa hóa đơn này?"
                ok-text="Xóa"
                cancel-text="Hủy"
                @confirm="confirmDeletePayment(record)"
              >
                <a-tooltip title="Xóa">
                  <a-button size="small" danger>
                    <template #icon><DeleteOutlined /></template>
                  </a-button>
                </a-tooltip>
              </a-popconfirm>
            </div>
          </template>
        </template>
      </a-table>
    </a-card>

    <!-- Create/Edit Modal -->
    <a-modal
      v-model:open="showEditDialog"
      :title="isEditMode ? 'Sửa Hóa Đơn' : 'Tạo Hóa Đơn Mới'"
      :width="580"
      :footer="null"
      @cancel="showEditDialog = false"
    >
      <a-form
        ref="formRef"
        :model="formData"
        :rules="rules"
        layout="vertical"
        @finish="savePayment"
      >
        <a-row :gutter="16">
          <a-col :span="12">
            <a-form-item label="Mã học viên" name="studentId">
              <a-input-number
                v-model:value="formData.studentId"
                style="width:100%"
                :min="1"
                placeholder="Nhập mã học viên"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="Mã khóa học" name="courseId">
              <a-input-number
                v-model:value="formData.courseId"
                style="width:100%"
                :min="1"
                placeholder="Nhập mã khóa học"
              />
            </a-form-item>
          </a-col>
        </a-row>

        <a-form-item label="Tên học viên" name="studentName">
          <a-input v-model:value="formData.studentName" placeholder="Nhập tên học viên" />
        </a-form-item>

        <a-form-item label="Tên khóa học" name="courseName">
          <a-input v-model:value="formData.courseName" placeholder="Nhập tên khóa học" />
        </a-form-item>

        <a-row :gutter="16">
          <a-col :span="12">
            <a-form-item label="Số tiền (VNĐ)" name="amount">
              <a-input-number
                v-model:value="formData.amount"
                style="width:100%"
                :min="1"
                :formatter="value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')"
                :parser="value => value.replace(/,/g, '')"
                placeholder="Nhập số tiền"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="Ngày đến hạn" name="dueDate">
              <a-date-picker
                v-model:value="formData.dueDate"
                style="width:100%"
                format="DD/MM/YYYY"
                :placeholder="'Chọn ngày'"
              />
            </a-form-item>
          </a-col>
        </a-row>

        <a-form-item v-if="isEditMode" label="Ghi chú" name="note">
          <a-textarea v-model:value="formData.note" :rows="2" placeholder="Ghi chú (tùy chọn)" />
        </a-form-item>

        <div class="modal-footer">
          <a-button @click="showEditDialog = false">Hủy bỏ</a-button>
          <a-button type="primary" html-type="submit" :loading="saving">
            {{ isEditMode ? 'Cập nhật' : 'Tạo mới' }}
          </a-button>
        </div>
      </a-form>
    </a-modal>

    <!-- Pay Modal -->
    <a-modal
      v-model:open="showPayDialog"
      title="Xác Nhận Thanh Toán"
      :width="480"
      :footer="null"
      @cancel="showPayDialog = false"
    >
      <div class="pay-summary">
        <div class="pay-row">
          <span>Học viên</span>
          <strong>{{ selectedPayment?.studentName }}</strong>
        </div>
        <div class="pay-row">
          <span>Khóa học</span>
          <strong>{{ selectedPayment?.courseName }}</strong>
        </div>
        <div class="pay-row">
          <span>Tổng tiền</span>
          <strong class="text-danger">{{ fmt(selectedPayment?.amount) }}</strong>
        </div>
        <div class="pay-row">
          <span>Đã thanh toán</span>
          <strong class="text-success">{{ fmt(selectedPayment?.paidAmount || 0) }}</strong>
        </div>
        <a-divider style="margin: 12px 0" />
        <div class="pay-row pay-remaining">
          <span>Còn lại</span>
          <strong class="text-danger">{{ fmt((selectedPayment?.amount || 0) - (selectedPayment?.paidAmount || 0)) }}</strong>
        </div>
      </div>

      <a-form layout="vertical" class="mt-4">
        <a-form-item label="Số tiền thanh toán" required>
          <a-input-number
            v-model:value="payAmount"
            style="width:100%"
            :min="1"
            :max="(selectedPayment?.amount || 0) - (selectedPayment?.paidAmount || 0)"
            :formatter="value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')"
            :parser="value => value.replace(/,/g, '')"
            size="large"
          />
        </a-form-item>

        <a-form-item label="Phương thức thanh toán">
          <a-select v-model:value="payMethod" size="large">
            <a-select-option value="CASH">Tiền mặt</a-select-option>
            <a-select-option value="BANK_TRANSFER">Chuyển khoản</a-select-option>
            <a-select-option value="VNPAY">VNPay</a-select-option>
            <a-select-option value="MOMO">MoMo</a-select-option>
            <a-select-option value="CARD">Thẻ</a-select-option>
          </a-select>
        </a-form-item>

        <a-form-item label="Ghi chú (tùy chọn)">
          <a-textarea v-model:value="payNote" :rows="2" placeholder="Ghi chú thanh toán" />
        </a-form-item>
      </a-form>

      <div class="modal-footer">
        <a-button @click="showPayDialog = false">Hủy bỏ</a-button>
        <a-button type="primary" :loading="paying" @click="processPayment">
          <template #icon><DollarOutlined /></template>
          Xác nhận thanh toán
        </a-button>
      </div>
    </a-modal>

    <!-- Snackbar / message -->
    <a-float-button
      v-if="snack"
      type="primary"
      style="position:fixed; top:80px; right:24px; z-index:9999"
    />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import {
  PlusOutlined,
  SearchOutlined,
  CheckCircleOutlined,
  CloseCircleOutlined,
  DollarOutlined,
  EditOutlined,
  DeleteOutlined,
  CalendarOutlined,
  AlertOutlined,
  ClockCircleOutlined,
  CheckCircleFilled,
} from '@ant-design/icons-vue'
import { message } from 'ant-design-vue'
import { Tooltip } from 'ant-design-vue'
import { usePaymentStore } from '@/stores/payment'
import { createPayment, updatePayment, payInvoice, deletePayment } from '@/modules/payments/api/paymentApi'

const payment = usePaymentStore()
const loading = computed(() => payment.loading)

const search = ref('')
const statusFilter = ref('Tất cả')
const showEditDialog = ref(false)
const showPayDialog = ref(false)
const isEditMode = ref(false)
const selectedPayment = ref(null)
const saving = ref(false)
const paying = ref(false)
const deleting = ref(false)
const snack = ref(false)

const payAmount = ref(0)
const payMethod = ref('CASH')
const payNote = ref('')

const formData = ref({
  studentId: null,
  courseId: null,
  studentName: '',
  courseName: '',
  amount: 0,
  dueDate: null,
  note: ''
})

const rules = {
  studentId: [{ required: true, message: 'Bắt buộc', trigger: 'change' }],
  courseId: [{ required: true, message: 'Bắt buộc', trigger: 'change' }],
  amount: [{ required: true, message: 'Bắt buộc', trigger: 'change' }],
}

const columns = [
  {
    title: 'Học viên',
    key: 'studentName',
    fixed: 'left',
    width: 200,
  },
  {
    title: 'Khóa học',
    key: 'courseName',
    width: 180,
  },
  {
    title: 'Tổng tiền',
    key: 'amount',
    align: 'right',
    width: 140,
  },
  {
    title: 'Đã TT',
    key: 'paidAmount',
    align: 'right',
    width: 130,
  },
  {
    title: 'Còn lại',
    key: 'remaining',
    align: 'right',
    width: 130,
  },
  {
    title: 'Ngày TT',
    key: 'paymentDate',
    align: 'center',
    width: 110,
  },
  {
    title: 'Hạn',
    key: 'dueDate',
    align: 'center',
    width: 140,
  },
  {
    title: 'Trạng thái',
    key: 'status',
    align: 'center',
    width: 130,
  },
  {
    title: 'Thao tác',
    key: 'actions',
    align: 'center',
    fixed: 'right',
    width: 140,
  },
]

const fmt = v => new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', maximumFractionDigits: 0 }).format(v || 0)

const filteredPayments = computed(() => {
  let list = payment.invoices
  if (search.value) {
    const q = search.value.toLowerCase()
    list = list.filter(p =>
      p.studentName?.toLowerCase().includes(q) ||
      p.courseName?.toLowerCase().includes(q) ||
      p.studentId?.toString().includes(q)
    )
  }
  if (statusFilter.value !== 'Tất cả') {
    list = list.filter(p => p.status === statusFilter.value)
  }
  return list
})

const paidCount = computed(() => payment.invoices.filter(p => p.status === 'Paid').length)
const unpaidCount = computed(() => payment.invoices.filter(p => p.status !== 'Paid').length)

const kpis = computed(() => [
  {
    label: 'Tổng hóa đơn',
    value: payment.invoices.length,
    icon: 'FileTextOutlined',
    color: '#2e7d32',
    bg: 'rgba(46,125,50,0.1)',
    gradient: 'linear-gradient(135deg, rgba(46,125,50,0.3), transparent)',
  },
  {
    label: 'Đã thanh toán',
    value: paidCount.value,
    icon: 'CheckCircleOutlined',
    color: '#1890ff',
    bg: 'rgba(24,144,255,0.1)',
    gradient: 'linear-gradient(135deg, rgba(24,144,255,0.3), transparent)',
  },
  {
    label: 'Còn nợ',
    value: unpaidCount.value,
    icon: 'AlertOutlined',
    color: '#ff4d4f',
    bg: 'rgba(255,77,79,0.1)',
    gradient: 'linear-gradient(135deg, rgba(255,77,79,0.3), transparent)',
  },
  {
    label: 'Tổng thu',
    value: fmt(payment.invoices.reduce((s, p) => s + (p.paidAmount || 0), 0)),
    icon: 'DollarOutlined',
    color: '#722ed1',
    bg: 'rgba(114,46,209,0.1)',
    gradient: 'linear-gradient(135deg, rgba(114,46,209,0.3), transparent)',
  },
])

function getInitials(name) {
  if (!name) return '?'
  return name.split(' ').map(n => n[0]).join('').substring(0, 2).toUpperCase()
}

function formatDate(date) {
  if (!date) return '—'
  return new Date(date).toLocaleDateString('vi-VN')
}

function getStatusColor(s) {
  return { Paid: 'success', Partial: 'warning', Unpaid: 'error' }[s] || 'default'
}

function getStatusIcon(s) {
  const icons = {
    Paid: CheckCircleOutlined,
    Partial: ClockCircleOutlined,
    Unpaid: CloseCircleOutlined
  }
  return icons[s] || AlertOutlined
}

function getStatusText(s) {
  return { Paid: 'Đã TT', Partial: 'Một phần', Unpaid: 'Chưa TT' }[s] || s
}

function getDueDateColor(dueDate, status) {
  if (status === 'Paid') return 'success'
  if (!dueDate) return 'default'
  const daysLeft = Math.ceil((new Date(dueDate) - new Date()) / (1000 * 60 * 60 * 24))
  if (daysLeft < 0) return 'error'
  if (daysLeft <= 7) return 'warning'
  return 'processing'
}

function getDueDateIcon(dueDate, status) {
  if (status === 'Paid') return CheckCircleFilled
  if (!dueDate) return CalendarOutlined
  const daysLeft = Math.ceil((new Date(dueDate) - new Date()) / (1000 * 60 * 60 * 24))
  if (daysLeft < 0) return AlertOutlined
  if (daysLeft <= 7) return ClockCircleOutlined
  return CalendarOutlined
}

function openCreateDialog() {
  isEditMode.value = false
  formData.value = {
    studentId: null,
    courseId: null,
    studentName: '',
    courseName: '',
    amount: 0,
    dueDate: null,
    note: ''
  }
  showEditDialog.value = true
}

function openEditDialog(item) {
  isEditMode.value = true
  selectedPayment.value = item
  formData.value = {
    studentId: item.studentId,
    courseId: item.courseId,
    studentName: item.studentName || '',
    courseName: item.courseName || '',
    amount: item.amount,
    dueDate: item.dueDate ? new Date(item.dueDate) : null,
    note: item.note || ''
  }
  showEditDialog.value = true
}

function openPayDialog(item) {
  selectedPayment.value = item
  payAmount.value = item.amount - (item.paidAmount || 0)
  payMethod.value = 'CASH'
  payNote.value = ''
  showPayDialog.value = true
}

async function savePayment() {
  saving.value = true
  try {
    if (isEditMode.value) {
      await updatePayment(selectedPayment.value.id, {
        amount: formData.value.amount,
        dueDate: formData.value.dueDate ? new Date(formData.value.dueDate).toISOString() : null,
        note: formData.value.note
      })
      message.success('Cập nhật hóa đơn thành công')
    } else {
      await createPayment({
        studentId: formData.value.studentId,
        courseId: formData.value.courseId,
        amount: formData.value.amount,
        dueDate: formData.value.dueDate ? new Date(formData.value.dueDate).toISOString() : null,
        note: formData.value.note
      })
      message.success('Tạo hóa đơn thành công')
    }
    showEditDialog.value = false
    await payment.fetchInvoices()
  } catch (e) {
    message.error('Có lỗi xảy ra: ' + (e.message || 'Không xác định'))
  }
  saving.value = false
}

async function processPayment() {
  if (payAmount.value <= 0) {
    message.error('Số tiền phải lớn hơn 0')
    return
  }
  paying.value = true
  try {
    await payInvoice(selectedPayment.value.id, {
      paidAmount: payAmount.value,
      paymentMethod: payMethod.value,
      note: payNote.value
    })
    showPayDialog.value = false
    message.success('Thanh toán thành công!')
    await payment.fetchInvoices()
  } catch (e) {
    message.error('Có lỗi xảy ra: ' + (e.message || 'Không xác định'))
  }
  paying.value = false
}

async function confirmDeletePayment(record) {
  deleting.value = true
  try {
    await deletePayment(record.id)
    message.success('Xóa hóa đơn thành công')
    await payment.fetchInvoices()
  } catch (e) {
    message.error('Có lỗi xảy ra: ' + (e.message || 'Không xác định'))
  }
  deleting.value = false
}

onMounted(() => payment.fetchInvoices())
</script>

<style scoped>
.payments-page {
  padding: 0;
  animation: fadeIn 0.5s ease;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(16px); }
  to { opacity: 1; transform: translateY(0); }
}

.mb-6 { margin-bottom: 24px; }
.mt-4 { margin-top: 16px; }

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

/* KPI Cards */
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

.kpi-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 8px 30px rgba(0,0,0,0.08);
}

.kpi-glow {
  position: absolute;
  top: -40%;
  right: -40%;
  width: 120%;
  height: 120%;
  opacity: 0.5;
}

.kpi-body {
  position: relative;
  z-index: 1;
  display: flex;
  justify-content: space-between;
  align-items: center;
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
  width: 48px;
  height: 48px;
  border-radius: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 22px;
  flex-shrink: 0;
}

/* Filter */
.filter-card {
  background: white;
  border-radius: 16px;
  padding: 20px 24px;
  border: 1px solid rgba(0,0,0,0.05);
  box-shadow: 0 2px 12px rgba(0,0,0,0.04);
}

.filter-row {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-wrap: wrap;
}

.filter-stats {
  margin-left: auto;
  display: flex;
  gap: 8px;
}

/* Table */
.table-card {
  border-radius: 16px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.04);
  border: 1px solid rgba(0,0,0,0.05);
  overflow: hidden;
}

.student-cell {
  display: flex;
  align-items: center;
  gap: 12px;
}

.student-name {
  font-weight: 600;
  font-size: 13px;
  color: #1a1a2e;
}

.student-id {
  font-size: 11px;
  color: #888;
}

.amount-text {
  font-weight: 600;
  color: #1a1a2e;
}

.text-success { color: #52c41a; }
.text-danger { color: #ff4d4f; }
.text-secondary { color: #888; font-size: 11px; }

.status-tag {
  border-radius: 20px;
  font-weight: 600;
}

.action-buttons {
  display: flex;
  gap: 4px;
  justify-content: center;
  align-items: center;
}

/* Pay Modal */
.pay-summary {
  background: #fafafa;
  border-radius: 12px;
  padding: 16px;
}

.pay-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 6px 0;
  font-size: 13px;
}

.pay-row span { color: #888; }
.pay-row strong { color: #1a1a2e; }
.pay-remaining strong { font-size: 16px; }

/* Modal Footer */
.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  margin-top: 24px;
  padding-top: 16px;
  border-top: 1px solid #f0f0f0;
}
</style>
