import axios from 'axios';

function getToken() {
    const auth = JSON.parse(window.localStorage.getItem('user-data'));
    return auth ? auth.accessToken : null;
}

const instance = axios.create({
    baseURL: window._env_?.VUE_APP_BACKEND_URL ? window._env_.VUE_APP_BACKEND_URL : process.env.VUE_APP_BACKEND_URL,
    timeout: 5000
});

instance.interceptors.request.use((req) => {
    const token = getToken();
    if (token) {
        req.headers.Authorization = `Bearer ${token}`;
    }

    return req;
});

export default instance;