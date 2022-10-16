<template>
  <div v-if="enemy === null && loading === true">
      <LoadingIconComponent />
  </div>
  <div v-else-if="enemy === null && loading === false">
      <h3 class="mb-2 mt-2">Edit Enemy</h3>
      <div className="alert alert-danger">{{error}}</div>
      <div class="mt-2 mb-2 justify-content-start">
        <RouterButtonComponent :namedRoute="{ name: 'all-enemies' }" :buttonClass="'btn btn-primary'" :buttonText="'Back to enemies'" />
      </div>
  </div>
  <div v-else>
    <h3>
        Edit Enemy {{ enemy.enemyName }}
    </h3>
    <div v-if="error" className="alert alert-danger">{{error}}</div>
    <div>
      <EnemyFormComponent :enemy="enemy" :strategiesIncreasing="strategiesIncreasing" :difficulties="difficulties" :categories="categories" @submitForm="submit" @cancel="cancel" />
    </div>
  </div>
</template>

<script>
import EnemyFormComponent from '@/components/Enemies/EnemyFormComponent.vue';
import axios from '@/axios-setup.js';
import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';
import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';

  export default {
    name: 'EditEnemyPage',
    components: {
      EnemyFormComponent,
      LoadingIconComponent,
      RouterButtonComponent
    },
    data() {
        return {
            loading: true,
            strategiesIncreasing: [{label: 'Additive', value: 'ADDITIVE'}, {label: 'Percentage', value: 'PERCENTAGE'}],
            difficulties: [{label: 'Easy', value: 'EASY'}, {label: 'Medium', value: 'MEDIUM'}, {label: 'Hard', value: 'HARD'}],
            categories: [{label: 'Knight', value: 'Knight'}, {label: 'Archer', value: 'Archer'}, {label: 'Dragon', value: 'Dragon'}],
            error: '',
            enemy: null,
        }
    },
    methods: {
        async fetchEnemy() {
            try {
                const response = await axios.get(`/api/enemies/${this.$route.params.enemyId}`);
                this.enemy = response.data;
            } catch(exception) {
                const message = exceptionMapper(exception);
                this.error = message;
                console.log(exception);
            }
        },
        async submit(formToSend) {
            this.error = '';
            try {
                await axios.put(`/api/enemies/${this.$route.params.enemyId}`, formToSend);
                this.$router.push({ name: 'all-enemies' });
            } catch(exception) {
                const message = exceptionMapper(exception);
                this.error = message;
                console.log(exception);
            }
        },
        cancel() {
            this.$router.push({ name: 'all-enemies' });
        }
    },
    async created() {
        await this.fetchEnemy();
        this.loading = false;
    }
  }
</script>

<style>
</style>