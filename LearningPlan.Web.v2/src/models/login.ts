import { Authenticate, type AuthMethods } from "@/models/authenticate";
import AuthenticationService from "@/services/auth-service"

export class Login extends Authenticate {

  formSubmit(method: AuthMethods): void {
    AuthenticationService.login(this.user)
      .catch((error) => {
        this.error = error.response.data.message;
      });
  };
};
