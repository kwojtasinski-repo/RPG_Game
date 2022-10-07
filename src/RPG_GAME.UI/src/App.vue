<template>
  <HeaderComponent />
  <router-view />
  <FooterComponent />
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
    background-color: #000000;
    width: 100vw;
    height: 100vh;
  }
</style>
