<script setup lang="ts">
import MenuVue from "@/components/Menu.vue";
import { useUserStore } from "@/stores/UserStore"
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';

const userStore = useUserStore();
const router = useRouter();

function goLoginIfNotAuthenticated() {

  if (!userStore.isAuthenticated) {
    router.push("/login");
  };
};

onMounted(() => {
  goLoginIfNotAuthenticated();
})

</script>

<template>
  <MenuVue :username="userStore.username" />
</template>
