<script setup lang="ts">
import { type Ref, ref, watch } from "vue";
import { useAuthentication, AuthenticationMethods, AuthenticationStatus } from "@/modules/authentication/authentication.controller"
import LoadingWheelVue from "@/components/icons/LoadingWheel.vue"
import ErrorPopupVue from "./ErrorPopup.vue";

const login = useAuthentication(AuthenticationMethods.Login);

const ErrorPopupAlert: Ref<string> = ref('');

const isLoading: Ref<boolean> = ref(false);

function eraseErrorMessageAfter3Sec() {
  if (login.errorMessage.value !== '') {
    setTimeout(() => login.errorMessage.value = '', 3000);
  }
}

watch(login.errorMessage, () => {
  ErrorPopupAlert.value = login.errorMessage.value;
  eraseErrorMessageAfter3Sec();
})

watch(login.status, (status) => {
  status === AuthenticationStatus.Pending ? isLoading.value = true : isLoading.value = false;
});

</script>

<template>
  <div class="flex flex-col gap-10 w-1/5">
    <h1 class="text-3xl font-bold">Welcome back</h1>
    <form class="auth-form" id="login-form" @submit.prevent="login.formSubmit()">
      <label for="username">Email</label>
      <input type="email" name="email" id="username" placeholder="Enter your email" class="auth-form__input"
        v-model.lazy="login.user.email" required />
      <label for="password">Password</label>
      <input type="password" name="password" id="password" placeholder="Enter password" class="auth-form__input"
        v-model.lazy="login.user.password" required>
      <button type="submit" class="auth-form__button mt-5">
        <div class="auth-form__button-content">
          <Transition mode="out-in">
            <LoadingWheelVue v-if="isLoading" class="-ml-10 mr-5 " />
          </Transition>
          Sign in
        </div>
      </button>
    </form>
    <div class="text-center">Dont have an account?<router-link to="signup" class="text-indigo-700"> Sign up
      </router-link>
    </div>
    <Transition name="slide-up" mode="out-in">
      <ErrorPopupVue v-if='ErrorPopupAlert' :errorMessage='ErrorPopupAlert' />
    </Transition>
  </div>
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
