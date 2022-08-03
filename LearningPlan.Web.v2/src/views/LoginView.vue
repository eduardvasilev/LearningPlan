<script setup lang="ts">
import LoginVue from '@/components/Login.vue';
import SignupVue from '@/components/Signup.vue';
import LoggedVue from '@/components/Logged.vue';
import { useUserStore } from "@/stores/UserStore"
import { useRoute } from 'vue-router';
import { onUpdated, type Ref, ref } from 'vue';

const route = useRoute();
const token = useUserStore().token;

let currentRoute: Ref<string> = ref(String(route.name));

function toggleComponentByRouteName(componentName: any) {
  currentRoute.value = componentName;
};

onUpdated(() => {
  toggleComponentByRouteName(route.name);
});

</script>
<template>
  <section class="flex justify-center mt-20">
    <object data="./logo.svg" type="image/svg+xml" title="Learning Plan"></object>
  </section>
  <section v-if="!token" class="flex justify-center">
    <Transition mode="out-in">
      <component class="mt-10" :is="currentRoute == 'login' ? LoginVue : SignupVue" />
    </Transition>
  </section>
  <section v-else class="flex justify-center">
    <LoggedVue />
  </section>
</template>

<style>
.v-enter-active,
.v-leave-active {
  transition: opacity 0.2s ease;
}

.v-enter-from,
.v-leave-to {
  opacity: 0;
}
</style>
