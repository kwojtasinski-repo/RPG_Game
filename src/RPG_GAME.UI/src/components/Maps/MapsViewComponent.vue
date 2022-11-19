<template>
    <div v-for="map of maps" class="map-view" :key="map.id">
        <div class="map-table-position">
            <div class="mt-2 map-prop-info mb-2">
                Maps:
            </div>
            <table class="table table-hover table-striped table-fit table-bordered">
                <thead>
                    <tr class="table-dark">
                        <th> 
                            Id
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
                    <tr :class="map.id === currentMapId ? 'bg-secondary' : null"  @click="() => mapMarked(map.id)">
                        <td>
                            {{map.id}}
                        </td>
                        <td>
                            {{map.name}}
                        </td>
                        <td>
                            {{map.difficulty}}
                        </td>
                        <td>
                            <button class="btn btn-primary" :disabled="map.id !== currentMapId" @click="showMap">Show map</button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div>
            <PopupComponent :open="openPopup" @popupClosed="popupClosed">
                <div class="map-show-conent">
                    <MapViewComponent :map="mapFetched" />
                </div>
                <div class="mt-2">
                    <button class="btn btn-secondary" @click="popupClosed">Close</button>
                </div>
            </PopupComponent>
        </div>
    </div>
</template>

<script>
import axios from "@/axios-setup.js"
import PopupComponent from '../Poupup/PopupComponent.vue';
import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';
import MapViewComponent from "./MapViewComponent.vue";

export default {
    name: "MapsViewComponent",
    props: {
        maps: {
            type: Object,
            required: true
        },
    },
    emits: ['markedMap'],
    data() {
        return {
            currentMapId: null,
            openPopup: false,
            mapFetched: null
        };
    },
    methods: {
        mapMarked(mapId) {
            if (this.currentMapId && this.currentMapId === mapId) {
                this.currentMapId = null;
                this.$emit('markedMap', this.currentMapId);
                return;
            }

            this.currentMapId = mapId;
            this.$emit('markedMap', this.currentMapId);
        },
        popupClosed() {
            this.openPopup = false;
            this.currentMapId = null;
            this.mapFetched = null;
        },
        async showMap() {
            await this.fetchMap(this.currentMapId);
            this.openPopup = true;
        },
        async fetchMap(mapId) {
            try {
                const response = await axios.get(`/api/maps/${mapId}`);
                this.mapFetched = response.data;
            } catch(exception) {
                const message = exceptionMapper(exception);
                console.error(message);
                console.log(exception);
            }
        },
    },
    components: { PopupComponent, MapViewComponent }
}

</script>

<style>
  .map-view {
    text-align: center;
  }

  .card-map {
    width: 55rem;
  }  

  .map-title {
    font-size: 1.5rem;
  }

  .map-prop-info {
    font-size: 1.4rem;
  }
  
  .card-body-content {
    width: 50rem !important;
    display: flex;
    flex-flow: row wrap;
  }

  .map-prop {
    flex-basis: 100%;
    display: flex;
    flex-flow: row nowrap;
  }
  
  .map-prop-decription {
    position: relative;
    display: flex;
    flex-flow: column nowrap;
    align-items: center;
    flex: 1 1 100%;
    overflow: hidden;
  }
  
  .map-prop-text {
    white-space: nowrap;
    font-size: 1.25rem;
  }

  .map-table-position {
    padding-left: 5%;
    padding-right: 5%;
  }

  .map-show-conent {
    width: 55rem;
  }
</style>