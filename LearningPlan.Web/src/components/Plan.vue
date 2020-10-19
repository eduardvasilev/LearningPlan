<template>
    <div>
        <div>{{ plan.id}}</div>
        <div>{{ plan.name}}</div>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import { Plan } from '../Model/plan';
    import PlanDataService from "@/services/plan-data-service.ts";
    @Component
    export default class PlanComponent extends Vue {
        private plan: Plan = {
            id: "",
            name: ""
        };

        constructor() {
            super();
            this.retrievePlan();
        }

        private retrievePlan() {
            const planId = this.$route.params.id;
            PlanDataService.getPlan(planId)
                .then((response) => {
                    this.plan = response.data;
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
