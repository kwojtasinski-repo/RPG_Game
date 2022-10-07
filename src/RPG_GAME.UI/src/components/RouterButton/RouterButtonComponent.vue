<template>
    <router-link v-if="namedRoute" :to="{name: namedRoute.name, params: namedRoute.params}" v-slot="{navigate}">
        <button :class="buttonClass ? buttonClass : 'btn btn-primary'" @click="navigate"
          :type="buttonType ? buttonType : 'button'">
          {{ buttonText }}
        </button>
    </router-link>
    <router-link v-else :to="{path: url}" v-slot="{navigate}">
        <button :class="buttonClass ? buttonClass : 'btn btn-primary'" @click="navigate"
          :type="buttonType ? buttonType : 'button'">
          {{ buttonText }}
        </button>
    </router-link>
</template>
<script>

    export default {
      name: 'RouterButtonComponent',
      props: [ 'url', 'namedRoute', 'buttonText', 'buttonClass', 'buttonType' ],
      components: {
      }
    }
  </script>
  
  <style>
  </style>