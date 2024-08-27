import { createStore } from 'vuex';

export const useStore = createStore({
    state: {
        currentUser: null,
        global: {
            hostURL: 'https://localhost:7004'
        }
    },
    getters: {
        'global/hostURL': state => state.global.hostURL
    },
    mutations: {
        SET_CURRENT_USER(state, user) {
            state.currentUser = user;
        },
    },
});
