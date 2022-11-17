<template>
    <div v-if="loading">
        <LoadingIconComponent />
    </div>
    <div v-else>
        Profile
        <div v-if="!player">
            <RouterButtonComponent :namedRoute="{ name: 'create-profile' }" :buttonText="'Create Hero'" :buttonClass="'btn btn-success'" />
        </div>
        <div v-else>
            <PlayerViewComponent :player="player" />
            <button class="btn btn-primary" @click="showHero">Show hero details</button>
        </div>
        <RouterButtonComponent :url="'/'" :buttonText="'Back to menu'" />
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
import PlayerViewComponent from '@/components/Players/PlayerViewComponent.vue';
import PopupComponent from '@/components/Poupup/PopupComponent.vue';
import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue'
import mapExceptionToMessage from '@/mappers/exceptionToMessageMapper.js';
import axios from '@/axios-setup.js';
import { mapGetters } from 'vuex';
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';

  export default {
    name: 'ProfilePage',
    components: {
        RouterButtonComponent,
        PlayerViewComponent,
        PopupComponent,
        LoadingIconComponent
    },
    data() {
        return {
            player: null,
            loading: true,
            error: '',
            heroFetched: null,
            openPopup: false,
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
    },
    async created() {
        await this.fetchPlayer();
        console.log(this.player);
        this.loading = false;
    },
    computed: {
        ...mapGetters(['user'])
    }
  }
</script>

<style>
    
</style>