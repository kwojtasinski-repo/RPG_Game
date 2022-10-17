<template>
  <div class="view-hero-page">
      <div v-if="enemy === null && loading === true">
          <LoadingIconComponent />
      </div>
      <div v-else-if="enemy === null && loading == false">
        <h3 class="mb-2 mt-2">View Hero</h3>
        <div className="alert alert-danger">{{error}}</div>
        <div class="mt-2 mb-2 justify-content-start">
          <RouterButtonComponent :namedRoute="{ name: 'all-enemies' }" :buttonClass="'btn btn-primary'" :buttonText="'Back to enemies'" />
        </div>
      </div>
      <div v-else>
        <div class="mt-2 mb-2 justify-content-start">
          <RouterButtonComponent :namedRoute="{ name: 'all-enemies' }" :buttonClass="'btn btn-primary'" :buttonText="'Back to enemies'" />
        </div>
        <div class="d-flex justify-content-center">
          <EnemyViewComponent :enemy="enemy" />
        </div>
        <div class="mt-2" v-if="enemy.mapsAssignedTo.length > 0 && user.role !== 'user'">
          <h4>Enemy assigned to maps:</h4>
          <div v-for="mapId in enemy.mapsAssignedTo" :key="mapId">
            <RouterButtonComponent :namedRoute="{ name: 'view-map', params: { mapId: mapId } }" :buttonText="`View Map ${mapId}`" :buttonClass="'btn btn-primary mt-2'" :target="'_blank'"/>
          </div>
        </div>
      </div>
  </div>
</template>

<script>
  import EnemyViewComponent from '../../components/Enemies/EnemyViewComponent.vue';
  import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';
  import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';
  import axios from '@/axios-setup.js';
  import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';
  import { mapGetters } from 'vuex';

  export default {
    name: 'ViewEnemyPage',
    components: {
      EnemyViewComponent,
      RouterButtonComponent,
      LoadingIconComponent
    },
    data() {
      return {
        loading: true,
        enemy: null,
        error: ''
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
    },
    async created() {
        await this.fetchEnemy();
        this.loading = false;
    },
    computed: {
      ...mapGetters(['user'])
    },
  }
</script>

<style>
  .view-enemy-page {
    height: 100%;
  }
</style>