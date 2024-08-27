import { createRouter, createWebHashHistory, createWebHistory } from 'vue-router';
import routes from '@/router/routes.js';

const router = createRouter({
    routes,
    history: createWebHistory()
})

export default router