import warriorImage from "@/assets/Warrior.png"

export default class HeroService {
    constructor() {
        this.frameX = 0;
        this.frameY = 0;
        this.image = new Image();
        this.image.src = warriorImage;
        this.fps = 20;
        this.frameInterval = 1000 / this.fps;
        this.frameTimer = 0;
        this.width = 69;
        this.height = 44;
        this.x = 330;
        this.y = 150;
        this.health = 100;
        this.currentHealth = 100;
        this.attack = {
            baseAttack: 10,
            skill: 25
        }
    }

    draw(context) {
        context.drawImage(this.image, this.frameX * this.width, this.frameY * this.height, this.width, this.height, this.x, this.y, this.width * 3, this.height * 3);
    }

    isDead() {
        return this.currentHealth <= 0;
    }
}