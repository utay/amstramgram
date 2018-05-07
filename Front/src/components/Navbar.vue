<template>
  <el-menu :default-active="activeIndex"
    class="el-menu-demo"
    mode="horizontal"
    @select="handleSelect">
    <el-menu-item index="1">
      AmStramGram
    </el-menu-item>
    <el-menu-item index="2">
      <el-input placeholder="Search"
        v-model="fixme">
        <i slot="suffix"
          class="el-input__icon el-icon-search"></i>
      </el-input>
    </el-menu-item>
    <el-submenu index="3">
      <template slot="title">Workspace</template>
      <el-menu-item index="3-1">item one</el-menu-item>
      <el-menu-item index="3-2">item two</el-menu-item>
      <el-menu-item index="3-3">item three</el-menu-item>>
    </el-submenu>
    <el-menu-item index="4">
      <i class="el-icon-edit"></i>
    </el-menu-item>
    <el-menu-item index="5">
      <i class="el-icon-info"></i>
    </el-menu-item>
  </el-menu>
</template>

<script>
export default {
  data() {
    return {
      activeIndex: "1",
      activeIndex2: "1",
      fixme: '',
    };
  },
  methods: {
    handleSelect(key, keyPath) {
      console.log(key, keyPath);
    }
  }
};
</script>
