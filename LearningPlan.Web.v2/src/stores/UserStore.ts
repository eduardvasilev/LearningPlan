import { defineStore } from "pinia";

import { CookieManager } from "@/tools/cookie-manager"

export const useUserStore = defineStore("UserStore", {
  state: () => {
    return {
      username: CookieManager.getCookie("username"),
      token: CookieManager.getCookie("token"),
      userId: CookieManager.getCookie("userId"),
      isAuthenticated: CookieManager.getCookie("token") != false
    };
  },

  actions: {
    UserAuthenticated(authResponse: any): void {
      this.username = authResponse.username;
      CookieManager.setCookie("username", authResponse.username);

      this.token = authResponse.token;
      CookieManager.setCookie("token", authResponse.token);

      this.userId = authResponse.userId;
      CookieManager.setCookie("userId", authResponse.userId);

      this.isAuthenticated = true;
    },
    UserLogout(): void {
      this.token = '';
      CookieManager.deleteCookie("token");
      this.isAuthenticated = false;
    }
  }
});
