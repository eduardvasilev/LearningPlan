import type { Plan } from "@/models/plan"

export interface PlanListModel {
  plans: Plan[];
  newPlanName: string;
  isAddingNew: boolean;
  isTemplate: boolean;
}
