import http from "./create-request";

import { useUserStore } from "@/stores/UserStore"
import type { User } from "@/models/user";

const userStore = useUserStore();

class AuthenticationService {
    public async login(user: User) {
        const response = await http().post('/user/authenticate', user);
        http().defaults.headers.common["Authorization"] = response.data.token;
        userStore.UserAuthenticated(response.data);
    }

    public register(user: User) {
        return http().post('/user/register', user);
    }

    public logout() {
        delete http().defaults.headers.common["Authorization"];
        userStore.UserLogout();
    }
}

export default new AuthenticationService();
