<template>
    <div class="heroes-page">
        <div class="mt-2 mb-2 d-flex justify-content-start">
            <RouterButtonComponent :namedRoute="{ name: 'add-hero' }" :buttonText="'Add Hero'" :buttonClass="'btn btn-success me-2'" />
            <RouterButtonComponent :url="'/'" :buttonText="'Back to menu'" />
        </div>
        <div>
            <table class="table">
                <thead>
                    <tr>
                    <th scope="col">#</th>
                    <th scope="col">Name</th>
                    <th scope="col">Health</th>
                    <th scope="col">Attack</th>
                    <th scope="col">Base Required Experience</th>
                    <th scope="col">Action</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="hero in heroes" :key="hero.id">
                        <td> {{ hero.id }} </td>
                        <td> {{ hero.heroName }} </td>
                        <td> {{ hero.health.value }} </td>
                        <td> {{ hero.attack.value }} </td>
                        <td> {{ hero.baseRequiredExperience.value }} </td>
                        <td>
                            <RouterButtonComponent :namedRoute="{ name: 'view-hero', params: { heroId: hero.id } }" :buttonText="'View'" :buttonClass="'btn btn-primary me-2'" />
                            <RouterButtonComponent :namedRoute="{ name: 'edit-hero', params: { heroId: hero.id } }" :buttonText="'Edit'" :buttonClass="'btn btn-warning me-2'" />
                            <button class="btn btn-danger">
                                Delete
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
  import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';
  import * as response from '@/stubs/heroes.json';

  export default {
    name: 'HeroesPage',
    components: {
        RouterButtonComponent
    },
    data() {
        return {
            heroes: []
        }
    },
    methods: {
        fetchHeroes() {
            return response.heroes;
        }
    },
    created() {
        this.heroes = this.fetchHeroes();
    }
  }
</script>

<style>
    .heroes-page {
        padding-left: 10%;
        padding-right: 10%;
    }
</style>