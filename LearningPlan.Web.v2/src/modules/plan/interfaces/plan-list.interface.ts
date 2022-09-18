import type { Plan } from "@/modules/plan/classes/plan.class"

export interface PlanListModel {
  plans: Plan[];
  newPlanName: string;
  isAddingNew: boolean;
  isTemplate: boolean;
}
