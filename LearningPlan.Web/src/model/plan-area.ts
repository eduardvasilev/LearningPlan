export class PlanArea {
    id: string;
    planId: string;
    name: string;
    constructor(id: string, planId: string, name: string) {
        this.id = id;
        this.planId = planId;
        this.name = name;
    }
}