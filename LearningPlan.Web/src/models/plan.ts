import {PlanArea} from "./plan-area"
export class Plan {
    id: string;
    name: string;
    planAreas: PlanArea[] = [];
    isTemplate: boolean;
    userId: string = "";
    constructor(id: string, name: string) {
        this.id = id;
        this.name = name;
        this.isTemplate = false;
    }
}