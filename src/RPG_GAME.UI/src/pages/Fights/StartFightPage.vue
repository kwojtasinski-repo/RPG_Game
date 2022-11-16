<template>
    <div class="page">
        StartFight
        <div v-if="loading">
            <LoadingIconComponent />
        </div>
        <div v-else-if="!loading && maps.length === 0">
            There are no maps available, the battle cannot be started
        </div>
        <div v-else-if="!loading && !player">
            You have to choose player. Click on this button and then go back to start fight
            <RouterButtonComponent url="/" /> <!-- todo add specific page -->
        </div>
        <div v-else-if="!loading && player">
            <PlayerViewComponent :player="player" />
            <button class="btn btn-primary" @click="showHero">Show hero details</button>
        </div>
        <div v-else>
            {{error}}
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
import PlayerViewComponent from '@/components/Players/PlayerViewComponent.vue';
import PopupComponent from '@/components/Poupup/PopupComponent.vue';
import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';
import mapExceptionToMessage from '@/mappers/exceptionToMessageMapper';
import { mapGetters } from 'vuex';

  export default {
    name: 'StartFightPage',
    components: {
        LoadingIconComponent,
        PlayerViewComponent,
        PopupComponent,
        HeroViewComponent,
        RouterButtonComponent
    },
    data() {
        return {
            player: null,
            maps: [],
            loading: true,
            error: '',
            heroFetched: null,
            openPopup: false,
        }
    },
    methods: {
        async fetchPlayer() {
            try {
                this.player = await axios.get(`/api/players/by-user?userId=${this.user.id}`);
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
                this.maps = await axios.get('/api/maps');
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
                const message = exceptionMapper(exception);
                console.error(message);
                console.log(exception);
            }
        },
        popupClosed() {
            this.openPopup = false;
            this.heroFetched = null;
        },
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
</style>