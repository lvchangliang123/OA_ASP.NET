import { createApp } from 'vue'
import App from './App.vue'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'
import router from '@/router/index.js'


const app = createApp(App)
app.config.globalProperties.$hostURL = 'https://localhost:7004'
app.use(ElementPlus).use(router).mount('#app')
