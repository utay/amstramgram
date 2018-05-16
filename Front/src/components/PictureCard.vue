<template>
  <el-card :style="`
    margin: 0 auto;
    width: 40%; 
    margin-bottom: 20px; 
    border-top: 2px solid #${picture.Color}`"
    :body-style='{"text-align": "left"}'>
    <div slot="header"
      class="center-vertically">
      <div class="round-icon flex"
        :style="{ 'background-image': 'url(' + picture.User.Picture + ')' }" />
      <a href="#"
        class="flex">{{ picture.User.Nickname }}</a>
    </div>
    <img :src="picture.Image"
      class="image">
    <div style="padding: 14px;">
      <el-button type="text"
        @click="toggleLikePicture"
        :icon="`el-icon-circle-check${liked ? '' : '-outline'}`"
        style="">{{ likesPhrase }}</el-button>
      <div>
        <a href="#">
          <span>{{ picture.User.Nickname }} </span>
        </a>
        <span class="legend">{{ picture.Description }} </span>
        <el-button class="tags"
          v-for="(tag, i) in picture.Tags"
          :key="i"
          type="text">#{{ tag.Text }}</el-button>
        <span class="time pull-right">{{ picture.CreatedAt | fromNow }}</span>
      </div>
      <div v-if="i < 5 || showMore"
        v-for="(comment, i) of orderedComments"
        :key="i">
        <a href="#">
          <span> {{ comment.user.nickname }}</span>
        </a>
        <span class="legend"> {{ comment.text }} </span>
        <span class="time pull-right">{{ comment.createdAt | fromNow }}</span>
      </div>
      <el-button type="text"
        v-if="orderedComments.length > 0 && !showMore"
        @click="showMore = true"
        class="button">
        Show more comments..
      </el-button>
    </div>
    <div @keyup.enter="createComment">
      <el-input type="text"
        :autosize="{ minRows: 1, maxRows: 4}"
        placeholder="Add a comment"
        v-model="comment">
        <el-button slot="append"
          @click="createComment"
          type="primary"
          icon="el-icon-edit-outline"></el-button>
      </el-input>
    </div>
  </el-card>
</template>

<script>
import moment from "moment";
import {
  getLikesAndComments,
  createComment,
  createLike,
  deleteLike
} from "@/api/picture";
import _ from "lodash";

export default {
  props: {
    picture: Object
  },

  data() {
    return {
      likes: [],
      comments: [],
      comment: "",
      showMore: false
    };
  },

  filters: {
    fromNow(date) {
      return moment.unix(date).fromNow();
    }
  },

  methods: {
    async toggleLikePicture() {
      if (!this.liked) {
        await createLike(this.picture.Id, this.$store.state.currentUser.id);
      } else {
        await deleteLike(this.picture.Id, this.$store.state.currentUser.id);
      }
      await this.refreshPicture();
    },
    async createComment() {
      await createComment(
        this.comment,
        this.picture.Id,
        store.getters.currentUser.id
      );
      await this.refreshPicture();
      this.comment = "";
    },
    async refreshPicture() {
      const response = await getLikesAndComments(this.picture.Id);
      this.likes = response.picture.likes;
      this.comments = response.picture.comments;
    }
  },

  async created() {
    await this.refreshPicture();
  },

  computed: {
    liked() {
      return !!this.likes.find(
        like => like.user.id === this.$store.state.currentUser.id
      );
    },
    orderedComments() {
      return _.orderBy(this.comments, ["createdAt"], ["asc"]);
    },

    likesPhrase() {
      return `${this.likes.length} ${this.likes.length > 1 ? "likes" : "like"}`;
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
