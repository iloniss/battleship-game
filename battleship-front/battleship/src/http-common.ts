import axios from "axios";

export default axios.create({
  baseURL: "https://localhost:443/api",
  headers: {
    "Content-type": "application/json",
    "Access-Control-Allow-Origin": "*",
  },
});
