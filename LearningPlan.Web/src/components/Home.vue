<template>
    <div class="home">
        <nav class="navbar navbar-expand-lg">
            <ul class="navbar-nav mr-auto">
                <li class="nav-item">
                    <router-link to="/" tag="a" class="nav-link">My Plans</router-link>
                </li>
                <li class="nav-item">
                    <router-link to="/templates" tag="a" class="nav-link">Explore Plans</router-link>
                </li>
            </ul>
            <ul class="navbar-nav">
                <li class="nav-item">
                    <a v-on:click="logout" tag="a" class="nav-link logout">Logout</a>
                </li>
            </ul>
        </nav>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import AuthenticationService from "../services/auth-service";

    @Component
    export default class Home extends Vue {
        data: any = {
            username: this.$store.state.user.username
        };

        private getBack() {
            this.$router.go(-1);
        }
        private logout() {
            AuthenticationService.logout();
            this.$router.go(-1);
        }
    }
   
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
