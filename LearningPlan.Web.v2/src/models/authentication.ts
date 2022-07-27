import type { User } from "@/models/user"

export enum AuthMethods {
  Login,
  Signup
};

interface FormSubmit {
  formSubmit(method: AuthMethods): void;
};

export abstract class Authentication implements FormSubmit {
  protected error: string;
  protected user: User;
  constructor() {
    this.user = {
      username: '',
      password: ''
    };
    this.error = '';
  };

  set username(theUsername: string) {
    this.user.username = theUsername;
  };

  set password(thePassword: string) {
    this.user.password = thePassword;
  };

  get errorMessage() {
    return this.error;
  };

  get hasError() {
    return this.error.length > 0;
  };

  abstract formSubmit(method: AuthMethods): void;
}
