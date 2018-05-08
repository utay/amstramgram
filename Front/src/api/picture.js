import request from "./index";

export async function getLikesAndComments(id) {
  return request(`{
    picture(id: ${id}) {
      comments {
        text
      }
      likes {
        createdAt
      }
    }
  }`);
}
