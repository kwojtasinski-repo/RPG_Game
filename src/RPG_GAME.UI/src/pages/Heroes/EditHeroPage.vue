<template>
    <div v-if="hero === null && loading === true">
        <LoadingIconComponent />
    </div>
    <div v-else-if="hero === null && loading === false">
        <h3 class="mb-2 mt-2">Edit Hero</h3>
        <div className="alert alert-danger">{{error}}</div>
        <div class="mt-2 mb-2 justify-content-start">
          <RouterButtonComponent :namedRoute="{ name: 'all-heroes' }" :buttonClass="'btn btn-primary'" :buttonText="'Back to heroes'" />
        </div>
    </div>
    <div v-else>
        <h3 class="mb-2 mt-2">
            Edit Hero {{hero.heroName}}
        </h3>
        <div v-if="error" className="alert alert-danger">{{error}}</div>
        <div>
            <HeroFormComponent :strategiesIncreasing="strategiesIncreasing" :hero="hero" @submitForm="submit" @cancel="cancel" />
        </div>
    </div>
</template>

<script>
  import HeroFormComponent from '@/components/Heroes/HeroFormComponent.vue';
  import axios from '@/axios-setup.js'
  import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';
  import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';
  import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';

  export default {
    name: 'EditHeroPage',
    components: {
        HeroFormComponent,
        LoadingIconComponent,
        RouterButtonComponent
    },
    data() {
        return {
            loading: true,
            strategiesIncreasing: [{label: 'Additive', value: 'ADDITIVE'}, {label: 'Percentage', value: 'PERCENTAGE'}],
            error: '',
            hero: null,
        }
    },
    methods: {
        async fetchHero() {
            try {
                const response = await axios.get(`/api/heroes/${this.$route.params.heroId}`);
                this.hero = response.data;
            } catch(exception) {
                const message = exceptionMapper(exception);
                this.error = message;
                console.log(exception);
            }
        },
        async submit(formToSend) {
            this.error = '';
            try {
                await axios.put(`/api/heroes/${this.$route.params.heroId}`, formToSend);
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
    },
    async created() {
        await this.fetchHero();
        this.loading = false;
    }
  }
</script>

<style>
</style>