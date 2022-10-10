<template>
    <h3>
        Add Enemy
    </h3>
    <div>
        <EnemyFormComponent :strategiesIncreasing="strategiesIncreasing" :difficulties="difficulties" :categories="categories" @submitForm="submit" @cancel="cancel" />
    </div>
</template>

<script>
import EnemyFormComponent from '@/components/Enemies/EnemyFormComponent.vue';

  export default {
    name: 'AddEnemyPage',
    components: {
      EnemyFormComponent
    },
    data() {
        return {
            strategiesIncreasing: [{label: 'Additive', value: 'ADDITIVE'}, {label: 'Percentage', value: 'PERCENTAGE'}],
            difficulties: [{label: 'Easy', value: 'EASY'}, {label: 'Medium', value: 'MEDIUM'}, {label: 'Hard', value: 'HARD'}],
            categories: [{label: 'Knight', value: 'Knight'}, {label: 'Archer', value: 'Archer'}, {label: 'Dragon', value: 'Dragon'}],
            error: ''
        }
    },
    methods: {
        submit(formToSend) {
            console.log('formToSend', formToSend);
        },
        cancel() {
            this.$router.push({ name: 'all-enemies' });
        }
    }
  }
</script>

<style>
</style>