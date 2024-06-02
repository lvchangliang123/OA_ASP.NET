const about = () => import("@/components/About.vue")
const admin = () => import('@/components/Admin.vue')
const blogdetail = () => import('@/components/BlogDetail.vue')
const bloghome = () => import('@/components/BlogHome.vue')
const blogedit = () => import('@/components/BlogEdit.vue')
const dailyroutine = () => import('@/components/DailyRoutine.vue')
const login = () => import('@/components/Login.vue')
const regis = () => import('@/components/Regis.vue')

const routes = [
    {
        name:'about',
        path: '/about',
        component:about,
    },
    {
        path: '/bloghome',
        component: bloghome,
    },
    {
        path: '/',
        component: bloghome,
    }
]

export default routes