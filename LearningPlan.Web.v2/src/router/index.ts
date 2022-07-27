import { createRouter, createWebHistory } from 'vue-router'
import LoginViewVue from "@/views/LoginView.vue"

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
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

export default router
