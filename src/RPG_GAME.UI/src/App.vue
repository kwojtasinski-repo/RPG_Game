<template>
  <div class="img">
  </div>
  <div class="mb-4" >
    <HeaderComponent />
  </div>
  <div class="mb-4" >
    <router-view />
  </div>
  <div class="mb-4" >
    <FooterComponent />
  </div>
  <notifications />
</template>

<script>
import FooterComponent from './components/Footer/FooterComponent.vue';
import HeaderComponent from './components/Header/HeaderComponent.vue';
import authService from './services/AuthService.js';
import { mapGetters } from 'vuex';

export default {
  name: 'App',
  components: {
    HeaderComponent,
    FooterComponent,
  },
  methods: {
    async verifiedAuthenticated() {
      try {
        const authenticated = await authService.isLogged();
        if (!authenticated) {
          const currentRoute = this.$router.currentRoute.value;
          if (this.user) {
            authService.logout();
          }
          
          if (currentRoute.meta.auth && currentRoute.meta.auth === true) {
            this.$router.push('/');
          }
        }
      } catch (exception) {
        console.error(exception);
      }
      setTimeout(this.verifiedAuthenticated, 60000);
    }
  },
  async created() {
    await this.verifiedAuthenticated();
  },
  computed: {
    ...mapGetters(['user'])
  },
}
</script>

<style>
  #app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: rgb(62, 62, 63);
  }

  .img {
    background-image: url('~@/assets/background-img.jpg');
    background-size: cover;
    position: fixed;
    background-repeat: no-repeat;
    top: 0;
    bottom: 0%;
    left: 0%;
    right: 0%;
    z-index: -1;
    opacity: 55%;
    width: 100%;
    height: auto;
  }

  .table {
    --bs-table-border-color: rgba(0, 0, 0) !important;
  }
</style>
