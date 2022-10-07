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
          { text: 'Start fight', action: () => this.$router.push({ name: 'menu' }) },
          { text: 'Profile', action: () => this.$router.push({ name: 'menu' }) },
          { text: 'History', action: () => this.$router.push({ name: 'menu' }) },
          { text: 'Characters', action: () => this.$router.push({ name: 'characters' }) },
          { text: 'Maps', action: () => this.$router.push({ name: 'all-maps' }) },
          { text: 'Logout', action: () => this.$router.push({ name: 'menu' }) }
        ]
      }
    }
}
</script>
  
<style>
</style>
  