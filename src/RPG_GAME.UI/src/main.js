import { createApp } from 'vue'
import App from './App.vue'
import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap"
import router from './router'
import Notifications from '@kyvg/vue3-notification'

createApp(App)
            .use(router)
            .use(Notifications)
            .mount('#app')
