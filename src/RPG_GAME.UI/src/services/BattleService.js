import HeroService from "./HeroService";

export default class BattleService {
    constructor() {
        this.heroService = new HeroService();
        this.inputHandler = new InputHandler();
    }

    start(context, deltaTime) {
        this.heroService.frameY = 0;
        if (this.inputHandler.key === 'Enter') {
            this.heroService.frameY = 3;
            if (this.heroService.frameTimer > this.heroService.frameInterval) {
                this.heroService.frameTimer = 0;
    
                if (this.heroService.frameX < 5) { // refactor to maxFrame = 5
                    this.heroService.frameX ++;
                } else {
                    this.heroService.frameX = 0;
                }
            } else {
                this.heroService.frameTimer +=  deltaTime;
            }
        }
        this.heroService.draw(context);
    }
}

class InputHandler {
    constructor() {
        this.key = null;
        window.addEventListener('keydown', e => {
            if (e.key === 'Enter') {
                this.key = e.key;
            }
        });

        window.addEventListener('keyup', e => {
            if (e.key === 'Enter') {
                this.key = null;
            }
        });
    }

}