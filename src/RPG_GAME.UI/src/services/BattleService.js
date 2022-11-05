import HeroService from "./HeroService";
import HeroStateService from "./HeroStateService";

export default class BattleService {
    constructor() {
        this.heroService = new HeroService();
        this.inputHandler = new InputHandler();
        this.heroStateService = new HeroStateService(this);
        this.currentKey = null;
        this.allowSelectState = true;
    }

    start(context, deltaTime) {
        this.heroStateService.selectState(this.inputHandler.key, deltaTime);
        this.heroService.draw(context);
    }
}

class InputHandler {
    constructor() {
        this.key = null;
        window.addEventListener('keydown', e => {
            if (e.key === 'Enter') {
                this.key = e.key;
            } else if (e.key === '1') {
                this.key = e.key;
            }
        });

        window.addEventListener('keyup', e => {
            if (e.key === 'Enter') {
                this.key = null;
            } else if (e.key === '1') {
                this.key = null;
            }
        });
    }
}

export const attackKeys = ['Enter', '1', '2', '3', '4', '5', '6', '7', '8', '9'];