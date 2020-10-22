import Vue from 'vue'
import Vuex from 'vuex'
import { CookieManager } from "../tools/cookie-manager"
Vue.use(Vuex);

export const store = new Vuex.Store({
    state: {
        user: {
            username: CookieManager.getCookie("username"),
            token: CookieManager.getCookie("token"),
            isAuthenticated: CookieManager.getCookie("token") != null //TODO
        }
    },
    mutations: {
        UserAuthenticated(state: any, authResponse: any) {
            state.user.username = authResponse.username;
            CookieManager.setCookie("username", authResponse.username);

            state.user.token = authResponse.token;
            CookieManager.setCookie("token", authResponse.token);

            state.user.isAuthenticated = true;
        }
    }
});