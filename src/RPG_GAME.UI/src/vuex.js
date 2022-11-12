import { createStore } from 'vuex'

const store = createStore({
    state() {
        return {
            user: null,
            player: null,
        }
    },
    getters: {
        user: (state) => { return state.user; },
        player: (state) => { return state.player; }
    },
    actions: {
        user(context, user) {
            context.commit('user', user);
        },
        player(context, player) {
            context.commit('player', player);
        }
    },
    mutations: {
        user(state, user) {
            state.user = user;
        },
        player(state, player) {
            state.player = player;
        }
    }
});

export default store;