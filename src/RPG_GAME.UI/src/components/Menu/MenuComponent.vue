<template>
    <div>
        <div class="menuImage"></div>
        <div class="mt-2 mb-2">
            <div class="justify-content-center align-items-center"
                v-for="menuItem of menuItems" :key="menuItem">
                <div>
                    <div @click="menuItemClicked(menuItem)">
                        <MenuItemComponent :linkClass="'menuElement text-warning mt-2 mb-2'"
                            :linkActiveClass="'.menuElementActive bg-dark'"
                            :textLink="menuItem" />
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
            console.log('menuItemClicked', menuItem);
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

    .menuImage {
        background-image: url('~@/assets/logo.png');
        background-size: 100%;
        position: absolute;
        background-repeat: no-repeat;
        background-position: center;
        top: 0;
        bottom: 0%;
        left: 0%;
        right: 0%;
        z-index: -1;
    }

    .menuElement {
        font-size: 3rem;
        padding-right: 1rem;
        padding-left: 1rem;
        display: inline-block;
    }

    .menuElementActive {
        background-size: 120% !important;
    }
</style>
  