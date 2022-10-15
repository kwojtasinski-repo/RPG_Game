<template>
    <div v-if="!linkActive"
         @mouseover="linkActive = true"
         :class="linkClass ? linkClass : ''" >
        <div>
            {{ textLink ? textLink : '' }}
        </div>
    </div>
    <div v-else
         @mouseleave="linkActive = false"
         :class="linkClass && linkActiveClass ? [linkClass, linkActiveClass, 'menuItemClick'] : linkClass ? [linkClass, 'bg-dark menuItemClick'] : 'bg-dark menuItemClick'" >
        <div>
            {{ textLink ? textLink : '' }}
        </div>
    </div>
</template>
  
<script>  
  export default {
    name: 'MenuItemComponent',
    props: {
        linkClass: {
            type: String
        },
        linkActiveClass: {
            type: String
        },
        textLink: {
            type: String
        }
    },
    components: {
    },
    data() {
        return {
            linkActive: false
        }
    },
    methods: {
    }
  }
</script>
  
<style>
    .menuItemClick {
        cursor: pointer;    
    }
</style>
  