<template>
  <div>
    <el-row :gutter="10"
      style="width: 50%; margin: 0 auto; padding-bottom: 25px"
      justify="center">
      <el-col :span="8">
        <img src="https://picsum.photos/1000"
          class="image-profile">
      </el-col>
      <el-col :span="16">
        <el-card :body-style='{"text-align": "left"}'>
          <div slot="header"
            style="text-align: left;">
            <span>User1</span>
            <el-button type="primary"
              style="float: right; padding: 4px 0"
              icon="el-icon-back"></el-button>
            <el-button style="float: right; padding: 3px 10px"
              type="text">Update profile</el-button>
          </div>
          <div>
            <span>16</span> Publications
            <span>16</span> Abonnés
            <span>16</span> Suivis
          </div>
          <div>
            Welcome to my profile! This is a short description!
          </div>
        </el-card>
      </el-col>
    </el-row>
    <el-row :gutter="10"
      style="width: 50%; margin: 0 auto;"
      justify="center">
      <el-col :span="8"
        v-for="i in 10"
        :key="i">
        <img src="https://picsum.photos/1000"
          class="image">
      </el-col>
    </el-row>
  </div>
</template>

<script>
export default {};
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
