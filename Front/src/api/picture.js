import { query } from "./index";

export async function getLikesAndComments(id) {
  return query(`{
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

export const createComment = async (comment, pictureId, userId) => {
  return query(`mutation {
    createComment(comment: {
      text: "${comment}",
      userId: ${userId},
      pictureId: ${pictureId},
    }) {
      id
    }
  }`); 
}

export async function createPicture(data) {
  return query(`mutation {
    createPicture(picture: {
      image: "${data.url}",
      description: "${data.description}",
      userId: ${1},
      tags: [${data.tags.map(tag => `{text:"${tag}"}`)}],
      color: "${data.color}"
    }) {
      id
    }
  }`);
}
