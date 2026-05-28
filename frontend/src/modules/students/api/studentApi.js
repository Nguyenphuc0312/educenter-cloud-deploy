import { API_BASE_URLS } from '@/config/apiBaseUrls'
import { createHttpClient } from '@/services/httpClient'

const client = createHttpClient(API_BASE_URLS.student)

function unwrap(response) {
  const data = response?.data
  if (data && typeof data === 'object' && 'data' in data) {
    return data.data
  }
  return data ?? null
}

function toList(response) {
  const data = unwrap(response)
  return Array.isArray(data) ? data : []
}

function getErrorMessage(error, fallback) {
  return error?.response?.data?.message || error?.message || fallback
}

export function extractApiErrorMessage(error, fallback = 'Request failed.') {
  return getErrorMessage(error, fallback)
}

export async function getStudents() {
  const response = await client.get('/students')
  return toList(response)
}

export async function createStudent(payload) {
  const response = await client.post('/students', payload)
  return unwrap(response)
}

export async function updateStudent(id, payload) {
  const response = await client.put(`/students/${id}`, payload)
  return unwrap(response)
}

export async function deleteStudent(id) {
  const response = await client.delete(`/students/${id}`)
  return unwrap(response)
}

export async function getEnrollments() {
  const response = await client.get('/enrollments')
  return toList(response)
}

export async function createEnrollment(payload) {
  const response = await client.post('/enrollments', payload)
  return unwrap(response)
}

export async function updateEnrollment(id, payload) {
  const response = await client.put(`/enrollments/${id}`, payload)
  return unwrap(response)
}

export async function deleteEnrollment(id) {
  const response = await client.delete(`/enrollments/${id}`)
  return unwrap(response)
}

export async function getAttendances() {
  const response = await client.get('/attendances')
  return toList(response)
}

export async function createAttendance(payload) {
  const response = await client.post('/attendances', payload)
  return unwrap(response)
}

export async function updateAttendance(id, payload) {
  const response = await client.put(`/attendances/${id}`, payload)
  return unwrap(response)
}

export async function deleteAttendance(id) {
  const response = await client.delete(`/attendances/${id}`)
  return unwrap(response)
}

export async function getResults() {
  const response = await client.get('/results')
  return toList(response)
}

export async function createResult(payload) {
  const response = await client.post('/results', payload)
  return unwrap(response)
}

export async function updateResult(id, payload) {
  const response = await client.put(`/results/${id}`, payload)
  return unwrap(response)
}

export async function deleteResult(id) {
  const response = await client.delete(`/results/${id}`)
  return unwrap(response)
}
