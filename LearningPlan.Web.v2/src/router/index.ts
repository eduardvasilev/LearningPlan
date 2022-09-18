import { createRouter, createWebHistory } from "vue-router";
import LoginViewVue from "@/views/LoginView.vue";
import PlanListView from "@/views/PlanListView.vue";
import HomeViewVue from "@/views/HomeView.vue";
import PlanVue from "@/components/Plan.vue";

const routes = [
  {
    path: "/login",
    name: "login",
    component: LoginViewVue,
  },
  {
    path: "/signup",
    name: "signup",
    component: LoginViewVue,
  },
  {
    path: "/",
    name: "home",
    children: [
      {
        path: "/plans",
        component: PlanListView,
      },
      {
        path: "/plan/:id",
        component: PlanVue,
      }
    ],
    component: HomeViewVue,
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
