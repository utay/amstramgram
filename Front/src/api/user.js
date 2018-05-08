import request from "./index";

export async function getUser(id) {
  return request(`{
    user(id: ${id}) {
      id
      nickname
      email
      password
      firstname
      lastname
      phone
      gender
      description
      private
    }
  }`);
}
