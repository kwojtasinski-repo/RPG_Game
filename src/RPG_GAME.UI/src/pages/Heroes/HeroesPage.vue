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
                    <th scope="col">First</th>
                    <th scope="col">Last</th>
                    <th scope="col">Handle</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                    <th scope="row">1</th>
                    <td>Mark</td>
                    <td>Otto</td>
                    <td>@mdo</td>
                    </tr>
                    <tr>
                    <th scope="row">2</th>
                    <td>Jacob</td>
                    <td>Thornton</td>
                    <td>@fat</td>
                    </tr>
                    <tr>
                    <th scope="row">3</th>
                    <td colspan="2">Larry the Bird</td>
                    <td>@twitter</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
  import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';
  //import * as heroes from '@/stubs/heroes.json';

  export default {
    name: 'HeroesPage',
    components: {
        RouterButtonComponent
    }
  }
</script>

<style>
    .heroes-page {
        padding-left: 10%;
        padding-right: 10%;
    }
</style>