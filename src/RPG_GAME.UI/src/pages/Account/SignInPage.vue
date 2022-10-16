<template>
    <div class="login-position">
        <h3>Login</h3>
        <form @submit.prevent="submit">
            <div>
                <InputComponent :label="'Email'" :type="'text'" :value="loginDto.email.value" 
                                v-model="loginDto.email.value" :showError="loginDto.email.showError" 
                                :error="loginDto.email.error"
                                @valueChanged="onChangeInput($event, 'email')"/>
            </div>
            <div>
                <InputComponent :label="'Password'" :type="'password'" :value="loginDto.password.value" 
                                v-model="loginDto.password.value" :showError="loginDto.password.showError" 
                                :error="loginDto.password.error"
                                @valueChanged="onChangeInput($event, 'password')"/>
            </div>
            <div v-if="error" className="alert alert-danger mt-2 mb-2">{{error}}</div>
            <div class="mt-2">
                <button class="btn btn-success me-2">Send</button>
                <button type="button" class="btn btn-secondary" @click="cancel">Cancel</button>
            </div>
        </form>
    </div>
</template>

<script>
import InputComponent from '@/components/Input/InputComponent.vue';
import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';
import authService from '@/services/AuthService';

  export default {
    name: 'SignInPage',
    components: {
        InputComponent
    },
    data() {
        return {
            error: '',
            loginDto: {
                email: {
                    value: '',
                    rules: [
                        v => v !== '' || 'Email name is required',
                        v => !(/^\s*$/.test(v)) || 'Email name is required' // white spaces
                    ]
                },
                password: {
                    value: '',
                    showError: false,
                    error: '',
                    rules: [
                        v => v !== '' || 'Password name is required',
                        v => !(/^\s*$/.test(v)) || 'Password is required' // white spaces
                    ]
                }
            }
        }
    },
    methods: {
        validate(value, fieldName) {
            const rules = this.loginDto[fieldName].rules;
            this.loginDto[fieldName].error = '';
            this.loginDto[fieldName].showError = false;
            
            for (const rule of rules) {
                const valid = rule(value);

                if (valid !== true) {
                    this.loginDto[fieldName].error = valid;
                    this.loginDto[fieldName].showError = true;
                    return valid;
                }
            }

            return '';
        },
        onChangeInput(value, fieldName) {
            this.validate(value, fieldName);
            this.loginDto[fieldName].value = value;
        },
        async submit() {
            this.error = '';
            const errors = [];
            for (const field in this.loginDto) {
                const error = this.validate(this.loginDto[field].value, field);
                if (error.length > 0) {
                    errors.push(error);
                }
            }

            if (errors.length > 0) {
                return;
            }

            const formToSend = {
               email: this.loginDto.email.value,
               password: this.loginDto.password.value
            };
            
            try {
               await authService.signIn(formToSend);
               this.$router.push('/');
            } catch(exception) {
               const message = exceptionMapper(exception);
               this.error = message;
               console.log(exception);
            }
        },
        cancel() {
            this.$router.push('/');
        }
    }
  }
</script>

<style>
    .login-position {
        margin: auto;
        width: 50%;
        text-align: center;
        justify-content: center;
    }
</style>