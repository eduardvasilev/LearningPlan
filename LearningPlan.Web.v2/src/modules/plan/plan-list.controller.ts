import type { Plan } from "@/modules/plan/classes/plan.class";
import PlanDataService from "@/modules/plan/plan.data.service";

export class PlanListGetterController {
  plans: Promise<Plan[]>
  error: string

  constructor() {
    this.error = '';
    this.plans = this.retrievePlans();
  }

  private async retrievePlans(): Promise<Plan[]> {
    try {
      const planList = await PlanDataService.getPlans();
      return planList.data
    } catch {
      console.log(this.error)
      return []
    }
  }
}
