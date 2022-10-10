<template>
  <div class="view-hero-page">
      <div class="mt-2 mb-2 justify-content-start">
        <RouterButtonComponent :namedRoute="{ name: 'all-enemies' }" :buttonClass="'btn btn-primary'" :buttonText="'Back to menu'" />
      </div>
      <div class="d-flex justify-content-center">
        <EnemyViewComponent :enemy="enemy" />
      </div>
  </div>
</template>

<script>
  import * as response from '@/stubs/enemies.json';
  import EnemyViewComponent from '../../components/Enemies/EnemyViewComponent.vue';
  import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';

  export default {
    name: 'ViewEnemyPage',
    components: {
      EnemyViewComponent,
      RouterButtonComponent
    },
    data() {
      return {
        enemy: null
      }
    },
    methods: {
      fetchEnemy() {
            const enemy = response.enemies.find(e => e.id == this.$route.params.enemyId);
            return enemy;
        },
    },
    created() {
        this.enemy = this.fetchEnemy();
    }
  }
</script>

<style>
  .view-enemy-page {
    height: 100%;
  }
</style>