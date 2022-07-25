import type { Topic } from "./topic";

export class PlanArea {
  id: string;
  planId: string;
  name: string;
  areaTopics: Topic[];
  constructor(name: string, planId: string) {
    this.id = '';
    this.planId = planId;
    this.name = name;
    this.areaTopics = [];
  }
}
