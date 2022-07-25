import axios from "axios";
import httpCommon from "@/http-common";
import { useUserStore } from "@/stores/UserStore"


export default function http() {
  const userStore = useUserStore();
  return axios.create({
    baseURL: httpCommon.baseURL,
    headers: {
      "Content-type": "application/json",
      'Authorization': userStore.token,
    }
  });
}
