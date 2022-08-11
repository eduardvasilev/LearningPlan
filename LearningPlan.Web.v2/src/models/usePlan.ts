import type { Plan } from "@/models/plan"
import type { PlanArea } from "@/models/plan-area";
import { useUserStore } from "@/stores/UserStore";
import { usePlanStore } from "@/stores/PlanStore";
import { useRouter } from 'vue-router';
import { useRoute } from 'vue-router';
import { reactive, type Ref, ref } from "vue"
import PlanDataService from "@/services/plan-data-service"



export function usePlan() {

  const router = useRouter();
  const route = useRoute();


  const plan: Plan = reactive({
      id: "",
      name: "",
      planAreas: [],
      isTemplate: false,
      userId: ""
  });


  const isNameEdit: Ref<boolean> = ref(false);
  const isCurrentUserOwner: Ref<boolean> = ref(false);


   function copyPlan() {
      PlanDataService.copyTemplatePlan(plan.id)
          .then(() => {
              router.push('/');
          })
          .catch((error) => {
              console.error(error);
          });
  }

  function deletePlan() {
      PlanDataService.deletePlan(plan.id)
          .then(() => {
              router.go(-1);
          })
          .catch((error) => {
              console.error(error);
          });
  }

  function onAreaDeleted(area: PlanArea) {
      const index: number = plan.planAreas.indexOf(area);
      if (index !== -1) {
          plan.planAreas.splice(index, 1);
      }
  }

  function retrievePlan() {
      const planId = route.params.id as string;
      PlanDataService.getPlan(planId)
          .then((response) => {
              plan.id = response.data.id;
              plan.name = response.data.name;
              plan.planAreas = response.data.planAreas;
              plan.isTemplate = response.data.isTemplate;
              plan.userId = response.data.userId;
              isCurrentUserOwner.value = plan.userId == useUserStore().userId;
              usePlanStore().PlanOpened(isCurrentUserOwner.value);

          })
          .catch((error) => {
              console.error(error);
          });

  }

  function showNameUpdate() {
      isNameEdit.value = !isNameEdit.value;
  }

  function updateName() {
      if (!plan.name) {
          return;
      }
      PlanDataService.updatePlan(plan)
          .then((response) => {
              showNameUpdate();
          })
          .catch((error) => {
              console.error(error);
          });
  }
  retrievePlan()


  return { plan, updateName, onAreaDeleted, deletePlan, copyPlan }
}
