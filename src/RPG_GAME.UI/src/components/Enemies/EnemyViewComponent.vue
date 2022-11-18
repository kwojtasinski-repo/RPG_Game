<template>
    <div class="row">
        <div class="enemy-view">
            <div class="card card-enemy">
                <div class="card-header">
                    <div class="enemy-title mt-2 mb-2 me-2">
                        <div>
                            {{enemy.enemyName}}
                        </div>
                        <div class="image-enemy ms-2">
                            <img :src="enemyImage"/>
                        </div>
                    </div>
                </div>
                <div class="card-body text-center">
                    <div class="enemy-prop-info">
                        Properties
                    </div>
                    <div class="card-body-content">
                        <div class="enemy-prop mt-2 mb-2">
                        <div class="enemy-prop-decription">
                            <div class="enemy-prop-text">
                                Attack:
                            </div>
                            <div>Strategy increasing
                            <span class="badge rounded-pill bg-success">{{enemy.baseAttack.increasingState.strategyIncreasing}}</span>
                            </div>
                        </div>
                        <div class="enemy-prop-decription">
                            <div class="enemy-prop-text">
                                {{enemy.baseAttack.value}}
                            </div>
                            <div>
                                {{enemy.baseAttack.increasingState.value}}
                            </div>
                        </div>
                        </div>
                    </div>
                    <div class="card-body-content">
                        <div class="enemy-prop mt-2 mb-2">
                        <div class="enemy-prop-decription">
                            <div class="enemy-prop-text">
                                Health:
                            </div>
                            <div>Strategy increasing
                            <span class="badge rounded-pill bg-success">{{enemy.baseHealth.increasingState.strategyIncreasing}}</span>
                            </div>
                        </div>
                        <div class="enemy-prop-decription">
                            <div class="enemy-prop-text">
                                {{enemy.baseHealth.value}}
                            </div>
                            <div>
                                {{enemy.baseHealth.increasingState.value}}
                            </div>
                        </div>
                        </div>
                    </div>
                    <div class="card-body-content">
                        <div class="enemy-prop mt-2 mb-2">
                        <div class="enemy-prop-decription">
                            <div class="enemy-prop-text">
                                Heal
                            </div>
                            <div>Strategy increasing
                            <span class="badge rounded-pill bg-success">{{enemy.baseHealLvl.increasingState.strategyIncreasing}}</span>
                            </div>
                        </div>
                        <div class="enemy-prop-decription">
                            <div class="enemy-prop-text">
                                {{enemy.baseHealLvl.value}}
                            </div>
                            <div>
                                {{enemy.baseHealLvl.increasingState.value}}
                            </div>
                        </div>
                        </div>
                    </div>
                    <div class="card-body-content">
                        <div class="enemy-prop mt-2 mb-2">
                        <div class="enemy-prop-decription">
                            <div class="enemy-prop-text">
                                Required experience:
                            </div>
                            <div>Strategy increasing
                            <span class="badge rounded-pill bg-success">{{enemy.experience.increasingState.strategyIncreasing}}</span>
                            </div>
                        </div>
                        <div class="enemy-prop-decription">
                            <div class="enemy-prop-text">
                                {{enemy.experience.value}}
                            </div>
                            <div>
                                {{enemy.experience.increasingState.value}}
                            </div>
                        </div>
                        </div>
                    </div>
                    <div class="card-body-content">
                        <div class="enemy-prop mt-2 mb-2">
                            <div class="enemy-prop-decription">
                                <div class="enemy-prop-text">
                                    Difficulty:
                                </div>
                            </div>
                            <div class="enemy-prop-decription">
                                <div class="enemy-prop-text">
                                    {{enemy.difficulty}}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-body-content">
                        <div class="enemy-prop mt-2 mb-2">
                            <div class="enemy-prop-decription">
                                <div class="enemy-prop-text">
                                    Category:
                                </div>
                            </div>
                            <div class="enemy-prop-decription">
                                <div class="enemy-prop-text">
                                    {{enemy.category}}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div v-if="enemy.skills.length > 0" class="mt-2 enemy-prop-info">
                        Skills
                    </div>
                    <div v-for="skill in enemy.skills" :key="skill.id">
                        <div class="card-body-content">
                            <div class="enemy-prop mt-2 mb-2">
                                <div class="enemy-prop-decription">
                                    <div class="enemy-prop-text">
                                        Name:
                                    </div>
                                </div>
                                <div class="enemy-prop-decription">
                                    <div class="enemy-prop-text">
                                        {{skill.name}}
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body-content">
                            <div class="enemy-prop mt-2 mb-2">
                                <div class="enemy-prop-decription">
                                    <div class="enemy-prop-text">
                                        Probability:
                                    </div>
                                </div>
                                <div class="enemy-prop-decription">
                                    <div class="enemy-prop-text">
                                        {{skill.probability}}%
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body-content">
                            <div class="enemy-prop mt-2 mb-2">
                                <div class="enemy-prop-decription">
                                    <div class="enemy-prop-text">
                                        Attack:
                                    </div>
                                    <div>Strategy increasing
                                        <span class="badge rounded-pill bg-success">{{skill.increasingState.strategyIncreasing}}</span>
                                    </div>
                                </div>
                                <div class="enemy-prop-decription">
                                    <div class="enemy-prop-text">
                                        {{skill.baseAttack}}
                                    </div>
                                    <div>
                                        {{skill.increasingState.value}}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import archerImage from '@/assets/archer/archer-img.png'
import knightImage from '@/assets/enemy-knight-img.png';
import dragonImage from '@/assets/dragon/dragon-knight-img.png'

  export default {
    name: 'EnemyViewComponent',
    props: {
        enemy: {
            type: Object,
            required: true
        }
    },
    components: {
    },
    data() {
        return {
            enemyImage: null
        }
    },
    created() {
        switch (this.enemy.category) {
            case 'Archer':
                this.enemyImage = archerImage;
                break;
            case 'Knight':
                this.enemyImage = knightImage;
                break;
            case 'Dragon':
                this.enemyImage = dragonImage;
                break;
            default:
                throw new Error(`There is no enemy with category ${this.enemy.category}`);
        }
    }
  }
</script>

<style>
  .row {
    width: auto;
  }

  .enemy-view {
    text-align: center;
  }

  .enemy-title {
    font-size: 1.5rem;
    display: flex;
    justify-content: center;
  }

  .image-enemy {
    transform: scale(1.5,1.5) translateX(15%);
  }

  .card-enemy {
    width: 45rem;
  }
  
  .card-body-content {
    width: 30rem;
    display: flex;
    flex-flow: row wrap;
  }

  .enemy-prop {
    flex-basis: 100%;
    display: flex;
    flex-flow: row nowrap;
  }
  
  .enemy-prop-decription {
    position: relative;
    display: flex;
    flex-flow: column nowrap;
    align-items: center;
    flex: 1 1 100%;
    overflow: hidden;
  }
  
  .enemy-prop-text {
    white-space: nowrap;
    font-size: 1.25rem;
  }

  .enemy-prop-info {
    font-size: 1.4rem;
  }
</style>