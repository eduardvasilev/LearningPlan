<script setup lang="ts">
import { RouterLink } from 'vue-router';

</script>

<template>
  <div class="flex flex-col gap-10 w-1/5 mt-10 ">
    <p class="leading-relaxed">
      You are already logged in and will be redirected back to Homepage shortly.<br>
      If you are not redirected automatically, follow this <RouterLink to="/" class="text-blue-700">link</RouterLink>.
    </p>
  </div>
</template>
