import Vue from "vue";
import Vuex from "vuex";
import { getUser, getAllCommentsAndLikes } from "@/api/user";

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
    connectUser: async (context, userId) => {
      const { user } = await getUser(userId);
      const notifications = await getAllCommentsAndLikes(userId);
      context.commit("setUser", { user, notifications });
    },
  },
  getters: {
    isConnected: state => !!state.currentUser,
  },
});
