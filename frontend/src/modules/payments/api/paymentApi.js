import { API_BASE_URLS } from '@/config/apiBaseUrls'
import { createHttpClient } from '@/services/httpClient'

const client = createHttpClient(API_BASE_URLS.payment)

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

// ── Auth ──
export async function login(email, password) {
  const response = await client.post('/auth/login', { email, password })
  return unwrap(response)
}

// ── Payments ──
export async function getPayments(params = {}) {
  const response = await client.get('/payments', { params })
  return toList(response)
}

export async function getPayment(id) {
  const response = await client.get(`/payments/${id}`)
  return unwrap(response)
}

export async function createPayment(payload) {
  const response = await client.post('/payments', payload)
  return unwrap(response)
}

export async function updatePayment(id, payload) {
  const response = await client.put(`/payments/${id}`, payload)
  return unwrap(response)
}

export async function deletePayment(id) {
  const response = await client.delete(`/payments/${id}`)
  return unwrap(response)
}

export async function payInvoice(id, paymentData) {
  const response = await client.post(`/payments/${id}/pay`, paymentData)
  return unwrap(response)
}

// ── Debts ──
export async function getDebts(params = {}) {
  const response = await client.get('/debts', { params })
  return toList(response)
}

export async function sendReminder(studentId) {
  const response = await client.post(`/debts/${studentId}/reminder`)
  return unwrap(response)
}

// ── Payroll ──
export async function getPayrolls(params = {}) {
  const response = await client.get('/payrolls', { params })
  return toList(response)
}

export async function createPayroll(payload) {
  const response = await client.post('/payrolls', payload)
  return unwrap(response)
}

export async function updatePayroll(id, payload) {
  const response = await client.put(`/payrolls/${id}`, payload)
  return unwrap(response)
}

export async function deletePayroll(id) {
  const response = await client.delete(`/payrolls/${id}`)
  return unwrap(response)
}

export async function markPayrollPaid(id) {
  const response = await client.post(`/payrolls/${id}/mark-paid`)
  return unwrap(response)
}

// ── Reports ──
export async function getDashboard() {
  const response = await client.get('/reports')
  return unwrap(response)
}

export async function getRevenueReport(params = {}) {
  const response = await client.get('/reports/revenue', { params })
  return unwrap(response)
}

export async function getReportByCourse() {
  const response = await client.get('/reports/by-course')
  return unwrap(response)
}

export async function getReportByClass() {
  const response = await client.get('/reports/by-class')
  return unwrap(response)
}

// ── Users ──
export async function getUsers(params = {}) {
  const response = await client.get('/users', { params })
  return toList(response)
}

export async function createUser(payload) {
  const response = await client.post('/users', payload)
  return unwrap(response)
}

export async function updateUser(id, payload) {
  const response = await client.put(`/users/${id}`, payload)
  return unwrap(response)
}

export async function deleteUser(id) {
  const response = await client.delete(`/users/${id}`)
  return unwrap(response)
}
