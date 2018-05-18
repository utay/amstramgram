import axios from "axios";

export async function query(query) {
  const response = await axios.post("https://amstramgram.insideapp.io/graphql", {
    query
  });
  return response.data.data;
}
