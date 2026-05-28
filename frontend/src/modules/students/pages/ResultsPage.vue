<script setup>
import { computed, h, onMounted, reactive, ref } from 'vue'
import { message } from 'ant-design-vue'
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
  ReloadOutlined,
  SearchOutlined,
  TrophyOutlined,
} from '@ant-design/icons-vue'
import {
  createResult,
  deleteResult,
  extractApiErrorMessage,
  getResults,
  updateResult,
} from '../api/studentApi'
import { useStudentDirectory } from '../composables/useStudentDirectory'
import {
  formatDate,
  formatScore,
  resultOutcomeColor,
  resultOutcomeLabel,
  toDateValue,
} from '../utils/studentUi'

function createResultForm() {
  return {
    studentId: undefined,
    courseId: undefined,
    score: 0,
    resultDate: toDateValue(new Date()),
    note: '',
  }
}

const formRef = ref()
const results = ref([])
const loading = ref(false)
const saving = ref(false)
const modalOpen = ref(false)
const editingId = ref(null)
const searchText = ref('')

const formState = reactive(createResultForm())

const { getStudentName, loadStudents, studentLoading, studentOptions } = useStudentDirectory()

const rules = {
  studentId: [{ required: true, message: 'Vui lòng chọn học viên.', trigger: 'change' }],
  courseId: [{ required: true, message: 'Vui lòng nhập CourseId.', trigger: 'blur' }],
  score: [{ required: true, message: 'Vui lòng nhập điểm.', trigger: 'change' }],
  resultDate: [{ required: true, message: 'Vui lòng chọn ngày đánh giá.', trigger: 'change' }],
}

const columns = [
  { title: 'Học viên', key: 'student', width: 260, ellipsis: true },
  { title: 'Course ID', key: 'courseId', width: 120 },
  { title: 'Điểm', key: 'score', width: 120 },
  { title: 'Kết quả', key: 'outcome', width: 120 },
  { title: 'Ngày đánh giá', key: 'resultDate', width: 140 },
  { title: 'Ghi chú', key: 'note', ellipsis: true },
  { title: 'Thao tác', key: 'actions', width: 160, fixed: 'right' },
]

const filteredResults = computed(() => {
  const keyword = searchText.value.trim().toLowerCase()
  const source = [...results.value].sort((left, right) => right.id - left.id)

  if (!keyword) {
    return source
  }

  return source.filter((result) => {
    const haystack = [
      getStudentName(result.studentId),
      result.courseId,
      result.score,
      result.note,
      resultOutcomeLabel(result.score),
    ]
      .map((value) => String(value ?? '').toLowerCase())
      .join(' ')

    return haystack.includes(keyword)
  })
})

const totalResults = computed(() => results.value.length)
const averageScore = computed(() => {
  if (!results.value.length) {
    return 0
  }

  const total = results.value.reduce((sum, item) => sum + Number(item.score || 0), 0)
  return Number((total / results.value.length).toFixed(2))
})
const passedCount = computed(
  () => results.value.filter((item) => Number(item.score) >= 5).length,
)
const highScoreCount = computed(
  () => results.value.filter((item) => Number(item.score) >= 8).length,
)

const modalTitle = computed(() => (editingId.value ? 'Cập nhật kết quả' : 'Thêm kết quả'))

function resetForm() {
  Object.assign(formState, createResultForm())
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
    score: Number(record.score ?? 0),
    resultDate: toDateValue(record.resultDate),
    note: record.note ?? '',
  })
  modalOpen.value = true
}

function closeModal() {
  modalOpen.value = false
  editingId.value = null
  resetForm()
  formRef.value?.clearValidate?.()
}

async function loadResults() {
  loading.value = true
  try {
    results.value = await getResults()
  } catch (error) {
    message.error(extractApiErrorMessage(error, 'Không tải được danh sách kết quả.'))
  } finally {
    loading.value = false
  }
}

async function reloadAll() {
  await Promise.allSettled([loadStudents(), loadResults()])
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
    score: Number(formState.score),
    resultDate: formState.resultDate ? `${formState.resultDate}T00:00:00` : null,
    note: formState.note?.trim() || null,
  }

  saving.value = true
  try {
    if (editingId.value) {
      await updateResult(editingId.value, payload)
      message.success('Đã cập nhật kết quả.')
    } else {
      await createResult(payload)
      message.success('Đã thêm kết quả.')
    }

    closeModal()
    await loadResults()
  } catch (error) {
    message.error(
      extractApiErrorMessage(
        error,
        editingId.value ? 'Không cập nhật được kết quả.' : 'Không tạo được kết quả.',
      ),
    )
  } finally {
    saving.value = false
  }
}

async function handleDelete(record) {
  try {
    await deleteResult(record.id)
    message.success('Đã xoá kết quả.')
    await loadResults()
  } catch (error) {
    message.error(extractApiErrorMessage(error, 'Không xoá được kết quả.'))
  }
}

onMounted(reloadAll)
</script>

<template>
  <section class="student-module">
    <a-card class="module-hero" :bordered="false">
      <div class="module-hero__body">
        <p class="eyebrow">Student, Attendance & Results Service</p>
        <h2>Results</h2>
        <p>
          Quản lý kết quả học tập theo từng học viên và khóa học. Dữ liệu chỉ lưu StudentId và
          CourseId, không join trực tiếp sang service khác.
        </p>
      </div>

      <div class="module-hero__actions">
        <span class="module-hero__actions-label">Quick actions</span>
        <span class="module-hero__actions-note">
          Review scores, classify outcomes, and keep notes for each course.
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
            Thêm kết quả
          </a-button>
        </div>
      </div>
    </a-card>

    <div class="module-metrics">
      <a-card class="module-metric-card" :bordered="false">
        <div class="metric-card-header">
          <div class="metric-icon-circle metric-icon-circle--blue">
            <TrophyOutlined />
          </div>
          <span class="metric-trend-badge metric-trend-badge--blue">Tổng cộng</span>
        </div>
        <div class="metric-card-value">{{ totalResults }}</div>
        <div class="metric-card-label">Tổng kết quả</div>
      </a-card>
      <a-card class="module-metric-card" :bordered="false">
        <div class="metric-card-header">
          <div :class="['metric-icon-circle', averageScore >= 7 ? 'metric-icon-circle--green' : 'metric-icon-circle--amber']">
            <i class="metric-icon-symbol">📈</i>
          </div>
          <span :class="['metric-trend-badge', averageScore >= 7 ? 'metric-trend-badge--green' : 'metric-trend-badge--amber']">
            Thang điểm 10
          </span>
        </div>
        <div class="metric-card-value">{{ formatScore(averageScore) }}</div>
        <div class="metric-card-label">Điểm trung bình</div>
      </a-card>
      <a-card class="module-metric-card" :bordered="false">
        <div class="metric-card-header">
          <div class="metric-icon-circle metric-icon-circle--teal">
            <i class="metric-icon-symbol">✔</i>
          </div>
          <span class="metric-trend-badge metric-trend-badge--teal">Đạt (Score >= 5)</span>
        </div>
        <div class="metric-card-value">{{ passedCount }}</div>
        <div class="metric-card-label">Số học viên Đạt</div>
      </a-card>
      <a-card class="module-metric-card" :bordered="false">
        <div class="metric-card-header">
          <div class="metric-icon-circle metric-icon-circle--violet">
            <i class="metric-icon-symbol">★</i>
          </div>
          <span class="metric-trend-badge metric-trend-badge--violet">Giỏi (Score >= 8)</span>
        </div>
        <div class="metric-card-value">{{ highScoreCount }}</div>
        <div class="metric-card-label">Số học viên Khá - Giỏi</div>
      </a-card>
    </div>

    <a-card class="module-table-card" :bordered="false">
      <div class="module-toolbar">
        <a-input-search
          v-model:value="searchText"
          class="module-search"
          placeholder="Tìm theo tên học viên, CourseId, điểm hoặc ghi chú..."
          :prefix="h(SearchOutlined)"
          allow-clear
        />
        <div class="toolbar-filters">
          <button
            :class="['filter-pill', searchText === '' ? 'filter-pill--active' : '']"
            @click="searchText = ''"
          >
            Tất cả
          </button>
          <button
            :class="['filter-pill', searchText.trim().toLowerCase() === 'passed' ? 'filter-pill--active-green filter-pill' : '']"
            @click="searchText = 'Passed'"
          >
            <span class="filter-pill__dot" style="background:#16a34a"></span>
            Đạt (Passed)
          </button>
          <button
            :class="['filter-pill', searchText.trim().toLowerCase() === 'failed' ? 'filter-pill--active-rose filter-pill' : '']"
            @click="searchText = 'Failed'"
          >
            <span class="filter-pill__dot" style="background:#e11d48"></span>
            Trượt (Failed)
          </button>
        </div>
        <div class="module-toolbar-count">
          Hiển thị <strong>{{ filteredResults.length }}</strong> / {{ totalResults }} kết quả
        </div>
      </div>

      <a-table
        class="module-table"
        :columns="columns"
        :data-source="filteredResults"
        :loading="loading || studentLoading"
        :pagination="{ pageSize: 6, showSizeChanger: false, hideOnSinglePage: true }"
        :scroll="{ x: 1040 }"
        row-key="id"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'student'">
            <div class="entity-cell">
              <a-avatar :style="{ backgroundColor: '#7c3aed' }">
                <TrophyOutlined />
              </a-avatar>
              <div class="entity-cell__meta">
                <strong>{{ getStudentName(record.studentId) }}</strong>
                <span>StudentId #{{ record.studentId }}</span>
              </div>
            </div>
          </template>

          <template v-else-if="column.key === 'courseId'">
            <a-tag color="purple">Course #{{ record.courseId }}</a-tag>
          </template>

          <template v-else-if="column.key === 'score'">
            <div class="score-cell">
              <a-tag color="blue" style="margin-right: 0; width: fit-content;">
                {{ formatScore(record.score) }}/10
              </a-tag>
              <div class="score-bar">
                <div class="score-bar__fill" :style="{ width: (record.score * 10) + '%' }"></div>
              </div>
            </div>
          </template>

          <template v-else-if="column.key === 'outcome'">
            <a-tag :color="resultOutcomeColor(record.score)">
              {{ resultOutcomeLabel(record.score) }}
            </a-tag>
          </template>

          <template v-else-if="column.key === 'resultDate'">
            {{ formatDate(record.resultDate) }}
          </template>

          <template v-else-if="column.key === 'note'">
            <span class="entity-note">{{ record.note || 'Không có ghi chú' }}</span>
          </template>

          <template v-else-if="column.key === 'actions'">
            <div class="entity-actions">
              <a-button type="text" size="small" @click="openEditModal(record)">
                <EditOutlined />
                Sửa
              </a-button>
              <a-popconfirm
                title="Xoá kết quả này?"
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
      width="780px"
      @ok="submitForm"
      @cancel="closeModal"
    >
      <a-form ref="formRef" :model="formState" :rules="rules" layout="vertical">
        <a-divider orientation="left">
          <span class="form-section-label">Thông tin kết quả học tập</span>
        </a-divider>
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
              <div class="form-hint">Lấy danh sách học viên từ Student Service.</div>
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
            <a-form-item label="Điểm" name="score">
              <a-input-number
                v-model:value="formState.score"
                :min="0"
                :max="10"
                :step="0.25"
                :precision="2"
                style="width: 100%"
                placeholder="8.5"
              />
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="Ngày đánh giá" name="resultDate">
              <a-date-picker
                v-model:value="formState.resultDate"
                style="width: 100%"
                format="DD/MM/YYYY"
                value-format="YYYY-MM-DD"
                placeholder="Chọn ngày"
              />
            </a-form-item>
          </a-col>
          <a-col :xs="24">
            <a-form-item label="Ghi chú">
              <a-textarea
                v-model:value="formState.note"
                :rows="3"
                placeholder="Nhận xét, điểm mạnh, điểm cần cải thiện..."
              />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-modal>
  </section>
</template>
