import { createApp } from 'vue'
import App from './App.vue'
import { createPinia } from 'pinia'

import Antd from 'ant-design-vue'
import 'ant-design-vue/dist/reset.css'
import vuetify from './plugins/vuetify'
import './assets/main.css'
import './modules/students/styles/studentModule.css'

const app = createApp(App)
app.use(createPinia())
app.use(Antd)
app.use(vuetify)
app.mount('#app')
