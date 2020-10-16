<template>
    <div>
        <h3>Register</h3>
        <div>{{errorMessage}}</div>
        <form id="register-form"
              @submit="formSubmit">
            <p>
                <label for="username">Username</label>
                <input id="reg-username"
                       v-model="user.username"
                       type="text"
                       name="username">
            </p>
            <p>
                <label for="username">Password</label>
                <input id="reg-password"
                       v-model="user.password"
                       type="password"
                       name="password">
            </p>
            <p>
                <label for="username">Confirm Password</label>
                <input id="reg-confirmpassword"
                       v-model="user.confirmpassword"
                       type="password"
                       name="confirmpassword">
            </p>

            <p>
                <input type="submit"
                       value="Register">
            </p>

        </form>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import axios from "axios";

    @Component
    export default class Register extends Vue {

        private error = "";

        private user: any = {
            username: "",
            password: "",
            confirmpassword: ""
        };

        get errorMessage() {
            return this.error;
        }

        public formSubmit(e: Event) {
            e.preventDefault();
            axios.post('https://localhost:44335/user/register', this.user)
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
