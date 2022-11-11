import BackgroundDrawService from "./BackgroundDrawService";
import EnemyService, { actions } from "./EnemyService";
import HeroService from "./HeroService";
import HeroStateService from "./HeroStateService";

let service = null;

export default class BattleService {
    constructor(storyService) {
        this.heroService = new HeroService();
        this.inputHandler = new InputHandler();
        this.heroStateService = new HeroStateService(this);
        this.enemyService = new EnemyService();
        this.backgroundDrawService = new BackgroundDrawService();
        this.currentKey = null;
        this.allowSelectState = true;
        this.gameState = gameStates.InProgress;
        this.storyService = storyService;
        service = this;
        document.addEventListener('attack', this.makeAttack);
        document.addEventListener('idle', this.idleHandler);
        document.addEventListener('deadHero', this.deadHeroHandler);
    }

    start(context, deltaTime) {
        this.storyService.showStoryText(context);
        if (!this.storyService.storyTextShow.includes(this.storyService.storyState)) {
            this.allowSelectState = false;
        } else if (!this.currentKey && this.gameState === gameStates.InProgress) {
            this.allowSelectState = true;
        }

        this.heroStateService.selectState(this.inputHandler.key, deltaTime);
        this.enemyService.selectAction(deltaTime);
        this.heroService.draw(context);
        this.enemyService.draw(context);

        if (this.heroService.currentDamageDealt) {
            this.backgroundDrawService.drawDamage(context, { x: this.enemyService.x + 80, y: this.enemyService.y }, this.heroService.currentDamageDealt);
        }
        if (this.enemyService.currentDamageDealt) {
            this.backgroundDrawService.drawDamage(context, { x: this.heroService.x + 80, y: this.heroService.y }, this.enemyService.currentDamageDealt);
        }

        if (this.gameState === gameStates.Won) {
            this.backgroundDrawService.drawWonGame(context);
        } else if (this.gameState === gameStates.Lost) {
            this.backgroundDrawService.drawLostGame(context);
        }
    }
    
    makeAttack(event) {
        console.log('ATTACK!', event.detail);
        service.enemyService.currentAction = actions.fight;
        service.heroService.currentDamageDealt = service.heroService.attack[event.detail.name];
        service.enemyService.currentHealth = service.enemyService.currentHealth - service.heroService.currentDamageDealt;
        service.enemyService.currentDamageDealt = Math.random() === 1 ? service.enemyService.attack.skill : service.enemyService.attack.baseAttack;
        service.heroService.currentHealth = service.heroService.currentHealth - service.enemyService.currentDamageDealt;
        const percentageCurrentHeroHealth = (service.heroService.currentHealth / service.heroService.health) * 100;
        const percentageCurrentEnemyHealth = (service.enemyService.currentHealth / service.enemyService.health) * 100;
        
        service.getPlayerHealthBar().style.width = (percentageCurrentHeroHealth > 0 ? percentageCurrentHeroHealth : 0) + '%';
        service.getEnemyHealthBar().style.width = (percentageCurrentEnemyHealth > 0 ? percentageCurrentEnemyHealth : 0) + '%';
        
        if (service.enemyService.isDead()) {
            service.enemyService.currentAction = actions.deadAnimation;
            service.deadEnemyHandler();
        }
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
        service.heroService.currentDamageDealt = null;
        service.enemyService.currentDamageDealt = null;
    }

    deadHeroHandler() {
        // logic pause game, text you lost
        service.heroService.currentDamageDealt = null;
        service.enemyService.currentDamageDealt = null;
        service.gameState = gameStates.Lost;
    }

    deadEnemyHandler() {
        // logic get next enemy or won
        // if every enemy was killed disable input for battle and set state game as won
        setTimeout(() => {
            service.heroService.currentDamageDealt = null;
            service.enemyService.currentDamageDealt = null;
        }, 500);
        service.gameState = gameStates.Won;
    }

    destroy() {
        document.removeEventListener('attack', this.makeAttack);
        document.removeEventListener('idle', this.idleHandler);
        document.removeEventListener('deadHero', this.deadHeroHandler);
    }
}

export const gameStates = { Won: 'Won', Lost: 'Lost', InProgress: 'InProgress' };

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