export abstract class User {
  protected username: string;
  protected password: string;
  protected confirmPassword: string;

  constructor(username: string, password: string, confirmPassword: string) {
    this.username = username;
    this.password = password;
    this.confirmPassword = confirmPassword;
  }

}
