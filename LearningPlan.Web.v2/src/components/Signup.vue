<script setup lang="ts">
import { AuthMethods } from '@/models/authentication';
import { Signup } from "@/models/signup"

const user = new Signup();
const method = AuthMethods.Signup;
</script>

<template>
  <section class="flex justify-center">
    <div class="flex flex-col gap-10 w-1/5">
      <h1 class="text-3xl font-bold">Just Welcome</h1>
      <form class="flex flex-col gap-4" id="login-form" @submit.prevent="user.formSubmit(method)">
        <label for="username">Username</label>
        <input type="text" name="username" id="username" placeholder="Enter your username" class="input"
          v-model="user.username" />
        <label for="password">Password</label>
        <input type="password" name="password" id="password" placeholder="Enter password" class="input"
          v-model="user.password">
        <label for="passwordConfirmation">Confirm password</label>
        <input type="password" name="passwordConfirmation" id="passwordConfirmation" placeholder="Confirm password"
          class="input" v-model="user.passwordConfirmation">
        <button type="submit"
          class="bg-yellow-400 font-semibold border border-amber-900 text-amber-900 rounded-md p-4 mt-5">Sign
          up</button>
      </form>
      <div class="text-center">Have an account already? <router-link to="/login" class="text-indigo-700">Log in
        </router-link>
      </div>
    </div>
  </section>
</template>
