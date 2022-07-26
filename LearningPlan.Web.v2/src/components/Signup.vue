<script setup lang="ts">
import { AuthMethods } from '@/models/authenticate';
import { Signup } from "@/models/signup"

const user = new Signup();
const method = AuthMethods.Signup;
</script>

<template>
  <section class="flex justify-center">
    <div class="flex flex-col gap-10 w-1/5">
      <h1 class="text-3xl font-bold">Welcome back</h1>
      <form class="flex flex-col gap-4" id="login-form" @submit.prevent="user.formSubmit(method)">
        <label for="username">Username</label>
        <input type="text" name="username" id="username" placeholder="Enter your username" class="input"
          v-model="user.username" />
        <label for="password">Password</label>
        <input type="password" name="password" id="password" placeholder="Enter password" class="input"
          v-model="user.password">
        <button type="submit" class="bg-black rounded-md p-2 text-white mt-5">Sign in</button>
      </form>
      <div class="text-center">Have an account already?<button class="text-blue-400">Log in</button></div>
    </div>
  </section>
</template>
