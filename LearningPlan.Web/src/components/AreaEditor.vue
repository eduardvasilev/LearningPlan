<template>
    <div>
        <div class="modal fade" id="addAreaModel" ref="modalElement" tabindex="-1" role="dialog" aria-labelledby="areaModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="areaModalLabel">Create area</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <input id="area-name"
                               type="text"
                               name="area-name"
                               class="form-control"
                               v-model="newAreaName"
                               placeholder="Area name">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal" v-on:click="addArea">Save</button>
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
      
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';

    import AreaDataService from "../services/area-data-service";
    import { PlanArea } from '../models/plan-area';

    @Component
    export default class AreaEditor extends Vue {

        public planId: string = this.$attrs.planId;
        private newAreaName: string = "";

        private addArea() {
            AreaDataService.addArea(new PlanArea(this.newAreaName, this.$attrs.planId))
                .then((response) => {
                    this.$emit("area-added", response.data);
                    this.newAreaName = "";
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
