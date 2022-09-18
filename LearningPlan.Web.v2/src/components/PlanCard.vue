<script setup lang="ts">
import type { Plan } from '@/modules/plan/classes/plan.class';
import PlanDataService from '@/modules/plan/plan.data.service'
import { type Ref, ref } from 'vue';
const props = defineProps<{ plan: Plan, isNew: boolean }>();
const plan = ref({
  ...props.plan
});
const placeholder = ref('New plan')

const isNew = ref(props.isNew);
function submitNewPlan() {
  isNew.value = false;
  plan.value.name = plan.value.name || placeholder.value;
  PlanDataService.addPlan(plan.value.name, plan.value.isTemplate)
};
</script>

<template>
  <div class="col-span-1 flex items-start flex-col gap-5 border border-gray-300 rounded-xl px-6 py-6"
    :class="{'border-dashed' : isNew}">
    <object :data="'./icon-2' + '.svg'" type="image/svg+xml" class="h-12"></object>
    <div class="flex flex-col gap-2">
      <router-link v-if="!isNew" :to="'/plan/' + plan.id"
        class="font-medium hover:underline hover:decoration-1 decoration-blue-500">
        {{
        plan.name
        }}</router-link>
      <input v-else type="text" v-model="plan.name" :placeholder="placeholder" autofocus class="form-control
        font-semibold
        text-gray-700
        bg-clip-padding
        rounded
        transition
        ease-in-out
        focus:text-gray-700 focus:outline-none" @blur="submitNewPlan()">
    </div>
  </div>
</template>
