<template>
    <div id="footerText" class="text-center m-3 text-primary">
        &copy; Kamil Wojtasiński {{year}}
    </div>
</template>

<script>
    export default {
        name: 'FooterComponent',
        data() {
            return {
                year: new Date().getFullYear()
            }
        }
    }
</script>

<style>
    #footerText {
        font-size: 0.8rem;
    }
</style>