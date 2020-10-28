import http from "../http-common";
import {store} from "../store/index";
import { PlanArea } from "../models/plan-area";

export class AreaDataService {
 
    public addArea(area: PlanArea) {
        return http.post("/area/", area, { headers: { 'Authorization': store.state.user.token }});
    }

    public deleteArea(area: PlanArea) {
        return http.delete(`/area/${area.id}`, { headers: { 'Authorization': store.state.user.token } });
    }
}

export default new AreaDataService();