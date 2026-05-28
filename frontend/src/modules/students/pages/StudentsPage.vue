<script setup>
import { computed, h, onMounted, reactive, ref } from 'vue'
import { message } from 'ant-design-vue'
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
  ReloadOutlined,
  SearchOutlined,
  UserOutlined,
} from '@ant-design/icons-vue'
import {
  createStudent,
  deleteStudent,
  extractApiErrorMessage,
  getStudents,
  updateStudent,
} from '../api/studentApi'
import {
  formatDate,
  initials,
  STUDENT_STATUS_OPTIONS,
  studentStatusColor,
  toDateValue,
} from '../utils/studentUi'

function createStudentForm() {
  return {
    fullName: '',
    email: '',
    phone: '',
    dateOfBirth: null,
    address: '',
    enrolledAt: toDateValue(new Date()),
    status: 'Active',
  }
}

const formRef = ref()
const students = ref([])
const loading = ref(false)
const saving = ref(false)
const modalOpen = ref(false)
const editingId = ref(null)
const searchText = ref('')

const formState = reactive(createStudentForm())

const rules = {
  fullName: [{ required: true, message: 'Vui lòng nhập họ tên học viên.', trigger: 'blur' }],
  email: [
    { required: true, message: 'Vui lòng nhập email.', trigger: 'blur' },
    { type: 'email', message: 'Email không hợp lệ.', trigger: 'blur' },
  ],
  dateOfBirth: [{ required: true, message: 'Vui lòng chọn ngày sinh.', trigger: 'change' }],
  enrolledAt: [{ required: true, message: 'Vui lòng chọn ngày ghi danh.', trigger: 'change' }],
  status: [{ required: true, message: 'Vui lòng chọn trạng thái.', trigger: 'change' }],
}

const columns = [
  { title: 'Học viên', key: 'student', width: 250, ellipsis: true },
  { title: 'Liên hệ', key: 'contact', width: 220, ellipsis: true },
  { title: 'Ngày sinh', key: 'dateOfBirth', width: 120 },
  { title: 'Địa chỉ', key: 'address', ellipsis: true },
  { title: 'Ghi danh', key: 'enrolledAt', width: 120 },
  { title: 'Trạng thái', key: 'status', width: 120 },
  { title: 'Thao tác', key: 'actions', width: 160, fixed: 'right' },
]

const filteredStudents = computed(() => {
  const keyword = searchText.value.trim().toLowerCase()
  const source = [...students.value].sort((left, right) => right.id - left.id)

  if (!keyword) {
    return source
  }

  return source.filter((student) => {
    const haystack = [
      student.fullName,
      student.email,
      student.phone,
      student.address,
      student.status,
    ]
      .map((value) => String(value ?? '').toLowerCase())
      .join(' ')

    return haystack.includes(keyword)
  })
})

const totalStudents = computed(() => students.value.length)
const activeStudents = computed(() => students.value.filter((student) => student.status === 'Active').length)
const inactiveStudents = computed(() => students.value.filter((student) => student.status === 'Inactive').length)
const visibleStudents = computed(() => filteredStudents.value.length)

const modalTitle = computed(() => (editingId.value ? 'Cập nhật học viên' : 'Thêm học viên'))

function resetForm() {
  Object.assign(formState, createStudentForm())
}

function openCreateModal() {
  editingId.value = null
  resetForm()
  modalOpen.value = true
}

function openEditModal(record) {
  editingId.value = record.id
  Object.assign(formState, {
    fullName: record.fullName ?? '',
    email: record.email ?? '',
    phone: record.phone ?? '',
    dateOfBirth: toDateValue(record.dateOfBirth),
    address: record.address ?? '',
    enrolledAt: toDateValue(record.enrolledAt),
    status: record.status ?? 'Active',
  })
  modalOpen.value = true
}

function closeModal() {
  modalOpen.value = false
  editingId.value = null
  resetForm()
  formRef.value?.clearValidate?.()
}

async function loadStudents() {
  loading.value = true
  try {
    students.value = await getStudents()
  } catch (error) {
    message.error(extractApiErrorMessage(error, 'Không tải được danh sách học viên.'))
  } finally {
    loading.value = false
  }
}

async function submitForm() {
  try {
    await formRef.value?.validate()
  } catch {
    return
  }

  const payload = {
    fullName: formState.fullName.trim(),
    email: formState.email.trim(),
    phone: formState.phone?.trim() || null,
    dateOfBirth: formState.dateOfBirth ? `${formState.dateOfBirth}T00:00:00` : null,
    address: formState.address?.trim() || null,
    enrolledAt: formState.enrolledAt ? `${formState.enrolledAt}T00:00:00` : null,
    status: formState.status,
  }

  saving.value = true
  try {
    if (editingId.value) {
      await updateStudent(editingId.value, payload)
      message.success('Đã cập nhật học viên.')
    } else {
      await createStudent(payload)
      message.success('Đã thêm học viên mới.')
    }

    closeModal()
    await loadStudents()
  } catch (error) {
    message.error(
      extractApiErrorMessage(
        error,
        editingId.value ? 'Không cập nhật được học viên.' : 'Không tạo được học viên.',
      ),
    )
  } finally {
    saving.value = false
  }
}

async function handleDelete(record) {
  try {
    await deleteStudent(record.id)
    message.success('Đã xoá học viên.')
    await loadStudents()
  } catch (error) {
    message.error(extractApiErrorMessage(error, 'Không xoá được học viên.'))
  }
}

onMounted(loadStudents)
</script>

<template>
  <section class="student-module">
    <a-card class="module-hero" :bordered="false">
      <div class="module-hero__body">
        <p class="eyebrow">Student & Attendance Service</p>
        <h2>Students</h2>
        <p>
          Quản lý hồ sơ học viên, ghi danh và trạng thái hoạt động trong cùng một màn hình CRUD.
        </p>
      </div>

      <div class="module-hero__actions">
        <span class="module-hero__actions-label">Quick actions</span>
        <span class="module-hero__actions-note">
          Manage student profiles and keep the dataset ready for the other modules.
        </span>
        <div class="module-hero__actions-stack">
          <a-button :icon="h(ReloadOutlined)" :loading="loading" @click="loadStudents">
            Tải lại
          </a-button>
          <a-button type="primary" :icon="h(PlusOutlined)" @click="openCreateModal">
            Thêm học viên
          </a-button>
        </div>
      </div>
    </a-card>

    <div class="module-metrics">
      <a-card class="module-metric-card" :bordered="false">
        <div class="metric-card-header">
          <div class="metric-icon-circle metric-icon-circle--blue">
            <UserOutlined />
          </div>
          <span class="metric-trend-badge metric-trend-badge--blue">Tổng cộng</span>
        </div>
        <div class="metric-card-value">{{ totalStudents }}</div>
        <div class="metric-card-label">Tổng học viên</div>
      </a-card>
      <a-card class="module-metric-card" :bordered="false">
        <div class="metric-card-header">
          <div class="metric-icon-circle metric-icon-circle--green">
            <i class="metric-icon-symbol">✔</i>
          </div>
          <span class="metric-trend-badge metric-trend-badge--green">Active</span>
        </div>
        <div class="metric-card-value">{{ activeStudents }}</div>
        <div class="metric-card-label">Đang hoạt động</div>
      </a-card>
      <a-card class="module-metric-card" :bordered="false">
        <div class="metric-card-header">
          <div class="metric-icon-circle metric-icon-circle--amber">
            <i class="metric-icon-symbol">✖</i>
          </div>
          <span class="metric-trend-badge metric-trend-badge--amber">Inactive</span>
        </div>
        <div class="metric-card-value">{{ inactiveStudents }}</div>
        <div class="metric-card-label">Ngừng hoạt động</div>
      </a-card>
      <a-card class="module-metric-card" :bordered="false">
        <div class="metric-card-header">
          <div class="metric-icon-circle metric-icon-circle--violet">
            <i class="metric-icon-symbol">🔍</i>
          </div>
          <span class="metric-trend-badge metric-trend-badge--violet">Kết quả lọc</span>
        </div>
        <div class="metric-card-value">{{ visibleStudents }}</div>
        <div class="metric-card-label">Đang hiển thị</div>
      </a-card>
    </div>

    <a-card class="module-table-card" :bordered="false">
      <div class="module-toolbar">
        <a-input-search
          v-model:value="searchText"
          class="module-search"
          placeholder="Tìm theo tên, email, số điện thoại, địa chỉ..."
          :prefix="h(SearchOutlined)"
          allow-clear
        />
        <div class="toolbar-filters">
          <button
            :class="['filter-pill', searchText === '' ? 'filter-pill--active' : '']"
            @click="searchText = ''"
          >Tất cả</button>
          <button
            :class="['filter-pill', searchText === 'active' ? 'filter-pill--active-green filter-pill' : '']"
            @click="searchText = 'active'"
          >
            <span class="filter-pill__dot" style="background:#16a34a"></span>
            Đang hoạt động
          </button>
          <button
            :class="['filter-pill', searchText === 'inactive' ? 'filter-pill--active-amber filter-pill' : '']"
            @click="searchText = 'inactive'"
          >
            <span class="filter-pill__dot" style="background:#d97706"></span>
            Ngừng hoạt động
          </button>
        </div>
        <div class="module-toolbar-count">
          Hiển thị <strong>{{ visibleStudents }}</strong> / {{ totalStudents }}
        </div>
      </div>

      <a-table
        class="module-table"
        :columns="columns"
        :data-source="filteredStudents"
        :loading="loading"
        :pagination="{ pageSize: 6, showSizeChanger: false, hideOnSinglePage: true }"
        :scroll="{ x: 1180 }"
        row-key="id"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'student'">
            <div class="entity-cell">
              <a-avatar :style="{ backgroundColor: '#0ea5e9' }">
                {{ initials(record.fullName) }}
              </a-avatar>
              <div class="entity-cell__meta">
                <strong>{{ record.fullName }}</strong>
                <span>#{{ record.id }}</span>
              </div>
            </div>
          </template>

          <template v-else-if="column.key === 'contact'">
            <div class="entity-cell__meta">
              <strong>{{ record.email }}</strong>
              <span>{{ record.phone || 'Chưa có số điện thoại' }}</span>
            </div>
          </template>

          <template v-else-if="column.key === 'dateOfBirth'">
            {{ formatDate(record.dateOfBirth) }}
          </template>

          <template v-else-if="column.key === 'address'">
            <span class="entity-note">{{ record.address || 'Chưa cập nhật' }}</span>
          </template>

          <template v-else-if="column.key === 'enrolledAt'">
            {{ formatDate(record.enrolledAt) }}
          </template>

          <template v-else-if="column.key === 'status'">
            <a-tag :color="studentStatusColor(record.status)">{{ record.status }}</a-tag>
          </template>

          <template v-else-if="column.key === 'actions'">
            <div class="entity-actions">
              <a-button type="text" size="small" @click="openEditModal(record)">
                <EditOutlined />
                Sửa
              </a-button>
              <a-popconfirm
                title="Xoá học viên này?"
                ok-text="Xoá"
                cancel-text="Huỷ"
                @confirm="() => handleDelete(record)"
              >
                <a-button type="text" size="small" danger>
                  <DeleteOutlined />
                  Xoá
                </a-button>
              </a-popconfirm>
            </div>
          </template>
        </template>
      </a-table>
    </a-card>

    <a-modal
      v-model:open="modalOpen"
      :title="modalTitle"
      :confirm-loading="saving"
      cancel-text="Huỷ"
      ok-text="Lưu"
      destroy-on-close
      width="860px"
      @ok="submitForm"
      @cancel="closeModal"
    >
      <a-form ref="formRef" :model="formState" :rules="rules" layout="vertical">
        <a-divider orientation="left">
          <span class="form-section-label">Thông tin cá nhân</span>
        </a-divider>
        <a-row :gutter="16">
          <a-col :xs="24" :md="12">
            <a-form-item label="Họ và tên" name="fullName">
              <a-input v-model:value="formState.fullName" placeholder="Nguyễn Văn A" />
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="Email" name="email">
              <a-input v-model:value="formState.email" placeholder="name@example.com" />
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="Số điện thoại">
              <a-input v-model:value="formState.phone" placeholder="0909000000" />
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="Ngày sinh" name="dateOfBirth">
              <a-date-picker
                v-model:value="formState.dateOfBirth"
                style="width: 100%"
                format="DD/MM/YYYY"
                value-format="YYYY-MM-DD"
                placeholder="Chọn ngày sinh"
              />
            </a-form-item>
          </a-col>
          <a-col :xs="24">
            <a-form-item label="Địa chỉ">
              <a-textarea
                v-model:value="formState.address"
                :rows="3"
                placeholder="Nhập địa chỉ liên hệ"
              />
            </a-form-item>
          </a-col>
        </a-row>

        <a-divider orientation="left">
          <span class="form-section-label">Thông tin ghi danh</span>
        </a-divider>
        <a-row :gutter="16">
          <a-col :xs="24" :md="12">
            <a-form-item label="Ngày ghi danh" name="enrolledAt">
              <a-date-picker
                v-model:value="formState.enrolledAt"
                style="width: 100%"
                format="DD/MM/YYYY"
                value-format="YYYY-MM-DD"
                placeholder="Chọn ngày ghi danh"
              />
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="Trạng thái" name="status">
              <a-select v-model:value="formState.status" placeholder="Chọn trạng thái">
                <a-select-option
                  v-for="option in STUDENT_STATUS_OPTIONS"
                  :key="option.value"
                  :value="option.value"
                >
                  {{ option.label }}
                </a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-modal>
  </section>
</template>
