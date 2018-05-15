<template>
  <el-card :style="`margin: 0 auto; width: 40%; margin-bottom: 20px; border-top: 2px solid #${picture.Color}`"
    :body-style='{"text-align": "left"}'>
    <div slot="header"
      class="clearfix">
      <div style=" overflow:hidden; height:50px; width:50px; border-radius:50%;">
        <img :src="picture.User.Picture"
          alt="Profile picture" />
      </div>
      <span style="margin-top: 15px;">{{ picture.User.Nickname }}</span>
    </div>
    <img :src="picture.Image"
      class="image">
    <div style="padding: 14px;">
      <el-button type="text"
        icon="el-icon-circle-check"
        style="">{{ likesPhrase }}</el-button>
      <div>
        <span>{{ picture.User.Nickname }} </span>
        <span class="legend">{{ picture.Description }} </span>
        <el-button v-for="(tag, i) in picture.Tags"
          :key="i"
          type="text">#{{ tag.Text }}</el-button>
      </div>
      <el-button type="text"
        class="button">
        Show comments
      </el-button>
      <div class="time">{{ fromNow }}</div>
    </div>
    <el-input type="text"
      :autosize="{ minRows: 1, maxRows: 4}"
      placeholder="Add a comment"
      v-model="comment">
      <el-button slot="append"
        @click="createComment"
        type="primary"
        icon="el-icon-edit-outline"></el-button>
    </el-input>
  </el-card>
</template>

<script>
import moment from "moment";
import { getLikesAndComments, createComment } from "@/api/picture";
import store from "@/store";

export default {
  props: {
    picture: Object
  },

  data() {
    return {
      likes: [],
      comments: [],
      comment: ""
    };
  },

  methods: {
    async createComment() {
      const data = await createComment(
        this.comment,
        this.picture.Id,
        store.getters.currentUser.id
      );
      console.log(data);
    }
  },

  async created() {
    const response = await getLikesAndComments(this.picture.Id);
    this.likes = response.picture.likes.map(like => like.createdAt);
    this.comments = response.picture.comments.map(comment => comment.text);
  },

  computed: {
    fromNow() {
      return moment.unix(this.picture.CreatedAt).fromNow();
    },

    likesPhrase() {
      return `${this.likes.length} ${this.likes.length > 1 ? "likes" : "like"}`;
    }
  }
};
</script>

<style scoped>
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
