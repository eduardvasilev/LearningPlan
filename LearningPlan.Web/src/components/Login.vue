<template>
    <div>
        <h3>Log in</h3>
        <div>{{errorMessage}}</div>
        <form id="login-form"
              @submit="formSubmit">
            <p>
                <label for="username">Username</label>
                <input id="login-username"
                       v-model="user.username"
                       type="text"
                       name="username">
            </p>
            <p>
                <label for="username">Password</label>
                <input id="login-password"
                       v-model="user.password"
                       type="password"
                       name="password">
            </p>  

            <p>
                <input type="submit"
                       value="Log in">
            </p>

        </form>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import AuthenticationService from "@/services/auth-service.ts";
    import { User } from '../model/user';

    @Component
    export default class Login extends Vue {
        private error = "";

        private user: User = {
            username: "",
            password: "",
            confirmPassword: ""
        };

        get errorMessage() {
            return this.error;
        }

        public formSubmit(e: Event) {
            e.preventDefault();
            AuthenticationService.login(this.user)
                .then((response) => {
                    this.$store.commit("UserAuthenticated", response.data);
                })
                .catch((error) => {
                    this.error = error.message;
                });
        }
    }

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
