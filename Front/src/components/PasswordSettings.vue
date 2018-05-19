<template>
  <el-form
    ref="form"
    :model="newForm" 
    label-width="120px">
    <el-form-item label="Current password">
      <el-input
        v-model="newForm.current"
        type="password"
      />
    </el-form-item>
    <el-form-item label="New password">
      <el-input
        v-model="newForm.new"
        type="password"
      />
    </el-form-item>
    <el-form-item label="Confirm new password">
      <el-input
        v-model="newForm.newConfirm"
        type="password"
      />
    </el-form-item>
    <el-form-item>
      <el-button
        type="primary" 
        @click="onSubmit">
        Update your password
      </el-button>
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
      newForm: {
        current: "",
        new: "",
        newConfirm: ""
      },
      form: {
        ...this.$store.state.currentUser
      }
    };
  },
  methods: {
    async onSubmit() {
      if (this.newForm.current !== this.form.password) {
        this.$notify.error({
          title: "Invalid!",
          message: "Verify your input!"
        });
        return;
      }
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