import { Topic } from "@/model/topic";

export class PlanArea {
    id: string ="";
    planId: string;
    name: string;
    areaTopics: Topic[] = [];
    constructor(name: string, planId: string) {
        this.planId = planId;
        this.name = name;
    }
}