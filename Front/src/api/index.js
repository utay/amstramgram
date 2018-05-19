import axios from "axios";

export async function query(query) {
  const response = await axios.post("https://amstramgram.insideapp.io/graphql", {
    query
  }, {
    withCredentials: true,
  });
  return response.data.data;
}
