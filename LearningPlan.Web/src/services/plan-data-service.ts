import http from "../http-common";
import {store} from "../store/index";

export class PlanDataService {
    public addPlan(name: string) {
        return http.post("/plan/", { name: name, planAreas: [] }, { headers: { 'Authorization': store.state.user.token } });
    }
    public getPlans() {
        return http.get("/plan", { headers: { 'Authorization': store.state.user.token }});
    }
    public getPlan(id: string) {
        return http.get(`/plan/${id}`, { headers: { 'Authorization': store.state.user.token }});
    }
    public deletePlan(id: string) {
        return http.delete(`/plan/${id}`, { headers: { 'Authorization': store.state.user.token }});
    }
}

export default new PlanDataService();