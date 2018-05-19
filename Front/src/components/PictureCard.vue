<template>
  <el-card
    v-if="pictureData && (pictureData.user.id === $store.state.currentUser.id || !pictureData.user.private)"
    :style="`
    margin: 0 auto;
    width: 40%;
    margin-bottom: 20px;
    border-top: 2px solid #${pictureData.color}`"
    :body-style="{'text-align': 'left'}">
    <div
      slot="header"
      class="center-vertically">
      <div
        :style="{ 'background-image': 'url(' + pictureData.user.picture + ')' }"
        class="round-icon flex" />
      <router-link
        :to="{name: 'user', params: { id: pictureData.user.id}}"
        class="flex nickname header">
        {{ pictureData.user.nickname }}
      </router-link>
      <el-button
        :type="followed ? 'primary' : ''"
        round
        style="margin-left: auto"
        icon="el-icon-plus"
        @click="toggleFollowUser(pictureData.user.id)">
        {{ followed ? "Unfollow": "Follow" }}
      </el-button>
    </div>
    <img
      :src="pictureData.image"
      class="image">
    <div style="padding: 14px;">
      <el-button
        :icon="`el-icon-circle-check${liked ? '' : '-outline'}`"
        circle
        @click="toggleLikePicture"/>
      <el-button 
        type="text"
        style=""
        @click="likesVisible = true">
        {{ likesPhrase }}
      </el-button>
      <div>
        <router-link
          :to="{name: 'user', params: { id: pictureData.user.id}}"
          class="nickname">
          <span>{{ pictureData.user.nickname }}</span>
        </router-link>
        <span class="legend">{{ pictureData.description }} </span>
        <el-button
          v-for="(tag, i) in pictureData.tags"
          :key="i"
          class="tags"
          type="text">
          #{{ tag.text }}
        </el-button>
        <span class="time pull-right">{{ pictureData.createdAt | fromNow }}</span>
      </div>
      <div
        v-for="(comment, i) of orderedComments"
        v-if="i < 5 || showMore"
        :key="i">
        
        <router-link
          :to="{name: 'user', params: { id: comment.user.id}}"
          class="nickname">
          {{ comment.user.nickname }}
        </router-link>
        <span class="legend">{{ comment.text }}</span>
        <span class="time pull-right">{{ comment.createdAt | fromNow }}</span>
      </div>
      <el-button
        v-if="orderedComments.length > 5 && !showMore"
        type="text"
        class="button"
        @click="showMore = true">
        Show more comments..
      </el-button>
    </div>
    <div @keyup.enter="createComment">
      <el-input
        :autosize="{ minRows: 1, maxRows: 4}"
        v-model="comment"
        type="text"
        placeholder="Add a comment">
        <el-button
          slot="append"
          type="primary"
          icon="el-icon-edit-outline"
          @click="createComment"/>
      </el-input>
    </div>
    <el-dialog
      :visible.sync="likesVisible"
      title="Likes"
      width="30%">
      <div
        v-for="(like, i) of pictureData.likes"
        :key="i"
        class="center-vertically">
        <div 
          :style="{ 'background-image': 'url(' + like.user.picture + ')' }"
          class="round-icon flex" />
        
        <router-link
          :to="{name: 'user', params: { id: like.user.id}}"
          class="flex nickname header black-text">
          {{ like.user.nickname }}
        </router-link>
        <span class="time pull-right">{{ like.createdAt | fromNow }}</span>
      </div>
    </el-dialog>
  </el-card>
</template>

<script>
import moment from "moment";
import {
  getLikesAndComments,
  createComment,
  createLike,
  deleteLike,
  getPicture
} from "@/api/picture";
import {
  followUser,
  unfollowUser,
} from "@/api/user";

import _ from "lodash";

export default {
  filters: {
    fromNow(date) {
      return moment.unix(date).fromNow();
    }
  },
  props: {
    id: {
      type: [String, Number],
      default: null
    },
    picture: {
      type: Object,
      default: null
    }
  },

  data() {
    return {
      likes: [],
      comments: [],
      comment: "",
      showMore: false,
      pictureData: null,
      likesVisible: false,
    };
  },

  computed: {
    followed(){
      return !!this.$store.state.currentUser
        .following.find(user => user.id === this.pictureData.user.id);
    },

    liked() {
      if (!this.$store.getters.isConnected) return false;
      return !!this.likes.find(like => {
        return like.user.id === this.$store.state.currentUser.id;
      });
    },

    orderedComments() {
      return _.orderBy(this.comments, ["createdAt"], ["asc"]);
    },

    likesPhrase() {
      return `${this.likes.length} ${this.likes.length > 1 ? "likes" : "like"}`;
    }
  },

  async created() {
    const res = await getPicture(this.id || this.picture.Id);
    this.pictureData = res.picture;
    console.log(res);
    await this.refreshPicture();
  },

  methods: {
    async toggleLikePicture() {
      if (!this.liked) {
        await createLike(this.pictureData.id, this.$store.state.currentUser.id);
      } else {
        await deleteLike(this.pictureData.id, this.$store.state.currentUser.id);
      }
      await this.refreshPicture();
    },
    
    async createComment() {
      await createComment(
        this.comment,
        this.pictureData.id,
        this.$store.state.currentUser.id
      );
      await this.refreshPicture();
      this.comment = "";
    },
    
    async refreshPicture() {
      const response = await getLikesAndComments(this.pictureData.id);
      this.likes = response.picture.likes;
      this.comments = response.picture.comments;
    },
    
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
.el-button.is-circle {
  margin-bottom: 5px;
  margin-right: 10px;
}
.header {
  padding-left: 10px;
  font-size: 18px;
}

.nickname {
  text-decoration: none;
  padding-right: 5px;
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

  background-repeat: no-repeat;
  background-position: center center;
  background-size: cover;
}

.tags {
  padding-bottom: 5px;
  padding-top: 5px;
}

.el-button + .el-button {
  margin-left: 2px;
}

.el-card {
  margin-bottom: 20px;
}

.time {
  font-size: 13px;
  color: #999;
}

.legend {
  font-size: 13px;
  color: #999;
}

.image {
  width: 100%;
  display: block;
}

.clearfix:before,
.clearfix:after {
  display: table;
  content: "";
}

.clearfix:after {
  clear: both;
}
</style>
