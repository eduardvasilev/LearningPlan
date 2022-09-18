<script setup lang="ts">
import { ref, type Ref } from "vue";
import PlanCardVue from "@/components/PlanCard.vue";
import { PlanListGetterController } from "@/modules/plan/plan-list.controller"

const controllerInstance = new PlanListGetterController()
const planList = ref(await controllerInstance.plans);
const errorMessage = controllerInstance.error;
const isNew = ref(false)
const addNew = () => {
  isNew.value = true
  planList.value.push({
    id: "string", name: "New plan", isTemplate: false,
    planAreas: [],
    userId: ""
  })
}
</script>

<template>

  <div class="grid grid-flow-row grid-cols-2 gap-10">
    <PlanCardVue v-for="(plan, index) in planList" :plan="plan" :isNew="isNew" />
  </div>
  {{errorMessage}}
  <div @click="addNew" class="flex cursor-pointer flex-row items-center gap-2 w-fit">
    <object data="./plus.svg" type="image/svg+xml"></object>
    <p class="font-semibold">Add new plan</p>
  </div>

</template>
