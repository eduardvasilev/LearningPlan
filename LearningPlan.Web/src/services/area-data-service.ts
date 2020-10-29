import http from "../http-common";
import { PlanArea } from "../models/plan-area";

export class AreaDataService {
 
    public addArea(area: PlanArea) {
        return http.post("/area/", area);
    }

    public deleteArea(area: PlanArea) {
        return http.delete(`/area/${area.id}`);
    }
}

export default new AreaDataService();