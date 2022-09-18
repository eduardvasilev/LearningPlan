import type { Topic } from "@/modules/topic/classes/topic.class";

export class PlanArea {
  id: string = '';
  planId: string;
  name: string;
  areaTopics: Topic[] = [];
  constructor(name: string, planId: string) {
    this.planId = planId;
    this.name = name;
  }
}
