<template>
      <el-card style="margin: 0 auto; width: 40%; margin-bottom: 20px; " :body-style='{"text-align": "left"}'>
        <div slot="header"
          class="clearfix">
          <div style=" overflow:hidden; height:50px; width:50px; border-radius:50%;">
            <img src="https://picsum.photos/1000"
              alt="Profile picture" />
          </div>
          <span style="margin-top: 15px;">feedthejim</span>
        </div>
        <img src="https://picsum.photos/1000"
          class="image">
        <div style="padding: 14px;">
          <el-button type="text"
            icon="el-icon-circle-check"
            style="">123 likes</el-button>
          <div>
            <span>feedthejim </span>
            <span class="legend">Such a good pic </span>
            <el-button v-for="i in 5"
              :key="i"
              type="text">#toto</el-button>
          </div>
          <el-button type="text"
            class="button">
            Afficher les autres commentaires
          </el-button>
          <div class="time">Il y a 12 heures</div>
        </div>
        <el-input type="textarea"
          :autosize="{ minRows: 1, maxRows: 4}"
          placeholder="Ajouter un commentaire"
          v-model="fixme">
        </el-input>
      </el-card>
</template>

<script>
export default {
  data() {
    return {
      fixme: ""
    };
  },
  methods: {}
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
