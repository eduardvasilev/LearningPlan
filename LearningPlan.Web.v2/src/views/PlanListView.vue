<script setup lang="ts">
import PlanListVue from '@/components/PlanList.vue';
</script>
<template>
  <section class="flex flex-col my-28 items-center">
    <div class="flex flex-col w-1/2 gap-10">
      <h1 class="text-3xl mb-5 w-full">Your learning plans</h1>
      <Suspense>
        <PlanListVue />
        <template #fallback>
          <p>loading...</p>
        </template>
      </Suspense>
    </div>
  </section>
</template>
