<script setup lang="ts">
import LoginVue from '@/components/Login.vue';
import SignupVue from '@/components/Signup.vue';
import { useRoute } from 'vue-router';
import { onUpdated, type Ref, ref } from 'vue';

const route = useRoute();

let currentRoute: Ref<string> = ref('login');

function changeComponent(componentName: any) {
  currentRoute.value = componentName;
}

onUpdated(() => {
  changeComponent(route.name)
})

</script>
<template>
  <section class="flex justify-center mt-20">
    <object data="./logo.svg" type="image/svg+xml" class="w-1/5 drop-shadow-xl" title="Learning Plan"></object>
  </section>
  <transition name="fade" mode="out-in">

    <component class="mt-10" :is="currentRoute == 'login' ? LoginVue : SignupVue" />
  </transition>
</template>

<style>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease-out;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
