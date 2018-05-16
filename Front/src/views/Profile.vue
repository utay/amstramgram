<template>
  <user :id="1" />
</template>

<script>
import User from "./User.vue";

export default {
  components: {
    user: User,
  },
};
</script>
