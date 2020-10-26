<template>
    <div>
        <h3>{{plan.name}}</h3>
        <button class="btn btn-primary" data-toggle="modal" data-target="#addAreaModel" type="button">Add area</button>
        <a class="btn btn-primary" target="_blank" :href="'https://t.me/learningplanbot?start=' + plan.id">Subscribe in telegram</a>
        <div id="accordion">
            <div v-for="(area, index) in plan.planAreas" v-bind:key="area.id">
                <PlanArea :area="area" />
            </div>
        </div>

        <AreaEditor :planId="plan.id" v-on:area-added="plan.planAreas.push($event)" />

    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import { Plan } from '../models/plan';
    import PlanDataService from "../services/plan-data-service";
    import Area from './Area.vue';
    import AreaEditor from './AreaEditor.vue';

    @Component({
        components: {
            PlanArea: Area,
            AreaEditor
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
