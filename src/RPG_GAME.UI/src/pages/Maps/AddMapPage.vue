<template>
    <h3>
        Add Map
    </h3>
    <div>
        <MapFormComponent :difficulties="difficulties" :enemies="enemies" :enemiesFields="enemiesFields" :enemyFilter="enemyFilter" @submitForm="submit" @cancel="cancel" />
    </div>
</template>

<script>
import * as response from '@/stubs/enemies.json';
import MapFormComponent from '@/components/Maps/MapFormComponent.vue';

  export default {
    name: 'AddMapPage',
    components: {
      MapFormComponent
    },
    data() {
      return {
        enemies: [],
        addMap: null,
        difficulties: [{label: 'Easy', value: 'EASY'}, {label: 'Medium', value: 'MEDIUM'}, {label: 'Hard', value: 'HARD'}],
        enemiesFields: ['#', 'Name', 'Attack', 'Health', 'Heal', 'Base Required Experience', 'Difficulty', 'Category'],
        enemyFilter: 'enemyName'
      }
    },
    methods: {
      cancel() {
        this.$router.push({ name: 'all-maps' });
      }
    },
    created() {
      this.enemies = response.enemies.map(e => ({
          id: e.id,
          enemyName: e.enemyName,
          baseAttack: e.baseAttack.value,
          baseHealth: e.baseHealth.value,
          baseHealLvl: e.baseHealLvl.value,
          experience: e.experience.value,
          difficulty: e.difficulty,
          category: e.category
        }));
    }
  }

</script>

<style>
</style>