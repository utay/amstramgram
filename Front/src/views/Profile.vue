<template>
  <user :id="currentUser.id" />
</template>

<script>
import User from "./User.vue";

export default {
  components: {
    user: User,
  },

  data() {
    return {
      currentUser: this.$store.state.currentUser,
    };
  },
};
</script>
