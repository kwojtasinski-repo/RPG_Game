<template>
    <div>
        <div class="mt-2 mb-2">
            <div class="justify-content-center align-items-center"
                v-for="menuItem of menuItems" :key="menuItem">
                <div>
                    <div @click="menuItem.action()">
                        <MenuItemComponent :linkClass="'menuElement text-primary mt-2 mb-2'"
                            :linkActiveClass="'menuElementActive bg-dark'"
                            :textLink="menuItem.text" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
  
<script>
import MenuItemComponent from './MenuItemComponent.vue'

  export default {
    name: 'MenuComponent',
    props: ["menuItems"],
    components: {
        MenuItemComponent
    },
    data() {
        return {
            linkActive: false
        }
    },
    methods: {
        menuItemClicked(menuItem) {
            this.$emit('menuItemClicked', menuItem);
        }
    }
  }
</script>
  
<style>
    .menu {
        display: flex;
        justify-content: center;
        align-items: center;
        overflow: hidden;
        position: relative;
    }

    .menuElement {
        font-size: 2.5rem;
        padding-right: 1rem;
        padding-left: 1rem;
        display: inline-block;
    }

    .menuElementActive {
        background-size: 120% !important;
    }
</style>
  