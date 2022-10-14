<template>
  <div class="view-hero-page">
      <div class="mt-2 mb-2 justify-content-start">
        <RouterButtonComponent :namedRoute="{ name: 'all-maps' }" :buttonClass="'btn btn-primary'" :buttonText="'Back to maps'" />
      </div>
      <div class="d-flex justify-content-center">
        <MapViewComponent :map="map" />
      </div>
  </div>
</template>

<script>
import * as response from '@/stubs/maps.json';
import MapViewComponent from '../../components/Maps/MapViewComponent.vue';
import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';

  export default {
    name: 'ViewMapPage',
    components: {
      MapViewComponent,
      RouterButtonComponent
    },
    data() {
      return {
        map: null
      }
    },
    methods: {
      fetchMap() {
            const map = response.maps.find(m => m.id == this.$route.params.mapId);
            return map;
        },
    },
    created() {
        this.map = this.fetchMap();
    }
  }
</script>

<style>
</style>