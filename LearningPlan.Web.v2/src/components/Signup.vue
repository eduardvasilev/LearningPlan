<script setup lang="ts">
import { useAuthentication, AuthenticationMethods, AuthenticationStatus } from "@/models/useAuthentication"
import LoadingWheelVue from "./icons/LoadingWheel.vue";
import ErrorPopupVue from "./ErrorPopup.vue";
import { type Ref, ref, watch } from "vue";

const signup = useAuthentication(AuthenticationMethods.Signup);

const ErrorPopupAlert: Ref<string> = ref('');

const isLoading: Ref<boolean> = ref(false);

function eraseErrorMessageAfter3Sec() {
  if (signup.errorMessage.value !== '') {
    setTimeout(() => signup.errorMessage.value = '', 3000);
  };
};

function handleSubmit() {
  if (signup.user.password !== signup.user.confirmPassword) {
    signup.errorMessage.value = 'Passwords do not match';
  } else {
    signup.formSubmit()
  }
}

watch(signup.errorMessage, () => {
  ErrorPopupAlert.value = signup.errorMessage.value;
  eraseErrorMessageAfter3Sec();
});

watch(signup.status, (status) => {
  status === AuthenticationStatus.Pending ? isLoading.value = true : isLoading.value = false;
});

</script>

<template>
  <section class="flex justify-center">
    <div class="flex flex-col gap-10 w-1/5">
      <h1 class="text-3xl font-bold">Just welcome</h1>
      <form class="auth-form" id="signup-form" @submit.prevent="handleSubmit()">
        <label for="username">Username</label>
        <input type="text" name="username" id="username" placeholder="Enter your username" class="auth-form__input"
          v-model="signup.user.username" required />
        <label for="password">Password</label>
        <input type="password" name="password" id="password" placeholder="Enter password" class="auth-form__input"
          v-model="signup.user.password" required>
        <label for="confirmPassword">Confirm password</label>
        <input type="password" name="confirmPassword" id="confirmPassword" placeholder="Confirm password"
          class="auth-form__input" v-model="signup.user.confirmPassword" required>
        <button type="submit" class="auth-form__button mt-5">
          <div class="auth-form__button-content">
            <Transition mode="out-in">
              <LoadingWheelVue v-if="isLoading" class="-ml-10 mr-5 " />
            </Transition>
            Sign up
          </div>
        </button>
      </form>
      <div class="text-center">Have an account already?<router-link to="/login" class="text-indigo-700"> Log in
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
