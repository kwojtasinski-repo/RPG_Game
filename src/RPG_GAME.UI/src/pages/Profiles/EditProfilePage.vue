<template>
    <!-- TODO think how to display and what to display -->
    <div>
        <div>
            Edit Profile Page
        </div>
        <div class="mt-2 mb-2">
            <RouterButtonComponent :url="'/'" :buttonText="'Back to menu'" />
        </div>
    </div>
</template>

<script>
  import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue'

  export default {
    name: 'EditProfilePage',
    components: {
        RouterButtonComponent
    }
  }
</script>

<style>
</style>