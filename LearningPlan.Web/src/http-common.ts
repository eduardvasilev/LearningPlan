import axios from "axios";
import { store } from "./store/index";

export default axios.create({
    baseURL: process.env.VUE_APP_WEB_API_BASE_URL,
    headers: {
        "Content-type": "application/json",
        'Authorization': store.state.user.token
    }
});

