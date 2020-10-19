<template>
    <div>
        <div v-for="plan in plans">
            <router-link :to="{ name: 'plan', params: { id: plan.id }}">
                <span>{{plan.name}}</span>
            </router-link>
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import { Plan } from '@/model/plan';
    import PlanDataService from "@/services/plan-data-service.ts";

    @Component
    export default class PlanList extends Vue {

        public plans: Plan[] = [];

        constructor() {
            super();
            this.retrievePlans()
        }

        private retrievePlans() {
            PlanDataService.getPlans()
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
