<template>
    <!-- TODO think how to display and what to display maybe use gRPC Web to send history? -->
    <div>
        <div>
            Battle History
        </div>
        <div class="mt-2 mb-2">
            <RouterButtonComponent :url="'/'" :buttonText="'Back to menu'" />
        </div>
    </div>
</template>

<script>
  import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue'

  export default {
    name: 'BattleHistoryPage',
    components: {
        RouterButtonComponent
    }
  }
</script>

<style>
</style>