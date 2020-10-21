<template>
    <div>
        <h3>{{ plan.name}}</h3>

        <div id="accordion">
            <div v-for="(area, index) in plan.planAreas" v-bind:key="area.id">
                <PlanArea :area="area" />
            </div>           
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import { Plan } from '../model/plan';
    import { Topic } from '../model/topic';
    import PlanDataService from "@/services/plan-data-service.ts";
    import Area from './Area.vue'

    @Component({
        components: {
            PlanArea: Area
        }
    })
    export default class PlanComponent extends Vue {
        private plan: Plan = {
            id: "",
            name: "",
            planAreas: []
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
