import { createStore } from 'vuex';

export const useStore = createStore({
    state: {
        currentUser: null,
        global: {
            hostURL: 'https://localhost:7004'
        },
        currentBlog: {
            userId: 0,
            blogId: 0,
        }
    },
    getters: {
        'global/hostURL': state => state.global.hostURL,
        'currentBlog/userId': state => state.currentBlog.userId,
        'currentBlog/blogTitle': state => state.currentBlog.blogTitle,
    },
    mutations: {
        SET_CURRENT_USER(state, user) {
            state.currentUser = user;
        },
        SET_CURRENT_BLOG(state, blog) {
            state.currentBlog.userId = blog.userId;
            state.currentBlog.blogId = blog.blogId;
        }
    },
});
