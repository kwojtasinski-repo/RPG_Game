<template>
    <div>
        <div class="mb-2">
            <h3>Start Fight</h3>
        </div>
        <div v-if="loading">
            <LoadingIconComponent />
        </div>
        <div v-else>
            <div v-if="error" className="alert alert-danger">{{error}}</div>
            <div v-if="maps.length === 0">
                <div class="text-info">
                    There are no maps available, the battle cannot be started
                </div>
                <div class="action-button">
                    <RouterButtonComponent url="/" :buttonText="'Back to menu'" :buttonClass="'btn btn-primary'" />
                </div>
            </div>
            <div v-else-if="!player">
                <div class="text-info">
                    You have to choose hero. Click on this button and then go back to start fight
                </div>
                <div class="action-buttons">
                    <RouterButtonComponent :namedRoute="{ name: 'create-profile' }" :buttonText="'Choose hero'" :buttonClass="'btn btn-success'" />
                    <RouterButtonComponent url="/" :buttonText="'Back to menu'" :buttonClass="'btn btn-primary'" />
                </div>
            </div>
            <div v-else-if="player">
                <h4>Choose map and start battle</h4>
                <div class="player-info-position">
                    <PlayerViewComponent :player="player" />
                </div>
                <div class="mt-2 mb-2">
                    <button class="btn btn-primary me-2" @click="showHero">Show hero details</button>
                </div>
                <div class="d-flex justify-content-center">
                    <MapsViewComponent :maps="maps" @markedMap="markedMap"/>
                </div>
                <div class="action-buttons">
                    <button :class="markedMapId ? 'btn btn-success me-2' : 'btn btn-success me-2 disabled'" @click="startBattleHandler">Start battle</button>
                    <RouterButtonComponent url="/" :buttonText="'Back to menu'" :buttonClass="'btn btn-primary'" />
                </div>
            </div>
        </div>
    </div>
    <div>
        <PopupComponent :open="openPopup" @popupClosed="popupClosed">
            <div class="hero-show-content">
                <HeroViewComponent :hero="heroFetched" />
            </div>
            <div class="mt-2">
                <button class="btn btn-secondary" @click="popupClosed">Close</button>
            </div>
        </PopupComponent>
    </div>
</template>

<script>
import axios from '@/axios-setup.js';
import HeroViewComponent from '@/components/Heroes/HeroViewComponent.vue';
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';
import MapsViewComponent from '@/components/Maps/MapsViewComponent.vue';
import PlayerViewComponent from '@/components/Players/PlayerViewComponent.vue';
import PopupComponent from '@/components/Poupup/PopupComponent.vue';
import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';
import grpcClient from '@/grpc-client/grpc-client-setup';
import mapExceptionToMessage from '@/mappers/exceptionToMessageMapper';
import { mapGetters } from 'vuex';
import { PrepareBattleRequest } from '@/grpc-client/battle_pb.js';

  export default {
    name: 'StartFightPage',
    components: {
        LoadingIconComponent,
        PlayerViewComponent,
        PopupComponent,
        HeroViewComponent,
        RouterButtonComponent,
        MapsViewComponent
    },
    data() {
        return {
            player: null,
            maps: [],
            loading: true,
            error: '',
            heroFetched: null,
            openPopup: false,
            markedMapId: null
        }
    },
    methods: {
        async fetchPlayer() {
            try {
                const response = await axios.get(`/api/players/by-user?userId=${this.user.id}`);
                this.player = response.data;
            } catch (exception) {
                if (exception?.response?.status === 404) {
                    console.error(exception);
                } else {
                    this.error = mapExceptionToMessage(exception);
                    console.error(exception);
                }
            }
        },
        async fetchMaps() {
            try {
                const response = await axios.get('/api/maps');
                this.maps = response.data;
            } catch (exception) {
                this.error = mapExceptionToMessage(exception);
                console.error(exception);
            }
        },
        async showHero() {
            await this.fetchHero(this.player.hero.id);
            this.openPopup = true;
        },
        async fetchHero(heroId) {
            try {
                const response = await axios.get(`/api/heroes/${heroId}`);
                this.heroFetched = response.data;
            } catch(exception) {
                const message = mapExceptionToMessage(exception);
                console.error(message);
                console.log(exception);
            }
        },
        popupClosed() {
            this.openPopup = false;
            this.heroFetched = null;
        },
        markedMap(mapId) {
            this.markedMapId = mapId;
        },
        async startBattleHandler() {
            try {
                const battle = await grpcClient.prepareBattle(new PrepareBattleRequest([this.user.id, this.markedMapId]));
                this.$router.push({ name: 'battle-start', params: { battleId: battle.getId() } });
            } catch (exception) {
                console.error(exception);
            }
        }
    },
    async created() {
        await this.fetchPlayer();
        await this.fetchMaps();
        this.loading = false;
    },
    computed: {
        ...mapGetters(['user'])
    }
  }
</script>

<style>
    .text-info {
        font-size: 20px;
        margin-bottom: 1rem;
    }

    .action-button {
        display: flex;
        justify-content: center;
        margin-top: 1rem;
    }
    
    .player-info-position {
        display: flex;
        justify-content: center;
        margin-bottom: 1rem;
    }

    .postion-buttons {
        display: flex;
        justify-content: center;
    }
</style>