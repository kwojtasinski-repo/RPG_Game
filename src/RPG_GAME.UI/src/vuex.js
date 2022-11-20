import { createStore } from 'vuex'

const store = createStore({
    state() {
        return {
            user: null,
            player: null,
            battle: null,
        }
    },
    getters: {
        user: (state) => { return state.user; },
        player: (state) => { return state.player; },
        battle: (state) => { return state.battle; }
    },
    actions: {
        user(context, user) {
            context.commit('user', user);
        },
        player(context, player) {
            context.commit('player', player);
        },
        battle(context, battle) {
            context.commit('battle', battle);
        }
    },
    mutations: {
        user(state, user) {
            state.user = user;
        },
        player(state, player) {
            state.player = player;
        },
        battle(state, battle) {
            state.battle = battle;
        }
    }
});

export default store;