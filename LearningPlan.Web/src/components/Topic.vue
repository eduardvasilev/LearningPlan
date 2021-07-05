<template>
    <div :class="status">
        <div class="card-header topic-header" :id="'headingOne' + topic.id">
            <h5 class="mb-0">
                <button class="btn btn-link" data-toggle="collapse" :data-target="'#p' + topic.id" aria-expanded="true" :aria-controls="'p' + topic.id">
                    {{topic.name}}
                </button>
            </h5>
        </div>
        <div :id="'p' + topic.id" class="collapse topic-body" :aria-labelledby="'headingOne' + topic.id" data-parent="#accordion">
            <div v-if="isEdit && isCurrentUserOwner">
                <TopicEditor :topic="topic" :planAreaId="planAreaId" v-on:topic-updated="onEdit" />
                <div class=" d-flex flex-row">
                    <div class="p-2">
                        <div class="btn-toolbar" role="toolbar">
                            <div class="btn-group mr-2" role="group">
                                <button class="btn" type="button" v-on:click="onEdit">
                                    <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-pencil" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                        <path fill-rule="evenodd" d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5L13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175l-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z" />
                                    </svg>
                                </button>
                                <button class="btn" type="button" v-on:click="deleteTopic(topic)">
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
            <div v-else>
                <div class="card-body container">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="source">Source: </label>
                                <div class="form-group">
                                    <a :href="topic.source" target="_blank">{{topic.source}}</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="description">Description: </label>
                                <p>{{topic.description}}</p>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="from">From: </label>
                                {{ locateStartDate }}
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="endDate">To: </label>
                                {{locateEndDate}}
                            </div>

                        </div>
                    </div>
                </div>
                <div v-if="isCurrentUserOwner" class=" d-flex flex-row">
                    <div class="p-2">
                        <div class="btn-toolbar" role="toolbar">
                            <div class="btn-group mr-2" role="group">
                                <button class="btn" type="button" v-on:click="onEdit">
                                    <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-pencil" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                        <path fill-rule="evenodd" d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5L13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175l-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z" />
                                    </svg>
                                </button>
                                <button class="btn" type="button" v-on:click="deleteTopic(topic)">
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
        </div>        
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import TopicDataService from "../services/topic-data-service";
    import { Topic } from '../models/topic';

    import TopicEditor from './TopicEditor.vue'

    @Component({
        components: {
            TopicEditor,
        }
    })
    export default class TopicComponent extends Vue {
        private topic: Topic | any = this.$attrs.topic as Topic | any;
        public planAreaId: string = this.$attrs.planAreaId;
        private isEdit: boolean = false;
        private isCurrentUserOwner: boolean = this.$store.state.plan.hasEditPermission;

        private deleteTopic(deletedTopic: Topic) {
            TopicDataService.deleteTopic(deletedTopic.id)
                .then(() => {
                    this.$emit("topic-deleted", deletedTopic);
                })
                .catch((error) => {
                    console.error(error);
                });;
        }

        get locateStartDate() {
            return (new Date(this.topic.startDate)).toDateString();
        }

        get locateEndDate() {
            return (new Date(this.topic.endDate)).toDateString();
        }

        get status() {

            const startDate = new Date(this.topic.startDate);
            const endDate = new Date(this.topic.endDate);
            const today = new Date();
            if (startDate > today) {
                return "in-future";
            } else if (startDate <= today && endDate >= today) {
                return "in-progress";
            }
            else {
                return "passed";
            }
        }

        private onEdit() {
            this.isEdit = !this.isEdit;
        }
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .in-future{
        
    }
    .in-progress {
        /*        background-color: #ab777c;*/
        background-color: #429842;
        border: 2px solid;
        border-color: #fefefe;
        box-shadow: 0px 0px 20px -3px #000000;
    }
    .passed {
        background-color: #429842;
    }
</style>
