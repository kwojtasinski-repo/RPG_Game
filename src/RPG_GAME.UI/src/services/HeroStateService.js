class BaseState {
    constructor(battleService) {
        this.battleService = battleService;
        this.eventSent = false;
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

    sendEventAndResetInput(event) {
        if(!this.eventSent) {
            setTimeout(() => {
                this.battleService.currentKey = null;
                this.battleService.allowSelectState = true;
                document.dispatchEvent(event);
                this.eventSent = false;
            }, 1000);
            this.eventSent = true;
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
        } else {
            this.idle(deltaTime);
        }
    }

    baseAttack(deltaTime) {
        this.battleService.heroService.frameY = 3;
        this.battleService.heroService.maxFrame = 5;
        this.battleService.allowSelectState = false;

        const nextFrame = this.battleService.heroService.frameX + 1;
        this.update(deltaTime);

        if (nextFrame === this.battleService.heroService.maxFrame) {
            this.sendEventAndResetInput(new CustomEvent('attack', { detail: { name: 'baseAttack' } }));
        }
    }

    skillAttack(deltaTime) {
        this.battleService.heroService.frameY = 13;
        this.battleService.heroService.maxFrame = 5;
        this.battleService.allowSelectState = false;

        const nextFrame = this.battleService.heroService.frameX + 1;
        this.update(deltaTime);

        if (nextFrame === this.battleService.heroService.maxFrame) {
            this.sendEventAndResetInput(new CustomEvent('attack', { detail: { name: 'skill' } }));
        }
    }

    idle(deltaTime) {
        this.battleService.heroService.frameY = 0;
        this.battleService.heroService.maxFrame = 5;
        this.battleService.allowSelectState = true;
        this.update(deltaTime);
    }
}