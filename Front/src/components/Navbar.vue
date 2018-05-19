<template>
  <div>
    <el-menu
      default-active="1"
      class="el-menu-demo"
      mode="horizontal"
      @select="handleSelect">
      <el-menu-item index="1">
        <img
          :src="require('../assets/logo.png')"
          :width="25"
          :height="25"
          style="margin-right: 5px" > AmStramGram
      </el-menu-item>
      <el-menu-item index="2">
        <autocomplete />
      </el-menu-item>
      <el-menu-item index="3">
        <i
          style="font-size: 1.2rem"
          class="fas fa-plus-square fa-2x"/>
      </el-menu-item>
      <el-menu-item index="4">
        <i
          style="font-size: 1.2rem"
          class="fas fa-cog fa-2x"/>
      </el-menu-item>
      <el-menu-item index="5">
        <i
          style="font-size: 1.2rem"
          class="fas fa-heart fa-2x"/>
        <el-dropdown
          v-if="!notifications"
          @command="e => $router.push({name: 'picture', params: { id: e}})">
          <el-dropdown-menu
            slot="dropdown">
            <el-dropdown-item
              v-for="(notification, i) of notifications"
              :key="i"
              :command="notification.picture.id">
              <div
                class="center-vertically">
                <div
                  :style="{ 'background-image': 'url(' + notification.user.picture + ')' }"
                  class="round-icon flex" />
                <div
                  class="flex nickname header black-text">
                  <div>
                    <span class="nickname-text">{{ notification.user.nickname }}</span>
                    <span>
                      {{ notification.type !== "comment" ? " liked "
                      : " commented : " }}
                    </span>
                    <span
                      v-if="notification.type === 'comment'">
                      "<span class="nickname-text">{{ notification.text }}</span>" on
                    </span>
                    your picture,
                    <span class="time">{{ notification.createdAt | fromNow }}</span>
                  </div>
                </div>
              </div>
            </el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </el-menu-item>
      <el-menu-item index="6">
        <i
          style="font-size: 1.2rem"
          class="fas fa-user"/>
      </el-menu-item>
    </el-menu>
    <el-dialog
      :visible.sync="dialogVisible"
      width="80%">
      <upload
        v-if="dialogVisible"
        @uploadDone="dialogVisible = false" />
    </el-dialog>
  </div>
</template>

<script>
import moment from "moment";
import Autocomplete from "./Autocomplete";
import Upload from "../views/Upload.vue";
import _ from "lodash";

export default {
  components: {
    Autocomplete,
    Upload
  },

  filters: {
    fromNow(date) {
      return moment.unix(date).fromNow();
    }
  },
  data() {
    return {
      dialogVisible: false
    };
  },

  computed: {
    notifications() {
      return _.orderBy(
        this.$store.state.notifications,
        ["createdAt"],
        ["desc"]
      );
    }
  },

  methods: {
    handleSelect(key) {
      /*eslint-disable */
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
    /*eslint-enable */
  }
};
</script>

<style scoped>
.nickname {
  text-decoration: none;
  padding-left: 5px;
  color: rgb(232, 0, 63);
}

.black-text {
  color: black;
}

.nickname-text {
  color: rgb(232, 0, 63);
}

.time {
  color: rgb(232, 0, 63);
}

.center-vertically {
  display: flex;
  align-items: center;
  /* flex-direction: column; */
  justify-content: flex-start;
}

.flex {
  display: flex;
}

.pull-right {
  float: right;
}

.pull-left {
  float: left;
}

.round-icon {
  display: inline-block;
  width: 50px;
  height: 50px;
  border-radius: 50%;
  margin-bottom: 5px;
  margin-top: 5px;
  background-repeat: no-repeat;
  background-position: center center;
  background-size: cover;
}

.el-dropdown-menu__item {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
}

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
