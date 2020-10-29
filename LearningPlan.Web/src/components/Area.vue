<template>
    <div>
        <div class="area-header">
            <div v-if="isNameEdit" class="card-body d-flex flex-row">
                <div class="input-group mb-3 p-2">
                    <input id="plan-name"
                           type="text"
                           name="plan-name"
                           class="form-control"
                           v-model="area.name"
                           placeholder="Area name">
                    <button class="input-group-append btn btn-primary btn-outline-secondary" v-on:click="updateName" type="button">OK</button>
                </div>
            </div>
            <div v-else class="card-body d-flex flex-row">
                <div class="p-2">
                    <h3>{{area.name}}</h3>
                </div>
                <div class="p-2">
                    <div class="btn-toolbar" role="toolbar">
                        <div class="btn-group mr-2" role="group">
                            <button class="btn" type="button" v-on:click="showNameUpdate">
                                <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-pencil" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                    <path fill-rule="evenodd" d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5L13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175l-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z" />
                                </svg>
                            </button>
                            <button class="btn" type="button" v-on:click="deleteArea(area)">
                                <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-trash" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                    <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" />
                                    <path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4L4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" />
                                </svg>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="card">
            <div v-for="(areaTopic, index) in area.areaTopics" v-bind:key="areaTopic.id" class="card">
                <Topic :topic="areaTopic" :planAreaId="area.id" v-on:topic-deleted="deletetopic($event)"  />
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
    import TopicComponent from './Topic.vue'
    import TopicDataService from "../services/topic-data-service";
    import AreaDataService from "../services/area-data-service";

    @Component({
        components: {
            TopicEditor,
            Topic: TopicComponent
        }
    })
    export default class Area extends Vue {
        private area: PlanArea | any = this.$attrs.area as PlanArea | any;

        private topic: Topic = new Topic();
        private isNameEdit: boolean = false;
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

        private deletetopic(deletedTopic: Topic) {
            const id = deletedTopic.id;
            const index: number = this.area.areaTopics.indexOf(deletedTopic);
            if (index !== -1) {
                this.area.areaTopics.splice(index, 1);
            }
        }

        private showNameUpdate() {
            this.isNameEdit = !this.isNameEdit;
        }

        public updateName() {
            if (!this.area.name) {
                return;
            }
            AreaDataService.updateArea(this.area)
                .then(() => {
                    this.showNameUpdate();
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
