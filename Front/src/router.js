import Vue from "vue";
import Router from "vue-router";
import Home from "./views/Home.vue";
import Profile from "./views/Profile.vue";
import User from "./views/User.vue";
import ElementUI from "element-ui";
import InstantSearch from "vue-instantsearch";
import VueObserveVisibility from "vue-observe-visibility";
import PictureCard from "./components/PictureCard.vue";
import Settings from "./views/Settings.vue";

Vue.use(Router);
Vue.use(ElementUI);
Vue.use(InstantSearch);
Vue.use(VueObserveVisibility);

const router = new Router({
  routes: [
    {
      path: "/feed",
      name: "home",
      component: Home,
    },
    {
      path: "/profile",
      name: "profile",
      component: Profile,
    },
    {
      path: "/user/:id",
      name: "user",
      component: User,
    },
    {
      path: "/settings",
      name: "settings",
      component: Settings,
    },
    {
      path: "/picture/:id",
      name: "picture",
      component: PictureCard,
      props: true,
    },
  ],
});

export default router;
