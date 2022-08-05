<script setup lang="ts">
import LoginVue from '@/components/Login.vue';
import SignupVue from '@/components/Signup.vue';
import { useUserStore } from "@/stores/UserStore"
import { useRouter, useRoute } from 'vue-router';
import { onUpdated, type Ref, ref, reactive, watch, onMounted } from 'vue';

const router = useRouter();
const route = useRoute();
const userStore = reactive(useUserStore());

let currentRoute: Ref<string> = ref(String(route.name));

function toggleComponentByRouteName(componentName: any) {
  currentRoute.value = componentName;
};

function goHomeIfAuthenticated() {
  if (userStore.isAuthenticated != false) {
    router.push('/');
  };
};

onMounted(() => {
  goHomeIfAuthenticated();
})

onUpdated(() => {
  toggleComponentByRouteName(route.name);
});

watch(userStore, () => {
  goHomeIfAuthenticated();
});

</script>
<template>
  <section class="flex justify-center mt-20">
    <object data="./logo.svg" type="image/svg+xml" title="Learning Plan"></object>
  </section>
  <section class="flex justify-center">
    <Transition mode="out-in">
      <component class="mt-10" :is="currentRoute == 'login' ? LoginVue : SignupVue" />
    </Transition>
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
