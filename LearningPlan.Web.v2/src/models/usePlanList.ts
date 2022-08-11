import { Plan } from "@/models/plan"
import type { PlanListModel } from "@/models/planList-model"
import PlanDataService from "@/services/plan-data-service";
import { reactive } from "vue";

export function usePlanList() {

  retrievePlans()

  const planList: PlanListModel = reactive({
    plans: [],
    newPlanName: '',
    isAddingNew: false,
    isTemplate: false
  });

  function addPlan() {
    if (planList.newPlanName) {
      return;
    }
    PlanDataService.addPlan(planList.newPlanName, planList.isTemplate)
      .then((response) => {
        planList.plans.push(new Plan(response.data.id, response.data.name));
        showPlanCreation();
        planList.newPlanName = '';
      })
      .catch((error) => {
        console.error(error);
      });
  }

  function showPlanCreation() {
    planList.isAddingNew = !planList.isAddingNew;
  }

  function retrievePlans(): void {
    PlanDataService.getPlans()
      .then((response) => {
        planList.plans = response.data;
      })
      .catch((error) => {
        console.error(error);
      })
  }

  return { planList, addPlan, showPlanCreation }
}
