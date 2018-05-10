import axios from "axios";

export async function query(query) {
  const response = await axios.post("http://localhost:5000/graphql", {
    query
  });
  return response.data.data;
}
