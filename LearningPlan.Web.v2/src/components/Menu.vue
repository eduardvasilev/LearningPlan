<script setup lang="ts">
import AuthenticationService from "../services/auth-service";
import { useRouter } from "vue-router";

const router = useRouter();

const props = defineProps<{
  username: string | boolean
}>();

function logout() {
  AuthenticationService.logout();
  router.push('/login');
};
</script>
<template>
  <header class="mx-auto px-5 container cols-6 h-20 items-center text-center border-b">
    <div class="flex"><object data="./logo_small.svg" class="h-5" type="image/svg+xml" title="Logo" /></div>
    <nav class="col-start-2 col-end-6">
      <ul class="flex flex-row gap-5 justify-start">
        <li class="font-bold uppercase tracking-[2px]">Plans</li>
        <li class="font-bold uppercase tracking-[2px]">Explore</li>
        <li class="font-bold uppercase tracking-[2px]">Activity</li>
      </ul>
    </nav>
    <div class="flex flex-row gap-10 justify-end">
      <p>{{ props.username }}</p>|<button @click.prevent="logout()"
        class="px-3 bg-gray-900 rounded-xl text-white font-bold uppercase tracking-[2px]">Logout</button>
    </div>
  </header>
</template>
