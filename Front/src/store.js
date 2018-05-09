import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    tags: [],
  },
  mutations: {
    addTag(state, tag) {
      state.tags = [tag, ...state.tags];
    },
    deleteTag(state, tag) {
      state.tags = state.tags.filter(t => t !== tag);
    },
  },
  actions: {
    addTag: (context, tag) => {
      context.commit('addTag', tag);
    },
    deleteTag(context, tag) {
      context.commit('deleteTag', tag);
    }
  },
  getters: {
    tags: state => {
      return state.tags;
    }
  },
});
