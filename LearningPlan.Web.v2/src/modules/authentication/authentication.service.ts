import createRequest from "@/modules/httprequest/create-request.service";
import { useUserStore } from "@/stores/UserStore"
import type { AuthModel } from "@/modules/authentication/interfaces/authentication.interface";

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
