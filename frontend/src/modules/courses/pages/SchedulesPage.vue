<script setup>
import { computed, h, onMounted, reactive, ref } from 'vue'
import { message } from 'ant-design-vue'
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
  ReloadOutlined,
  SearchOutlined,
} from '@ant-design/icons-vue'
import {
  createSchedule,
  deleteSchedule,
  extractScheduleApiErrorMessage,
  getSchedules,
  updateSchedule,
} from '../api/scheduleApi'
import { getClasses } from '../api/classApi'

const formRef = ref()
const loading = ref(false)
const saving = ref(false)
const modalOpen = ref(false)
const editingId = ref(null)
const searchText = ref('')
const schedules = ref([])
const classes = ref([])

const SCHEDULE_STATUS_OPTIONS = [
  { label: 'Planned', value: 'Planned' },
  { label: 'Active', value: 'Active' },
  { label: 'Completed', value: 'Completed' },
  { label: 'Cancelled', value: 'Cancelled' },
]

function createScheduleForm() {
  return {
    classId: null,
    scheduleDate: null,
    startTime: null,
    endTime: null,
    room: '',
    status: 'Planned',
    note: '',
  }
}

const formState = reactive(createScheduleForm())

const rules = {
  classId: [
    {
      validator: async (_rule, value) => {
        if (value === null || value === undefined || value === '') {
          throw new Error('Vui long nhap ClassId.')
        }
        if (!Number.isInteger(Number(value)) || Number(value) <= 0) {
          throw new Error('ClassId phai la so nguyen duong.')
        }
      },
      trigger: 'change',
    },
  ],
  scheduleDate: [{ required: true, message: 'Vui long chon ngay hoc.', trigger: 'change' }],
  startTime: [{ required: true, message: 'Vui long chon gio bat dau.', trigger: 'change' }],
  endTime: [
    { required: true, message: 'Vui long chon gio ket thuc.', trigger: 'change' },
    {
      validator: async (_rule, value) => {
        if (!value || !formState.startTime) {
          return
        }

        const start = toMinutes(formState.startTime)
        const end = toMinutes(value)
        if (Number.isNaN(start) || Number.isNaN(end)) {
          return
        }

        if (end <= start) {
          throw new Error('Gio ket thuc phai lon hon gio bat dau.')
        }
      },
      trigger: 'change',
    },
  ],
}

const columns = [
  { title: 'ID', key: 'id', width: 90 },
  { title: 'Lop hoc', key: 'class', width: 210, ellipsis: true },
  { title: 'Ngay hoc', key: 'date', width: 130 },
  { title: 'Thoi gian', key: 'time', width: 150 },
  { title: 'Phong', key: 'room', width: 130, ellipsis: true },
  { title: 'Trang thai', key: 'status', width: 130 },
  { title: 'Ghi chu', key: 'note', ellipsis: true },
  { title: 'Thao tac', key: 'actions', width: 170, fixed: 'right' },
]

function firstNonEmptyValue(record, keys) {
  for (const key of keys) {
    const value = record?.[key]
    if (value !== undefined && value !== null && value !== '') {
      return value
    }
  }
  return null
}

function toText(value, fallback = '') {
  if (value === undefined || value === null) {
    return fallback
  }
  const text = String(value).trim()
  return text || fallback
}

function normalizeDateOnly(value) {
  if (!value) {
    return null
  }

  const raw = String(value).trim()
  if (!raw) {
    return null
  }

  if (raw.includes('T')) {
    return raw.split('T')[0]
  }

  const match = raw.match(/^(\d{4}-\d{2}-\d{2})/)
  if (match?.[1]) {
    return match[1]
  }

  const parsed = new Date(raw)
  if (!Number.isNaN(parsed.getTime())) {
    return parsed.toISOString().split('T')[0]
  }

  return null
}

function normalizeTimeOnly(value) {
  if (!value) {
    return null
  }

  const raw = String(value).trim()
  if (!raw) {
    return null
  }

  const match = raw.match(/(\d{1,2}):(\d{2})/)
  if (!match) {
    return null
  }

  const hours = match[1].padStart(2, '0')
  const minutes = match[2]
  return `${hours}:${minutes}`
}

function toMinutes(value) {
  const normalized = normalizeTimeOnly(value)
  if (!normalized) {
    return Number.NaN
  }

  const [hours, minutes] = normalized.split(':').map(Number)
  return hours * 60 + minutes
}

function formatDateText(value) {
  const normalized = normalizeDateOnly(value)
  if (!normalized) {
    return '-'
  }

  const date = new Date(`${normalized}T00:00:00`)
  if (Number.isNaN(date.getTime())) {
    return normalized
  }

  return date.toLocaleDateString('vi-VN')
}

function formatTimeText(start, end) {
  const normalizedStart = normalizeTimeOnly(start)
  const normalizedEnd = normalizeTimeOnly(end)
  if (normalizedStart && normalizedEnd) {
    return `${normalizedStart} - ${normalizedEnd}`
  }
  if (normalizedStart) {
    return normalizedStart
  }
  if (normalizedEnd) {
    return normalizedEnd
  }
  return '-'
}

function normalizeClass(record, index) {
  const rawId = firstNonEmptyValue(record, ['id', 'classId', 'Id', 'ClassId'])
  const id = Number(rawId)
  const safeId = Number.isFinite(id) ? id : null
  const name = toText(
    firstNonEmptyValue(record, ['className', 'name', 'code', 'ClassName', 'Name', 'Code']),
    safeId ? `Class #${safeId}` : `Class row-${index + 1}`,
  )

  return {
    id: safeId,
    name,
  }
}

const classMap = computed(() => {
  const entries = classes.value
    .map((item, index) => normalizeClass(item, index))
    .filter((item) => item.id !== null)
    .map((item) => [item.id, item.name])

  return new Map(entries)
})

function normalizeSchedule(record, index) {
  const rawId = firstNonEmptyValue(record, ['id', 'scheduleId', 'Id', 'ScheduleId'])
  const scheduleId = Number(rawId)
  const safeId = Number.isFinite(scheduleId) ? scheduleId : `row-${index + 1}`

  const classIdRaw = firstNonEmptyValue(record, ['classId', 'ClassId'])
  const classIdNumber = Number(classIdRaw)
  const classId = Number.isFinite(classIdNumber) ? classIdNumber : null
  const className =
    classMap.value.get(classId) ||
    toText(firstNonEmptyValue(record, ['className', 'ClassName']), classId ? `#${classId}` : '-')

  const scheduleDate = normalizeDateOnly(
    firstNonEmptyValue(record, ['scheduleDate', 'sessionDate', 'date', 'ScheduleDate', 'SessionDate', 'Date']),
  )
  const startTime = normalizeTimeOnly(
    firstNonEmptyValue(record, ['startTime', 'startAt', 'StartTime', 'StartAt']),
  )
  const endTime = normalizeTimeOnly(firstNonEmptyValue(record, ['endTime', 'endAt', 'EndTime', 'EndAt']))

  return {
    key: String(safeId),
    id: safeId,
    classId,
    className,
    scheduleDate,
    startTime,
    endTime,
    room: toText(firstNonEmptyValue(record, ['room', 'classroom', 'Room', 'Classroom']), '-'),
    status: toText(firstNonEmptyValue(record, ['status', 'Status']), 'Planned'),
    note: toText(firstNonEmptyValue(record, ['note', 'description', 'Note', 'Description']), '-'),
  }
}

const normalizedSchedules = computed(() =>
  schedules.value.map((record, index) => normalizeSchedule(record, index)),
)

const filteredSchedules = computed(() => {
  const keyword = searchText.value.trim().toLowerCase()
  const source = [...normalizedSchedules.value].sort((left, right) => {
    const leftTime = `${left.scheduleDate || ''} ${left.startTime || ''}`.trim()
    const rightTime = `${right.scheduleDate || ''} ${right.startTime || ''}`.trim()
    return rightTime.localeCompare(leftTime)
  })

  if (!keyword) {
    return source
  }

  return source.filter((item) => {
    const haystack = [
      item.id,
      item.classId,
      item.className,
      item.scheduleDate,
      item.startTime,
      item.endTime,
      item.room,
      item.status,
      item.note,
    ]
      .map((value) => toText(value))
      .join(' ')
      .toLowerCase()

    return haystack.includes(keyword)
  })
})

const totalSchedules = computed(() => normalizedSchedules.value.length)
const visibleSchedules = computed(() => filteredSchedules.value.length)
const modalTitle = computed(() => (editingId.value ? 'Cap nhat lich hoc' : 'Them lich hoc moi'))

function resetForm() {
  Object.assign(formState, createScheduleForm())
}

function openCreateModal() {
  editingId.value = null
  resetForm()
  modalOpen.value = true
}

function openEditModal(record) {
  editingId.value = record.id
  Object.assign(formState, {
    classId: record.classId,
    scheduleDate: record.scheduleDate,
    startTime: record.startTime,
    endTime: record.endTime,
    room: record.room === '-' ? '' : record.room,
    status: record.status || 'Planned',
    note: record.note === '-' ? '' : record.note,
  })
  modalOpen.value = true
}

function closeModal() {
  modalOpen.value = false
  editingId.value = null
  resetForm()
  formRef.value?.clearValidate?.()
}

function normalizeTimeForPayload(value) {
  const normalized = normalizeTimeOnly(value)
  return normalized ? `${normalized}:00` : null
}

function buildPayload() {
  return {
    classId: Number(formState.classId),
    scheduleDate: formState.scheduleDate,
    startTime: normalizeTimeForPayload(formState.startTime),
    endTime: normalizeTimeForPayload(formState.endTime),
    room: toText(formState.room, '') || null,
    status: toText(formState.status, 'Planned'),
    note: toText(formState.note, '') || null,
  }
}

async function loadSchedules() {
  loading.value = true
  try {
    schedules.value = await getSchedules()
  } catch (error) {
    message.error(extractScheduleApiErrorMessage(error, 'Khong tai duoc danh sach lich hoc.'))
  } finally {
    loading.value = false
  }
}

async function loadClasses() {
  try {
    classes.value = await getClasses()
  } catch {
    classes.value = []
  }
}

async function submitForm() {
  try {
    await formRef.value?.validate()
  } catch {
    return
  }

  saving.value = true
  try {
    const payload = buildPayload()
    if (editingId.value) {
      await updateSchedule(editingId.value, payload)
      message.success('Da cap nhat lich hoc.')
    } else {
      await createSchedule(payload)
      message.success('Da them lich hoc moi.')
    }

    closeModal()
    await loadSchedules()
  } catch (error) {
    message.error(
      extractScheduleApiErrorMessage(
        error,
        editingId.value ? 'Khong cap nhat duoc lich hoc.' : 'Khong tao duoc lich hoc.',
      ),
    )
  } finally {
    saving.value = false
  }
}

async function handleDelete(record) {
  try {
    await deleteSchedule(record.id)
    message.success('Da xoa lich hoc.')
    await loadSchedules()
  } catch (error) {
    message.error(extractScheduleApiErrorMessage(error, 'Khong xoa duoc lich hoc.'))
  }
}

onMounted(async () => {
  await Promise.all([loadSchedules(), loadClasses()])
})
</script>

<template>
  <section class="student-module">
    <a-card class="module-hero" :bordered="false">
      <div class="module-hero__body">
        <p class="eyebrow">Course & Schedule Service</p>
        <h2>Schedules</h2>
        <p>Quan ly lich hoc theo tung lop: xem danh sach, them moi, chinh sua va xoa.</p>
      </div>

      <div class="module-hero__actions">
        <a-button :icon="h(ReloadOutlined)" :loading="loading" @click="loadSchedules">
          Tai lai
        </a-button>
        <a-button type="primary" :icon="h(PlusOutlined)" @click="openCreateModal">
          Them lich hoc
        </a-button>
      </div>
    </a-card>

    <a-card class="module-table-card" :bordered="false">
      <div class="module-toolbar">
        <a-input-search
          v-model:value="searchText"
          class="module-search"
          :prefix="h(SearchOutlined)"
          placeholder="Tim theo lop hoc, ngay hoc, phong, trang thai..."
          allow-clear
        />

        <div class="schedule-toolbar-right">
          <a-tag color="blue">Tong: {{ totalSchedules }}</a-tag>
          <a-tag color="cyan">Dang hien thi: {{ visibleSchedules }}</a-tag>
        </div>
      </div>

      <a-table
        :columns="columns"
        :data-source="filteredSchedules"
        :loading="loading"
        :pagination="{ pageSize: 8, showSizeChanger: false, hideOnSinglePage: true }"
        :scroll="{ x: 1180 }"
        row-key="key"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'id'">
            <a-tag color="geekblue">#{{ record.id }}</a-tag>
          </template>

          <template v-else-if="column.key === 'class'">
            <div class="entity-cell__meta">
              <strong>{{ record.className }}</strong>
              <span>ClassId: {{ record.classId ?? '-' }}</span>
            </div>
          </template>

          <template v-else-if="column.key === 'date'">
            {{ formatDateText(record.scheduleDate) }}
          </template>

          <template v-else-if="column.key === 'time'">
            {{ formatTimeText(record.startTime, record.endTime) }}
          </template>

          <template v-else-if="column.key === 'room'">
            <span class="entity-note">{{ record.room }}</span>
          </template>

          <template v-else-if="column.key === 'status'">
            <a-tag :color="record.status === 'Cancelled' ? 'red' : record.status === 'Completed' ? 'green' : 'blue'">
              {{ record.status }}
            </a-tag>
          </template>

          <template v-else-if="column.key === 'note'">
            <span class="entity-note">{{ record.note }}</span>
          </template>

          <template v-else-if="column.key === 'actions'">
            <div class="entity-actions">
              <a-button type="link" @click="openEditModal(record)">
                <EditOutlined />
                Sua
              </a-button>
              <a-popconfirm
                title="Xoa lich hoc nay?"
                ok-text="Xoa"
                cancel-text="Huy"
                @confirm="() => handleDelete(record)"
              >
                <a-button type="link" danger>
                  <DeleteOutlined />
                  Xoa
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
      ok-text="Luu"
      cancel-text="Huy"
      destroy-on-close
      width="860px"
      @ok="submitForm"
      @cancel="closeModal"
    >
      <a-form ref="formRef" :model="formState" :rules="rules" layout="vertical">
        <a-row :gutter="16">
          <a-col :xs="24" :md="12">
            <a-form-item label="ClassId" name="classId">
              <a-input-number
                v-model:value="formState.classId"
                :min="1"
                :precision="0"
                :step="1"
                style="width: 100%"
                placeholder="Nhap ClassId"
              />
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="Ngay hoc" name="scheduleDate">
              <a-date-picker
                v-model:value="formState.scheduleDate"
                style="width: 100%"
                value-format="YYYY-MM-DD"
                format="DD/MM/YYYY"
                placeholder="Chon ngay hoc"
              />
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="Gio bat dau" name="startTime">
              <a-time-picker
                v-model:value="formState.startTime"
                style="width: 100%"
                value-format="HH:mm"
                format="HH:mm"
                placeholder="Chon gio bat dau"
              />
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="Gio ket thuc" name="endTime">
              <a-time-picker
                v-model:value="formState.endTime"
                style="width: 100%"
                value-format="HH:mm"
                format="HH:mm"
                placeholder="Chon gio ket thuc"
              />
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="Phong hoc">
              <a-input v-model:value="formState.room" placeholder="VD: B-302" />
            </a-form-item>
          </a-col>
          <a-col :xs="24" :md="12">
            <a-form-item label="Trang thai">
              <a-select v-model:value="formState.status" placeholder="Chon trang thai">
                <a-select-option
                  v-for="option in SCHEDULE_STATUS_OPTIONS"
                  :key="option.value"
                  :value="option.value"
                >
                  {{ option.label }}
                </a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :xs="24">
            <a-form-item label="Ghi chu">
              <a-textarea
                v-model:value="formState.note"
                :rows="3"
                placeholder="Ghi chu them cho buoi hoc (neu co)"
              />
            </a-form-item>
          </a-col>
        </a-row>

        <div v-if="classes.length > 0" class="form-hint">
          Lop hoc hien co: {{ classes.length }}. Ban co the nhap ClassId theo du lieu cua service Classes.
        </div>
      </a-form>
    </a-modal>
  </section>
</template>

<style scoped>
.schedule-toolbar-right {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}
</style>
