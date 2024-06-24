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
        path:'/about',
        component:about,
    },
    {
        name:'bloghome',
        path:'/bloghome',
        component: bloghome,
    },
    {
        name:'admin',
        path:'/admin',
        component: admin,
    },
    {
        name: 'blogdetail',
        path: '/blogdetail',
        component: blogdetail,
    },
    {
        name: 'blogedit',
        path: '/blogedit',
        component: blogedit,
    },
    {
        name: 'dailyroutine',
        path: '/dailyroutine',
        component: dailyroutine,
    },
    {
        name: 'login',
        path: '/login',
        component: login,
    },
    {
        name: 'regis',
        path: '/regis',
        component: regis,
    },
    {
        path: '/',
        component: bloghome,
    }
]

export default routes