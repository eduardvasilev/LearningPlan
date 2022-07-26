import { defineStore } from 'pinia'

export const usePlanStore = defineStore("PlanStore", {
  state: () => ({ hasEditPermission: false }),

  actions: {
    PlanOpened(hasEditPermission: boolean): void {
      this.hasEditPermission = hasEditPermission;
    },
  }
});
