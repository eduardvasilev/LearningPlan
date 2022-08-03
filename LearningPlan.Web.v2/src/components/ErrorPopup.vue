<script setup lang="ts">
const props = defineProps<{
  errorMessage: string;
}>();
</script>

<template>

  <div class="error-popup"><object data="./exclamation.svg" class="h-5" type="image/svg+xml" title="Error"></object>{{
      props.errorMessage
  }}</div>
</template>
