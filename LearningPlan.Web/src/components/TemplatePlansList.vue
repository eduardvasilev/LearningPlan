<template>
    <div>
         <div class="jumbotron">
            <div class="d-flex flex-row">
                <div class="p-2">
                    <h3 class="display-4">Your learning plans</h3>
                </div>
            </div>
        </div>
        <div class="plan-box">
            <div v-for="plan in plans">
                <div class="card plan-card">
                    <div class="card-body">
                        <router-link :to="{ name: 'plan', params: { id: plan.id }}">{{plan.name}}</router-link>
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>

<script lang="ts">
    import { Plan } from "../models/plan";
    import { Component, Vue } from 'vue-property-decorator';
    import PlanDataService from "../services/plan-data-service";

    @Component
    export default class TemplatePlansList extends Vue {

        public plans: Plan[] = [];

        constructor() {
            super();
            this.retrieveTemplatePlans()
        }
      
        private retrieveTemplatePlans() {
            PlanDataService.getTemplatePlans()
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
        display: inline-flex;
        align-items: stretch;
        flex-direction: row;
        flex-wrap: wrap;
    }

    .plan-box div {
        width: auto;
        height: auto;
        margin: 10px;
    }
</style>
