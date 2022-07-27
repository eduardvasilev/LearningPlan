<script setup lang="ts">
import { AuthMethods } from "@/models/authentication"
import { Login } from "@/models/login"

const user = new Login();
const method = AuthMethods.Login
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
        <button type="submit"
          class="rounded-md p-4 bg-yellow-400 border border-amber-900 text-amber-900 font-semibold mt-5">Sign
          in</button>
      </form>
      <div class="text-center">Dont have an account? <router-link to="signup" class="text-indigo-700">Sign up
        </router-link>
      </div>
    </div>
  </section>
</template>
