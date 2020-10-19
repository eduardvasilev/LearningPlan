<template>
    <div class="home">
        <div v-on:click ="getBack">Back</div>
        <h3>Welcome, {{this.$store.state.user.username}}!</h3>
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
