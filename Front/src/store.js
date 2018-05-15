import Vue from 'vue';
import Vuex from 'vuex';
import { getUser } from "@/api/user";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    tags: [],
    currentUser: undefined,
  },
  mutations: {
    addTag(state, tag) {
      state.tags = [tag, ...state.tags];
    },
    deleteTag(state, tag) {
      state.tags = state.tags.filter(t => t !== tag);
    },
    setUser(state, user) {
      state.currentUser = user
    },
  },
  actions: {
    addTag: (context, tag) => {
      context.commit('addTag', tag);
    },
    deleteTag(context, tag) {
      context.commit('deleteTag', tag);
    },
    connectUser: async (context, userId) => {
      const { user } = await getUser(userId)
      context.commit('setUser', user)
    },
  },
  getters: {
    tags: state => {
      return state.tags;
    },
    currentUser: state => state.currentUser,
    isConnected: state => !!state.currentUser,
  },
});
