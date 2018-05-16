<template>
  <div>
    <el-menu default-active="1"
      class="el-menu-demo"
      mode="horizontal"
      @select="handleSelect">
      <el-menu-item index="1">
        <img :src="require('../assets/logo.png')"
          :width="25"
          :height="25"
          style="margin-right: 5px" /> AmStramGram
      </el-menu-item>
      <el-menu-item index="2">
        <autocomplete />
      </el-menu-item>
      <el-menu-item index="3">
        <i style="font-size: 1.2rem"
          class="fas fa-plus-square fa-2x"></i>
      </el-menu-item>
      <el-menu-item index="4">
        <i style="font-size: 1.2rem"
          class="fas fa-cog fa-2x"></i>
      </el-menu-item>
      <el-menu-item index="5">
        <i style="font-size: 1.2rem"
          class="fas fa-heart fa-2x"></i>
      </el-menu-item>
      <el-menu-item index="6">
        <i style="font-size: 1.2rem"
          class="fas fa-user"></i>
      </el-menu-item>
    </el-menu>
    <el-dialog
      :visible.sync="dialogVisible"
      width="80%">
      <upload v-if="dialogVisible" @uploadDone="dialogVisible = false"/>
    </el-dialog>
  </div>
</template>

<script>
import Autocomplete from "./Autocomplete";
import Upload from "../views/Upload.vue";

export default {
  data() {
    return {
      dialogVisible: false
    };
  },

  components: {
    Autocomplete,
    Upload,
  },

  methods: {
    handleSelect(key, keyPath) {
      switch (key) {
        case "2":
          break;
        case "3":
          this.dialogVisible = true;
          // this.$router.push({ name: "upload" });
          break;
        case "4":
          this.$router.push({ name: "settings" });
          break;
        case "5":
          break;
        case "6":
          this.$router.push({ name: "profile" });
          break;
        default:
          this.$router.push({ name: "home" });
          break;
      }
    }
  }
};
</script>

<style scoped>
.el-menu-item:nth-child(3) {
  position: absolute;
  right: 180px;
}

.el-menu-item:nth-child(4) {
  position: absolute;
  right: 120px;
}

.el-menu-item:nth-child(5) {
  position: absolute;
  right: 60px;
}

.el-menu-item:last-child {
  position: absolute;
  right: 0;
}
</style>
