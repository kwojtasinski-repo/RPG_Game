<template>
    <div>
        MapsPage
    </div>
    <div class="mt-2 mb-2">
        <RouterButtonComponent :url="'/'" :buttonText="'Back to menu'" />
    </div>
</template>

<script>
  import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue'

  export default {
    name: 'MapsPage',
    components: {
        RouterButtonComponent
    }
  }
</script>

<style>
</style>