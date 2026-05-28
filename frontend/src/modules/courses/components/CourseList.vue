<script setup>
import { computed, h, onMounted, ref } from 'vue'
import { message } from 'ant-design-vue'
import { ReloadOutlined, SearchOutlined } from '@ant-design/icons-vue'
import { extractCourseApiErrorMessage, getCourses } from '../api/courseApi'

const loading = ref(false)
const searchText = ref('')
const courses = ref([])

const columns = [
  { title: 'ID', key: 'id', width: 96 },
  { title: 'Khoa hoc', key: 'course', ellipsis: true },
  { title: 'Ma khoa hoc', key: 'code', width: 160, ellipsis: true },
  { title: 'TeacherId', key: 'teacherId', width: 120 },
  { title: 'Mo ta', key: 'description', ellipsis: true },
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

function toNumber(value) {
  const numericValue = Number(value)
  return Number.isFinite(numericValue) ? numericValue : null
}

function normalizeCourse(record, index) {
  const rawId = firstNonEmptyValue(record, ['id', 'courseId', 'Id', 'CourseId'])
  const numericId = toNumber(rawId)
  const idText = toText(rawId, `row-${index + 1}`)
  const name = toText(
    firstNonEmptyValue(record, ['name', 'courseName', 'title', 'Name', 'CourseName']),
    'Unnamed course',
  )
  const code = toText(firstNonEmptyValue(record, ['code', 'courseCode', 'Code', 'CourseCode']), '-')
  const teacherId = toText(
    firstNonEmptyValue(record, ['teacherId', 'TeacherId', 'instructorId', 'InstructorId']),
    '-',
  )
  const description = toText(
    firstNonEmptyValue(record, ['description', 'detail', 'Description', 'Detail']),
    '-',
  )

  return {
    key: idText,
    id: numericId ?? idText,
    idText,
    name,
    code,
    teacherId,
    description,
  }
}

const normalizedCourses = computed(() =>
  courses.value.map((record, index) => normalizeCourse(record, index)),
)

const filteredCourses = computed(() => {
  const keyword = searchText.value.trim().toLowerCase()
  const source = [...normalizedCourses.value].sort((left, right) =>
    String(right.id).localeCompare(String(left.id), undefined, { numeric: true }),
  )

  if (!keyword) {
    return source
  }

  return source.filter((item) => {
    const haystack = [item.idText, item.name, item.code, item.teacherId, item.description]
      .join(' ')
      .toLowerCase()
    return haystack.includes(keyword)
  })
})

const totalCourses = computed(() => normalizedCourses.value.length)
const visibleCourses = computed(() => filteredCourses.value.length)

async function loadCourses() {
  loading.value = true
  try {
    courses.value = await getCourses()
  } catch (error) {
    message.error(extractCourseApiErrorMessage(error, 'Khong tai duoc danh sach khoa hoc.'))
  } finally {
    loading.value = false
  }
}

onMounted(loadCourses)

defineExpose({
  reload: loadCourses,
})
</script>

<template>
  <a-card class="module-table-card" :bordered="false">
    <div class="module-toolbar">
      <a-input-search
        v-model:value="searchText"
        class="module-search"
        :prefix="h(SearchOutlined)"
        placeholder="Tim theo ten, ma khoa hoc, TeacherId..."
        allow-clear
      />

      <div class="course-list-toolbar-right">
        <a-tag color="blue">Tong: {{ totalCourses }}</a-tag>
        <a-tag color="cyan">Dang hien thi: {{ visibleCourses }}</a-tag>
        <a-button :icon="h(ReloadOutlined)" :loading="loading" @click="loadCourses">
          Tai lai
        </a-button>
      </div>
    </div>

    <a-table
      :columns="columns"
      :data-source="filteredCourses"
      :loading="loading"
      :pagination="{ pageSize: 8, showSizeChanger: false, hideOnSinglePage: true }"
      :scroll="{ x: 980 }"
      row-key="key"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'id'">
          <a-tag color="geekblue">#{{ record.idText }}</a-tag>
        </template>

        <template v-else-if="column.key === 'course'">
          <div class="entity-cell__meta">
            <strong>{{ record.name }}</strong>
            <span>{{ record.description === '-' ? 'No description' : record.description }}</span>
          </div>
        </template>

        <template v-else-if="column.key === 'code'">
          <a-tag color="default">{{ record.code }}</a-tag>
        </template>

        <template v-else-if="column.key === 'teacherId'">
          <span class="teacher-id">{{ record.teacherId }}</span>
        </template>

        <template v-else-if="column.key === 'description'">
          <span class="entity-note">{{ record.description }}</span>
        </template>
      </template>
    </a-table>
  </a-card>
</template>

<style scoped>
.course-list-toolbar-right {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  align-items: center;
}

.teacher-id {
  font-weight: 600;
}
</style>
