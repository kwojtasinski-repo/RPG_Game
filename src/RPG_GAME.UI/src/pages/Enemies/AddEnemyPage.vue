<template>
    <h3>
        Add Enemy
    </h3>
    <div v-if="error" className="alert alert-danger">{{error}}</div>
    <div>
        <EnemyFormComponent :strategiesIncreasing="strategiesIncreasing" :difficulties="difficulties" :categories="categories" @submitForm="submit" @cancel="cancel" />
    </div>
</template>

<script>
import EnemyFormComponent from '@/components/Enemies/EnemyFormComponent.vue';
import axios from '@/axios-setup.js';
import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';

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
        async submit(formToSend) {
            this.error = '';
            try {
                await axios.post('/api/enemies', formToSend);
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
    }
  }
</script>

<style>
</style>