<template>
    <div id="app">
        <div v-if="this.$store.state.user.isAuthenticated">
            <Home />
        </div>
        <div v-else>
            <Register />
            Or
            <Login />
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import Home from './components/Home.vue';
    import Register from './components/Register.vue';
    import Login from './components/Login.vue';

    @Component({
        components: {
            Home,
            Register,
            Login,
        }
    })
    export default class App extends Vue {
        data: any = {
            isAuthenticated: this.$store.state.user.isAuthenticated
        };}
</script>

<style>
</style>
