<script setup lang="ts">
import { useAuthentication, AuthenticationMethods } from "@/models/useAuthentication"
import ErrorPopupVue from "./ErrorPopup.vue";
import { type Ref, ref, watch } from "vue";

const login = useAuthentication();

const ErrorPopupAlert: Ref<string> = ref('');

function eraseErrorMessageAfter3Sec() {
  if (login.errorMessage.value !== '') {
    setTimeout(() => login.errorMessage.value = '', 3000);
  }
}

watch(login.errorMessage, () => {
  ErrorPopupAlert.value = login.errorMessage.value;
  eraseErrorMessageAfter3Sec();
})

</script>

<template>
  <section class="flex justify-center">
    <div class="flex flex-col gap-10 w-1/5">
      <h1 class="text-3xl font-bold">Welcome back</h1>
      <form class="auth-form" id="login-form" @submit.prevent="login.formSubmit(AuthenticationMethods.Login)">
        <label for="username">Username</label>
        <input type="text" name="username" id="username" placeholder="Enter your username" class="auth-form__input"
          v-model.lazy="login.user.username" required />
        <label for="password">Password</label>
        <input type="password" name="password" id="password" placeholder="Enter password" class="auth-form__input"
          v-model.lazy="login.user.password" required>
        <button type="submit" class="auth-form__button mt-5">Sign
          in</button>
      </form>
      <div class="text-center">Dont have an account?<router-link to="signup" class="text-indigo-700"> Sign up
        </router-link>
      </div>
      <Transition name="slide-up" mode="out-in">
        <ErrorPopupVue v-if='ErrorPopupAlert' :errorMessage='ErrorPopupAlert' />
      </Transition>
    </div>
  </section>
</template>
<style>
.slide-up-enter-active,
.slide-up-leave-active {
  transition: all 0.25s ease-in-out;
}

.slide-up-enter-from {
  opacity: 0;
  transform: translateY(30px);
}

.slide-up-leave-to {
  opacity: 0;
  transform: translateY(30px);
}
</style>
