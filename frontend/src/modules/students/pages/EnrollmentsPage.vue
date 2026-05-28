<script setup>
import { computed, h, onMounted, reactive, ref } from 'vue'
import { message } from 'ant-design-vue'
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
  ReloadOutlined,
  SearchOutlined,
  BookOutlined,
} from '@ant-design/icons-vue'
import {
  createEnrollment,
  deleteEnrollment,
  extractApiErrorMessage,
  getEnrollments,
  updateEnrollment,
} from '../api/studentApi'
import { useStudentDirectory } from '../composables/useStudentDirectory'
import {
  ENROLLMENT_STATUS_OPTIONS,
  enrollmentStatusColor,
  formatDate,
  toDateValue,
} from '../utils/studentUi'

function createEnrollmentForm() {
  return {
    studentId: undefined,
    courseId: undefined,
    enrolledDate: toDateValue(new Date()),
    status: 'Enrolled',
  }
}

const formRef = ref()
const enrollments = ref([])
const loading = ref(false)
const saving = ref(false)
const modalOpen = ref(false)
const editingId = ref(null)
const searchText = ref('')

const formState = reactive(createEnrollmentForm())

const { getStudentName, loadStudents, studentLoading, studentOptions } = useStudentDirectory()

const rules = {
  studentId: [{ required: true, message: 'Vui lòng chọn học viên.', trigger: 'change' }],
  courseId: [{ required: true, message: 'Vui lòng nhập CourseId.', trigger: 'blur' }],
  enrolledDate: [{ required: true, message: 'Vui lòng chọn ngày đăng ký.', trigger: 'change' }],
  status: [{ required: true, message: 'Vui lòng chọn trạng thái.', trigger: 'change' }],
}

const columns = [
  { title: 'Học viên', key: 'student', width: 260, ellipsis: true },
  { title: 'Course ID', key: 'courseId', width: 120 },
  { title: 'Ngày đăng ký', key: 'enrolledDate', width: 130 },
  { title: 'Trạng thái', key: 'status', width: 120 },
  { title: 'Thao tác', key: 'actions', width: 160, fixed: 'right' },
]

const filteredEnrollments = computed(() => {
  const keyword = searchText.value.trim().toLowerCase()
  const source = [...enrollments.value].sort((left, right) => right.id - left.id)

  if (!keyword) {
    return source
  }

  return source.filter((enrollment) => {
    const studentName = getStudentName(enrollment.studentId)
    const haystack = [studentName, enrollment.courseId, enrollment.status]
      .map((value) => String(value ?? '').toLowerCase())
      .join(' ')

    return haystack.includes(keyword)
  })
})

const totalEnrollments = computed(() => enrollments.value.length)
const enrolledCount = computed(
  () => enrollments.value.filter((item) => item.status === 'Enrolled').length,
)
const pendingCount = computed(
  () => enrollments.value.filter((item) => item.status === 'Pending').length,
)
const completedCount = computed(
  () => enrollments.value.filter((item) => item.status === 'Completed').length,
)

const modalTitle = computed(() => (editingId.value ? 'Cập nhật đăng ký' : 'Thêm đăng ký'))

function resetForm() {
  Object.assign(formState, createEnrollmentForm())
}

function openCreateModal() {
  editingId.value = null
  resetForm()
  modalOpen.value = true
}

function openEditModal(record) {
  editingId.value = record.id
  Object.assign(formState, {
    studentId: record.studentId,
    courseId: record.courseId,
    enrolledDate: toDateValue(record.enrolledDate),
    status: record.status ?? 'Enrolled',
  })
  modalOpen.value = true
}

function closeModal() {
  modalOpen.value = false
  editingId.value = null
  resetForm()
  formRef.value?.clearValidate?.()
}

async function loadEnrollments() {
  loading.value = true
  try {
    enrollments.value = await getEnrollments()
  } catch (error) {
    message.error(extractApiErrorMessage(error, 'Không tải được danh sách đăng ký.'))
  } finally {
    loading.value = false
  }
}

async function reloadAll() {
  await Promise.allSettled([loadStudents(), loadEnrollments()])
}

async function submitForm() {
  try {
    await formRef.value?.validate()
  } catch {
    return
  }

  const payload = {
    studentId: Number(formState.studentId),
    courseId: Number(formState.courseId),
    enrolledDate: formState.enrolledDate ? `${formState.enrolledDate}T00:00:00` : null,
    status: formState.status,
  }

  saving.value = true
  try {
    if (editingId.value) {
      await updateEnrollment(editingId.value, payload)
      message.success('Đã cập nhật đăng ký.')
    } else {
      await createEnrollment(payload)
      message.success('Đã tạo đăng ký mới.')
    }

    closeModal()
    await loadEnrollments()
  } catch (error) {
    message.error(
      extractApiErrorMessage(
        error,
        editingId.value ? 'Không cập nhật được đăng ký.' : 'Không tạo được đăng ký.',
      ),
    )
  } finally {
    saving.value = false
  }
}

async function handleDelete(record) {
  try {
    await deleteEnrollment(record.id)
    message.success('Đã xoá đăng ký.')
    await loadEnrollments()
  } catch (error) {
    message.error(extractApiErrorMessage(error, 'Không xoá được đăng ký.'))
  }
}

onMounted(reloadAll)
</script>

<template>
  <section class="student-module">
    <a-card class="module-hero" :bordered="false">
      <div class="module-hero__body">
        <p class="eyebrow">Student & Attendance Service</p>
        <h2>Enrollments</h2>
        <p>
          Đăng ký khóa học cho học viên. Mỗi bản ghi chỉ lưu StudentId và CourseId, đúng theo
          microservices.
        </p>
      </div>

      <div class="module-hero__actions">
        <span class="module-hero__actions-label">Quick actions</span>
        <span class="module-hero__actions-note">
          Link students with a course id and keep the enrollment state in sync.
        </span>
        <div class="module-hero__actions-stack">
          <a-button
            :icon="h(ReloadOutlined)"
            :loading="loading || studentLoading"
            @click="reloadAll"
          >
            Tải lại
          </a-button>
          <a-button type="primary" :icon="h(PlusOutlined)" @click="openCreateModal">
            Thêm đăng ký
          </a-button>
        </div>
      </div>
    </a-card>

    <div class="module-metrics">
      <a-card class="module-metric-card" :bordered="false">
        <div class="metric-card-header">
          <div class="metric-icon-circle metric-icon-circle--blue">
            <BookOutlined />
          </div>
          <span class="metric-trend-badge metric-trend-badge--blue">Tổng cộng</span>
        </div>
        <div class="metric-card-value">{{ totalEnrollments }}</div>
        <div class="metric-card-label">Tổng đăng ký</div>
      </a-card>
      <a-card class="module-metric-card" :bordered="false">
        <div class="metric-card-header">
          <div class="metric-icon-circle metric-icon-circle--green">
            <i class="metric-icon-symbol">✔</i>
          </div>
          <span class="metric-trend-badge metric-trend-badge--green">Enrolled</span>
        </div>
        <div class="metric-card-value">{{ enrolledCount }}</div>
        <div class="metric-card-label">Đang theo học</div>
      </a-card>
      <a-card class="module-metric-card" :bordered="false">
        <div class="metric-card-header">
          <div class="metric-icon-circle metric-icon-circle--amber">
            <i class="metric-icon-symbol">⏳</i>
          </div>
          <span class="metric-trend-badge metric-trend-badge--amber">Pending</span>
        </div>
        <div class="metric-card-value">{{ pendingCount }}</div>
        <div class="metric-card-label">Chờ xử lý</div>
      </a-card>
      <a-card class="module-metric-card" :bordered="false">
        <div class="metric-card-header">
          <div class="metric-icon-circle metric-icon-circle--violet">
            <i class="metric-icon-symbol">🎓</i>
          </div>
          <span class="metric-trend-badge metric-trend-badge--violet">Completed</span>
        </div>
        <div class="metric-card-value">{{ completedCount }}</div>
        <div class="metric-card-label">Đã hoàn thành</div>
      </a-card>
    </div>

    <a-card class="module-table-card" :bordered="false">
      <div class="module-toolbar">
        <a-input-search
          v-model:value="searchText"
          class="module-search"
          placeholder="Tìm theo tên học viên, CourseId hoặc trạng thái..."
          :prefix="h(SearchOutlined)"
          allow-clear
        />
        <div class="toolbar-filters">
          <button
            :class="['filter-pill', searchText === '' ? 'filter-pill--active' : '']"
            @click="searchText = ''"
          >Tất cả</button>
          <button
            :class="['filter-pill', searchText === 'enrolled' ? 'filter-pill--active-green filter-pill' : '']"
            @click="searchText = 'enrolled'"
          >
            <span class="filter-pill__dot" style="background:#16a34a"></span>
            Enrolled
          </button>
          <button
            :class="['filter-pill', searchText === 'pending' ? 'filter-pill--active-amber filter-pill' : '']"
            @click="searchText = 'pending'"
          >
            <span class="filter-pill__dot" style="background:#d97706"></span>
            Pending
          </button>
          <button
            :class="['filter-pill', searchText === 'completed' ? 'filter-pill--active-violet filter-pill' : '']"
            @click="searchText = 'completed'"
          >
            <span class="filter-pill__dot" style="background:#7c3aed"></span>
            Completed
          </button>
        </div>
        <div class="module-toolbar-count">
          Hiển thị <strong>{{ filteredEnrollments.length }}</strong> / {{ totalEnrollments }}
        </div>
      </div>

      <a-table
        class="module-table"
        :columns="columns"
        :data-source="filteredEnrollments"
        :loading="loading || studentLoading"
        :pagination="{ pageSize: 6, showSizeChanger: false, hideOnSinglePage: true }"
        :scroll="{ x: 900 }"
        row-key="id"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'student'">
            <div class="entity-cell">
              <a-avatar :style="{ backgroundColor: '#2563eb' }">
                <BookOutlined />
              </a-avatar>
              <div class="entity-cell__meta">
                <strong>{{ getStudentName(record.studentId) }}</strong>
                <span>StudentId #{{ record.studentId }}</span>
              </div>
            </div>
          </template>

          <template v-else-if="column.key === 'courseId'">
            <a-tag color="blue" style="display: inline-flex; align-items: center; gap: 4px;">
              <span>📖</span>
              <span>ID: {{ record.courseId }}</span>
            </a-tag>
          </template>

          <template v-else-if="column.key === 'enrolledDate'">
            {{ formatDate(record.enrolledDate) }}
          </template>

          <template v-else-if="column.key === 'status'">
            <a-tag :color="enrollmentStatusColor(record.status)">{{ record.status }}</a-tag>
          </template>

          <template v-else-if="column.key === 'actions'">
            <div class="entity-actions">
              <a-button type="text" size="small" @click="openEditModal(record)">
                <EditOutlined />
                Sửa
              </a-button>
              <a-popconfirm
                title="Xoá đăng ký này?"
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
      width="760px"
      @ok="submitForm"
      @cancel="closeModal"
    >
      <a-form ref="formRef" :model="formState" :rules="rules" layout="vertical">
        <a-row :gutter="16">
          <a-col :xs="24" :md="12">
            <a-form-item label="Học viên" name="studentId">
              <a-select
                v-model:value="formState.studentId"
                :options="studentOptions"
                placeholder="Chọn học viên"
                show-search
                option-filter-prop="label"
              />
              <div class="form-hint">Dữ liệu lấy trực tiếp từ Student Service.</div>
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="CourseId" name="courseId">
              <a-input-number
                v-model:value="formState.courseId"
                :min="1"
                style="width: 100%"
                placeholder="101"
              />
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="Ngày đăng ký" name="enrolledDate">
              <a-date-picker
                v-model:value="formState.enrolledDate"
                style="width: 100%"
                format="DD/MM/YYYY"
                value-format="YYYY-MM-DD"
                placeholder="Chọn ngày"
              />
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="Trạng thái" name="status">
              <a-select v-model:value="formState.status" placeholder="Chọn trạng thái">
                <a-select-option
                  v-for="option in ENROLLMENT_STATUS_OPTIONS"
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
