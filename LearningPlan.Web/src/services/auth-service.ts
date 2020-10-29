import http from "../http-common";
import { User } from "../models/user";
import { store } from "../store/index";

class AuthenticationService {
    public login(user: User) {
        return http.post('/user/authenticate', user)
            .then((response) => {
                http.defaults.headers["Authorization"] = response.data.token;
                store.commit("UserAuthenticated", response.data);
            });
    }

    public register(user: User) {
        return http.post('/user/register', user);
    }
}

export default new AuthenticationService();