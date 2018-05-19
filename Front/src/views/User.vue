<template>
  <div v-if="!isLoading">
    <el-row 
      :gutter="10"
      style="width: 50%; margin: 0 auto; padding-bottom: 25px"
      justify="center">
      <el-col :span="8">
        <div 
          :style="{ 'background-image': 'url(' + user.picture+ ')' }"
          class="round-icon" />
      </el-col>
      <el-col :span="16">
        <el-card
          :body-style="{'text-align': 'left'}">
          <div
            slot="header"
            class="center-vertically">
            <span class="flex">{{ user.firstname }} {{ user.lastname }} </span>
            <span class="flex legend">@{{ user.nickname }}</span>
            <div
              v-if="currentUserProfile"
              style="margin-left: auto"
              class="flex">
              <el-button
                round
                size="small"
                style="margin-left: auto"
                type="primary"
                @click="$router.push({name: 'settings'})">Update profile
              </el-button>
              <el-button
                round
                size="small"
                type="primary"
                style="margin-left: auto"
                icon="el-icon-back"/> 
            </div>
            <el-button
              v-else
              :type="followed ? 'primary' : ''"
              round
              style="margin-left: auto"
              icon="el-icon-plus"
              @click="toggleFollowUser(id)">
              {{ followed ? "Unfollow": "Follow" }}
            </el-button>
          </div>
          <div>
            <span>{{ user.pictures.length }}</span> Publications
            <span>{{ user.followers.length }}</span> Abonnés
            <span>{{ user.following.length }}</span> Suivis
          </div>
          <div>
            {{ user.description }}
          </div>
        </el-card>
      </el-col>
    </el-row>
    <el-row
      :gutter="10"
      style="width: 50%; margin: 0 auto;"
      justify="center">
      <el-col
        v-for="(picture, i) in user.pictures"
        :span="8"
        :key="i">
        <img
          :src="picture.image"
          class="image"
          @click="$router.push({name: 'picture', params: { id: picture.id}})">
      </el-col>
    </el-row>
  </div>
</template>

<script>
import { getUser } from "@/api/user";
import {
  followUser,
  unfollowUser,
} from "@/api/user";

export default {
  props: {
    id: {
      type: [Number, String],
      default: 1
    }
  },

  data() {
    return {
      user: {},
      myProfile: false,
      isLoading: true
    };
  },

  computed: {
    currentUserProfile() {
      return this.$store.state.currentUser.id === +this.id;
    },

    followed(){
      return !!this.$store.state.currentUser
        .following.find(user => user.id === this.pictureData.user.id);
    },
  },

  async created() {
    this.isLoading = true;
    if (!this.currentUserProfile) {
      const response = await getUser(this.id);
      this.user = response.user;
    } else {
      this.user = this.$store.state.currentUser;
    }
    this.isLoading = false;
  },

  methods: {   
    async toggleFollowUser(id) {
      if (this.followed) {
        await unfollowUser(id, this.$store.state.currentUser.id);
      } else {
        await followUser(id, this.$store.state.currentUser.id);
      }
      this.$store.dispatch("connectUser");
    }
  }
};
</script>

<style scoped>
.center-vertically {
  display: flex;
  align-items: center;
  /* flex-direction: column; */
  justify-content: flex-start;
}

.legend {
  font-size: 13px;
  color: #999;
}

.image {
  width: 100%;
}

.flex {
  display: flex;
}

.image-profile {
  width: 80%;
  border-radius: 50%;
}

.round-icon {
  display: inline-block;
  width: 150px;
  height: 150px;
  border-radius: 50%;

  background-repeat: no-repeat;
  background-position: center center;
  background-size: cover;
}
</style>
