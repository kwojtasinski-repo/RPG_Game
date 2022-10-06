<template>
  <MenuComponent :menuItems="menuItems()" />
</template>
  
<script>
import MenuComponent from '@/components/Menu/MenuComponent.vue';  

export default {
    name: 'MenuPage',
    components: {
        MenuComponent
    },
    methods: {
      menuItems() {
        return [
          'Start fight',
          'Profile',
          'History',
          'Logout'
        ]
      }
    }
}
</script>
  
<style>
</style>
  