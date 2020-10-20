<template>
    <div class="home">
        <nav class="navbar">
            <a class="navbar-brand" href="#" v-on:click="getBack">Go back!</a>
            <a class="navbar-brand" href="/">{{this.$store.state.user.username}}</a>
        </nav>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    @Component
    export default class Home extends Vue {
        data: any = {
            username: this.$store.state.user.username
        };

        private getBack() {
            this.$router.go(-1);
        }
    }
   
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
