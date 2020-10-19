import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";

import PlanComponent from "@/components/Plan.vue"
import PlanList from "@/components/PlanList.vue"

Vue.use(VueRouter);

const routes: Array<RouteConfig>  = [
  {
    path: "/plan/:id",
    name: "plan",
    component: PlanComponent
    },
    {
    path: "/",
    name: "plans",
    component: PlanList
    }
];

export const  router = new VueRouter({
    mode: "history",
    base: process.env.BASE_URL,
    routes
});
