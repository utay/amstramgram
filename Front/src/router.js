import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
import About from './views/About.vue';
import Profile from './views/Profile.vue';
import User from './views/User.vue';
import ElementUI from 'element-ui';
import InstantSearch from 'vue-instantsearch';
import VueObserveVisibility from 'vue-observe-visibility'

Vue.use(Router);
Vue.use(ElementUI);
Vue.use(InstantSearch);
Vue.use(VueObserveVisibility);

export default new Router({
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
    },
    {
      path: '/profile',
      name: 'profile',
      component: Profile,
    },
    {
      path: '/user/:id',
      name: 'user',
      component: User,
    },
  ],
});
