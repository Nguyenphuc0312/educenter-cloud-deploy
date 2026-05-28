import { API_BASE_URLS } from '@/config/apiBaseUrls'
import { createHttpClient } from '@/services/httpClient'

const client = createHttpClient(API_BASE_URLS.course)
const SCHEDULE_ENDPOINT = '/schedules'

function normalizeApiEnvelope(response, fallbackMessage = 'Request completed.') {
  const payload = response?.data

  if (payload && typeof payload === 'object' && 'success' in payload) {
    return {
      success: Boolean(payload.success),
      message: payload.message || fallbackMessage,
      data: payload.data ?? null,
    }
  }

  return {
    success: true,
    message: fallbackMessage,
    data: payload ?? null,
  }
}

function throwIfFailed(envelope, fallbackMessage = 'Request failed.') {
  if (envelope.success) {
    return envelope.data
  }

  const error = new Error(envelope.message || fallbackMessage)
  error.apiMessage = envelope.message || fallbackMessage
  throw error
}

function toArray(value) {
  return Array.isArray(value) ? value : []
}

function cleanQueryParams(params) {
  if (!params || typeof params !== 'object') {
    return undefined
  }

  return Object.fromEntries(
    Object.entries(params).filter(([, value]) => value !== undefined && value !== null && value !== ''),
  )
}

function extractEnvelopeData(response, fallbackMessage) {
  const envelope = normalizeApiEnvelope(response, fallbackMessage)
  return throwIfFailed(envelope, fallbackMessage)
}

function buildRequestConfig({ params, payload } = {}) {
  const config = {}

  const cleanedParams = cleanQueryParams(params)
  if (cleanedParams && Object.keys(cleanedParams).length > 0) {
    config.params = cleanedParams
  }

  if (payload !== undefined) {
    config.data = payload
  }

  return config
}

function request(method, url, options) {
  return client.request({
    method,
    url,
    ...buildRequestConfig(options),
  })
}

export function extractScheduleApiErrorMessage(error, fallback = 'Schedule API request failed.') {
  return error?.response?.data?.message || error?.apiMessage || error?.message || fallback
}

export async function getSchedules(params = {}) {
  const response = await request('get', SCHEDULE_ENDPOINT, { params })
  return toArray(extractEnvelopeData(response, 'Failed to load schedules.'))
}

export async function getScheduleById(id) {
  const response = await request('get', `${SCHEDULE_ENDPOINT}/${id}`)
  return extractEnvelopeData(response, 'Failed to load schedule details.')
}

export async function createSchedule(payload) {
  const response = await request('post', SCHEDULE_ENDPOINT, { payload })
  return extractEnvelopeData(response, 'Failed to create schedule.')
}

export async function updateSchedule(id, payload) {
  const response = await request('put', `${SCHEDULE_ENDPOINT}/${id}`, { payload })
  return extractEnvelopeData(response, 'Failed to update schedule.')
}

export async function deleteSchedule(id) {
  const response = await request('delete', `${SCHEDULE_ENDPOINT}/${id}`)
  return extractEnvelopeData(response, 'Failed to delete schedule.')
}
