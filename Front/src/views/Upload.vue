<template>
  <div>
    <a @click="upload">Upload an image</a>
    <img v-if="url !== ''" :src="url" :width="600" />
    <p>Description: {{ description }}</p>
    <p>Hashtags: #{{ tags.join(' #') }}</p>
    <a @click="analyze">Analyze</a>
    <div v-if="isLoading">LOADING</div>
  </div>
</template>

<script>
import axios from 'axios';
import { createPicture } from '@/api/picture';

export default {
  data() {
    return {
      url: "",
      description: "",
      tags: [],
      color: "",
      isLoading: false,
    };
  },

  methods: {
    upload() {
      cloudinary.openUploadWidget(
        { cloud_name: "dnrtun0ab", upload_preset: "vus5ebhc" },
        (error, result) => {
          this.url = result[0].url;
        }
      );
    },

    async analyze() {
      this.isLoading = true;

      let response = await axios.post('https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze?visualFeatures=Categories,Tags,Description,Color&details=Celebrities', {
        url: this.url,
      }, {
        headers: { 'Ocp-Apim-Subscription-Key': '44761e6481ae47b19bf3cffc331e56a8' }
      });

      this.description = response.data.description.captions[0].text;
      this.tags = response.data.tags.map(tag => tag.name);
      this.color = response.data.color.accentColor;

      response = await createPicture({
        url: this.url,
        description: this.description,
        tags: this.tags,
        color: this.color,
      });

      this.isLoading = false;
      console.log(response);
    },
  }
};
</script>
