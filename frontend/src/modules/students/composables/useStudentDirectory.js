import { computed, ref } from 'vue'
import { getStudents } from '../api/studentApi'
import { studentLabel } from '../utils/studentUi'

export function useStudentDirectory() {
  const students = ref([])
  const studentLoading = ref(false)

  const studentMap = computed(() => {
    return new Map(students.value.map((student) => [student.id, student]))
  })

  const studentOptions = computed(() => {
    return students.value.map((student) => ({
      label: studentLabel(student),
      value: student.id,
    }))
  })

  const getStudentName = (studentId) => {
    const student = studentMap.value.get(studentId)
    return student ? student.fullName : `#${studentId}`
  }

  async function loadStudents() {
    studentLoading.value = true
    try {
      students.value = await getStudents()
      return students.value
    } catch (error) {
      students.value = []
      console.error('Failed to load student directory.', error)
      return []
    } finally {
      studentLoading.value = false
    }
  }

  return {
    getStudentName,
    loadStudents,
    studentLoading,
    studentMap,
    studentOptions,
    students,
  }
}
