<template>
    <div v-if="loading">
        <LoadingIconComponent />
    </div>
    <div v-else>
      <h3>
          Add Map
      </h3>
      <div>
          <MapFormComponent :difficulties="difficulties" :enemies="enemies" :enemiesFields="enemiesFields" :enemyFilter="enemyFilter" @submitForm="submit" @cancel="cancel" />
      </div>
    </div>
</template>

<script>
import MapFormComponent from '@/components/Maps/MapFormComponent.vue';
import axios from '@/axios-setup.js';
import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';


  export default {
    name: 'AddMapPage',
    components: {
      MapFormComponent,
      LoadingIconComponent
    },
    data() {
      return {
        loading: true,
        enemies: [],
        addMap: null,
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
                baseHealth: e.baseHealth,
                baseAttack: e.baseAttack,
                experience: e.experience,
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
      async submit(formToSend) {
        this.error = '';
        try {
            await axios.post('/api/maps', formToSend);
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
      this.loading = false;
    }
  }

</script>

<style>
</style>