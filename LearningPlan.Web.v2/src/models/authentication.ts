import type { AuthModel } from "@/models/auth-model"

interface FormSubmit {
  formSubmit(): void;
};

export abstract class Authentication implements FormSubmit {
  protected error: string;
  protected user: AuthModel;
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

  set errorMessage(errorMessage: string) {
    this.error = errorMessage;
  };

  get errorMessage() {
    return this.error;
  };

  get hasError() {
    return this.error.length > 0;
  };

  abstract formSubmit(): void;
}
