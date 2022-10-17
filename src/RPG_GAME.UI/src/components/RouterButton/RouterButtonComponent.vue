<template>
    <router-link v-if="namedRoute" :to="{name: namedRoute.name, params: namedRoute.params}" :target="target"
      :class="buttonClass ? buttonClass : 'btn btn-primary'">
          {{ buttonText }}
    </router-link>
    <router-link v-else :to="{path: url}" :target="target"
      :class="buttonClass ? buttonClass : 'btn btn-primary'" >
          {{ buttonText }}
    </router-link>
</template>
<script>

    export default {
      name: 'RouterButtonComponent',
      props: { 
        url: {
          type: String
        },
        namedRoute: {
          type: Object
        },
        buttonText: {
          type: String
        },
        buttonClass: {
          type: String
        },
        target: {
          type: String
        }
      },
      components: {
      }
    }
  </script>
  
  <style>
  </style>