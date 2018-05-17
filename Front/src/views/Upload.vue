<template>
  <el-row 
    v-loading="isLoading"
    :gutter="10"
    style="height:500px">
    <el-col 
      :span="12"
      style="height:100%">
      <div 
        v-if="!imageUploaded"
        class="red-border parent"
        style="height:100%"
        @click="uploadCloudinary">
        Upload a picture!
      </div>
      <img 
        v-else
        :src="image.url"
        style="height:100%; width: 100%">
    </el-col>
    <el-col 
      :span="12"
      style="height:100%">
      <h1>Description</h1>
      <div class="red-border">
        <el-input 
          :autosize="{
            minRows: 5, maxRows: 5
          }"
          v-model="image.description"
          :height="5"
          type="textarea"
          placeholder="Your image description"/>
      </div>
      <h2>Tags</h2>
      <div 
        class="red-border"
        style="
          height:35%;
          margin-top: 3%;
          margin-bottom: 3%;
          text-align: left;
          padding: 10px;">
        <el-tag 
          v-for="tag in image.tags"
          :key="tag"
          :disable-transitions="false"
          closable
          type="primary"
          @close="handleClose(tag)">
          #{{ tag }}
        </el-tag>
        <el-input 
          v-if="inputVisible"
          ref="saveTagInput"
          v-model="inputValue"
          class="input-new-tag"
          size="mini"
          @keyup.enter.native="handleInputConfirm"
          @blur="handleInputConfirm"/>
        <el-button 
          v-else
          class="button-new-tag"
          size="small"
          @click="showInput">
          + New Tag
        </el-button>
      </div>
      <el-button 
        type="danger"
        round
        icon="el-icon-check"
        @click=" upload">{{ 'Upload' }}</el-button>
    </el-col>
  </el-row>
</template>

<script>
import axios from "axios";
import { createPicture } from "@/api/picture";

export default {
  data() {
    return {
      image: {
        url: "",
        description: "",
        tags: [],
        color: ""
      },
      isLoading: false,
      imageUploaded: false,
      inputVisible: false,
      inputValue: ""
    };
  },

  methods: {
    handleClose(tag) {
      this.image.tags.splice(this.image.tags.indexOf(tag), 1);
    },

    showInput() {
      this.inputVisible = true;
      this.$nextTick(() => {
        this.$refs.saveTagInput.$refs.input.focus();
      });
    },

    handleInputConfirm() {
      let inputValue = this.inputValue;
      if (inputValue) {
        this.image.tags.push(inputValue);
      }
      this.inputVisible = false;
      this.inputValue = "";
    },

    uploadCloudinary() {
      window.cloudinary.openUploadWidget(
        {
          cloud_name: "dnrtun0ab",
          upload_preset: "vus5ebhc",
          multiple: false,
          cropping: "server",
          cropping_aspect_ratio: 1,
          theme: "minimal"
        },
        (error, result) => {
          this.image.url = result[0].url;
          this.imageUploaded = true;
          this.analyze();
        }
      );
    },

    async analyze() {
      this.isLoading = true;

      let response = await axios.post(
        "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze?visualFeatures=Categories,Tags,Description,Color&details=Celebrities",
        {
          url: this.image.url
        },
        {
          headers: {
            "Ocp-Apim-Subscription-Key": "aae09155fc6749e3bfb16d3a7c898ed2"
          }
        }
      );

      this.image.description = response.data.description.captions[0].text;
      this.image.tags = response.data.tags.map(tag => tag.name);
      this.image.color = response.data.color.accentColor;
      this.isLoading = false;
    },
    async upload() {
      try {
        this.isLoading = true;
        await createPicture({
          url: this.image.url,
          description: this.image.description,
          tags: this.image.tags,
          color: this.image.color
        });
        this.$emit("uploadDone");
        this.$notify({
          title: "Upload succeeded",
          message: "Enjoy your new success!"
        });
        this.isLoading = false;
      } catch (exception) {
        this.$notify({
          title: "Upload failed...",
          message: "Please retry!"
        });
      }
    }
  }
};
</script>

<style scoped>
.parent {
  display: flex;
  justify-content: center;
  align-items: center;
}

.overlay {
  background-color: #000;
  opacity: 0.75;
  z-index: 9999999;
}

.red-border {
  border: 2px solid #ee527c;
  border-radius: 6px;
}

.el-tag {
  margin-bottom: 5px;
  color: white;
  background-color: #ee527c;
}

.el-button--danger {
    color: #fff;
    background-color: #ee527c;
    border-color: #ee527c;
}

.el-tag + .el-tag {
  margin-left: 10px;
}

.button-new-tag {
  margin-left: 10px;
  height: 32px;
  line-height: 30px;
  padding-top: 0;
  padding-bottom: 0;
  border-radius: 4px;
}
.input-new-tag {
  width: 90px;
  margin-left: 10px;
  border-radius: 4px;
  border: 1px solid #f56c6c;
  vertical-align: bottom;
}
</style>
