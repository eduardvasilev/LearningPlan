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

            <!-- <a class="navbar-brand" href="/">{{this.$store.state.user.username}}</a> -->
            <!-- <a class="navbar-brand" href="#" v-on:click="getBack">
                <svg width="2em" height="2em" viewBox="0 0 16 16" class="bi bi-arrow-left-circle" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                    <path fill-rule="evenodd" d="M8 15A7 7 0 1 0 8 1a7 7 0 0 0 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z" />
                    <path fill-rule="evenodd" d="M12 8a.5.5 0 0 1-.5.5H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5a.5.5 0 0 1 .5.5z" />
                </svg>
            </a> -->
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
