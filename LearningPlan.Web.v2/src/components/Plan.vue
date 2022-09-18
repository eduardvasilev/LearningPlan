<script setup lang="ts">
import { usePlan } from "@/modules/plan/plan.controller"

const planController = usePlan();

</script>
<template>
  <section class="px-5 container cols-6 mx-auto my-28">
    <div class="grid grid-flow-row col-start-2 col-end-5 gap-10">
      <h1 class="text-3xl">{{ planController.plan.name }}</h1>
      <div class="border border-gray-400 rounded-lg" v-for="area in planController.plan.planAreas">
        <h2 class="text-xl font-semibold p-5">{{ area.name }}</h2>
        <div v-for="topic in area.areaTopics" class="border-t border-gray-400 flex flex-col p-5 gap-5">
          <h4 class="font-semibold text-lg">{{ topic.name }}</h4>
          <p class="font-semibold">Description:</p>
          <p>{{ topic.description }}</p>
        </div>
      </div>
    </div>
  </section>
  <pre>{{ planController.plan }}</pre>
</template>
