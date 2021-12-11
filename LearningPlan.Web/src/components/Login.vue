<template>
    <div>
        <div class="mt-1" v-bind:class="{'alert': hasError , 'alert-danger': hasError }" role="alert">{{errorMessage}}</div>
        <form id="login-form" @submit="formSubmit">
            <div class="form-group">
                <label for="username">Username</label>
                <input id="login-username"
                       v-model="user.username"
                       type="text"
                       name="username"
                       class="form-control"
                       placeholder="Enter username">
            </div>

            <div class="form-group">
                <label for="password">Password</label>
                <input id="login-password"
                       v-model="user.password"
                       type="password"
                       name="password" class="form-control"
                       placeholder="Enter password">
            </div>

            <div class="form-group">
                <input class="btn btn-primary" type="submit"
                       value="Log in">
            </div>
        </form>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import AuthenticationService from "../services/auth-service";
    import { User } from '../models/user';

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

        get hasError() {
            return this.error.length > 0;
        }

        public formSubmit(e: Event) {
            e.preventDefault();
            AuthenticationService.login(this.user)
                .catch((error) => {
                    this.error = error.response.data.message;
                });
        }
    }

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
