import axios from 'axios'

const tokenStorageKey = 'educenter_access_token'

export function createHttpClient(baseURL) {
  const resolvedBaseUrl = String(baseURL ?? '').trim().replace(/\/+$/, '')
  if (!resolvedBaseUrl) {
    throw new Error(
      'Missing API base URL. Please define VITE_API_BASE_URL in .env and restart Vite dev server.',
    )
  }

  const client = axios.create({
    baseURL: resolvedBaseUrl,
    timeout: 10000,
  })

  client.interceptors.request.use((config) => {
    const token = localStorage.getItem(tokenStorageKey)
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }

    return config
  })

  client.interceptors.response.use(
    (response) => response,
    async (error) => {
      throw error
    },
  )

  return client
}

export function saveAccessToken(token) {
  localStorage.setItem(tokenStorageKey, token)
}

export function clearAccessToken() {
  localStorage.removeItem(tokenStorageKey)
}
