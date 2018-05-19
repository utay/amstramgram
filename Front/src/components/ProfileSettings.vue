<template>
  <el-form
    ref="form"
    :model="form" 
    label-width="120px">
    <el-form-item label="Name">
      <el-input v-model="form.lastname" />
    </el-form-item>
    <el-form-item label="Firstname">
      <el-input v-model="form.firstname"/>
    </el-form-item>
    <el-form-item label="E-mail">
      <el-input v-model="form.email"/>
    </el-form-item>
    <el-form-item label="Phone number">
      <el-input v-model="form.phone"/>
    </el-form-item>
    <el-form-item label="Gender">
      <el-input v-model="form.gender"/>
    </el-form-item>
    <el-form-item label="Private account">
      <el-switch v-model="form.private"/>
    </el-form-item>
    <el-form-item label="Description">
      <el-input
        v-model="form.description"
        type="textarea"/>
    </el-form-item>
    <el-form-item>
      <el-button
        type="primary" 
        @click="onSubmit">Update your profile</el-button>
    </el-form-item>
  </el-form>
</template>

<style>
</style>

<script>
import { updateUser } from "../api/user";

export default {
  data() {
    return {
      loading: false,
      form: {
        ...this.$store.state.currentUser
      }
    };
  },
  methods: {
    async onSubmit() {
      this.loading = true;
      await updateUser(this.form);
      this.loading = false;
      this.$store.dispatch("connectUser");
      this.$notify({
        title: "Update succeeded",
        message: "Enjoy your new success!"
      });
    }
  }
};
</script>