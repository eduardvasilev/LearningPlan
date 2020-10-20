<template>
    <div>
        <p><h3>Your plans</h3></p>
        <div class="plan-box">
            <div v-for="plan in plans">
                <div class="card">
                    <div class="card-body">
                        <router-link :to="{ name: 'plan', params: { id: plan.id }}">{{plan.name}}</router-link>
                    </div>
                </div>
            </div>
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
    .plan-box {
        display: flex;
        align-items: stretch;
    }

    .plan-box div {
        width: auto;
        height: auto;
        margin: 10px;
    }
</style>
