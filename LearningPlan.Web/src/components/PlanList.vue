<template>
    <div>
        <div v-for="plan in plans">
            <div>{{ plan.name}}</div>
            
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import { Plan } from '@/Model/plan';

    import axios from "axios";

    @Component
    export default class PlanList extends Vue {

        public plans: Plan[] = [];

        constructor() {
            super();
            this.retrivePlans()
        }

        private retrivePlans() {
            axios.get('https://localhost:44335/plan', { headers: { 'Authorization': this.$store.state.user.token } })
                .then((response) => {
                    this.plans = response.data;
                })
                .catch((error) => {
                    console.error(error);
                });
        }
    }

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
