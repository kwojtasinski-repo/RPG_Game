<template>
    <h3>
        Add Hero
    </h3>
    <div v-if="error" className="alert alert-danger">{{error}}</div>
    <div>
        <HeroFormComponent :strategiesIncreasing="strategiesIncreasing" @submitForm="submit" @cancel="cancel" />
    </div>
</template>

<script>
  import HeroFormComponent from '@/components/Heroes/HeroFormComponent.vue';
  import axios from '@/axios-setup.js';
  import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';
  
  export default {
    name: 'AddHeroPage',
    components: {
        HeroFormComponent
    },
    data() {
        return {
            strategiesIncreasing: [{label: 'Additive', value: 'ADDITIVE'}, {label: 'Percentage', value: 'PERCENTAGE'}],
            error: ''
        }
    },
    methods: {
        async submit(formToSend) {
            this.error = '';
            try {
                await axios.post('/api/heroes', formToSend);
                this.$router.push({ name: 'all-heroes' });
            } catch(exception) {
                const message = exceptionMapper(exception);
                this.error = message;
                console.log(exception);
            }
        },
        cancel() {
            this.$router.push({ name: 'all-heroes' });
        }
    }
  }
</script>

<style>
</style>