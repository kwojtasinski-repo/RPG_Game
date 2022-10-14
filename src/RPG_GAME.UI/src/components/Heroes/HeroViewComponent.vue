<template>
    <div class="row">
        <div class="hero-view">
            <div class="card card-hero">
                <div class="card-header">
                <div class="hero-title mt-2 mb-2">
                    {{hero.heroName}}
                </div>
                </div>
                <div class="card-body text-center">
                <div class="hero-prop-info">
                    Properties
                </div>
                <div class="card-body-content">
                    <div class="hero-prop mt-2 mb-2">
                    <div class="hero-prop-decription">
                        <div class="hero-prop-text">
                        Attack:
                        </div>
                        <div>Strategy increasing
                        <span class="badge rounded-pill bg-success">{{hero.attack.increasingState.strategyIncreasing}}</span>
                        </div>
                    </div>
                    <div class="hero-prop-decription">
                        <div class="hero-prop-text">
                        {{hero.attack.value}}
                        </div>
                        <div>
                        {{hero.attack.increasingState.value}}
                        </div>
                    </div>
                    </div>
                </div>
                <div class="card-body-content">
                    <div class="hero-prop mt-2 mb-2">
                    <div class="hero-prop-decription">
                        <div class="hero-prop-text">
                        Health:
                        </div>
                        <div>Strategy increasing
                        <span class="badge rounded-pill bg-success">{{hero.health.increasingState.strategyIncreasing}}</span>
                        </div>
                    </div>
                    <div class="hero-prop-decription">
                        <div class="hero-prop-text">
                        {{hero.health.value}}
                        </div>
                        <div>
                        {{hero.health.increasingState.value}}
                        </div>
                    </div>
                    </div>
                </div>
                <div class="card-body-content">
                    <div class="hero-prop mt-2 mb-2">
                    <div class="hero-prop-decription">
                        <div class="hero-prop-text">
                        Required experience:
                        </div>
                        <div>Strategy increasing
                        <span class="badge rounded-pill bg-success">{{hero.baseRequiredExperience.increasingState.strategyIncreasing}}</span>
                        </div>
                    </div>
                    <div class="hero-prop-decription">
                        <div class="hero-prop-text">
                        {{hero.baseRequiredExperience.value}}
                        </div>
                        <div>
                        {{hero.baseRequiredExperience.increasingState.value}}
                        </div>
                    </div>
                    </div>
                </div>
                <div class="mt-2 hero-prop-info">
                    Skills
                </div>
                <div v-for="skill in hero.skills" :key="skill.id">
                    <div class="card-body-content">
                    <div class="hero-prop mt-2 mb-2">
                        <div class="hero-prop-decription">
                        <div class="hero-prop-text">
                            Name:
                        </div>
                        </div>
                        <div class="hero-prop-decription">
                        <div class="hero-prop-text">
                            {{skill.name}}
                        </div>
                        </div>
                    </div>
                    </div>
                    <div class="card-body-content">
                    <div class="hero-prop mt-2 mb-2">
                        <div class="hero-prop-decription">
                        <div class="hero-prop-text">
                            Attack:
                        </div>
                        <div>Strategy increasing
                            <span class="badge rounded-pill bg-success">{{skill.increasingState.strategyIncreasing}}</span>
                        </div>
                        </div>
                        <div class="hero-prop-decription">
                        <div class="hero-prop-text">
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
  export default {
    name: 'HeroViewComponent',
    props: ['hero'],
    components: {
    },
  }
</script>

<style>
  .row {
    width: auto;
  }

  .hero-view {
    text-align: center;
  }

  .hero-title {
    font-size: 1.5rem;
  }

  .card-hero {
    width: 45rem;
  }
  
  .card-body-content {
    width: 30rem;
    display: flex;
    flex-flow: row wrap;
  }

  .hero-prop {
    flex-basis: 100%;
    display: flex;
    flex-flow: row nowrap;
  }
  
  .hero-prop-decription {
    position: relative;
    display: flex;
    flex-flow: column nowrap;
    align-items: center;
    flex: 1 1 100%;
    overflow: hidden;
  }
  
  .hero-prop-text {
    white-space: nowrap;
    font-size: 1.25rem;
  }

  .hero-prop-info {
    font-size: 1.4rem;
  }
</style>