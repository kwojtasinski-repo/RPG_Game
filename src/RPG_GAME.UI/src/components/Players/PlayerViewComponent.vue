<template>
    <div class="row">
        <div class="player-view">
            <div class="card card-player">
                <div class="card-header">
                    <div class="player-title mt-2 mb-2">
                        {{player.name}}
                    </div>
                </div>
                <div class="card-body text-center">
                    <div class="player-prop-info">
                        Properties
                    </div>
                    <div class="card-body-content">
                        <div class="player-prop mt-2 mb-2">
                            <div class="player-prop-decription">
                                <div class="player-prop-text">
                                    Level:
                                </div>
                            </div>
                            <div class="player-prop-decription">
                                <div class="player-prop-text">
                                    {{player.level}}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-body-content">
                        <div class="player-prop mt-2 mb-2">
                            <div class="player-prop-decription">
                                <div class="player-prop-text">
                                    Current Experience:
                                </div>
                            </div>
                            <div class="player-prop-decription">
                                <div class="player-prop-text">
                                    {{player.currentExp}}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-body-content">
                        <div class="player-prop mt-2 mb-2">
                            <div class="player-prop-decription">
                                <div class="player-prop-text">
                                    Required Experience:
                                </div>
                            </div>
                            <div class="player-prop-decription">
                                <div class="player-prop-text">
                                    {{player.requiredExp}}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-body-content">
                        <div class="player-prop mt-2 mb-2">
                            <div class="player-prop-decription">
                                <div class="player-prop-text">
                                    Hero Name:
                                </div>
                            </div>
                            <div class="player-prop-decription">
                                <div class="player-prop-text">
                                    {{player.hero.heroName}}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-body-content">
                        <div class="player-prop mt-2 mb-2">
                            <div class="player-prop-decription">
                                <div class="player-prop-text">
                                    Health:
                                </div>
                            </div>
                            <div class="player-prop-decription">
                                <div class="player-prop-text">
                                    {{player.hero.health}}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-body-content">
                        <div class="player-prop mt-2 mb-2">
                            <div class="player-prop-decription">
                                <div class="player-prop-text">
                                    Attack:
                                </div>
                            </div>
                            <div class="player-prop-decription">
                                <div class="player-prop-text">
                                    {{player.hero.attack}}
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
        name: 'PlayerViewComponent',
        props: {
            player: {
                type: Object
            }
        }
    }
</script>

<style>
    .row {
        width: auto;
    }
    
    .player-view {
        text-align: center;
    }
    
    .card-player {
        width: 45rem;
    }

    .player-title {
        font-size: 1.5rem;
    }

    .player-prop-info {
        font-size: 1.4rem;
    }

    .player-prop {
        flex-basis: 100%;
        display: flex;
        flex-flow: row nowrap;
    }
        
    .player-prop-decription {
        position: relative;
        display: flex;
        flex-flow: column nowrap;
        align-items: center;
        flex: 1 1 100%;
        overflow: hidden;
    }
  
    .player-prop-text {
        white-space: nowrap;
        font-size: 1.25rem;
    }

    .player-prop-info {
        font-size: 1.4rem;
    }
</style>