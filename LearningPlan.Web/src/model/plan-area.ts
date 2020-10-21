import { Topic } from "@/model/topic";

export class PlanArea {
    id: string;
    planId: string;
    name: string;
    areaTopics: Topic[] = [];
    constructor(id: string, planId: string, name: string) {
        this.id = id;
        this.planId = planId;
        this.name = name;
    }
}