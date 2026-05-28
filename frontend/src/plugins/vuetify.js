import 'vuetify/styles'
import '@mdi/font/css/materialdesignicons.css'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

export default createVuetify({
  components,
  directives,
  theme: {
    defaultTheme: 'n3Theme',
    themes: {
      n3Theme: {
        dark: false,
        colors: {
          primary: '#2E7D32',
          secondary: '#424242',
          success: '#388E3C',
          warning: '#F57C00',
          error: '#D32F2F',
          info: '#1976D2',
          background: '#FAFAFA',
          surface: '#FFFFFF',
        },
      },
    },
  },
  defaults: {
    VCard: { rounded: 'lg', elevation: 1 },
    VBtn: { rounded: 'lg' },
    VTextField: { rounded: 'lg', variant: 'outlined', density: 'comfortable' },
    VSelect: { rounded: 'lg', variant: 'outlined', density: 'comfortable' },
  },
})
