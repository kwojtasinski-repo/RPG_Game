export default class BackgroundDrawService {

    update() {
        this.x += (this.targetX - this.x) * 0.03;
        this.y += (this.targetY - this.y) * 0.03;
        this.timer++;

        if (this.timer > 1000) {
            this.markedForDeletion = true;
        }
    }
    
    drawDamage(context, position, value) {
        context.font = '20px Creepster';
        context.fillStyle = 'red';
        context.fillText(value, position.x, position.y);
        context.fillStyle = 'black';
        context.fillText(value, position.x - 2, position.y - 2);
    }
    
    drawWonGame(context) {
        context.font = '30px Bangers';
        context.fillStyle = 'white';
        context.fillText('You Won!', 400, 25);
    }
    
    drawLostGame(context) {
        context.font = '30px Bangers';
        context.fillStyle = 'white';
        context.fillText('You Lost!', 400, 25);
    }
}