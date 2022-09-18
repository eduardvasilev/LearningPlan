import type { PlanArea } from "@/modules/plan/classes/plan-area.class"

export class Plan {
  id: string
  name: string
  planAreas: PlanArea[] = []
  isTemplate: boolean = false
  userId: string = ''
  constructor(id: string, name: string) {
    this.id = id
    this.name = name
  }
}
