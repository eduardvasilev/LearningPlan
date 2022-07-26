import { Authenticate, type AuthMethods } from "@/models/authenticate";
import AuthenticationService from "@/services/auth-service"

export class Signup extends Authenticate {

  formSubmit(method: AuthMethods): void {
    AuthenticationService.register(this.user)
      .catch((error) => {
        this.error = error.response.data.message;
      });
  };

  set passwordConfirmation(thePasswordConfirmation: string) {
    this.user.password = thePasswordConfirmation;
  };
}
