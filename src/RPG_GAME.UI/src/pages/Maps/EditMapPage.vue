<template>
    <div v-if="map === null && loading === true">
        <LoadingIconComponent />
    </div>
    <div v-else-if="map === null && loading === false">
        <h3 class="mb-2 mt-2">Edit Map</h3>
        <div className="alert alert-danger">{{error}}</div>
        <div class="mt-2 mb-2 justify-content-start">
          <RouterButtonComponent :namedRoute="{ name: 'all-maps' }" :buttonClass="'btn btn-primary'" :buttonText="'Back to maps'" />
        </div>
    </div>
    <div v-else>
      <h3>
          Edit Map {{ map.name }}
      </h3>
      <div v-if="error" className="alert alert-danger">{{error}}</div>
      <div>
          <MapFormComponent :map="map" :difficulties="difficulties" :enemies="enemies" :enemiesFields="enemiesFields" :enemyFilter="enemyFilter" @submitForm="submit" @cancel="cancel" />
      </div>
    </div>
</template>

<script>
import MapFormComponent from '@/components/Maps/MapFormComponent.vue';
import axios from '@/axios-setup.js';
import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';
import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';

  export default {
    name: 'EditMapPage',
    components: {
      MapFormComponent,
      LoadingIconComponent,
      RouterButtonComponent
    },
    data() {
      return {
        loading: true,
        enemies: [],
        editMap: null,
        map: null,
        difficulties: [{label: 'Easy', value: 'EASY'}, {label: 'Medium', value: 'MEDIUM'}, {label: 'Hard', value: 'HARD'}],
        enemiesFields: ['#', 'Name', 'Attack', 'Health', 'Heal', 'Base Required Experience', 'Difficulty', 'Category'],
        enemyFilter: 'enemyName',
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
              baseAttack: e.baseAttack.value,
              baseHealth: e.baseHealth.value,
              baseHealLvl: e.baseHealLvl.value,
              experience: e.experience.value,
              difficulty: e.difficulty,
              category: e.category
          }));
        } catch(exception) {
          console.log(exception);
        }
      },
      cancel() {
        this.$router.push({ name: 'all-maps' });
      },
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
      async submit(formToSend) {
        this.error = '';
        try {
          await axios.put(`/api/maps/${this.$route.params.mapId}`, formToSend);
          this.$router.push({ name: 'all-maps' });
        } catch(exception) {
          const message = exceptionMapper(exception);
          this.error = message;
          console.log(exception);
        }
      }
    },
    async created() {
      await this.fetchEnemies();
      await this.fetchMap();
      this.loading = false;
    }
  }
</script>

<style>
</style>