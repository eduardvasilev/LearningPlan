import axios from "axios";
import { store } from "./store/index";

export default axios.create({
    baseURL: "https://localhost:44335",
    headers: {
        "Content-type": "application/json",
        'Authorization': store.state.user.token
    }
});

