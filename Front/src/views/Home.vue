<template>
  <div class="home">
    <picture-card v-for="i in 10"
      :key="i"></picture-card>
  </div>
</template>

<script>
// @ is an alias to /src
import PictureCard from "@/components/PictureCard.vue";

export default {
  name: "home",
  components: {
    PictureCard
  }
};
</script>
