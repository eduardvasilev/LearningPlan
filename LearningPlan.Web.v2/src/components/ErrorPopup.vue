<script setup lang="ts">
const props = defineProps<{
  errorMessage: string;
}>();
</script>

<template>
  <div class="error-popup">{{ props.errorMessage }}</div>
</template>
