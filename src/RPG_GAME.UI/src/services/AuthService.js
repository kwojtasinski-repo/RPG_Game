import jwt_decode from 'jwt-decode';
import axios from '@/axios-setup.js';
import store from '@/vuex.js';
import { notify } from "@kyvg/vue3-notification";

class AuthService {
    constructor(axios) {
        this._axios = axios;
    }

    async signIn(dto) {
        if (!dto) {
            throw new Error('The dto is an object and cannot be null or undefined');
        }

        if (!dto.email) {
            throw new Error(`The dto has required field 'email'`)
        }

        if (!dto.password) {
            throw new Error(`The dto has required field 'password'`)
        }

        const response = await this._axios.post('/api/account/sign-in', {
            email: dto.email,
            password: dto.password
        });
        storeUserToken(response.data);
        notify({
             type: "success",
             title: 'Login',
             text: 'Successfully login'
        });
    }

    getUser() {
        let user = store.getters.user;

        if (user) {
            return user;
        }

        const tokenData = JSON.parse(window.localStorage.getItem('user-data'));
        
        if (!tokenData) {
            return null;
        }

        const tokenDecoded = jwt_decode(tokenData.accessToken);
        user = mapToUser(tokenDecoded);
        store.dispatch('user', user);
        return user;
    }

    async isLogged() {
        const user = this.getUser();
        
        if (!user) {
            return false;
        }
        
        const MS_PER_MINUTE = 60000;
        const tokenExpiryDate = new Date(user.tokenExpiry - MS_PER_MINUTE * 5);
        const currentDate = new Date();
        
        if (tokenExpiryDate < currentDate) {
            const token = await this.refreshToken();

            if (!token) {
                return false;
            }

            storeUserToken(token);
        }

        return true;
    }

    async refreshToken() {
        const tokenData = JSON.parse(window.localStorage.getItem('user-data'));
    
        if (!tokenData) {
            return null;
        }
    
        try {
            const response = await this._axios.post('/api/refresh-tokens/use', {
                token: tokenData.refreshToken
            });
            return response.data;
        } catch(exception) {
            console.log(exception);
    
            if (exception?.response?.status) {
                if (exception.response.status === 400) {
                    this.logout();
                }
            }
        }
    
        return null;
    }

    logout() {
        notify({
             type: "info",
             title: 'Logout',
             text: `You've been logged out`
         });
        store.dispatch('user', null);
        window.localStorage.removeItem('user-data');
    }
}

function mapToUser(decodedToken) {
    return ({
        id: decodedToken.sub,
        email: decodedToken.email,
        role: decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
        tokenExpiry: decodedToken.exp * 1000
    });
}

function storeUserToken(tokenData) {
    const tokenDecoded = jwt_decode(tokenData.accessToken);
    window.localStorage.setItem('user-data', JSON.stringify(tokenData));
    const user = mapToUser(tokenDecoded);
    store.dispatch('user', user);
}

const authService = new AuthService(axios);

export default authService;
