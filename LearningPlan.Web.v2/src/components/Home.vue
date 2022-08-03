<script setup lang="ts">
import MenuVue from "@/components/Menu.vue";
import { useUserStore } from "@/stores/UserStore"

const username = useUserStore().username

</script>

<template>
  <MenuVue :username="username" />
</template>
