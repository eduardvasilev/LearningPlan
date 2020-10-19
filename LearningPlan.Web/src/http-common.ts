import axios from "axios";

export default axios.create({
    baseURL: "https://localhost:44335",
    headers: {
        "Content-type": "application/json"
    }
});