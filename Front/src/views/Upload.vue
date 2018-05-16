<template>
  <el-row :gutter="10"
    v-loading="isLoading"
    style="height:500px">
    <el-col :span="12"
      style="height:100%">
      <div v-if="!imageUploaded"
        class="red-border parent"
        style="height:100%"
        @click="uploadCloudinary">
        Upload a picture!
      </div>
      <img v-else
        style="height:100%; width: 100%"
        :src="image.url">
    </el-col>
    <el-col :span="12"
      style="height:100%">
      <div class="red-border">
        <el-input type="textarea"
          placeholder="Your image description"
          :height="5"
          :autosize="{
             minRows: 10, maxRows: 10
            }"
          v-model="image.description">
        </el-input>
      </div>
      <div class="red-border"
        style="height:40%; margin-top: 5%; margin-bottom: 5%">
        <!-- Hashtags: #{{ image.tags.join(' #') }} -->
        <el-tag :key="tag"
          v-for="tag in image.tags"
          closable
          :disable-transitions="false"
          @close="handleClose(tag)">
          #{{tag}}
        </el-tag>
        <el-input class="input-new-tag"
          v-if="inputVisible"
          v-model="inputValue"
          ref="saveTagInput"
          size="mini"
          @keyup.enter.native="handleInputConfirm"
          @blur="handleInputConfirm">
        </el-input>
        <el-button v-else
          class="button-new-tag"
          size="small"
          @click="showInput">
          + New Tag
        </el-button>
      </div>
      <el-button type="danger"
        @click="!analyzed ? analyze() : upload()"
        round>{{ !analyzed ? 'Analyze' : 'Upload'}}</el-button>
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
      analyzed: false,
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
      this.$nextTick(_ => {
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
      cloudinary.openUploadWidget(
        {
          cloud_name: "dnrtun0ab",
          upload_preset: "vus5ebhc",
          multiple: false,
          cropping: "server",
          cropping_aspect_ratio: 1,
        },
        (error, result) => {
          this.image.url = result[0].url;
          this.imageUploaded = true;
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
            "Ocp-Apim-Subscription-Key": "44761e6481ae47b19bf3cffc331e56a8"
          }
        }
      );

      this.image.description = response.data.description.captions[0].text;
      this.image.tags = response.data.tags.map(tag => tag.name);
      this.image.color = response.data.color.accentColor;
      this.analyzed = true;
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
}
.input-new-tag {
  width: 90px;
  margin-left: 10px;
  vertical-align: bottom;
}
</style>
