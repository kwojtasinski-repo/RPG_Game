<template>
    <div class="maps-page">
      <div v-if="loading">
          <LoadingIconComponent />
      </div>
      <div v-else>
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
        <div>
            <PopupComponent :open="openPopup" @popupClosed="popupClosed">
                <div>Do you wish to delete map: {{mapToDelete.name}} ?</div>
                <div v-if="error" className="alert alert-danger mt-2 mb-2">{{error}}</div>
                <div class="mt-2">
                    <button class="btn btn-danger me-2" @click="confirmDelete">Yes</button>
                    <button class="btn btn-secondary" @click="popupClosed">No</button>
                </div>
            </PopupComponent>
        </div>
      </div>
    </div>
</template>

<script>
  import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue'
  import PopupComponent from '@/components/Poupup/PopupComponent.vue';
  import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';
  import axios from '@/axios-setup.js';
  import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';

  export default {
    name: 'MapsPage',
    components: {
      RouterButtonComponent,
      PopupComponent,
      LoadingIconComponent
    },
    data() {
      return {
        loading: true,
        maps: [],
        mapToDelete: null,
        openPopup: false,
        error: ''
      }
    },
    methods: {
      async fetchMaps() {
        try {
          const response = await axios.get('/api/maps');
          this.maps = response.data.map(m => ({
            id: m.id,
            name: m.name,
            difficulty: m.difficulty
          }));
        } catch(exception) {
            console.log(exception);
        }
      },
      onDelete(map) {
        this.mapToDelete = map;
        this.openPopup = true;
      },
      async confirmDelete() {
        try {
          this.error = '';
          await axios.delete(`/api/maps/${this.mapToDelete.id}`);
          await this.fetchMaps();
          this.popupClosed();
        } catch(exception) {
          const message = exceptionMapper(exception);
          this.error = message;
          console.log(exception);
        }
      },
      popupClosed() {
        this.mapToDelete = null;
        this.openPopup = false;
      },
    },
    async created() {
      await this.fetchMaps();
      this.loading = false;
    }
  }
</script>

<style>
  .maps-page {
    padding-left: 10%;
    padding-right: 10%;
  }
</style>