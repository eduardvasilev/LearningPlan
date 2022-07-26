import createRequest from "./create-request";

import { useUserStore } from "@/stores/UserStore"
import type { User } from "@/models/user";

class AuthenticationService {
    public async login(user: User) {
        const response = await createRequest().post('/user/authenticate', user);
        createRequest().defaults.headers.common["Authorization"] = response.data.token;
        useUserStore().UserAuthenticated(response.data);
    }

    public register(user: User) {
        return createRequest().post('/user/register', user);
    }

    public logout() {
        delete createRequest().defaults.headers.common["Authorization"];
        useUserStore().UserLogout();
    }
}

export default new AuthenticationService();
