<template>
    <div class="header">
        <div class="clock">
            {{ currentTime }}
        </div>
    </div>
</template>

<script>
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
        }
    }
</script>

<style>
    .header {
        display: flex;
        justify-content: end;
        margin-right: 1rem;
        align-items: center;
        overflow: hidden;
        position: relative;
    }

    .clock {
        font-size: 1.5rem;
    }
</style>