import axios from 'axios';

const instance = axios.create({
    baseURL: window._env_?.VUE_APP_BACKEND_URL ? window._env_.VUE_APP_BACKEND_URL : process.env.VUE_APP_BACKEND_URL
});

export default instance;