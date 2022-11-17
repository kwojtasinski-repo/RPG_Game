<template>
    <div v-if="loading">
        <LoadingIconComponent />
    </div>
    <div v-else>
        <div v-if="currentHero" class="select-position-hero mt-2 mb-2">
            <form @submit.prevent="submit">
                <transition name="slide-fade" mode="out-in">
                    <div class="hero-component-center" :key="currentHero.id">
                        <HeroViewComponent :hero="currentHero" />
                    </div>
                </transition>
                <div class="player-name">
                    <InputComponent :label="'Player Name'" :type="'text'" :value="newPlayer.name.value" 
                            v-model="newPlayer.name.value" :showError="newPlayer.name.showError" 
                            :error="newPlayer.name.error"
                            @valueChanged="onChangeInput($event, 'name')"/>
                </div>
                <div class="selection-hero-buttons">
                    <div :class="heroes.length > 1 ? 'left-arrow' : 'left-arrow disabled'">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="icon-arrow" viewBox="0 0 16 16" @click="selectHeroOnLeft">
                            <path d="M16 14a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V2a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v12zm-4.5-6.5H5.707l2.147-2.146a.5.5 0 1 0-.708-.708l-3 3a.5.5 0 0 0 0 .708l3 3a.5.5 0 0 0 .708-.708L5.707 8.5H11.5a.5.5 0 0 0 0-1z"/>
                        </svg>
                    </div>
                    <div class="selection-button">
                        <button class="btn btn-success button-height">
                            Create
                        </button>
                    </div>
                    <div :class="heroes.length > 1 ? 'right-arrow' : 'right-arrow disabled'">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="icon-arrow" viewBox="0 0 16 16" @click="selectHeroOnRight">
                            <path d="M0 14a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2a2 2 0 0 0-2 2v12zm4.5-6.5h5.793L8.146 5.354a.5.5 0 1 1 .708-.708l3 3a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708-.708L10.293 8.5H4.5a.5.5 0 0 1 0-1z"/>
                        </svg>
                    </div>
                </div>
            </form>
        </div>
        <div v-if="error" className="alert alert-danger">{{error}}</div>
        <div>
            <RouterButtonComponent :url="'/'" :buttonText="'Back to menu'" />
        </div>
    </div>
</template>

<script>
import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue'
import mapExceptionToMessage from '@/mappers/exceptionToMessageMapper.js';
import axios from '@/axios-setup.js';
import { mapGetters } from 'vuex';
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';
import HeroViewComponent from '@/components/Heroes/HeroViewComponent.vue';
import InputComponent from '@/components/Input/InputComponent.vue';

  export default {
    name: 'ProfilePage',
    components: {
        RouterButtonComponent,
        LoadingIconComponent,
        HeroViewComponent,
        InputComponent
    },
    data() {
        return {
            newPlayer: {
                name: {
                    value: null,
                    showError: false,
                    error: '',
                    rules: [
                        v => v !== null || 'Player name is required',
                        v => v.length > 0 || 'Player name is required',
                        v => v.length < 100 || 'Player name cannot be higher than 100 characters',
                        v => v.length > 2 || 'Player name should have at least 3 characters',
                        v => !/^\s+$/.test(v) || 'Player name cannot contain white spaces'
                    ]
                }
            },
            currentHero: null,
            heroes: null,
            loading: true,
            error: '',
        }
    },
    methods: {
        async fetchHeroes() {
            try {
                const response = await axios.get(`/api/heroes`);
                this.heroes = response.data;
            } catch (exception) {
                this.error = mapExceptionToMessage(exception);
                console.error(exception);
            }
        },
        selectHeroOnLeft() {
            const lastIndex = this.heroes.length - 1;
            const currentIndex = this.heroes.indexOf(this.currentHero);
            const nextIndex = currentIndex - 1;
            
            if (nextIndex < 0) {
                this.currentHero = this.heroes[lastIndex];
            } else {
                this.currentHero = this.heroes[nextIndex];
            }
        },
        selectHeroOnRight() {
            const lastIndex = this.heroes.length - 1;
            const currentIndex = this.heroes.indexOf(this.currentHero);
            const nextIndex = currentIndex + 1;
            
            if (nextIndex > lastIndex) {
                this.currentHero = this.heroes[0];
            } else {
                this.currentHero = this.heroes[nextIndex];
            }
        },
        onChangeInput(value, fieldName) {
            this.validate(value, fieldName);
            this.newPlayer[fieldName].value = value;
        },
        validate(value, fieldName) {
            const rules = this.newPlayer[fieldName].rules;
            this.newPlayer[fieldName].error = '';
            this.newPlayer[fieldName].showError = false;
            
            for (const rule of rules) {
                const valid = rule(value);

                if (valid !== true) {
                    this.newPlayer[fieldName].error = valid;
                    this.newPlayer[fieldName].showError = true;
                    return valid;
                }
            }

            return '';
        },
        async submit() {
            this.error = '';
            const errors = [];
            for (const field in this.newPlayer) {
                const error = this.validate(this.newPlayer[field].value, field);
                if (error.length > 0) {
                    errors.push(error);
                }
            }

            if (errors.length > 0) {
                return;
            }

            const formToSend = {
                name: this.newPlayer.name.value,
                heroId: this.currentHero.id,
                userId: this.user.id
            };
            
            try {
                await axios.post('/api/players', formToSend);
                this.$router.push('/profile');
            } catch (exception) {
                console.error(exception);
                this.error = mapExceptionToMessage(exception);
            }
        }
    },
    async created() {
        await this.fetchHeroes();
        if (this.heroes.length > 0) {
            this.currentHero = this.heroes[0];
        }
        this.loading = false;
    },
    computed: {
        ...mapGetters(['user'])
    }
  }
</script>

<style>
    .slide-fade-enter-active {
        transition: all .3s ease;
    }

    .slide-fade-leave-active {
        transition: all .8s cubic-bezier(1.0, 0.5, 0.8, 1.0);
    }

    .slide-fade-enter, .slide-fade-leave-to {
        opacity: 0;
    }

    .select-position-hero {
        display: flex;
        justify-content: center;
    }

    .selection-hero-buttons {
        display: flex;
        justify-content: center;
    }

    .left-arrow {
        font-size: 3rem;
        margin-right: 1rem;
        cursor: pointer;
    }

    .selection-button {
        margin-top: 1rem;
        margin-right: 1rem;
    }

    .button-height {
        height: 3rem;
    }

    .right-arrow {
        font-size: 3rem;
        cursor: pointer;
    }

    .icon-arrow {
        width: 1em;
        height: 1em;
    }

    .icon-arrow:hover {
        -moz-box-shadow: inset 0 0 100px 100px rgba(0, 0, 0, 0.1);
        -webkit-box-shadow: inset 0 0 100px 100px rgba(0, 0, 0, 0.1);
        box-shadow: inset 0 0 100px 100px rgba(0, 0, 0, 0.1);
    }

    .icon-arrow:active {
        -moz-box-shadow: inset 0 0 100px 100px rgba(0, 0, 0, 0.1);
        -webkit-box-shadow: inset 0 0 100px 100px rgba(0, 0, 0, 0.1);
        box-shadow: inset 0 0 100px 100px rgba(0, 0, 0, 0.1);
    }

    .disabled {
        pointer-events: none;
        opacity: 0.4;
    }
    
    .hero-component-center {
        display: flex;
        justify-content: center;
        margin-bottom: 1rem;
    }
</style>