<template>
    <div class="maps-page">
      <div class="mt-2 mb-2 d-flex justify-content-start">
          <RouterButtonComponent :namedRoute="{ name: 'add-map' }" :buttonText="'Add Map'" :buttonClass="'btn btn-success me-2'" />
          <RouterButtonComponent :url="'/'" :buttonText="'Back to menu'" />
      </div>
      <div>
        <table class="table table-hover table-striped table-fit table-bordered">
            <thead>
                <tr class="table-dark">
                    <th> 
                      #
                    </th>
                    <th> 
                      Name
                    </th>
                    <th> 
                      Difficulty
                    </th>
                    <th> 
                      Action
                    </th>
                </tr>
            </thead>
                <tbody class="table-group-divider">
                    <tr v-for="map in maps" :key="map.id" class="table-light">
                      <td>{{ map.id }}</td>
                      <td>{{ map.name }}</td>
                      <td>{{ map.difficulty }}</td>
                      <td>
                          <RouterButtonComponent :namedRoute="{ name: 'view-map', params: { mapId: map.id } }" :buttonText="'View'" :buttonClass="'btn btn-primary me-2'" />
                          <RouterButtonComponent :namedRoute="{ name: 'edit-map', params: { mapId: map.id } }" :buttonText="'Edit'" :buttonClass="'btn btn-warning me-2'" />
                          <button class="btn btn-danger" @click="onDelete(map)">
                              Delete
                          </button>
                      </td>
                  </tr>
            </tbody>
        </table> 
      </div>
    </div>
</template>

<script>
  import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue'
  import * as response from '@/stubs/maps.json';

  export default {
    name: 'MapsPage',
    components: {
      RouterButtonComponent
    },
    date() {
      return {
        maps: []
      }
    },
    methods: {
      fetchMaps() {
          return response.maps;
      },
      onDelete(map) {
        console.log(map);
      }
    },
    created() {
        this.maps = this.fetchMaps();
    }
  }
</script>

<style>
  .maps-page {
      padding-left: 10%;
      padding-right: 10%;
  }
</style>