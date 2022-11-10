import { gameStates } from "./BattleService";

class BaseState {
    constructor(battleService) {
        this.battleService = battleService;
        this.eventSent = false;
        this.stateReset = false;
        this.deadAnimationInvoked = false;
    }

    update(deltaTime) {
        if (this.battleService.heroService.frameTimer > this.battleService.heroService.frameInterval) {
            this.battleService.heroService.frameTimer = 0;

            if (this.battleService.heroService.frameX < this.battleService.heroService.maxFrame) {
                this.battleService.heroService.frameX ++;
            } else {
                this.battleService.heroService.frameX = 0;
            }
        } else {
            this.battleService.heroService.frameTimer +=  deltaTime;
        }
    }

    sendEvent(event) {
        if(!this.eventSent) {
            document.dispatchEvent(event);
            this.eventSent = true;
        }
    }

    resetState(ms) {
        if(!this.stateReset) {
            setTimeout(() => {
                this.battleService.currentKey = null;
                this.battleService.allowSelectState = true;
                this.eventSent = false;
                this.stateReset = false;
            }, ms ? ms : 1000);
            this.stateReset = true;
        }
    }
}

export default class HeroStateService extends BaseState {
    constructor(battleService) {
        super(battleService);
    }

    selectState(key, deltaTime) {
        if (this.battleService.allowSelectState) {
            this.battleService.currentKey = key;
        }

        if (key === 'Esc') {
            // pause
            this.battleService.allowSelectState = false;
        }

        if (this.battleService.currentKey === 'Enter') {
            this.baseAttack(deltaTime);
        } else if (parseInt(this.battleService.currentKey, 10) === 1) {
            this.skillAttack(deltaTime);
        } else if (this.battleService.heroService.isDead()) {
            this.dead(deltaTime);
        } else if (this.battleService.gameState === gameStates.Won) {
            this.won(deltaTime);
        } else {
            this.idle(deltaTime);
        }
    }

    baseAttack(deltaTime) {
        this.battleService.heroService.frameY = 3;
        this.battleService.heroService.maxFrame = 5;
        this.battleService.allowSelectState = false;
        this.sendEvent(new CustomEvent('attack', { detail: { name: 'baseAttack' } }));

        const nextFrame = this.battleService.heroService.frameX + 1;
        this.update(deltaTime);

        if (nextFrame === this.battleService.heroService.maxFrame) {
            this.resetState();
        }
    }

    skillAttack(deltaTime) {
        this.battleService.heroService.frameY = 13;
        this.battleService.heroService.maxFrame = 5;
        this.battleService.allowSelectState = false;
        this.sendEvent(new CustomEvent('attack', { detail: { name: 'skill' } }));

        const nextFrame = this.battleService.heroService.frameX + 1;
        this.update(deltaTime);

        if (nextFrame === this.battleService.heroService.maxFrame) {
            this.resetState();
        }
    }

    idle(deltaTime) {
        this.battleService.heroService.frameY = 0;
        this.battleService.heroService.maxFrame = 5;
        this.battleService.allowSelectState = true;
        this.sendEvent(new CustomEvent('idle'));
        this.resetState(5);
        this.update(deltaTime);
    }

    dead(deltaTime) {
        this.battleService.heroService.frameY = 6;
        this.battleService.heroService.maxFrame = 0;

        if (this.deadAnimationInvoked) {
            this.battleService.heroService.frameX = 0;
            return;
        }

        this.battleService.heroService.frameX = this.battleService.heroService.frameX === 0 ? 5 : this.battleService.heroService.frameX;
        this.battleService.allowSelectState = false;

        if (this.battleService.heroService.frameTimer > this.battleService.heroService.frameInterval) {
            this.battleService.heroService.frameTimer = 0;

            if (this.battleService.heroService.frameX > this.battleService.heroService.maxFrame + 1) {
                this.battleService.heroService.frameX--;
            } else {
                this.battleService.heroService.frameX = 0;
                this.deadAnimationInvoked = true;
                document.dispatchEvent(new CustomEvent('deadHero'));
            }
        } else {
            this.battleService.heroService.frameTimer += deltaTime;
        }
    }

    won(deltaTime) {
        this.battleService.heroService.frameY = 9;
        this.battleService.heroService.maxFrame = 5;
        this.battleService.allowSelectState = false;
        this.update(deltaTime);
    }
}