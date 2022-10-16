<template>
  <MenuComponent :menuItems="menuItems()" />
</template>
  
<script>
import MenuComponent from '@/components/Menu/MenuComponent.vue';
import authService from '@/services/AuthService.js';
import { mapGetters } from 'vuex';


export default {
    name: 'MenuPage',
    components: {
        MenuComponent
    },
    methods: {
      menuItems() {
        const user = this.$store.getters.user;
        let routes = [];

        if (!user) {
          routes.push({ text: 'Login', action: () => this.$router.push({ name: 'login' }) })
          routes.push({ text: 'Register', action: () => this.$router.push({ name: 'register' }) })
          return routes;
        }

        routes = routes.concat(
          { text: 'Start fight', action: () => this.$router.push({ name: 'menu' }) }, // TODO Add Start fight page
          { text: 'Profile', action: () => this.$router.push({ name: 'menu' }) }, // TODO Profile page
          { text: 'History', action: () => this.$router.push({ name: 'menu' }) } // TODO History page
        );

        if (user.role.toLowerCase() === 'admin') {
          routes = routes.concat(
            { text: 'Characters', action: () => this.$router.push({ name: 'characters' }) },
            { text: 'Maps', action: () => this.$router.push({ name: 'all-maps' }) }
          );
        }

        routes.push({ text: 'Logout', action: () => authService.logout() });
        return routes;
      }
    },
    computed: {
      ...mapGetters(['user'])
    }
}
</script>
  
<style>
</style>
  