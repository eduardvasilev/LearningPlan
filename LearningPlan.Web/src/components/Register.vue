<template>
    <div>
        <div class="mt-1" v-bind:class="{'alert': hasError , 'alert-danger': hasError }" role="alert">{{errorMessage}}</div>
        <form id="register-form" @submit="formSubmit">
            <div class="form-group">
                <label for="username">Username</label>
                <input id="reg-username"
                       v-model="user.username"
                       type="text"
                       name="username"
                       class="form-control"
                       placeholder="Enter username">
            </div>

            <div class="form-group">
                <label for="password">Password</label>
                <input id="reg-password"
                       v-model="user.password"
                       type="password"
                       name="password" class="form-control"
                       placeholder="Enter password">
            </div>

            <div class="form-group">
                <label for="password">Confirm Password</label>
                <input id="reg-confirmpassword"
                       v-model="user.confirmpassword"
                       type="password"
                       name="confirmpassword" class="form-control"
                       placeholder="Confirm Password">
            </div>

            <div class="form-group">
                <input class="btn btn-primary" type="submit"
                       value="Register">
            </div>
        </form>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import { User } from '../models/user';
    import AuthenticationService from "../services/auth-service";

    @Component
    export default class Register extends Vue {

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
            AuthenticationService.register(this.user)
                .then(function (response) {
                   //;
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
