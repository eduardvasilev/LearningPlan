import AuthenticationService from "@/services/auth-service"
import { reactive, ref, computed, type Ref, type WritableComputedRef } from "vue";

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
  const user = reactive({
    username: '',
    password: '',
    confirmPassword: ''
  });

  const status: Ref<AuthenticationStatus> = ref(AuthenticationStatus.Initialized);

  const error: Ref<string> = ref('');
  const errorMessage: WritableComputedRef<string> = computed({
    get() {
      return error.value;
    },
    set(errorMessage: string) {
      error.value = errorMessage;
    }
  });

  function formSubmit(): void {
    status.value = AuthenticationStatus.Pending;
    if (authMethod === AuthenticationMethods.Login) {
      AuthenticationService.login(user)
        .catch((error) => {
          errorMessage.value = error.response.data.message;
        }).finally(() => {
          status.value = AuthenticationStatus.Settled;
        });
    } else if (authMethod === AuthenticationMethods.Signup) {
      AuthenticationService.signup(user)
        .catch((error) => {
          errorMessage.value = error.response.data.message;
        }).finally(() => {
          status.value = AuthenticationStatus.Settled;
        });
    };
  };

  return { user, status, errorMessage, formSubmit };
};
