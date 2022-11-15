<template>
    <div class="header">
        <div class="clock text-primary">
            {{$store.getters.user ? $store.getters.user.email : null}} {{ currentTime }}
        </div>
    </div>
    <div class="justify-content-center align-items-center text-warning">
        <div>
        <div class="headerText bg-dark">
            RPG GAME
        </div></div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex';

    export default {
        name: 'HeaderComponent',
        data() {
            return {
                currentTime: new Date().toLocaleString()
            }
        },
        methods: {
            getTime() {
                setInterval(() => {
                    this.currentTime = new Date().toLocaleString();
                }, 1000);
            }
        },
        created() {
            this.getTime();
        },
        computed: {
            ...mapGetters(['user'])
        }
    }
</script>

<style>
    .header {
        display: flex;
        justify-content: flex-end;
        margin-right: 1rem;
    }

    .clock {
        font-size: 1.5rem;
    }

    .headerText {
        padding-left: 10%;
        padding-right: 10%;
        font-size: 3.5rem;
        padding-right: 1rem;
        padding-left: 1rem;
        display: inline-block;
    }
</style>