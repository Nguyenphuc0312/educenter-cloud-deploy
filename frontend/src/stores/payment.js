import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import {
  getPayments,
  getPayment,
  createPayment,
  payInvoice,
  getDebts,
  sendReminder,
  getDashboard,
  getRevenueReport,
  getReportByCourse,
  getReportByClass,
  getPayrolls,
  markPayrollPaid,
  getUsers,
} from '@/modules/payments/api/paymentApi'

export const usePaymentStore = defineStore('payment', () => {
  const invoices = ref([])
  const debts = ref([])
  const payrolls = ref([])
  const dashboard = ref(null)
  const users = ref([])
  const loading = ref(false)
  const error = ref(null)

  const totalRevenue = computed(() =>
    invoices.value.filter((i) => i.status === 'Paid').reduce((s, i) => s + (i.paidAmount || 0), 0)
  )
  const totalDebt = computed(() =>
    invoices.value.filter((i) => i.status !== 'Paid').reduce((s, i) => s + ((i.amount || 0) - (i.paidAmount || 0)), 0)
  )

  async function safeFetch(fn) {
    loading.value = true
    error.value = null
    try { return await fn() }
    catch (e) { error.value = e.message; return null }
    finally { loading.value = false }
  }

  async function fetchInvoices(params = {}) {
    const data = await safeFetch(() => getPayments(params))
    if (data) invoices.value = data
  }

  async function fetchInvoice(id) {
    return await safeFetch(() => getPayment(id))
  }

  async function createPaymentAction(payload) {
    return await api(createPayment, payload)
  }

  async function payInvoiceAction(id, paymentData) {
    const data = await api(payInvoice, id, paymentData)
    if (data) {
      const inv = invoices.value.find((i) => i.id === id)
      if (inv) { inv.status = 'Paid'; inv.paidAmount = inv.amount }
    }
    return data
  }

  async function fetchDebts(params = {}) {
    const data = await safeFetch(() => getDebts(params))
    if (data) debts.value = data
  }

  async function sendReminderAction(studentId) {
    return await api(sendReminder, studentId)
  }

  async function fetchDashboard() {
    const data = await safeFetch(() => getDashboard())
    if (data) dashboard.value = data
  }

  async function fetchRevenueReport(params = {}) {
    return await safeFetch(() => getRevenueReport(params))
  }

  async function fetchByCourse() {
    return await safeFetch(() => getReportByCourse())
  }

  async function fetchByClass() {
    return await safeFetch(() => getReportByClass())
  }

  async function fetchPayrolls(params = {}) {
    const data = await safeFetch(() => getPayrolls(params))
    if (data) payrolls.value = data
  }

  async function markPayrollPaidAction(id) {
    const data = await api(markPayrollPaid, id)
    if (data) {
      const p = payrolls.value.find((x) => x.id === id)
      if (p) { p.status = 'Paid'; p.paidDate = new Date().toISOString() }
    }
    return data
  }

  async function fetchUsers(params = {}) {
    const data = await safeFetch(() => getUsers(params))
    if (data) users.value = data
  }

  async function api(fn, ...args) {
    loading.value = true
    error.value = null
    try { return await fn(...args) }
    catch (e) { error.value = e.message; return null }
    finally { loading.value = false }
  }

  return {
    invoices, debts, payrolls, dashboard, users, loading, error,
    totalRevenue, totalDebt,
    fetchInvoices, fetchInvoice, createPayment: createPaymentAction, payInvoice: payInvoiceAction,
    fetchDebts, sendReminder: sendReminderAction,
    fetchDashboard, fetchRevenueReport, fetchByCourse, fetchByClass,
    fetchPayrolls, markPayrollPaid: markPayrollPaidAction,
    fetchUsers,
  }
})
