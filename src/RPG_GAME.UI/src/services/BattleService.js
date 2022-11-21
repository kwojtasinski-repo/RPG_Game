import BackgroundDrawService from "./BackgroundDrawService";
import EnemyService, { actions } from "./EnemyService";
import HeroService from "./HeroService";
import HeroStateService from "./HeroStateService";
import { storyStates } from "./StoryService";
import store from '@/vuex.js';
import { AddBattleEventRequest } from "@/grpc-client/battle_pb.js";
import grpcClient from "@/grpc-client/grpc-client-setup";

let service = null;

export default class BattleService {
    constructor(storyService) {
        this.heroService = new HeroService(store.getters.player.hero);
        this.inputHandler = new InputHandler();
        this.heroStateService = new HeroStateService(this);
        this.enemyService = new EnemyService(storyService.getNextEnemy().enemy);
        this.backgroundDrawService = new BackgroundDrawService();
        this.currentKey = null;
        this.allowSelectState = true;
        this.gameState = gameStates.InProgress;
        this.storyService = storyService;
        this.respawnNewEnemy = false;
        service = this;
        document.addEventListener('attack', this.makeAttack);
        document.addEventListener('idle', this.idleHandler);
        document.addEventListener('deadHero', this.deadHeroHandler);
        document.addEventListener('storyAccepted', this.storyAcceptedHandler);
        document.addEventListener('respawnNewEnemy', this.respawnNewEnemyHandler);
    }

    start(context, deltaTime) {
        this.storyService.showStoryText(context);
        if (!this.storyService.storyTextShow.includes(this.storyService.storyState)) {
            this.setAllowSelectState(false);
        }

        this.heroStateService.selectState(this.inputHandler.key, deltaTime);
        this.enemyService.selectAction(deltaTime);
        this.heroService.draw(context);
        this.enemyService.draw(context);
        this.setPlayerHealthBarValue(`${this.heroService.currentHealth} / ${this.heroService.health}`);
        this.setEnemyHealthBarValue(`${this.enemyService.currentHealth} / ${this.enemyService.health}`);

        if (this.heroService.currentDamageDealt) {
            this.backgroundDrawService.drawDamage(context, { x: this.enemyService.enemy.x + 80, y: this.enemyService.enemy.y }, this.heroService.currentDamageDealt);
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
    
    makeAttack = async (event) => {
        // eslint-disable-next-line
        debugger;
        const battle = store.getters.battle;
        const player = store.getters.player;
        console.log('ATTACK!', event.detail);
        const response = await grpcClient.addBattleEvent(new AddBattleEventRequest([battle.id, player.id, service.enemyService.id, event.detail.name]));
        const battleEventResponse = mapBattleEventResponse(response);
        console.log(response);
        console.log(battleEventResponse);
        service.enemyService.setCurrentAction(actions.fight);
        service.heroService.currentDamageDealt = service.enemyService.currentHealth - battleEventResponse.action.health;
        service.enemyService.currentHealth = battleEventResponse.action.health;
        service.enemyService.currentDamageDealt = battleEventResponse.action.damageDealt;
        service.heroService.currentHealth = service.heroService.currentHealth - battleEventResponse.action.damageDealt;
                
        if (service.enemyService.isDead()) {
            service.enemyService.setCurrentAction(actions.deadAnimation);
            service.deadEnemyHandler();
        }

        player.currentExp = battleEventResponse.currentExp;
        player.requiredExp = battleEventResponse.requiredExp;
        player.level = battleEventResponse.level;
        store.dispatch('player', player);
    }

    getPlayerHealthBar() {
        if (!this.playerHealthBar) {
            this.playerHealthBar = document.querySelector('#playerHealth');
        }

        return this.playerHealthBar;
    }

    setPlayerHealthBarValue(value) {
        const healthBarValue = document.querySelector('#playerHealthValue');
        healthBarValue.innerHTML = value;
        const percentageCurrentHeroHealth = (service.heroService.currentHealth / service.heroService.health) * 100;
        this.getPlayerHealthBar().style.width = (percentageCurrentHeroHealth > 0 ? percentageCurrentHeroHealth : 0) + '%';
    }

    getEnemyHealthBar() {
        if (!this.enemyHealthBar) {
            this.enemyHealthBar = document.querySelector('#enemyHealth');
        }

        return this.enemyHealthBar;
    }

    setEnemyHealthBarValue(value) {
        const healthBarValue = document.querySelector('#enemyHealthValue');
        healthBarValue.innerHTML = value;
        const percentageCurrentEnemyHealth = (service.enemyService.currentHealth / service.enemyService.health) * 100;
        this.getEnemyHealthBar().style.width = (percentageCurrentEnemyHealth > 0 ? percentageCurrentEnemyHealth : 0) + '%';
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
        this.storyService.addEnemyKilled(service.storyService.currentEnemy.enemy);
        setTimeout(() => {
            this.heroService.currentDamageDealt = null;
            this.enemyService.currentDamageDealt = null;
        }, 500);

        this.storyService.selectStoryState();

        if (this.storyService.storyState === storyStates.End && this.heroService.currentHealth > 0) {
            service.gameState = gameStates.Won;
            return;
        }

        this.respawnNewEnemy = true;
        const nextEnemy = this.storyService.getNextEnemy().enemy;
        this.enemyService.setEnemy(nextEnemy);
    }

    setAllowSelectState(allowSelectState) {
        if (this.respawnNewEnemy) {
            this.allowSelectState = false;
            return;
        }

        this.allowSelectState = allowSelectState;
    }

    storyAcceptedHandler() {
        service.setAllowSelectState(true);
    }

    respawnNewEnemyHandler() {
        service.respawnNewEnemy = false;
        service.setAllowSelectState(true);
    }

    destroy() {
        document.removeEventListener('attack', this.makeAttack);
        document.removeEventListener('idle', this.idleHandler);
        document.removeEventListener('deadHero', this.deadHeroHandler);
        document.removeEventListener('storyAccepted', this.storyAcceptedHandler);
        document.removeEventListener('respawnNewEnemy', this.respawnNewEnemyHandler);
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

const mapBattleEventResponse = (response) => {
    return {
        id: response.getLevel(),
        battleId: response.getBattleid(),
        created: new Date(response.getCreated().toDate()),
        action: {
            characterId: response.getAction().getCharacterid(),
            character: response.getAction().getCharacter(),
            name: response.getAction().getName(),
            damageDealt: response.getAction().getDamagedealt(),
            health: response.getAction().getHealth(),
            attackInfo: response.getAction().getAttackinfo()
        },
        currentExp: Number(response.getCurrentexp().getUnits() + '.' + response.getCurrentexp().getNanos()),
        requiredExp: Number(response.getRequiredexp().getUnits() + '.' + response.getRequiredexp().getNanos()),
        level: response.getLevel()
    }
};