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
