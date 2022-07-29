import { createRouter, createWebHistory } from 'vue-router'
import LoginViewVue from "@/views/LoginView.vue"
import PlanListVue from '@/components/PlanList.vue'

const routes = [
  {
    path: "/login",
    name: 'login',
    component: LoginViewVue
  },
  {
    path: '/signup',
    name: 'signup',
    component: LoginViewVue
  },
  {
    path: '/plans',
    name: 'plans',
    component: PlanListVue
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

export default router
