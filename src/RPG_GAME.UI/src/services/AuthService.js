import jwt_decode from 'jwt-decode';
import axios from '@/axios-setup';

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
        const tokenDecoded = jwt_decode(response.data.accessToken);        
        window.localStorage.setItem('user-data', JSON.stringify(response.data));
        return tokenDecoded;
    }
}

const authService = new AuthService(axios);

export default authService;