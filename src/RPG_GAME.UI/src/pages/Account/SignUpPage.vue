<template>
    <div class="register-position">
        <h3>Register</h3>
        <form @submit.prevent="submit">
            <div>
                <InputComponent :label="'Email'" :type="'text'" :value="registerDto.email.value" 
                            v-model="registerDto.email.value" :showError="registerDto.email.showError" 
                            :error="registerDto.email.error"
                            @valueChanged="onChangeInput($event, 'email')"/>
            </div>
            <div>
                <InputComponent :label="'Password'" :type="'password'" :value="registerDto.password.value" 
                            v-model="registerDto.password.value" :showError="registerDto.password.showError" 
                            :error="registerDto.password.error"
                            @valueChanged="onChangeInput($event, 'password')"/>
            </div>
            <div>
                <InputComponent :label="'Confirm Password'" :type="'password'" :value="registerDto.passwordConfirm.value" 
                            v-model="registerDto.passwordConfirm.value" :showError="registerDto.passwordConfirm.showError" 
                            :error="registerDto.passwordConfirm.error"
                            @valueChanged="onChangeInput($event, 'passwordConfirm')"/>
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
import axios from '@/axios-setup.js';
import exceptionMapper from '@/mappers/exceptionToMessageMapper.js';
import { notify } from "@kyvg/vue3-notification";

  export default {
    name: 'SignUpPage',
    components: {
        InputComponent
    },
    data() {
        return {
            error: '',
            registerDto: {
                email: {
                    value: '',
                    rules: [
                        v => v !== '' || 'Email name is required',
                        v => !(/^\s*$/.test(v)) || 'Email name is required', // white spaces
                        v => /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(v) //eslint-disable-line
                            || 'Email is invalid' // email regex
                    ]
                },
                password: {
                    value: '',
                    showError: false,
                    error: '',
                    rules: [
                        v => v !== '' || 'Password name is required',
                        v => !(/^\s*$/.test(v)) || 'Password is required', // white spaces
                        v => /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d\w\W]{8,}$/.test(v) || 'Password should contain at least 8 characters, including one upper letter and one number'
                        //^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d\\w\\W]{8,}$
                    ]
                },
                passwordConfirm: {
                    value: '',
                    showError: false,
                    error: '',
                    rules: [
                        v => v !== '' || 'Password name is required',
                        v => !(/^\s*$/.test(v)) || 'Password is required', // white spaces
                        v => /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d\w\W]{8,}$/.test(v) || 'Password should contain at least 8 characters, including one upper letter and one number'
                    ]
                }
            }
        }
    },
    methods: {
        validate(value, fieldName) {
            const rules = this.registerDto[fieldName].rules;
            this.registerDto[fieldName].error = '';
            this.registerDto[fieldName].showError = false;
            
            for (const rule of rules) {
                const valid = rule(value);

                if (valid !== true) {
                    this.registerDto[fieldName].error = valid;
                    this.registerDto[fieldName].showError = true;
                    return valid;
                }
            }

            return '';
        },
        onChangeInput(value, fieldName) {
            this.validate(value, fieldName);
            this.registerDto[fieldName].value = value;
        },
        validateModel() {
            const errors = [];
            for (const field in this.registerDto) {
                const error = this.validate(this.registerDto[field].value, field);
                if (error.length > 0) {
                    errors.push(error);
                }
            }

            if (this.registerDto.password.value !== this.registerDto.passwordConfirm.value) {
                this.registerDto.password.error = 'Passwords are not same'
                errors.push(this.registerDto.password.error);
                this.registerDto.password.showError = true;
                this.registerDto.passwordConfirm.error = 'Passwords are not same'
                errors.push(this.registerDto.passwordConfirm.error);
                this.registerDto.passwordConfirm.showError = true;
            }

            return errors;
        },
        async submit() {
            this.error = '';
            const errors = this.validateModel();

            if (errors.length > 0) {
                return;
            }

            const formToSend = {
               email: this.registerDto.email.value,
               password: this.registerDto.password.value,
               role: 'user'
            };
            
            try {
               await axios.post('/api/account/sign-up', formToSend);
               notify({
                    type: "success",
                    title: 'Register',
                    text: 'Successfully register'
                });
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
    .register-position {
        margin: auto;
        width: 50%;
        text-align: center;
        justify-content: center;
    }
</style>