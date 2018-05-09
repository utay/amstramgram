<template>
  <div>
    <el-input placeholder="Search" v-model="query" />

    <div v-if="query !== ''" class="search-results">
      <h3>Users</h3>

      <ais-index
        app-id="A71NP8C36C"
        api-key="2251b2c1751fee3ffef49c37eedf28d4"
        index-name="Amstramgram_users"
        :query="query"
      >
        <ais-results>
          <template slot-scope="{ result }">
            <p @click="toUserPage(result.Id)">
              <span v-html="result._highlightResult.Nickname.value + ' '" />
              <span class="search-result-name" v-html="result._highlightResult.Firstname.value + ' ' + result._highlightResult.Lastname.value" />
            </p>
          </template>
        </ais-results>

        <ais-no-results>
            <template slot-scope="props">
              <p>No users found for <i>{{ props.query }}</i>.</p>
            </template>
        </ais-no-results>
      </ais-index>

      <h3>Tags</h3>

      <ais-index
        app-id="A71NP8C36C"
        api-key="2251b2c1751fee3ffef49c37eedf28d4"
        index-name="Amstramgram_tags"
        :query="query"
      >
        <ais-results>
          <template slot-scope="{ result }">
            <p>
              <span v-html="result._highlightResult.Text.value" />
            </p>
          </template>
        </ais-results>

        <ais-no-results>
            <template slot-scope="props">
              <p>No tags found for <i>{{ props.query }}</i>.</p>
            </template>
        </ais-no-results>
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

  methods: {
    toUserPage(id) {
      this.$router.push({ name: 'user', params: { id } });
    }
  },
};
</script>

<style scoped>
.search-results {
  background-color: white;
  box-shadow: 0 4px 6px 2px rgba(0, 0, 0, .10);
  text-align: left;
}

.search-results h3 {
  margin: 0;
  padding-left: 10px;
  line-height: 40px;
}

.search-results p {
  padding-left: 10px;
  margin: 0;
}

.search-results p i {
  font-style: normal;
  font-weight: 800;
  color: #EE527C;
}

.search-results p:hover {
  background-color: #fde8ed;
}

.search-result-name {
  color: grey;
  font-size: 10px;
}
</style>