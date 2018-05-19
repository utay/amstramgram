import Vue from "vue";
import Vuex from "vuex";
import { getCurrentUser, getAllCommentsAndLikes, createUser } from "@/api/user";
import { getUser } from "./api/user";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    tags: [],
    currentUser: undefined,
    notifications: []
  },
  mutations: {
    addTag(state, tag) {
      state.tags = [tag, ...state.tags];
    },
    deleteTag(state, tag) {
      state.tags = state.tags.filter(t => t !== tag);
    },
    setUser(state, payload) {
      state.currentUser = payload.user;
      state.notifications = payload.notifications;
    },
  },
  actions: {
    addTag: (context, tag) => {
      context.commit("addTag", tag);
    },
    deleteTag(context, tag) {
      context.commit("deleteTag", tag);
    },
    createUser: async () => {
      await createUser();
    },
    connectUser: async (context, ) => {
      const me = await getCurrentUser();
      const { user } = await getUser(me.id);
      const notifications = await getAllCommentsAndLikes(user.id);
      context.commit("setUser", { user, notifications });
    },
  },
  getters: {
    isConnected: state => !!state.currentUser,
  },
});
