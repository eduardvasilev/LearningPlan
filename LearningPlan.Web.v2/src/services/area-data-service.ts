import http from "./create-request";
import type { PlanArea } from "../models/plan-area";

export class AreaDataService {

    public addArea(area: PlanArea) {
        return http().post("/area/", area);
    }

    public deleteArea(area: PlanArea) {
        return http().delete(`/area/${area.id}`);
    }

    public updateArea(area: PlanArea) {
        return http().put("/area/", area);
    }

}

export default new AreaDataService();
