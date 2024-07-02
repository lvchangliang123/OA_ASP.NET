import { createStore } from 'vuex';

export const useStore = createStore({
    state: {
        currentUser: null,
    },
    mutations: {
        SET_CURRENT_USER(state, user) {
            state.currentUser = user;
        },
    },
});