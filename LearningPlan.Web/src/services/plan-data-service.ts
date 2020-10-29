import http from "../http-common";
import { Plan } from "../models/plan";

export class PlanDataService {
    public addPlan(name: string) {
        return http.post("/plan/", { name: name, planAreas: [] });
    }
    public getPlans() {
        return http.get("/plan");
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