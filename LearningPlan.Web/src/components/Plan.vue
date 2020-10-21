<template>
    <div>
        <h3>{{ plan.name}}</h3>

        <div id="accordion">
            <div v-for="(area, index) in plan.planAreas" v-bind:key="area.id" class="card">
                <h5>{{area.name}}</h5>
                <div v-for="(areaTopic, index) in area.areaTopics" v-bind:key="areaTopic.id" class="card">
                    <div class="card-header" :id="'headingOne' + index">
                        <h5 class="mb-0">
                            <button class="btn btn-link" data-toggle="collapse" :data-target="'#p' + areaTopic.id" aria-expanded="true" :aria-controls="'p' + areaTopic.id">
                                {{areaTopic.name}}
                            </button>
                        </h5>
                    </div>

                    <div :id="'p' + areaTopic.id" class="collapse" :aria-labelledby="'headingOne' + areaTopic.id" data-parent="#accordion">
                        <div class="card-body container">
                            <div class="row">
                                <a :href="areaTopic.source" target="_blank">{{areaTopic.source}}</a>
                            </div>
                            <div class="row">
                                <div class="col">
                                    {{areaTopic.startDate}}
                                </div>
                                <div class="col">
                                    {{areaTopic.endDate}}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-header">
                    <h5 class="mb-0">
                        <button class="btn btn-link" v-on:click="addTopic">
                            Add new topic
                        </button>
                    </h5>
                </div>

                <TopicEditor :planAreaId="area.id" v-on:topic-added="area.areaTopics.push($event)" v-if="isAddingNew"/>
            </div>           
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import { Plan } from '../model/plan';
    import { Topic } from '../model/topic';
    import PlanDataService from "@/services/plan-data-service.ts";
    import TopicEditor from './TopicEditor.vue'

    @Component({
        components: {
            TopicEditor
        }
    })
    export default class PlanComponent extends Vue {
        private plan: Plan = {
            id: "",
            name: "",
            planAreas: []
        };

        private topic: Topic = new Topic();
        private isAddingNew: boolean = false;

        constructor() {
            super();
            this.retrievePlan();
        }

        private addTopic() {
            this.isAddingNew = !this.isAddingNew;
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
