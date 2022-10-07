<template>
    <h3>
        Edit Hero
    </h3>
    <div>
        <HeroFormComponent :strategiesIncreasing="strategiesIncreasing" :hero="hero" />
    </div>
</template>

<script>
  import HeroFormComponent from '@/components/Heroes/HeroFormComponent.vue';
  import * as response from '@/stubs/heroes.json';

  export default {
    name: 'EditHeroPage',
    components: {
        HeroFormComponent
    },
    data() {
        return {
            strategiesIncreasing: [{label: 'Additive', value: 'ADDITIVE'}, {label: 'Percentage', value: 'PERCENTAGE'}],
            error: '',
            hero: null
        }
    },
    methods: {
        fetchHero() {
            const hero = response.heroes.find(h => h.id == this.$route.params.heroId);
            return hero;
        }
    },
    created() {
        this.hero = this.fetchHero();
    }
  }
</script>

<style>
</style>