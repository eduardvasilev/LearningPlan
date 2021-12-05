<template>
    <div class="container">
        <div class="card-body">
            <form>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="username">Name</label>
                            <input :id="planAreaId + 'topic-name'"
                                   v-model="topic.name"
                                   type="text"
                                   name="name"
                                   class="form-control"
                                   placeholder="Enter topic name">
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="source">Source</label>
                            <input :id="planAreaId + 'topic-source'"
                                   v-model="topic.source"
                                   type="url"
                                   name="source"
                                   class="form-control"
                                   placeholder="Enter topic source">
                        </div>
                    </div>
                </div>
                <div class="row" v-if="!isTemplate">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="from">From</label>
                            <input :id="planAreaId + 'topic-from'"
                                   v-model="topic.startDate"
                                   type="date"
                                   name="from"
                                   class="form-control"
                                   placeholder="Enter topic start date">
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="endDate">To</label>
                            <input :id="planAreaId + 'topic-end'"
                                   v-model="topic.endDate"
                                   type="date"
                                   name="endDate"
                                   class="form-control"
                                   placeholder="Enter topic end date">
                        </div>

                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label for="description">Description</label>
                            <textarea :id="planAreaId + 'topic-description'"
                                      v-model="topic.description"
                                      name="from"
                                      class="form-control"
                                      placeholder="Enter Description" />
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <input class="btn btn-primary" v-on:click="createOrUpdate"
                           :value="topic.id ? 'Update' : 'Create'">
                </div>
            </form>
            </div>
        </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import TopicDataService from "../services/topic-data-service";
    import { Topic } from '../models/topic';
    @Component

    export default class TopicEditor extends Vue {
        public planAreaId: string = this.$attrs.planAreaId;
        private isTemplate: boolean = this.$attrs.isTemplate as boolean | any;

        private isTemplatePlan: boolean = false;
        private topic: Topic = {
            id: "",
            name: "",
            planAreaId: this.planAreaId,
            endDate: "",
            source: "",
            startDate: "",
            isTemplate: this.isTemplate
        };

        constructor() {
            super();

            if (this.$attrs.topic) {
                this.topic = this.$attrs.topic as any;
            }
        }

        private createOrUpdate() {
            if (this.topic.id) {
                TopicDataService.update(this.topic)
                    .then((response) => {
                        this.$emit("topic-updated", this.topic);
                    })
                    .catch((error) => {
                        console.error(error);
                    });
            }
            else {
                TopicDataService.addTopic(this.topic)
                    .then((response) => {
                        this.topic.id = response.data.id;
                        this.$emit("topic-added", this.topic);
                        this.topic = {
                            id: response.data.id,
                            name: "",
                            planAreaId: this.planAreaId,
                            endDate: "",
                            source: "",
                            startDate: "",
                            isTemplate: this.isTemplate
                        };
                    })
                    .catch((error) => {
                        console.error(error);
                    });
            }
        }
    }

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
