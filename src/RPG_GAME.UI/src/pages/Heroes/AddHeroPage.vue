<template>
    <h3>
        Add Hero
    </h3>
    <div>
        <HeroFormComponent :strategiesIncreasing="strategiesIncreasing" />
    </div>
</template>

<script>
  import HeroFormComponent from '@/components/Heroes/HeroFormComponent.vue';
  
  export default {
    name: 'AddHeroPage',
    components: {
        HeroFormComponent
    },
    data() {
        return {
            strategiesIncreasing: [{label: 'Additive', value: 'ADDITIVE'}, {label: 'Percentage', value: 'PERCENTAGE'}],
            error: ''
        }
    },
  }
</script>

<style>
</style>