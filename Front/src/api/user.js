import { query } from "./index";

export async function getUser(id) {
  return query(`{
    user(id: ${id}) {
      id
      nickname
      email
      password
      firstname
      lastname
      picture
      phone
      gender
      description
      private
      pictures {
        image
      }
      followers {
        user {
          id
        }
      }
      following {
        user {
          id
        }
      }
    }
  }`);
}


export const getAllCommentsAndLikes = async (id) => {
  const { user } = await query(`{
    user(id: ${id}) {
      pictures {
        image 
        comments {
          text
          createdAt
        }
        likes {
          createdAt
      
        }
      }
    }
  }`);

  const res = [];
  user.pictures.forEach(picture => {
    picture.comments.forEach(comment => {
      res.push({
        type: "comment",
        ...comment,
        picture
      });
    });
    picture.likes.forEach(comment => {
      res.push({
        type: "likes",
        ...comment,
        picture
      });
    });
  });
  return res;
};
