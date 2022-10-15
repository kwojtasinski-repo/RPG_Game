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
        buttonType: {
          type: String
        }
      },
      components: {
      }
    }
  </script>
  
  <style>
  </style>