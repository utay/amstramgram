<template>
  <div>
    <el-input placeholder="Search" v-model="query" />

    <div v-show="query !== ''" class="search-results">
      <!--<ais-index
        app-id="A71NP8C36C"
        api-key="2251b2c1751fee3ffef49c37eedf28d4"
        index-name="Amstramgram_tags"
        :query="query"
      >
        <ais-results>
          <template slot-scope="{ result }">
            <span v-html="result._highlightResult.Text.value" />
          </template>
        </ais-results>
      </ais-index>-->

      <ais-index
        app-id="A71NP8C36C"
        api-key="2251b2c1751fee3ffef49c37eedf28d4"
        index-name="Amstramgram_users"
        :query="query"
      >
        <ais-results>
          <template slot-scope="{ result }">
            {{ result.Nickname }} {{ result.Firstname }} {{ result.Lastname }}
          </template>
        </ais-results>
      </ais-index>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      query: '',
    };
  },

  methods: {},
};
</script>

<style scoped>
.search-results {
  background-color: white;
}
</style>
