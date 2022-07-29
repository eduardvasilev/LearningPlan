import AuthenticationService from "@/services/auth-service"
import { reactive, ref, computed, type Ref, type WritableComputedRef } from "vue";

export enum AuthenticationMethods {
  Login,
  Signup
}

export function useAuthentication() {
  const user = reactive({
    username: '',
    password: '',
    confirmPassword: ''
  });
  const error: Ref<string> = ref('');
  const errorMessage: WritableComputedRef<string> = computed({
    get() {
      return error.value;
    },
    set(errorMessage: string) {
      error.value = errorMessage;
    }
  })

  function formSubmit(authMethod: AuthenticationMethods): void {
    if (authMethod === AuthenticationMethods.Login) {
      AuthenticationService.login(user)
        .catch((error) => {
          errorMessage.value = error.response.data.message;
        });
    } else if (authMethod === AuthenticationMethods.Signup) {
      AuthenticationService.signup(user)
        .catch((error) => {
          errorMessage.value = error.response.data.message;
        });
    }
  }

  return { user, errorMessage, formSubmit }
}
