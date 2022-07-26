import createRequest from "./create-request";

import type { Plan } from "@/models/plan";

export class PlanDataService {
    public addPlan(name: string, isTemplate: boolean) {
        return createRequest().post("/plan/", { name: name, planAreas: [], isTemplate: isTemplate });
    }
    public getPlans() {
        return createRequest().get("/plan");
    }
    public getTemplatePlans() {
        return createRequest().get("/plan/templates");
    }
    public getPlan(id: string) {
        return createRequest().get(`/plan/${id}`);
    }
    public deletePlan(id: string) {
        return createRequest().delete(`/plan/${id}`);
    }
    public updatePlan(plan: Plan) {
        return createRequest().put("/plan", plan);
    }
    public copyTemplatePlan(id: string) {
        return createRequest().post(`/plan/copy/${id}`);
    }
}

export default new PlanDataService();
