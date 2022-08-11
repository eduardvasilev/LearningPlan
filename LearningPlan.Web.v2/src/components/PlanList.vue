<script setup lang="ts">
import { ref } from "vue";
import { usePlanList } from "@/models/usePlanList";

const list = usePlanList();
const isLoading = ref(list.planList.isAddingNew);
</script>

<template>
  <section class="px-5 container cols-6 mx-auto my-28">
    <div class="grid grid-flow-row grid-cols-2 col-start-2 col-end-5 gap-5">
      <h1 class="text-3xl mb-5 col-span-2">Your learning plans</h1>
      <div v-for="(plan, index) in list.planList.plans"
        class="flex items-start flex-col gap-5 border border-gray-300 rounded-xl px-6 py-6">
        <object :data="'./icon-' + (index + 1) + '.svg'" type="image/svg+xml" class="h-12"></object>
        <div class="flex flex-col gap-2">
          <router-link :to="'/plan/' + plan.id"
            class="font-medium hover:underline hover:decoration-1 decoration-blue-500">{{
                plan.name
            }}</router-link>
        </div>
      </div>
      <button class="flex flex-row items-center gap-2">
        <object data="./plus.svg" type="image/svg+xml"></object>
        <p class="font-semibold">Add new plan</p>
      </button>
    </div>
  </section>
  <pre>{{ list }}</pre>
</template>
