<template>
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

export default {
  name: 'App',
  components: {
    HeaderComponent,
    FooterComponent,
  }
}
</script>

<style>
  #app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #255db9;
    background-image: url('~@/assets/background-img.jpg');
    --bs-table-border-color: rgba(0, 0, 0) !important;
    background-size: 100%;
    position: absolute;
    background-repeat: no-repeat;
    width: 100vw !important;
    height: 100vh !important;
  }
</style>
