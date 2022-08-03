<script setup lang="ts">
import AuthenticationService from "../services/auth-service";
import { useRouter } from "vue-router";

const router = useRouter();

const props = defineProps<{
  username: string | boolean
}>();

function logout() {
  AuthenticationService.logout();
  router.push("/login");
}
</script>
<template>
  <header class="mx-10 grid grid-flow-col grid-col-3 h-10 items-center text-center">
    <div>Logo</div>
    <nav>
      <ul class="flex flex-row gap-5 justify-start">
        <li>Plans</li>
        <li>Explore</li>
        <li>Activity</li>
      </ul>
    </nav>
    <div class="flex flex-row gap-10 justify-end">
      <p>{{ props.username }}</p>|<button @click.prevent="logout()"
        class="px-3 bg-gray-900 rounded-xl text-white">Logout</button>
    </div>
  </header>
</template>
