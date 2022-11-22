<template>
    <div>
        BattleResumePage
        <!-- using js load the latest battle but first user have to confirm that maybe use in store to add resumed battle? -->
    </div>
    <RouterButtonComponent url="/" :buttonText="'Back to menu'" :buttonClass="'btn btn-primary'" />
</template>

<script>
import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';

  export default {
    name: 'BattleResumePage',
    components: { RouterButtonComponent },
 }
</script>

<style>
</style>