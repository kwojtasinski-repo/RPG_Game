<template>
    <div class="hero-form-position">
        <form class="hero-form" @submit.prevent="submit">
            <div>
                <InputComponent :label="'Hero Name'" :type="'text'" :value="newHero.heroName.value" 
                        v-model="newHero.heroName.value" :showError="newHero.heroName.showError" 
                        :error="newHero.heroName.error" 
                        @valueChanged="onChangeInput($event, 'heroName')"/>
            </div>
            <div>
                <InputComponent :label="'Health'" :type="'number'" :value="newHero.health.value" 
                        v-model="newHero.health.value" :showError="newHero.health.showError" 
                        :error="newHero.health.error" 
                        @valueChanged="onChangeInput($event, 'health')" :step="0.01"/>
            </div>
            <div>
                <InputComponent :label="'Health strategy increasing'" :type="'select'" :value="newHero.healthIncreasingState.value" 
                        v-model="newHero.healthIncreasingState.value" :showError="newHero.healthIncreasingState.showError" 
                        :error="newHero.healthIncreasingState.error" 
                        :options="strategiesIncreasing" 
                        @valueChanged="onChangeInput($event, 'healthIncreasingState')"/>
            </div>
            <div>
                <InputComponent :label="'Health strategy increasing value'" :type="'number'" :value="newHero.healthIncreasingStateValue.value" 
                        v-model="newHero.healthIncreasingStateValue.value" :showError="newHero.healthIncreasingStateValue.showError" 
                        :error="newHero.healthIncreasingStateValue.error" 
                        @valueChanged="onChangeInput($event, 'healthIncreasingStateValue')" :step="0.01"/>
            </div>
            <div>
                <InputComponent :label="'Attack'" :type="'number'" :value="newHero.attack.value" 
                        v-model="newHero.attack.value" :showError="newHero.attack.showError" 
                        :error="newHero.attack.error" 
                        @valueChanged="onChangeInput($event, 'attack')" :step="0.01"/>
            </div>
            <div>
                <InputComponent :label="'Attack strategy increasing'" :type="'select'" :value="newHero.attackIncreasingState.value" 
                        v-model="newHero.attackIncreasingState.value" :showError="newHero.attackIncreasingState.showError" 
                        :error="newHero.attackIncreasingState.error" 
                        :options="strategiesIncreasing" 
                        @valueChanged="onChangeInput($event, 'attackIncreasingState')"/>
            </div>
            <div>
                <InputComponent :label="'Attack strategy increasing value'" :type="'number'" :value="newHero.attackIncreasingStateValue.value" 
                        v-model="newHero.attackIncreasingStateValue.value" :showError="newHero.attackIncreasingStateValue.showError" 
                        :error="newHero.attackIncreasingStateValue.error" 
                        @valueChanged="onChangeInput($event, 'attackIncreasingStateValue')" :step="0.01"/>
            </div>
            <div>
                <InputComponent :label="'Base Required Experience'" :type="'number'" :value="newHero.baseRequiredExperience.value" 
                        v-model="newHero.baseRequiredExperience.value" :showError="newHero.baseRequiredExperience.showError" 
                        :error="newHero.baseRequiredExperience.error" 
                        @valueChanged="onChangeInput($event, 'baseRequiredExperience')" :step="0.01"/>
            </div>
            <div>
                <InputComponent :label="'Base Required Experience strategy increasing'" :type="'select'" :value="newHero.baseRequiredExperienceIncreasingState.value" 
                        v-model="newHero.baseRequiredExperienceIncreasingState.value" :showError="newHero.baseRequiredExperienceIncreasingState.showError" 
                        :error="newHero.baseRequiredExperienceIncreasingState.error" 
                        :options="strategiesIncreasing" 
                        @valueChanged="onChangeInput($event, 'baseRequiredExperienceIncreasingState')"/>
            </div>
            <div>
                <InputComponent :label="'Base Required Experience strategy increasing value'" :type="'number'" :value="newHero.baseRequiredExperienceIncreasingStateValue.value" 
                        v-model="newHero.baseRequiredExperienceIncreasingStateValue.value" :showError="newHero.baseRequiredExperienceIncreasingStateValue.showError" 
                        :error="newHero.baseRequiredExperienceIncreasingStateValue.error" 
                        @valueChanged="onChangeInput($event, 'baseRequiredExperienceIncreasingStateValue')" :step="0.01"/>
            </div>
            <div class="mt-2">
                <button type="button" class="btn btn-outline-secondary me-2" @click="reset">
                    Reset
                </button>
                <button type="button" class="btn btn-secondary me-2">
                    Cancel
                </button>
                <button class="btn btn-success">
                    Send
                </button>
            </div>
        </form>
    </div>
</template>

<script>
    import InputComponent from '../Input/InputComponent.vue';
    
    export default {
        name: 'HeroFormComponent',
        props: ['hero', 'strategiesIncreasing'],
        components: {
            InputComponent
        },
        data() {
            return {
                newHero: this.initHero()
            }
        },
        methods: {
            initHero() {
                return {
                    id: {
                        value: this.hero?.id ?? null,
                        rules: []
                    },
                    heroName: {
                        value: this.hero?.heroName ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Hero name is required',
                            v => v.length > 0 || 'Hero name is required',
                            v => v.length < 100 || 'Hero name cannot be higher than 100 characters',
                            v => v.length > 2 || 'Hero name should have at least 3 characters',
                            v => !/^\s+$/.test(v) || 'Hero name cannot contain white spaces'
                        ]
                    },
                    health: {
                        value: this.hero?.health?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Health is required',
                            v => v.toString().length > 0 || 'Health is required',
                            v => v >= 0 || 'Health cannot be negative'
                        ]
                    },
                    attack: {
                        value: this.hero?.attack?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Attack is required',
                            v => v.toString().length > 0 || 'Attack is required',
                            v => v >= 0 || 'Attack cannot be negative'
                        ]
                    },
                    baseRequiredExperience: {
                        value: this.hero?.baseRequiredExperience?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Base required experience is required',
                            v => v.toString().length > 0 || 'Base required experience is required',
                            v => v >= 0 || 'Base required experience cannot be negative'
                        ]
                    },
                    healthIncreasingState: {
                        value: this.hero?.health?.increasingState?.strategyIncreasing ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Health increasing state is required',
                            v => v.length > 0 || 'Health increasing state is required',
                        ]
                    },
                    healthIncreasingStateValue: {
                        value: this.hero?.health?.increasingState?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                        v => v !== null || 'Health increasing state value is required',
                            v => v.toString().length > 0 || 'Health increasing state value is required',
                            v => v >= 0 || 'Health increasing state value cannot be negative'
                        ]
                    },
                    attackIncreasingState: {
                        value: this.hero?.attack?.increasingState?.strategyIncreasing ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Attack increasing state is required',
                            v => v.length > 0 || 'Attack increasing state is required',
                        ]
                    },
                    attackIncreasingStateValue: {
                        value: this.hero?.attack?.increasingState?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                        v => v !== null || 'Attack increasing state value is required',
                            v => v.toString().length > 0 || 'Attack increasing state value is required',
                            v => v >= 0 || 'Attack increasing state value cannot be negative'
                        ]
                    },
                    baseRequiredExperienceIncreasingState: {
                        value: this.hero?.baseRequiredExperience?.increasingState?.strategyIncreasing  ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Base required experience increasing state is required',
                            v => v.length > 0 || 'Base required experience increasing state is required',
                        ]
                    },
                    baseRequiredExperienceIncreasingStateValue: {
                        value: this.hero?.baseRequiredExperience?.increasingState?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                        v => v !== null || 'Base required experience increasing state value is required',
                            v => v.toString().length > 0 || 'Base required experience increasing state value is required',
                            v => v >= 0 || 'Base required experience increasing state value cannot be negative'
                        ]
                    },
                }
            },
            submit() {
                const errors = [];
                for (const field in this.newHero) {
                    const error = this.validate(this.newHero[field].value, field);
                    if (error.length > 0) {
                        errors.push(error);
                    }
                }

                if (errors.length > 0) {
                    return;
                }

                const formToSend = {
                    id: this.newHero.id.value,
                    heroName: this.newHero.heroName.value,
                    health: {
                        value: this.newHero.health.value,
                        increasingState: {
                            strategyIncreasing: this.newHero.healthIncreasingState.value,
                            value: this.newHero.healthIncreasingStateValue.value,
                        }
                    },
                    attack: {
                        value: this.newHero.health.value,
                        increasingState: {
                            strategyIncreasing: this.newHero.attackIncreasingState.value,
                            value: this.newHero.attackIncreasingStateValue.value,
                        }
                    },
                    baseRequiredExperience: {
                        value: this.newHero.health.value,
                        increasingState: {
                            strategyIncreasing: this.newHero.baseRequiredExperienceIncreasingState.value,
                            value: this.newHero.baseRequiredExperienceIncreasingStateValue.value,
                        }
                    }
                };
                
                if (formToSend.id === null){
                    delete formToSend.id;
                }
                this.$emit('submitForm', formToSend);
            },
            onChangeInput(value, fieldName) {
                this.validate(value, fieldName);
                this.newHero[fieldName].value = value;
            },
            reset() {
                this.newHero = this.initProduct();
            },
            validate(value, fieldName) {
                const rules = this.newHero[fieldName].rules;
                this.newHero[fieldName].error = '';
                this.newHero[fieldName].showError = false;
                
                for (const rule of rules) {
                    const valid = rule(value);

                    if (valid !== true) {
                        this.newHero[fieldName].error = valid;
                        this.newHero[fieldName].showError = true;
                        return valid;
                    }
                }

                return '';
            }
        }
    }
</script>

<style>
    .hero-form-position {
        display: flex;
        justify-content: center;
        text-align: center;
        align-items: center;
    }

    .hero-form {
        display: flex;
        flex-direction: column;
        width: 50%;
        padding-left: 5px;
        padding-right: 5px;
    }
</style>