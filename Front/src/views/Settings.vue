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
          @select="handleOpen">
          <el-menu-item index="1">
            <i class="el-icon-setting"/>
            <span slot="title">
              Update profile
            </span>
          </el-menu-item>
          <el-menu-item 
            index="2">
            <i class="el-icon-edit"/>
            <span
              slot="title">
              Change password
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
import PasswordSettings from "../components/PasswordSettings.vue";

export default {
  components: {
    ProfileSettings,
    PasswordSettings
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
    handleOpen(key) {
      /* eslint-disable */
      switch (key) {
        case "1":
          this.current = "profile-settings";
          break;
        case "2":
          this.current = "password-settings";
          break;
        default:
          break;
      }
    }
  }
};
</script>