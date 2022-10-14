<template>
    <div v-if="open===true" class="popup-window">
        <div class="popup-window-inner">
            <div class="popup-window-content">
                <slot />
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'PopupComponent',
        props: ['open'],
        methods: {
            close() {
                this.$emit('popupClosed');
            }
        },
        created() {
            const that = this;
            document.addEventListener('keyup', function (event) {
                if (event.key === 'Escape') {
                    that.close();
                }
            });
        },
    }
</script>

<style>
    *,
    ::before,
    ::after {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
    }

    .popup-window {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        overflow-x: hidden;
        overflow-y: auto;
        background-color: rgba(0, 0, 0, 0.4);
        z-index: 1;
        text-align: center;
    }

    .popup-window-inner {
        max-width: 100rem;
        margin: 2rem auto;
        position: absolute;
        left: 50%;
        top: 50%;
        transform: translate(-50%, -50%);
    }

    .popup-window-content {
        position: relative;
        background-color: #fff;
        border: 1px solid rgba(0, 0, 0, 0.3);
        background-clip: padding-box;
        border-radius: 0.3rem;
        padding: 1rem;
    }
</style>