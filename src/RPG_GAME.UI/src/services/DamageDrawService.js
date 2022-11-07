export default class DamageDrawService {
    constructor() {
        this.font = '20px Creepster';
    }
    
    update() {
        this.x += (this.targetX - this.x) * 0.03;
        this.y += (this.targetY - this.y) * 0.03;
        this.timer++;

        if (this.timer > 1000) {
            this.markedForDeletion = true;
        }
    }
    
    draw(context, position, value) {
        context.font = this.font;
        context.fillStyle = 'red';
        context.fillText(value, position.x, position.y);
        context.fillStyle = 'black';
        context.fillText(value, position.x - 2, position.y - 2);
    }
}