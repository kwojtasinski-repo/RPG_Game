<template>
    <div class="header">
        <div class="clock text-info">
            user@user.com {{ currentTime }}
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
    }

    .clock {
        font-size: 1.5rem;
    }
</style>