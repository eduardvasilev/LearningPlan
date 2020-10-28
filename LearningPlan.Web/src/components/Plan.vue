<template>
    <div>
        <h3>{{plan.name}}</h3>
        <button class="btn btn-primary" data-toggle="modal" data-target="#addAreaModel" type="button">Add area</button>
        <a class="btn btn-primary" target="_blank" :href="'https://t.me/learningplanbot?start=' + plan.id">Subscribe in telegram</a>
        <button class="btn btn-danger" type="button" v-on:click="deletePlan">Delete plan</button>

        <div id="accordion">
            <div v-for="(area, index) in plan.planAreas" v-bind:key="area.id">
                <PlanArea :area="area" v-on:area-deleted="onareadeleted($event)"  />
            </div>
        </div>

        <AreaEditor :planId="plan.id" v-on:area-added="plan.planAreas.push($event)" />
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import { PlanArea } from '../models/plan-area';
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

        private deletePlan() {
            PlanDataService.deletePlan(this.plan.id)
                .then(() => {
                    this.$router.go(-1);
                })
                .catch((error) => {
                    console.error(error);
                });
        }

        public onareadeleted(area: PlanArea) {
            const index: number = this.plan.planAreas.indexOf(area);
            if (index !== -1) {
                this.plan.planAreas.splice(index, 1);
            }
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
