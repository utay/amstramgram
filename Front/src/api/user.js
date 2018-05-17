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
        id
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

export const createUser = async () => {
  return await query(`mutation {
    createUser(user:{
      nickname:"utay",
      email:"yannickutard@gmail.com",
      password:"password",
      firstname:"Yannick",
      lastname:"Utard",
      picture:"http://cdn-europe1.new2.ladmedia.fr/var/europe1/storage/images/le-lab/francois-hollande-ne-va-pas-partir-aux-champignons-fin-mai-selon-jean-christophe-cambadelis-2980766/33011039-1-fre-FR/Francois-Hollande-ne-va-pas-partir-aux-champignons-fin-mai-selon-Jean-Christophe-Cambadelis.jpg",
      phone:"06 tamere",
      gender:"male",
      description:"Curious and passionate developer. Student at EPITA.",
      private:true
    }) {
      nickname
    }
  }`);
};

export const getAllCommentsAndLikes = async (id) => {
  const { user } = await query(`{
    user(id: ${id}) {
      pictures {
        id
        image 
        comments {
          text
          createdAt
          user {
            id
            nickname
            picture
          }  
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
    }
  }`);

  const res = [];
  user.pictures.forEach(picture => {
    picture.comments.forEach(comment => {
      res.push({
        type: "comment",
        ...comment,
        picture,
      });
    });
    picture.likes.forEach(comment => {
      res.push({
        type: "like",
        ...comment,
        picture
      });
    });
  });
  return res;
};
