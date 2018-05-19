import { query } from "./index";

export async function getLikesAndComments(id) {
  return query(`{
    picture(id: ${id}) {
      comments {
        text
        user {
          nickname
          id
          picture
        }
        createdAt
      }
      likes {
        createdAt
        user {
          id
          nickname
          picture
        }
      }
    }
  }`);
}

export async function getPicture(id) {
  return query(`{
    picture(id: ${id}) {
      id
      image
      description
      color
      createdAt
      updatedAt
      user {
        id
        private
        nickname
        picture
      }
      tags {
        text
      }
      comments {
        text
        user {
          nickname
          id
          picture
        }
        createdAt
      }
      likes {
        createdAt
        user {
          id
          nickname
          picture
        }
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
};

export const createLike = async (pictureId, userId) => {
  return query(`mutation {
    createLike(like: {
      userId: ${userId},
      pictureId: ${pictureId},
    }) {
      createdAt
    }
  }`);
};

export const deleteLike = async (pictureId, userId) => {
  return query(`mutation {
    deleteLike(like: {
      userId: ${userId},
      pictureId: ${pictureId},
    }) {
      createdAt
    }
  }`);
};

export async function createPicture(data) {
  return query(`mutation {
    createPicture(picture: {
      image: "${data.url}",
      description: "${data.description}",
      userId: ${data.id},
      tags: [${data.tags.map(tag => `{text:"${tag}"}`)}],
      color: "${data.color}"
    }) {
      id
    }
  }`);
}
