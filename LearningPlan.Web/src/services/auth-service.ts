import http from "../http-common";
import { User } from "../models/user";

class AuthenticationService {
    public login(user: User) {
       return http.post('/user/authenticate', user);
    }

    public register(user: User) {
        return http.post('/user/register', user);
    }
}

export default new AuthenticationService();