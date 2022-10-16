<template>
  <div class="view-hero-page">
      <div v-if="map === null && loading">
          <LoadingIconComponent />
      </div>
      <div v-else-if="map === null && !loading">
        <h3 class="mb-2 mt-2">View Map</h3>
        <div className="alert alert-danger">{{error}}</div>
        <div class="mt-2 mb-2 justify-content-start">
          <RouterButtonComponent :namedRoute="{ name: 'all-maps' }" :buttonClass="'btn btn-primary'" :buttonText="'Back to maps'" />
        </div>
      </div>
      <div v-else>
        <div class="mt-2 mb-2 justify-content-start">
          <RouterButtonComponent :namedRoute="{ name: 'all-maps' }" :buttonClass="'btn btn-primary'" :buttonText="'Back to maps'" />
        </div>
        <div class="d-flex justify-content-center">
          <MapViewComponent :map="map" />
        </div>
      </div>
  </div>
</template>

<script>
import MapViewComponent from '../../components/Maps/MapViewComponent.vue';
import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';
import axios from '@/axios-setup.js';
import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';

  export default {
    name: 'ViewMapPage',
    components: {
      MapViewComponent,
      RouterButtonComponent,
      LoadingIconComponent
    },
    data() {
      return {
        loading: true,
        map: null
      }
    },
    methods: {
      async fetchMap() {
        try {
          const response = await axios.get(`/api/maps/${this.$route.params.mapId}`);
          this.map = response.data;
        } catch(exception) {
          const message = exceptionMapper(exception);
          this.error = message;
          console.log(exception);
        }
      },
    },
    async created() {
      await this.fetchMap();
      this.loading = false;
    }
  }
</script>

<style>
</style>