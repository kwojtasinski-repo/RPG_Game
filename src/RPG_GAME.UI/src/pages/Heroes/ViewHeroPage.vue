<template>
    <div class="view-hero-page">
      <div v-if="hero === null && loading === true">
          <LoadingIconComponent />
      </div>
      <div v-else-if="hero === null && loading == false">
        <h3 class="mb-2 mt-2">View Hero</h3>
        <div className="alert alert-danger">{{error}}</div>
        <div class="mt-2 mb-2 justify-content-start">
          <RouterButtonComponent :namedRoute="{ name: 'all-heroes' }" :buttonClass="'btn btn-primary'" :buttonText="'Back to heroes'" />
        </div>
      </div>
      <div v-else>
          <div class="mt-2 mb-2 justify-content-start">
            <RouterButtonComponent :namedRoute="{ name: 'all-heroes' }" :buttonClass="'btn btn-primary'" :buttonText="'Back to heroes'" />
          </div>
          <div class="d-flex justify-content-center">
            <HeroViewComponent :hero="hero" />
          </div>
          <div class="mt-2" v-if="hero.playersAssignedTo.length > 0 && user.role !== 'user'">
            <h4>Hero assigned to players:</h4>
            <div v-for="playerId in hero.playersAssignedTo" :key="playerId">
              <RouterButtonComponent :namedRoute="{ name: 'edit-profile', params: { playerId: playerId } }" :buttonText="`View Player ${playerId}`" :buttonClass="'btn btn-primary mt-2'" :target="'_blank'"/>
            </div>
        </div>
      </div>
    </div>
</template>

<script>
  import HeroViewComponent from '../../components/Heroes/HeroViewComponent.vue';
  import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';
  import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';
  import axios from '@/axios-setup.js';
  import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';
  import { mapGetters } from 'vuex';  

  export default {
    name: 'ViewHeroPage',
    components: {
      HeroViewComponent,
      RouterButtonComponent,
      LoadingIconComponent
    },
    data() {
      return {
        loading: true,
        hero: null,
        error: ''
      }
    },
    methods: {
      async fetchHero() {
        try {
          const response = await axios.get(`/api/heroes/${this.$route.params.heroId}`);
          this.hero = {
            id: response.data.id,
            heroName: response.data.heroName,
            health: response.data.health,
            attack: response.data.attack,
            baseRequiredExperience: response.data.baseRequiredExperience,
            skills: response.data.skills,
            playersAssignedTo: response.data.playersAssignedTo
          };
        } catch(exception) {
          const message = exceptionMapper(exception);
          this.error = message;
          console.log(exception);
        }
      },
    },
    async created() {
        await this.fetchHero();
        this.loading = false;
    },
    computed: {
        ...mapGetters(['user'])
    }
  }
</script>

<style>

  .view-hero-page {
    height: 100%;
  }
</style>