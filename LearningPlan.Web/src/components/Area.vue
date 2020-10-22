<template>
    <div>
        <div class="card">
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
            <div class="card-header" :id="'addtopic' + area.id">
                <h5 class="mb-0">
                    <button class="btn btn-link" data-toggle="collapse" :data-target="'#addTopic' + area.id" aria-expanded="true" :aria-controls="'addTopic' + area.id">
                        Add new topic
                    </button>
                </h5>
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
    }

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
