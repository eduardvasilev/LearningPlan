<template>
    <div class="home">
        <h3>Welcome, {{this.$store.state.user.username}}!</h3>
        <PlanList />
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import PlanList from '@/components/PlanList.vue';

    @Component({
        components: {
            PlanList
        }
    })
    export default class Home extends Vue {
        data: any = {
            username: this.$store.state.user.username
        };
    }
   
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
