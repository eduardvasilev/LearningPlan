<script setup lang="ts">
import AuthenticationService from "@/modules/authentication/authentication.service";
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
  <header class="flex flex-row justify-between h-8 absolute top-0 w-full px-4 items-center bg-neutral-800 text-white">
    <div class="flex"><object data="./logo_small.svg" class="h-5" type="image/svg+xml" title="Logo" /></div>
    <nav class="col-start-2 col-end-6">
      <ul class="flex flex-row gap-5 justify-start">
        <li>Plans</li>
        <li>Explore</li>
        <li>Activity</li>
      </ul>
    </nav>
    <div class="flex flex-row gap-10 justify-end">
      <p>{{ props.username }}</p>|<button @click.prevent="logout()"
        class="px-3 bg-gray-900 rounded-xl text-white ">Logout</button>
    </div>
  </header>
</template>
