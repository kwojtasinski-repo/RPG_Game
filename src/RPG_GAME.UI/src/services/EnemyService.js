import enemyKnight from "@/assets/enemy-knight-reverse.png"
import archerIdle from "@/assets/archer/idle-and-running.png"
import archerAttack from "@/assets/archer/normal-attack.png"
import archerDeath from "@/assets/archer/death.png"
import dragonIdle from "@/assets/dragon/dragon-knight-idle.png"
import dragonAttack from "@/assets/dragon/dragon-knight-attack.png"
import dragonDeath from "@/assets/dragon/dragon-knight-death.png"

export const actions = {
    idle: 1,
    fight: 2,
    deadAnimation: 3,
    dead: 4
}

export default class EnemyService {
    constructor(enemy) {
        this.initEnemy(enemy);
        this.enemy = getEnemyKind(enemy.category);
    }

    initEnemy(enemy) {
        console.log(enemy);
        this.id = enemy.id;
        this.health = enemy.health;
        this.currentHealth = enemy.health
    }

    draw(context) {
        this.enemy.draw(context);
    }

    setCurrentAction(currentAction) {
        this.enemy.currentAction = currentAction;
    }

    selectAction(deltaTime) {
        this.enemy.selectAction(deltaTime);
    }

    isDead() {
        return this.currentHealth <= 0;
    }

    setEnemy(enemy) {
        // depends on category Archer, Knight, Dragon
        setTimeout(() => {
            assignBaseStats(this, enemy);
            document.dispatchEvent(new CustomEvent('respawnNewEnemy', { detail: { enemy: this } }));
        }, 1500);
    }
}

const assignBaseStats = (enemyTarget, enemy) => {
    enemyTarget.initEnemy(enemy);
    enemyTarget.enemy = getEnemyKind(enemy.category);
}

const getEnemyKind = (category) => {
    switch (category) {
        case 'Archer': 
            return new Archer();
        case 'Knight':
            return new Knight();
        case 'Dragon':
            return new Dragon();
        default:
            throw new Error(`There is no category enemy for ${category}`);
    }
}

class Enemy {
    update(deltaTime) {
        if (this.frameTimer > this.frameInterval) {
            this.frameTimer = 0;

            if (this.frameX > this.minFrame) {
                this.frameX--;
            } else {
                this.frameX = this.maxFrame;
            }
        } else {
            this.frameTimer +=  deltaTime;
        }
    }
}

class Archer extends Enemy {
    constructor() {
        super();
        this.frameX = 7;
        this.maxFrame = 7;
        this.frameY = 0;
        this.image = new Image();
        this.image.src = archerIdle;
        this.fps = 15;
        this.frameInterval = 1000 / this.fps;
        this.frameTimer = 0;
        this.width = 64;
        this.height = 65;
        this.x = 530;
        this.y = 245;
        this.offsetY = 0;
        this.minFrame = 7;
        this.currentAction = actions.idle;
    }

    draw(context) {
        context.drawImage(this.image, this.frameX * this.width, (this.frameY * this.height) + this.offsetY, this.width, this.height, this.x, this.y, this.width * 2.25, this.height * 2.25);
    }

    selectAction(deltaTime) {
        if (this.currentAction === actions.idle) {
            this.offsetY = 0;
            this.image.src = archerIdle;
            this.minFrame = 7;
            this.frameY = 0;
            this.update(deltaTime);
        } else if (this.currentAction === actions.fight) {
            this.offsetY = 0;
            this.image.src = archerAttack;
            this.currentFrame = this.currentFrame ? this.currentFrame : 0;
            const frames = [ { id: 0, x: 7, y: 0 }, { id: 1, x: 6, y: 0 }, { id: 2, x: 5, y: 0 }, { id: 3, x: 4, y: 0 }, { id: 4, x: 3, y: 0 }, { id: 5, x: 2, y: 0 }, { id: 6, x: 1, y: 0 }, 
                { id: 7, x: 0, y: 0 }, { id: 8, x: 7, y: 1 }, { id: 9, x: 6, y: 1 }, { id: 10, x: 5, y: 1 } ];
            
            if (this.frameTimer > this.frameInterval) {
                this.frameTimer = 0;
    
                if (this.currentFrame < 10) {
                    this.frameX = frames[this.currentFrame].x;
                    this.frameY = frames[this.currentFrame].y;
                    this.currentFrame++;
                } else {
                    this.currentFrame = null;
                    this.currentAction = actions.idle;
                }
            } else {
                this.frameTimer +=  deltaTime;
            }
        } else if (this.currentAction === actions.deadAnimation) {
            this.offsetY = 65;
            this.image.src = archerDeath;
            this.frameY = 0;
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
            this.offsetY = 65;
            this.image.src = archerDeath;
            this.frameY = 1;
            this.minFrame = 7;
            this.update(deltaTime);
        }
    }   
}

class Knight extends Enemy {
    constructor() {
        super();
        this.frameX = 7;
        this.maxFrame = 7;
        this.frameY = 0;
        this.image = new Image();
        this.image.src = enemyKnight;
        this.fps = 20;
        this.frameInterval = 1000 / this.fps;
        this.frameTimer = 0;
        this.width = 56;
        this.height = 55;
        this.x = 520;
        this.y = 255;
        this.offsetY = 15;
        this.minFrame = 2;
        this.actionInvoked = false;
        this.currentAction = actions.idle;
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
}

class Dragon extends Enemy {
    constructor() {
        super();
        this.frameX = 0;
        this.maxFrame = 3;
        this.frameY = 0;
        this.image = new Image();
        this.image.src = dragonIdle;
        this.fps = 10;
        this.frameInterval = 1000 / this.fps;
        this.frameTimer = 0;
        this.width = 475;
        this.height = 120;
        this.x = 550;
        this.y = 255;
        this.offsetX = 250;
        this.offsetY = 215;
        this.minFrame = 0;
        this.actionInvoked = false;
        this.currentAction = actions.idle;
    }
    
    draw(context) {
        context.drawImage(this.image, (this.frameX * this.width) + this.offsetX, (this.frameY * this.height) + this.offsetY, this.width, this.height, this.x, this.y, this.width * 1, this.height * 1);
    }

    selectAction(deltaTime) {
        if (this.currentAction === actions.idle) {
            this.image.src = dragonIdle;
            this.fps = 1;
            this.width = 480;
            this.height = 120;
            this.offsetX = 250;
            this.offsetY = 215;
            this.minFrame = 0;
            this.frameX = 0;
            this.frameY = 0;
        } else if (this.currentAction === actions.fight) {
            this.minFrame = 0;
            this.image.src = dragonAttack;
            this.fps = 1;
            this.width = 480;
            this.height = 120;
            this.offsetX = 150;
            this.offsetY = 215;
            this.minFrame = 0;
            this.frameY = 0;
            const nextFrame = this.frameX - 1;
            this.update(deltaTime);
            if (nextFrame < this.minFrame) {
                if (!this.actionInvoked) {
                    setTimeout(() => {
                        this.currentAction = actions.idle;
                        this.frameX = 0;
                        this.actionInvoked = false;
                    }, 500);
                    this.actionInvoked = true;
                }
            }
        } else if (this.currentAction === actions.deadAnimation) {
            this.image.src = dragonAttack;dragonDeath
            this.fps = 1;
            this.width = 480;
            this.height = 120;
            this.offsetX = 150;
            this.offsetY = 215;
            this.minFrame = 0;
            this.frameY = 0;
            this.update(deltaTime);
            if (!this.actionInvoked) {
                setTimeout(() => {
                    this.currentAction = actions.dead;
                    this.actionInvoked = false;
                }, 500);
                this.actionInvoked = true;
            }
        } else if (this.currentAction === actions.dead) {
            this.image.src = dragonDeath;
            this.fps = 1;
            this.width = 480;
            this.height = 120;
            this.offsetX = 150;
            this.offsetY = 215;
            this.minFrame = 0;
            this.frameX = 0;
            this.frameY = 0;
        }
    }
}