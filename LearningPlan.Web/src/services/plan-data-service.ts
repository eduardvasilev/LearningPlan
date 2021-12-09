import http from "../http-common";
import { Plan } from "../models/plan";

export class PlanDataService {
    public addPlan(name: string, isTemplate: boolean) {
        return http.post("/plan/", { name: name, planAreas: [] , isTemplate: isTemplate});
    }
    public getPlans() {
        return http.get("/plan");
    }
    public getTemplatePlans() {
        return http.get("/plan/templates");
    }
    public getPlan(id: string) {
        return http.get(`/plan/${id}`);
    }
    public deletePlan(id: string) {
        return http.delete(`/plan/${id}`);
    }
    public updatePlan(plan: Plan) {
        return http.put("/plan", plan);
    }
}

export default new PlanDataService();