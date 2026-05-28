import { API_BASE_URLS } from '@/config/apiBaseUrls'
import { createHttpClient } from '@/services/httpClient'

const client = createHttpClient(API_BASE_URLS.course)
const COURSE_ENDPOINT = '/courses'

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

export function extractCourseApiErrorMessage(error, fallback = 'Course API request failed.') {
  return error?.response?.data?.message || error?.apiMessage || error?.message || fallback
}

export async function getCourses(params = {}) {
  const response = await request('get', COURSE_ENDPOINT, { params })
  return toArray(extractEnvelopeData(response, 'Failed to load courses.'))
}

export async function getCourseById(id) {
  const response = await request('get', `${COURSE_ENDPOINT}/${id}`)
  return extractEnvelopeData(response, 'Failed to load course details.')
}

export async function createCourse(payload) {
  const response = await request('post', COURSE_ENDPOINT, { payload })
  return extractEnvelopeData(response, 'Failed to create course.')
}

export async function updateCourse(id, payload) {
  const response = await request('put', `${COURSE_ENDPOINT}/${id}`, { payload })
  return extractEnvelopeData(response, 'Failed to update course.')
}

export async function deleteCourse(id) {
  const response = await request('delete', `${COURSE_ENDPOINT}/${id}`)
  return extractEnvelopeData(response, 'Failed to delete course.')
}
