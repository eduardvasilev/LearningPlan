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
    } catch (error: any) {
      this.error = error;
      return []
    }
  }
}

export class PlanListAdderController {
  async addPlan(newPlanName: string, isTemplate: boolean): Promise<string> {
    try {
      const id: string = (await PlanDataService.addPlan(newPlanName, isTemplate)).data.id
      return id
    } catch (error: any) {
      console.log(error)
      return error
    }
  }
}
