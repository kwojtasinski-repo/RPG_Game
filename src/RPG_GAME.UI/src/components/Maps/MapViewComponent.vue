<template>
    <div class="map-view">
        <div class="card card-map">
            <div class="card-header">
                <div class="map-title mt-2 mb-2">
                    {{map.name}}
                </div>
            </div>
            <div class="card-body text-center m-auto">
                <div class="map-prop-info">
                    Properties:
                </div>
                <div class="card-body-content">
                    <div class="map-prop mt-2 mb-2">
                        <div class="map-prop-decription">
                            <div class="map-prop-text">
                                Id:
                            </div>
                        </div>
                        <div class="map-prop-decription">
                            <div class="map-prop-text">
                                {{map.id}}
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-body-content">
                    <div class="map-prop mt-2 mb-2">
                        <div class="map-prop-decription">
                            <div class="map-prop-text">
                                Difficulty :
                            </div>
                        </div>
                        <div class="map-prop-decription">
                            <div class="map-prop-text">
                                {{map.difficulty}}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="map-table-position">
                <div class="mt-2 map-prop-info mb-2">
                    Enemies:
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
                                Category
                            </th>
                            <th> 
                                Quantity
                            </th>
                            <th> 
                                Action
                            </th>
                        </tr>
                    </thead>
                    <tbody class="table-group-divider">
                        <tr v-for="enemy in map.enemies" :key="enemy.enemy.id" :class="enemy.enemy.id === currentEnemyId ? 'bg-secondary' : null"  @click="() => enemyMarked(enemy.enemy.id)">
                            <td>
                                {{enemy.enemy.id}}
                            </td>
                            <td>
                                {{enemy.enemy.enemyName}}
                            </td>
                            <td>
                                {{enemy.enemy.difficulty}}
                            </td>
                            <td>
                                {{enemy.enemy.category}}
                            </td>
                            <td>
                                {{enemy.quantity}}
                            </td>
                            <td>
                                <button class="btn btn-primary" :disabled="enemy.enemy.id !== currentEnemyId" @click="showEnemy">Show enemy</button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div>
            <PopupComponent :open="openPopup" @popupClosed="popupClosed">
                <div class="enemy-show-conent">
                    <EnemyViewComponent :enemy="enemyFetched" />
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
import EnemyViewComponent from '../Enemies/EnemyViewComponent.vue';
import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';

export default {
    name: "MapFormComponent",
    props: {
        map: {
            type: Object,
            required: true
        },
    },
    data() {
        return {
            currentEnemyId: null,
            openPopup: false,
            enemyFetched: null
        };
    },
    methods: {
        enemyMarked(enemyId) {
            if (this.currentEnemyId && this.currentEnemyId === enemyId) {
                this.currentEnemyId = null;
                return;
            }

            this.currentEnemyId = enemyId;
        },
        popupClosed() {
            this.openPopup = false;
            this.currentEnemyId = null;
            this.enemyFetched = null;
        },
        async showEnemy() {
            await this.fetchEnemy(this.currentEnemyId);
            this.openPopup = true;
        },
        async fetchEnemy(enemyId) {
            try {
                const response = await axios.get(`/api/enemies/${enemyId}`);
                this.enemyFetched = response.data;
            } catch(exception) {
                const message = exceptionMapper(exception);
                console.error(message);
                console.log(exception);
            }
        },
    },
    components: { PopupComponent, EnemyViewComponent }
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

  .enemy-show-conent {
    width: 45rem;
  }
</style>