<template>
  <div v-if="!isLoading">
    <el-row :gutter="10"
      style="width: 50%; margin: 0 auto; padding-bottom: 25px"
      justify="center">
      <el-col :span="8">
        <img :src="user.picture" class="image-profile">
      </el-col>
      <el-col :span="16">
        <el-card :body-style='{"text-align": "left"}'>
          <div slot="header"
            style="text-align: left;">
            <span>{{ user.firstname }} {{ user.lastname }}</span>
            <el-button type="primary"
              style="float: right; padding: 4px 0"
              icon="el-icon-back"></el-button>
            <el-button v-if="myProfile" style="float: right; padding: 3px 10px"
              type="text">Update profile</el-button>
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
    <el-row :gutter="10"
      style="width: 50%; margin: 0 auto;"
      justify="center">
      <el-col :span="8" v-for="(picture, i) in user.pictures" :key="i">
        <img :src="picture.image" class="image">
      </el-col>
    </el-row>
  </div>
</template>

<script>
import { getUser } from '@/api/user';

export default {
  props: {
    id: Number,
  },

  data() {
    return {
      user: {},
      myProfile: false,
      isLoading: true,
    };
  },

  async created() {
    this.myProfile = this.id !== undefined;
    const id = this.myProfile ? this.id : this.$route.params.id;
    const response = await getUser(id);
    this.user = response.user;
    this.isLoading = false;
  },
};
</script>

<style scoped>
.image {
  width: 100%;
}

.image-profile {
  width: 80%;
  border-radius: 50%;
}
</style>
