import axios from "axios";

export const HTTPClient = axios.create({
    baseURL: "http://localhost:5104",
    withCredentials: true,
    headers: {
        "Content-Type": "application/json;charset=UTF-8"
    }
});
