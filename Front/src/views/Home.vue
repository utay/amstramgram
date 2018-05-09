<template>
  <div class="home">
    <ais-index
      app-id="A71NP8C36C"
      api-key="2251b2c1751fee3ffef49c37eedf28d4"
      index-name="Amstramgram_pictures"
      :query-parameters="{ page: (page === 0 ? 1 : page), hitsPerPage: 4 }"
    >
      <ais-results :stack="true">
        <template slot-scope="{ result }">
          <picture-card :picture="result"></picture-card>
        </template>
      </ais-results>

      <ais-no-results class="no-results">
        <template slot-scope="props">
          <p>This is the very beginning of your adventure on Amstramgram.</p>
          <p>Follow some people to fill your news feed!</p>
        </template>
      </ais-no-results>

      <div v-observe-visibility="loadMore"></div>
    </ais-index>
  </div>
</template>

<script>
// @ is an alias to /src
import PictureCard from "@/components/PictureCard.vue";

export default {
  name: "home",

  components: {
    PictureCard,
  },

  data() {
    return {
      page: 0,
    };
  },

  methods: {
    loadMore(isVisible) {
      if (isVisible) {
        this.page++;
      }
    }
  },
};
</script>

<style scoped>
.no-results {
  margin-top: 50px;
  color: grey;
}
</style>
