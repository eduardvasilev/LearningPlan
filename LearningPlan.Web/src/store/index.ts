import Vue from 'vue'
import Vuex from 'vuex'
import { CookieManager } from "../tools/cookie-manager"
Vue.use(Vuex);

export const store = new Vuex.Store({
    state: {
        user: {
            username: CookieManager.getCookie("username"),
            token: CookieManager.getCookie("token"),
            userId: CookieManager.getCookie("userId"),
            isAuthenticated: CookieManager.getCookie("token") != null //TODO
        },
        plan: {
            hasEditPermission: false
        }
    },
    mutations: {
        PlanOpened(state: any, hasEditPermission: boolean){
            state.plan.hasEditPermission = hasEditPermission;
        },
        UserAuthenticated(state: any, authResponse: any) {
            state.user.username = authResponse.username;
            CookieManager.setCookie("username", authResponse.username);

            state.user.token = authResponse.token;
            CookieManager.setCookie("token", authResponse.token);

            state.user.userId = authResponse.userId;
            CookieManager.setCookie("userId", authResponse.userId);

            state.user.isAuthenticated = true;
        },
        UserLogout(state: any, authResponse: any) {
            state.user.token = null;
            CookieManager.deleteCookie("token");

            state.user.isAuthenticated = false;
        }
    }
});