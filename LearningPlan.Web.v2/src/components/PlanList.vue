<script setup lang="ts">
import { Plan } from "@/models/plan";
import PlanDataService from "@/services/plan-data-service";

class PlanList {
  public plans: Plan[];
  private newPlanName: string;
  private isAddingNew: boolean;
  private isTemplate: boolean;

  constructor() {
    this.isAddingNew = false;
    this.newPlanName = '';
    this.isTemplate = false;
    this.plans = [];
    this.retrievePlans()
  }

  private addPlan() {
    if (!this.newPlanName) {
      return;
    }
    PlanDataService.addPlan(this.newPlanName, this.isTemplate)
      .then((response) => {
        this.plans.push(new Plan(response.data.id, response.data.name));
        this.showPlanCreation();
        this.newPlanName = '';
      })
      .catch((error) => {
        console.error(error);
      });
  }

  private showPlanCreation() {
    this.isAddingNew = !this.isAddingNew;
  }

  private retrievePlans() {
    PlanDataService.getPlans()
      .then((response) => {
        this.plans = response.data;
      })
      .catch((error) => {
        console.error(error);
      })
  }
}

const planList = new PlanList();
</script>

<template>
  <div>
    <h1>Your learning plans</h1>
  </div>
</template>
