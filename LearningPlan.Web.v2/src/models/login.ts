import { Authentication } from "@/models/authentication";
import AuthenticationService from "@/services/auth-service"

export class Login extends Authentication {

  formSubmit() {
    AuthenticationService.login(this.user)
      .catch((error) => {
        this.error = error.response.data.message;
      });
  };
};
