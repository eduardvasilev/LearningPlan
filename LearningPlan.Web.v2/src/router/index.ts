import { createRouter, createWebHistory } from 'vue-router'
import PlanList from "@/components/PlanList.vue"
import WelcomeItem from '@/components/WelcomeItem.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/plans",
      name: 'plans',
      component: PlanList
    },
    {
      path: "/welcome",
      name: 'welcome',
      component: WelcomeItem
    },
  ]
})

export default router
