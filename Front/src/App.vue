<template>
  <div id="app">
    <el-container>
      <el-header>
        <Navbar/>
      </el-header>
      <el-main
        v-loading.fullscreen.lock="!$store.getters.isConnected">
        <router-view
          v-if="$store.getters.isConnected"/>
      </el-main>
    </el-container>
  </div>
</template>

<script>
import store from "@/store";
import Navbar from "./components/Navbar.vue";

export default {
  components: {
    Navbar
  },

  async created() {
    // store.dispatch("createUser");
    store.dispatch("connectUser");
  }
};
</script>

<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}
</style>
