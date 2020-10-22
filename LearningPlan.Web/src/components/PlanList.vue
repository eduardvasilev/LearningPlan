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
            <div>
                <div class="card">
                    <div v-if="isAddingNew" class="">
                        <div class="input-group mb-3">

                            <input id="plan-name"
                                   type="text"
                                   name="plan-name"
                                   class="form-control"
                                   v-model="newPlanName"
                                   placeholder="Plan name">
                                <button class="input-group-append btn btn-primary btn-outline-secondary" v-on:click="addPlan" type="button">OK</button>
                        </div>
                    </div>
                    <div v-else class="card-body">
                        <a v-on:click="showPlanCreation" role="button" aria-expanded="false" aria-controls="add-plan">
                            Add new plan
                        </a>
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
    export default class PlanList extends Vue {

        private isAddingNew: boolean = false;
        private newPlanName: string = "";
        public plans: Plan[] = [];

        constructor() {
            super();
            this.retrievePlans()
        }

        private addPlan() {
            const regex = /\s/;
            if (!this.newPlanName || regex.test(this.newPlanName)) {
                return;
            }
            PlanDataService.addPlan(this.newPlanName)
                .then((response) => {
                    this.plans.push(new Plan(response.data.id, response.data.name));
                    this.showPlanCreation();
                    this.newPlanName = "";
                })
                .catch((error) => {
                    console.error(error);
                });
        }

        private showPlanCreation() {
            this.isAddingNew = !this.isAddingNew;
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
