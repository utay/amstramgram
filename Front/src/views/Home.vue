<template>
  <div class="home">
    <ais-index
      app-id="A71NP8C36C"
      api-key="2251b2c1751fee3ffef49c37eedf28d4"
      index-name="Amstramgram_pictures"
    >
      <ais-results>
        <template slot-scope="{ result }">
          <picture-card :picture="result"></picture-card>
        </template>
      </ais-results>
    </ais-index>
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
