import axios from "axios";

export default async function request(query) {
  const response = await axios.post("http://localhost:5000/graphql", {
    query
  });
  return response.data.data;
}
