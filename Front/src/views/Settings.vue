<template>
  <el-card 
    :body-style="{'text-align': 'left'}"
    style="
    margin: 0 auto;
    width: 60%;  
    ">
    <el-row>
      <el-col :span="8">
        <el-menu
          default-active="1" 
          class="el-menu-vertical-demo" 
          @open="handleOpen">
          <el-menu-item index="1">
            <i class="el-icon-setting"/>
            <span slot="title">
              Modifier le profil
            </span>
          </el-menu-item>
          <el-menu-item 
            index="2" 
            disabled>
            <i class="el-icon-edit"/>
            <span
              slot="title">
              Changer le mot de passe
            </span>
          </el-menu-item>
          <el-menu-item
            index="3">
            <i class="el-icon-bell"/>
            <span
              slot="title">
              Notifications
            </span>
          </el-menu-item>
        </el-menu>
      </el-col>
      <el-col :span="16">
        <component :is="current"/>
      </el-col>
    </el-row>
  </el-card>
</template>

<style>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  /* width: 300px; */
  min-height: 600px;
}
</style>

<script>
import ProfileSettings from "../components/ProfileSettings.vue";

export default {
  components: {
    ProfileSettings,
  },
  data() {
    return {
      current: "profile-settings",
      form: {
        name: "",
        region: "",
        date1: "",
        date2: "",
        delivery: false,
        type: [],
        resource: "",
        desc: ""
      }
    };
  },
  methods: {
    handleOpen(key, keyPath) {

      console.log(key, keyPath);
    },
    handleClose(key, keyPath) {
      console.log(key, keyPath);
    }
  }
};
</script>