<template>
    <canvas id="battle"></canvas>
    <img id="magicSpell" src="@/assets/magicspell_spritesheet.png">
</template>

<script>
import BattleService from "@/services/BattleService.js"

    export default {
        name: 'BattleComponent',
        data() {
            return {
                canvasContext: null,
                battleService: null
            }
        },
        methods: {
            startDrawing() {
                let lastTime = 0;
                var draw = () => {
                    const timeStamp = Date.now();
                    const deltaTime = timeStamp - lastTime;
                    lastTime = timeStamp;
                    this.canvasContext.clearRect(0, 0, this.canvasContext.width, this.canvasContext.height);
                    this.battleService.start(this.canvasContext, deltaTime);
                    requestAnimationFrame(draw);
                }

                draw();
            }
        },
        mounted() {
            const context = document.getElementById("battle");
            this.canvasContext = context.getContext("2d");
            this.canvasContext.height = 900;
            this.canvasContext.width = 900;
            this.battleService = new BattleService();
            this.startDrawing();
        }
    }
</script>

<style>
    #warrior, #magicSpell {
        display: none;
    }
</style>