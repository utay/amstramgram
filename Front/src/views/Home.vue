<template>
  <div class="home">
    <div class="tags">
      <el-tag 
        v-for="(tag, i) in tags"
        :key="i"
        closable
        type="danger"
        @close="handleClose(tag)">
        {{ tag }}
      </el-tag>
    </div>

    <ais-index 
      :search-store="searchStore"
      :query-parameters="{
        page,
        hitsPerPage: 4,
        filters: tags.length === 0 ? '' : `Tags.Text:'${tags.join('\' AND Tags.Text:\'')}'`
      }"
      index-name="Amstramgram_pictures">
      <ais-results :stack="true">
        <template slot-scope="{ result }">
          <picture-card :picture="result"/>
        </template>

        <template slot="footer">
          <div v-observe-visibility="loadMore"/>
        </template>
      </ais-results>

      <ais-no-results class="no-results">
        <template slot-scope="props">
          <p>This is the very beginning of your adventure on AmStramGram.</p>
          <p>Follow some people to fill your news feed!</p>
        </template>
      </ais-no-results>
    </ais-index>
  </div>
</template>

<script>
import PictureCard from "@/components/PictureCard.vue";
import store from "@/store";
import { createFromAlgoliaCredentials } from "vue-instantsearch";

const searchStore = createFromAlgoliaCredentials(
  "II4W4PPGW1",
  "633ea05725cb749e80a21ba50e779a9c"
);

export default {
  name: "Home",

  components: {
    PictureCard
  },

  data() {
    return {
      searchStore,
      page: 1
    };
  },

  computed: {
    tags() {
      return store.getters.tags;
    }
  },

  methods: {
    loadMore(isVisible) {
      if (isVisible) {
        if (this.page < searchStore.totalPages) {
          this.page++;
        }
      }
    },

    handleClose(tag) {
      store.dispatch("deleteTag", tag);
    }
  },
};
</script>

<style scoped>
.no-results {
  margin-top: 50px;
  color: grey;
}

.tags {
  margin-bottom: 20px;
}
</style>
