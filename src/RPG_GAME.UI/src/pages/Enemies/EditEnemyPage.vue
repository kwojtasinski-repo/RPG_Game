<template>
  <h3>
      Edit Hero
  </h3>
  <div>
    <EnemyFormComponent :enemy="enemy" :strategiesIncreasing="strategiesIncreasing" :difficulties="difficulties" :categories="categories" @submitForm="submit" @cancel="cancel" />
  </div>
</template>

<script>
import * as response from '@/stubs/enemies.json';
import EnemyFormComponent from '@/components/Enemies/EnemyFormComponent.vue';

  export default {
    name: 'EditEnemyPage',
    components: {
      EnemyFormComponent
    },
    data() {
        return {
            strategiesIncreasing: [{label: 'Additive', value: 'ADDITIVE'}, {label: 'Percentage', value: 'PERCENTAGE'}],
            difficulties: [{label: 'Easy', value: 'EASY'}, {label: 'Medium', value: 'MEDIUM'}, {label: 'Hard', value: 'HARD'}],
            categories: [{label: 'Knight', value: 'Knight'}, {label: 'Archer', value: 'Archer'}, {label: 'Dragon', value: 'Dragon'}],
            error: '',
            enemy: null,
        }
    },
    methods: {
        fetchEnemy() {
            const enemy = response.enemies.find(e => e.id == this.$route.params.enemyId);
            return enemy;
        },
        submit(formToSend) {
            console.log('formToSend', formToSend);
        },
        cancel() {
            this.$router.push({ name: 'all-enemies' });
        }
    },
    created() {
        this.enemy = this.fetchEnemy();
    }
  }
</script>

<style>
</style>