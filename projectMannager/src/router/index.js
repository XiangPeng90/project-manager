import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  { path: '/', redirect: '/projects' },
  { path: '/login', name: 'Login', component: () => import('@/views/Login.vue') },
  { path: '/projects', name: 'Projects', component: () => import('@/views/ProjectList.vue') },
  { path: '/projects/:id', name: 'ProjectDetail', component: () => import('@/views/ProjectDetail.vue') },
  { path: '/workpackages/:id', name: 'WorkPackageDetail', component: () => import('@/views/WorkPackageDetail.vue') },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

router.beforeEach((to) => {
  const token = localStorage.getItem('token')
  if (to.name !== 'Login' && !token) {
    return { name: 'Login' }
  }
})

export default router
