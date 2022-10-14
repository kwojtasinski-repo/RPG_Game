<template>
    <h3>
        Edit Map {{ map?.name }}
    </h3>
    <div>
        <MapFormComponent :map="editMap" :difficulties="difficulties" :enemies="enemies" :enemiesFields="enemiesFields" :enemyFilter="enemyFilter" @submitForm="submit" @cancel="cancel" />
    </div>
</template>

<script>
import * as responseEnemy from '@/stubs/enemies.json';
import * as response from '@/stubs/maps.json';
import MapFormComponent from '@/components/Maps/MapFormComponent.vue';

  export default {
    name: 'EditMapPage',
    components: {
      MapFormComponent
    },
    data() {
      return {
        enemies: [],
        editMap: null,
        map: null,
        difficulties: [{label: 'Easy', value: 'EASY'}, {label: 'Medium', value: 'MEDIUM'}, {label: 'Hard', value: 'HARD'}],
        enemiesFields: ['#', 'Name', 'Attack', 'Health', 'Heal', 'Base Required Experience', 'Difficulty', 'Category'],
        enemyFilter: 'enemyName'
      }
    },
    methods: {
      cancel() {
        this.$router.push({ name: 'all-maps' });
      },
      fetchMap() {
        const map = response.maps.find(m => m.id === this.$route.params.mapId);
        return map;
      },
      submit(dataToSend) {
        console.log('From EditMapPage', dataToSend);
      }
    },
    created() {
      this.enemies = responseEnemy.enemies.map(e => ({
          id: e.id,
          enemyName: e.enemyName,
          baseAttack: e.baseAttack.value,
          baseHealth: e.baseHealth.value,
          baseHealLvl: e.baseHealLvl.value,
          experience: e.experience.value,
          difficulty: e.difficulty,
          category: e.category
        }));
      this.map = this.fetchMap();
      this.editMap = {
        id: this.map.id,
        name: this.map.name,
        difficulty: this.map.difficulty,
        enemies: this.map.enemies.map(e => ({
          enemyId: e.enemy.id,
          enemyName: e.enemy.enemyName,
          quantity: e.quantity
        }))
      };
    }
  }
</script>

<style>
</style>