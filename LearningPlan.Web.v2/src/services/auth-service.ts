import createRequest from "./create-request";

import { useUserStore } from "@/stores/UserStore"
import type { AuthModel } from "@/models/auth-model";

class AuthenticationService {
    public async login(user: AuthModel) {
        const response = await createRequest().post('/user/authenticate', user);
        createRequest().defaults.headers.common["Authorization"] = response.data.token;
        useUserStore().UserAuthenticated(response.data);
    }

    public signup(user: AuthModel) {
        return createRequest().post('/user/register', user);
    }

    public logout() {
        delete createRequest().defaults.headers.common["Authorization"];
        useUserStore().UserLogout();
    }
}

export default new AuthenticationService();
