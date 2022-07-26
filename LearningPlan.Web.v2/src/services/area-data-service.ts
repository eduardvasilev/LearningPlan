import createRequest from "./create-request";
import type { PlanArea } from "../models/plan-area";

export class AreaDataService {

    public addArea(area: PlanArea) {
        return createRequest().post("/area/", area);
    }

    public deleteArea(area: PlanArea) {
        return createRequest().delete(`/area/${area.id}`);
    }

    public updateArea(area: PlanArea) {
        return createRequest().put("/area/", area);
    }

}

export default new AreaDataService();
