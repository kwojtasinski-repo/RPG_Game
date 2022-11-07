import DamageDrawService from "./DamageDrawService";
import EnemyService, { actions } from "./EnemyService";
import HeroService from "./HeroService";
import HeroStateService from "./HeroStateService";

let service = null;

export default class BattleService {
    constructor() {
        this.heroService = new HeroService();
        this.inputHandler = new InputHandler();
        this.heroStateService = new HeroStateService(this);
        this.enemyService = new EnemyService();
        this.damageDrawService = new DamageDrawService();
        this.currentKey = null;
        this.allowSelectState = true;
        service = this;
        document.addEventListener('attack', this.makeAttack);
        document.addEventListener('idle', this.idleHandler);
    }

    start(context, deltaTime) {
        this.heroStateService.selectState(this.inputHandler.key, deltaTime);
        this.enemyService.selectAction(deltaTime);
        this.heroService.draw(context);
        this.enemyService.draw(context);
        if (this.heroService.currentDamageDealt) {
            this.damageDrawService.draw(context, { x: this.enemyService.x + 80, y: this.enemyService.y }, this.heroService.currentDamageDealt);
        }
        if (this.enemyService.currentDamageDealt) {
            this.damageDrawService.draw(context, { x: this.heroService.x + 80, y: this.heroService.y }, this.enemyService.currentDamageDealt);
        }
    }
    
    makeAttack(event) {
        console.log('ATTACK!', event.detail);
        service.enemyService.currentAction = actions.fight;
        const currentPlayerHealth = service.getPlayerHealthBar().style.width ? Number(service.getPlayerHealthBar().style.width.replace('%','')) : 100;
        service.heroService.currentDamageDealt = 10;
        const currentEnemyHealth = service.getEnemyHealthBar().style.width ? Number(service.getEnemyHealthBar().style.width.replace('%','')) : 100;
        service.enemyService.currentDamageDealt = 10;
        service.getPlayerHealthBar().style.width = (currentPlayerHealth - 10) + '%';
        service.getEnemyHealthBar().style.width = (currentEnemyHealth - 10) + '%';
    }

    getPlayerHealthBar() {
        if (!this.playerHealthBar) {
            this.playerHealthBar = document.querySelector('#playerHealth');
        }

        return this.playerHealthBar;
    }

    getEnemyHealthBar() {
        if (!this.enemyHealthBar) {
            this.enemyHealthBar = document.querySelector('#enemyHealth');
        }

        return this.enemyHealthBar;
    }

    idleHandler() {
        console.log('idle');
        service.heroService.currentDamageDealt = null;
        service.enemyService.currentDamageDealt = null;
    }

    destroy() {
        document.removeEventListener('attack', this.makeAttack);
        document.removeEventListener('idle', this.idleHandler);
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