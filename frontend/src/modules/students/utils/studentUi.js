import dayjs from 'dayjs'

export const STUDENT_STATUS_OPTIONS = [
  { label: 'Active', value: 'Active' },
  { label: 'Inactive', value: 'Inactive' },
  { label: 'Suspended', value: 'Suspended' },
]

export const ENROLLMENT_STATUS_OPTIONS = [
  { label: 'Enrolled', value: 'Enrolled' },
  { label: 'Pending', value: 'Pending' },
  { label: 'Completed', value: 'Completed' },
  { label: 'Cancelled', value: 'Cancelled' },
]

export function formatDate(value) {
  if (!value) {
    return '-'
  }

  return dayjs(value).format('DD/MM/YYYY')
}

export function formatDateTime(value) {
  if (!value) {
    return '-'
  }

  return dayjs(value).format('DD/MM/YYYY HH:mm')
}

export function toDateValue(value) {
  if (!value) {
    return null
  }

  return dayjs(value).format('YYYY-MM-DD')
}

export function initials(name) {
  if (!name) {
    return '--'
  }

  return name
    .split(/\s+/)
    .filter(Boolean)
    .slice(0, 2)
    .map((part) => part[0])
    .join('')
    .toUpperCase()
}

export function studentStatusColor(status) {
  switch (status) {
    case 'Active':
      return 'green'
    case 'Inactive':
      return 'volcano'
    case 'Suspended':
      return 'gold'
    default:
      return 'blue'
  }
}

export function enrollmentStatusColor(status) {
  switch (status) {
    case 'Enrolled':
      return 'blue'
    case 'Pending':
      return 'gold'
    case 'Completed':
      return 'green'
    case 'Cancelled':
      return 'red'
    default:
      return 'default'
  }
}

export function attendanceStateColor(isPresent) {
  return isPresent ? 'green' : 'volcano'
}

export function attendanceStateLabel(isPresent) {
  return isPresent ? 'Present' : 'Absent'
}

export function studentLabel(student) {
  if (!student) {
    return '-'
  }

  return `${student.fullName} (#${student.id})`
}

export function formatCurrency(value) {
  return new Intl.NumberFormat('vi-VN').format(value || 0)
}

export function formatScore(value) {
  if (value === null || value === undefined || value === '') {
    return '-'
  }

  const numeric = Number(value)
  if (Number.isNaN(numeric)) {
    return '-'
  }

  return numeric.toFixed(2).replace(/\.00$/, '').replace(/(\.\d*[1-9])0+$/, '$1')
}

export function resultOutcomeLabel(score) {
  return Number(score) >= 5 ? 'Passed' : 'Failed'
}

export function resultOutcomeColor(score) {
  const numeric = Number(score)

  if (numeric >= 8) {
    return 'green'
  }

  if (numeric >= 5) {
    return 'gold'
  }

  return 'red'
}
