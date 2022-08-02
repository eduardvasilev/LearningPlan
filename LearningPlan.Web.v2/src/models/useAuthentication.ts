import AuthenticationService from "@/services/auth-service"
import { reactive, ref, computed, type Ref, type WritableComputedRef } from "vue";
import type { AuthModel } from "./auth-model";

export enum AuthenticationMethods {
  Login,
  Signup
};

export enum AuthenticationStatus {
  Initialized,
  Pending,
  Settled
};

export function useAuthentication(authMethod: AuthenticationMethods) {
  const user: AuthModel = reactive({
    username: '',
    password: '',
    confirmPassword: ''
  });

  const status: Ref<AuthenticationStatus> = ref(AuthenticationStatus.Initialized);

  const error: Ref<string> = ref('');
  const errorMessage: WritableComputedRef<string> = computed({
    get: () => error.value,
    set: (errorMessage: string) => error.value = errorMessage
  });

  function handleSubmit(callback: Function) {
    status.value = AuthenticationStatus.Pending;
    callback()
      .catch((error: any) => errorMessage.value = error.response.data.message)
      .finally(() => status.value = AuthenticationStatus.Settled);
  };

  function formSubmit(): void {
    switch (authMethod) {
      case AuthenticationMethods.Login: {
        handleSubmit(() => AuthenticationService.login(user));
        break;
      }
      case AuthenticationMethods.Signup: {
        handleSubmit(() => AuthenticationService.signup(user));
        break
      }
    }
  };

  return { user, status, errorMessage, formSubmit };
};
