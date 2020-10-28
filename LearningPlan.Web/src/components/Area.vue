<template>
    <div>
        <div class="card">
           <div> 
               {{area.name}}

               <button type="button"  v-on:click="deleteArea(area)" class="close" aria-label="Close">
                   <span aria-hidden="true">&times;</span>
               </button>
            </div>
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
                    <button class="btn btn-link" v-on:click="deleteTopic(areaTopic, area)">
                        Delete topic
                    </button>
                </div>
            </div>
            <div class="card-header" :id="'addtopic' + area.id">
                <button class="btn btn-link" data-toggle="collapse" :data-target="'#addTopic' + area.id" aria-expanded="true" :aria-controls="'addTopic' + area.id">
                    Add new topic
                </button>
            </div>
            <div :id="'addTopic' +  area.id" class="collapse" :aria-labelledby="'addtopic' + area.id" data-parent="#accordion">
                <TopicEditor :planAreaId="area.id" v-on:topic-added="area.areaTopics.push($event)" />
            </div>
        </div>           
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import { PlanArea } from '../models/plan-area';
    import { Topic } from '../models/topic';
    import TopicEditor from './TopicEditor.vue'
    import TopicDataService from "../services/topic-data-service";
    import AreaDataService from "../services/area-data-service";
    @Component({
        components: {
            TopicEditor
        }
    })
    export default class Area extends Vue {
        private area: PlanArea | any = this.$attrs.area as PlanArea | any;

        private topic: Topic = new Topic();

        constructor() {
            super();
            if (!this.area.areaTopics) {
                this.area.areaTopics = [];
            }
        }

        private deleteArea(area: PlanArea) {
            AreaDataService.deleteArea(area)
                .then(() => {
                    this.$emit("area-deleted", area);
                })
                .catch((error) => {
                    console.error(error);
                });
        }

        private deleteTopic(deletedTopic: Topic, area: PlanArea) {
            const id = deletedTopic.id;
            const index: number = area.areaTopics.indexOf(deletedTopic);
            if (index !== -1) {
                area.areaTopics.splice(index, 1);
            }
            TopicDataService.deleteTopic(id);
        }
    }

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
