import { Authentication } from "@/models/authentication";
import AuthenticationService from "@/services/auth-service"

export class Signup extends Authentication {

  formSubmit(): void {
    AuthenticationService.signup(this.user)
      .catch((error) => {
        this.error = error.response.data.message;
      });
  };

  set passwordConfirmation(thePasswordConfirmation: string) {
    this.user.password = thePasswordConfirmation;
  };
}
