<template>
    <div>
        <div>
            <div class="jumbotron">
                    <div v-if="isNameEdit" class="card-body d-flex flex-row">
                        <div class="input-group mb-3 p-2">
                            <input id="plan-name"
                                   type="text"
                                   name="plan-name"
                                   class="form-control"
                                   v-model="plan.name"
                                   placeholder="Plan name">
                            <button class="input-group-append btn btn-primary btn-outline-secondary" v-on:click="updateName" type="button">OK</button>
                        </div>
                    </div>
                    <div v-else class="card-body d-flex flex-row">
                        <div class="p-2">
                            <h3 class="display-4">{{plan.name}}</h3>
                        </div>
                        <div class="p-2">
                            <div class="btn-toolbar" role="toolbar">
                                <div class="btn-group mr-2" role="group">
                                    <button  v-if="isCurrentUserOwner"  class="btn" type="button" v-on:click="showNameUpdate">
                                        <svg width="2em" height="2em" viewBox="0 0 16 16" class="bi bi-pencil" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                            <path fill-rule="evenodd" d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5L13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175l-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z" />
                                        </svg>
                                    </button>
                                    <button  v-if="isCurrentUserOwner"  class="btn" type="button" v-on:click="deletePlan">
                                        <svg width="2em" height="2em" viewBox="0 0 16 16" class="bi bi-trash" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" />
                                            <path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4L4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" />
                                        </svg>
                                    </button>
                                     <button  v-if="plan.isTemplate"  class="btn" type="button" v-on:click="copyPlan">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="2em" height="2em" fill="currentColor" class="bi bi-clipboard-plus" viewBox="0 0 16 16">
                                            <path fill-rule="evenodd" d="M8 7a.5.5 0 0 1 .5.5V9H10a.5.5 0 0 1 0 1H8.5v1.5a.5.5 0 0 1-1 0V10H6a.5.5 0 0 1 0-1h1.5V7.5A.5.5 0 0 1 8 7z"/>
                                            <path d="M4 1.5H3a2 2 0 0 0-2 2V14a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V3.5a2 2 0 0 0-2-2h-1v1h1a1 1 0 0 1 1 1V14a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V3.5a1 1 0 0 1 1-1h1v-1z"/>
                                            <path d="M9.5 1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5h3zm-3-1A1.5 1.5 0 0 0 5 1.5v1A1.5 1.5 0 0 0 6.5 4h3A1.5 1.5 0 0 0 11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3z"/>
                                        </svg>
                                    </button>
                                    <a class="btn"  v-if="!plan.isTemplate" target="_blank" :href="'https://t.me/learningplanbot?start=' + plan.id">
                                        <svg width="2em" height="2em"  xmlns="http://www.w3.org/2000/svg" viewBox="0 0 496 512"><path fill="currentColor" d="M248 8C111 8 0 119 0 256s111 248 248 248 248-111 248-248S385 8 248 8zm121.8 169.9l-40.7 191.8c-3 13.6-11.1 16.9-22.4 10.5l-62-45.7-29.9 28.8c-3.3 3.3-6.1 6.1-12.5 6.1l4.4-63.1 114.9-103.8c5-4.4-1.1-6.9-7.7-2.5l-142 89.4-61.2-19.1c-13.3-4.2-13.6-13.3 2.8-19.7l239.1-92.2c11.1-4 20.8 2.7 17.2 19.5z"></path></svg>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
            </div>
            
        </div>
        
        <div id="accordion">
            <div v-for="(area, index) in plan.planAreas" v-bind:key="area.id">
                <PlanArea :area="area" :isTemplate="plan.isTemplate" v-on:area-deleted="onareadeleted($event)" />
            </div>
          <div  v-if="isCurrentUserOwner">
                <a data-toggle="modal" data-target="#addAreaModel">
                <div class="card-body d-flex justify-content-center new-area">
                    <svg width="4em" height="4em" viewBox="0 0 16 16" class="bi bi-plus" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                        <path fill-rule="evenodd" d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z" />
                    </svg>
                </div>
            </a>
        </div>
        </div>
        <AreaEditor :planId="plan.id" :isTemplate="plan.isTemplate" v-on:area-added="plan.planAreas.push($event)" />
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import { PlanArea } from '../models/plan-area';
    import { Plan } from '../models/plan';
    import PlanDataService from "../services/plan-data-service";
    import Area from './Area.vue';
    import AreaEditor from './AreaEditor.vue';
import { store } from '@/store';

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
            planAreas: [],
            isTemplate: false,
            userId: ""
        };

        private isNameEdit: boolean = false;
        private isCurrentUserOwner: boolean = false;

        constructor() {
            super();
            this.retrievePlan();
        }

         private copyPlan() {
            PlanDataService.copyTemplatePlan(this.plan.id)
                .then(() => {
                    this.$router.push('/');
                })
                .catch((error) => {
                    console.error(error);
                });
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
                    this.isCurrentUserOwner = this.plan.userId == this.$store.state.user.userId;
                    this.$store.commit("PlanOpened",  this.isCurrentUserOwner);

                })
                .catch((error) => {
                    console.error(error);
                });
        }

        private showNameUpdate() {
            this.isNameEdit = !this.isNameEdit;
        }

        public updateName() {
            if (!this.plan.name) {
                return;
            }
            PlanDataService.updatePlan(this.plan)
                .then((response) => {
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
    .new-area {
        margin-top: 20px;
        margin-bottom: 20px;
        border: 2px dashed;
        border-color: #fefefe;
        box-shadow: 0px 0px 20px -3px #000000;
    }
</style>
