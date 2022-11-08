import enemyKnight from "@/assets/enemy-knight-reverse.png"

export const actions = {
    idle: 1,
    fight: 2,
    deadAnimation: 3,
    dead: 4
}

export default class EnemyService {
    constructor() {
        this.frameX = 7;
        this.frameY = 0;
        this.image = new Image();
        this.image.src = enemyKnight;
        this.fps = 20;
        this.frameInterval = 1000 / this.fps;
        this.frameTimer = 0;
        this.width = 56;
        this.height = 55;
        this.x = 420;
        this.y = 155;
        this.offsetY = 15;
        this.minFrame = 2;
        this.health = 100;
        this.currentHealth = 100;
        this.attack = {
            baseAttack: 15,
            skill: 30
        }
        this.currentAction = actions.idle;
        this.actionInvoked = false;
    }

    update(deltaTime) {
        if (this.frameTimer > this.frameInterval) {
            this.frameTimer = 0;

            if (this.frameX > this.minFrame) {
                this.frameX--;
            } else {
                this.frameX = 7;
            }
        } else {
            this.frameTimer +=  deltaTime;
        }
    }

    draw(context) {
        context.drawImage(this.image, this.frameX * this.width, (this.frameY * this.height) + this.offsetY, this.width, this.height, this.x, this.y, this.width * 3, this.height * 3);
    }

    selectAction(deltaTime) {
        if (this.currentAction === actions.idle) {
            this.minFrame = 2;
            this.frameY = 0;
            this.update(deltaTime);
        } else if (this.currentAction === actions.fight) {
            this.minFrame = 0;
            this.frameY = 1;
            const nextFrame = this.frameX - 1;
            this.update(deltaTime);
            if (nextFrame < this.minFrame) {
                if (!this.actionInvoked) {
                    setTimeout(() => {
                        this.currentAction = actions.idle;
                        this.frameX = 7;
                        this.actionInvoked = false;
                    }, 500);
                    this.actionInvoked = true;
                }
            }
        } else if (this.currentAction === actions.deadAnimation) {
            this.frameY = 6;
            this.minFrame = 0;
            this.update(deltaTime);
            if (!this.actionInvoked) {
                setTimeout(() => {
                    this.currentAction = actions.dead;
                    this.actionInvoked = false;
                }, 500);
                this.actionInvoked = true;
            }
        } else if (this.currentAction === actions.dead) {
            this.frameY = 7;
            this.minFrame = 7;
            this.update(deltaTime);
        }
    }

    isDead() {
        return this.currentHealth <= 0;
    }
}