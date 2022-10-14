<template>
    <div class="view-hero-page">
        <div class="mt-2 mb-2 justify-content-start">
          <RouterButtonComponent :namedRoute="{ name: 'all-heroes' }" :buttonClass="'btn btn-primary'" :buttonText="'Back to heroes'" />
        </div>
        <div class="d-flex justify-content-center">
          <HeroViewComponent :hero="hero" />
        </div>
    </div>
</template>

<script>
  import * as response from '@/stubs/heroes.json';
  import HeroViewComponent from '../../components/Heroes/HeroViewComponent.vue';
  import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';

  export default {
    name: 'ViewHeroPage',
    components: {
      HeroViewComponent,
      RouterButtonComponent
    },
    data() {
      return {
        hero: null
      }
    },
    methods: {
      fetchHero() {
            const hero = response.heroes.find(h => h.id == this.$route.params.heroId);
            return hero;
        },
    },
    created() {
        this.hero = this.fetchHero();
    }
  }
</script>

<style>

  .view-hero-page {
    height: 100%;
  }
</style>