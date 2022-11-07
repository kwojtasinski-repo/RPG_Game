<template>
    <div class="battleInfo">
        <div class="positionNames">
            <div id="playerName">TestPlayer</div>
            <div id="enemyName">TestEnemy</div>
        </div>
        <div class="statsInfo">
            <!-- player health -->
            <div class="playerHealthBorder">
                <div class="playerHealth"></div>
                <div id="playerHealth" class="currentPlayerHealth"></div>
            </div>
            <!-- timer -->
            <div id="timer">1000</div>
            <!-- enemy health -->
            <div class="enemyHealthBorder">
                <div class="enemyHealth"></div>
                <div id="enemyHealth" class="currentEnemyHealth"></div>
            </div>
        </div>
        <canvas id="battle"></canvas>
    </div>
</template>

<script>
import BattleService from "@/services/BattleService.js"

    export default {
        name: "BattleComponent",
        data() {
            return {
                canvasContext: null,
                battleService: null,
                requestAnimationId: null
            };
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
                    this.requestAnimationId = requestAnimationFrame(draw);
                };
                draw();
            }
        },
        mounted() {
            const context = document.getElementById("battle");
            this.canvasContext = context.getContext("2d");
            this.canvasContext.height = 300;
            this.canvasContext.width = 900;
            context.width = this.canvasContext.width;
            context.height = this.canvasContext.height;
            this.battleService = new BattleService();
            this.startDrawing();
        },
        beforeUnmount() {
            window.cancelAnimationFrame(this.requestAnimationId);
            this.battleService.destroy();
            this.requestAnimationId = null;
        }
    }
</script>

<style>
    .battleInfo {
        position: relative;
        display: inline-block;
        background-image: url('@/assets/background-fight.jpg');
        background-repeat: no-repeat;
        background-position: center;
        background-size: cover;
    }

    .statsInfo {
        position: absolute;
        display: flex;
        width: 100%;
        align-items: center;
        padding: 20px;
    }

    .playerHealthBorder {
        position: relative;
        width: 100%;
        display: flex;
        justify-content: flex-end;
        border-top: 4px solid white;
        border-left: 4px solid white;
        border-bottom: 4px solid white;
    }

    .enemyHealthBorder {
        position: relative;
        width: 100%;
        display: flex;
        justify-content: flex-end;
        border-top: 4px solid white;
        border-right: 4px solid white;
        border-bottom: 4px solid white;
    }

    #timer {
        background-color: black;
        width: 100px;
        height: 50px;
        flex-shrink: 0;
        display: flex;
        align-items: center;
        justify-content: center;
        color: white;
        border: 4px solid white;
        font-family: Creepster;
        font-size: 25px;
    }

    .playerHealth {
        display: flex;
        background-color: red;
        height: 30px; 
        width: 100%;
    }

    .currentPlayerHealth {
        position: absolute;
        background: #818cf8;
        top: 0;
        right: 0;
        bottom: 0;
        width: 100%;
    }

    .enemyHealth {
        background-color: red;
        height: 30px;
        width: 100%;
    }

    .currentEnemyHealth {
        position: absolute;
        background: #818cf8;
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
    }

    #battle {
        display: flex;
        position: relative;
        margin-top: 10rem;
    }

    .positionNames {
        display: grid;
        grid-template-columns: 1fr 1fr;
        margin-bottom: -20px;
        font-size: 25px;
    }

    #playerName {
        font-family: Bangers;
        color: #f8f9f6
    }

    #enemyName {
        font-family: Bangers;
        color: #f8f9f6
    }
    
    @font-face {
        font-family: 'Bangers';
        src: url('@/assets/Bangers-Regular.ttf') format('truetype');
    }

    @font-face {
        font-family: 'Creepster';
        src: url('@/assets/Creepster-Regular.ttf') format('truetype');
    }
</style>