<template>
  <div class="enemies-page">
        <div v-if="loading">
            <LoadingIconComponent />
        </div>
        <div v-else>
            <div class="mt-2 mb-2 d-flex justify-content-start">
                <RouterButtonComponent :namedRoute="{ name: 'add-enemy' }" :buttonText="'Add Enemy'" :buttonClass="'btn btn-success me-2'" />
                <RouterButtonComponent :url="'/'" :buttonText="'Back to menu'" />
            </div>
            <div>
                <table class="table table-hover table-striped table-fit table-bordered">
                    <thead>
                        <tr class="table-dark">
                            <th>#</th>
                            <th>Name</th>
                            <th>Health</th>
                            <th>Heal</th>
                            <th>Attack</th>
                            <th>Base Required Experience</th>
                            <th>Difficulty</th>
                            <th>Category</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="enemy in enemies" :key="enemy.id" class="table-light">
                            <td> {{ enemy.id }} </td>
                            <td> {{ enemy.enemyName }} </td>
                            <td> {{ enemy.baseHealth.value }} </td>
                            <td> {{ enemy.baseHealLvl.value }} </td>
                            <td> {{ enemy.baseAttack.value }} </td>
                            <td> {{ enemy.experience.value }} </td>
                            <td> {{ enemy.difficulty }} </td>
                            <td> {{ enemy.category }} </td>
                            <td>
                                <RouterButtonComponent :namedRoute="{ name: 'view-enemy', params: { enemyId: enemy.id } }" :buttonText="'View'" :buttonClass="'btn btn-primary me-2'" />
                                <RouterButtonComponent :namedRoute="{ name: 'edit-enemy', params: { enemyId: enemy.id } }" :buttonText="'Edit'" :buttonClass="'btn btn-warning me-2'" />
                                <button class="btn btn-danger" @click="onDelete(enemy)">
                                    Delete
                                </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div>
                <PopupComponent :open="openPopup" @popupClosed="popupClosed">
                    <div>Do you wish to delete enemy: {{enemyToDelete.enemyName}}?</div>
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
import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';
import PopupComponent from '@/components/Poupup/PopupComponent.vue';
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';
import axios from '@/axios-setup.js';
import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';

  export default {
    name: 'EnemiesPage',
    components: {
      RouterButtonComponent,
      PopupComponent,
      LoadingIconComponent
    },
    data() {
        return {
            loading: true,
            enemies: [],
            enemyToDelete: null,
            openPopup: false,
            error: ''
        }
    },
    methods: {
        async fetchEnemies() {
            try {
                const response = await axios.get('/api/enemies');
                this.enemies = response.data.map(e => ({
                    id: e.id,
                    enemyName: e.enemyName,
                    baseHealth: e.baseHealth,
                    baseHealLvl: e.baseHealLvl,
                    baseAttack: e.baseAttack,
                    experience: e.experience,
                    difficulty: e.difficulty,
                    category: e.category
                }));
            } catch(exception) {
                console.log(exception);
            }
        },
        onDelete(enemy) {
            this.enemyToDelete = enemy;
            this.openPopup = true;
        },
        async confirmDelete() {
            try {
                this.error = '';
                await axios.delete(`/api/enemies/${this.enemyToDelete.id}`);
                await this.fetchEnemies();
                this.popupClosed();
            } catch(exception) {
                const message = exceptionMapper(exception);
                this.error = message;
                console.log(exception);
            }
        },
        popupClosed() {
            this.enemyToDelete = null;
            this.openPopup = false;
        },
    },
    async created() {
        await this.fetchEnemies();
        this.loading = false;
    }
  }
</script>

<style>
  .enemies-page {
      padding-left: 10%;
      padding-right: 10%;
  }
</style>