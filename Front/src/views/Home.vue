<template>
  <div class="home">
    <el-tag
      v-for="(tag, i) in tags"
      :key="i"
      @close="handleClose(tag)"
      closable
      type="danger">
      {{ tag }}
    </el-tag>

    <ais-index
      app-id="A71NP8C36C"
      api-key="2251b2c1751fee3ffef49c37eedf28d4"
      index-name="Amstramgram_pictures"
      :query-parameters="{
        page: (page === 0 ? 1 : page),
        hitsPerPage: 4,
        filters: tags.length === 0 ? '' : `Tags.Text:'${tags.join('\' AND Tags.Text:\'')}'`
      }"
    >
      <ais-results :stack="true">
        <template slot-scope="{ result }">
          <picture-card :picture="result"></picture-card>
        </template>

        <template slot="footer">
          <div v-observe-visibility="loadMore"></div>
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
import { mapGetters } from 'vuex';
import PictureCard from "@/components/PictureCard.vue";
import store from '@/store';

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
    },

    handleClose(tag) {
      store.dispatch('deleteTag', tag);
    },
  },

  computed: {
    ...mapGetters(['tags']),
  },
};
</script>

<style scoped>
.no-results {
  margin-top: 50px;
  color: grey;
}
</style>
