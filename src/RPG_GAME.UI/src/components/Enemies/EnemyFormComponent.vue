<template>
    <div class="enemy-form-page-position">
        <div>
            <button class="btn btn-success" v-if="!manageSkills" @click="() => manageSkills = true">Manage enemy skills</button>
        </div>
        <div class="enemy-form-position">
            <form class="enemy-form" @submit.prevent="submit">
                <div>
                    <InputComponent :label="'Enemy Name'" :type="'text'" :value="newEnemy.enemyName.value" 
                            v-model="newEnemy.enemyName.value" :showError="newEnemy.enemyName.showError" 
                            :error="newEnemy.enemyName.error" 
                            :readonly="manageSkills"
                            @valueChanged="onChangeInput($event, 'enemyName')"/>
                </div>
                <div>
                    <InputComponent :label="'Health'" :type="'number'" :value="newEnemy.baseHealth.value" 
                            v-model="newEnemy.baseHealth.value" :showError="newEnemy.baseHealth.showError" 
                            :error="newEnemy.baseHealth.error" 
                            :readonly="manageSkills"
                            @valueChanged="onChangeInput($event, 'baseHealth')" :step="0.01"/>
                </div>
                <div>
                    <InputComponent :label="'Health strategy increasing'" :type="'select'" :value="newEnemy.baseHealthIncreasingState.value" 
                            v-model="newEnemy.baseHealthIncreasingState.value" :showError="newEnemy.baseHealthIncreasingState.showError" 
                            :error="newEnemy.baseHealthIncreasingState.error" 
                            :options="strategiesIncreasing" 
                            :readonly="manageSkills"
                            @valueChanged="onChangeInput($event, 'baseHealthIncreasingState')"/>
                </div>
                <div>
                    <InputComponent :label="'Health strategy increasing value'" :type="'number'" :value="newEnemy.baseHealthIncreasingStateValue.value" 
                            v-model="newEnemy.baseHealthIncreasingStateValue.value" :showError="newEnemy.baseHealthIncreasingStateValue.showError" 
                            :error="newEnemy.baseHealthIncreasingStateValue.error" 
                            :readonly="manageSkills"
                            @valueChanged="onChangeInput($event, 'baseHealthIncreasingStateValue')" :step="0.01"/>
                </div>
                <div>
                    <InputComponent :label="'Heal'" :type="'number'" :value="newEnemy.baseHealLvl.value" 
                            v-model="newEnemy.baseHealLvl.value" :showError="newEnemy.baseHealLvl.showError" 
                            :error="newEnemy.baseHealLvl.error" 
                            :readonly="manageSkills"
                            @valueChanged="onChangeInput($event, 'baseHealLvl')" :step="0.01"/>
                </div>
                <div>
                    <InputComponent :label="'Heal strategy increasing'" :type="'select'" :value="newEnemy.baseHealLvlIncreasingState.value" 
                            v-model="newEnemy.baseHealLvlIncreasingState.value" :showError="newEnemy.baseHealLvlIncreasingState.showError" 
                            :error="newEnemy.baseHealLvlIncreasingState.error" 
                            :readonly="manageSkills"
                            :options="strategiesIncreasing" 
                            @valueChanged="onChangeInput($event, 'baseHealLvlIncreasingState')"/>
                </div>
                <div>
                    <InputComponent :label="'Heal strategy increasing value'" :type="'number'" :value="newEnemy.baseHealLvlIncreasingStateValue.value" 
                            v-model="newEnemy.baseHealLvlIncreasingStateValue.value" :showError="newEnemy.baseHealLvlIncreasingStateValue.showError" 
                            :error="newEnemy.baseHealLvlIncreasingStateValue.error" 
                            :readonly="manageSkills"
                            @valueChanged="onChangeInput($event, 'baseHealLvlIncreasingStateValue')" :step="0.01"/>
                </div>
                <div>
                    <InputComponent :label="'Attack'" :type="'number'" :value="newEnemy.baseAttack.value" 
                            v-model="newEnemy.baseAttack.value" :showError="newEnemy.baseAttack.showError" 
                            :error="newEnemy.baseAttack.error" 
                            :readonly="manageSkills"
                            @valueChanged="onChangeInput($event, 'baseAttack')" :step="0.01"/>
                </div>
                <div>
                    <InputComponent :label="'Attack strategy increasing'" :type="'select'" :value="newEnemy.baseAttackIncreasingState.value" 
                            v-model="newEnemy.baseAttackIncreasingState.value" :showError="newEnemy.baseAttackIncreasingState.showError" 
                            :error="newEnemy.baseAttackIncreasingState.error" 
                            :readonly="manageSkills"
                            :options="strategiesIncreasing" 
                            @valueChanged="onChangeInput($event, 'baseAttackIncreasingState')"/>
                </div>
                <div>
                    <InputComponent :label="'Attack strategy increasing value'" :type="'number'" :value="newEnemy.baseAttackIncreasingStateValue.value" 
                            v-model="newEnemy.baseAttackIncreasingStateValue.value" :showError="newEnemy.baseAttackIncreasingStateValue.showError" 
                            :error="newEnemy.baseAttackIncreasingStateValue.error" 
                            :readonly="manageSkills"
                            @valueChanged="onChangeInput($event, 'baseAttackIncreasingStateValue')" :step="0.01"/>
                </div>
                <div>
                    <InputComponent :label="'Base Required Experience'" :type="'number'" :value="newEnemy.experience.value" 
                            v-model="newEnemy.experience.value" :showError="newEnemy.experience.showError" 
                            :error="newEnemy.experience.error" 
                            :readonly="manageSkills"
                            @valueChanged="onChangeInput($event, 'experience')" :step="0.01"/>
                </div>
                <div>
                    <InputComponent :label="'Base Required Experience strategy increasing'" :type="'select'" :value="newEnemy.experienceIncreasingState.value" 
                            v-model="newEnemy.experienceIncreasingState.value" :showError="newEnemy.experienceIncreasingState.showError" 
                            :error="newEnemy.experienceIncreasingState.error" 
                            :options="strategiesIncreasing" 
                            :readonly="manageSkills"
                            @valueChanged="onChangeInput($event, 'experienceIncreasingState')"/>
                </div>
                <div>
                    <InputComponent :label="'Base Required Experience strategy increasing value'" :type="'number'" :value="newEnemy.experienceIncreasingStateValue.value" 
                            v-model="newEnemy.experienceIncreasingStateValue.value" :showError="newEnemy.experienceIncreasingStateValue.showError" 
                            :error="newEnemy.experienceIncreasingStateValue.error"
                            :readonly="manageSkills"
                            @valueChanged="onChangeInput($event, 'experienceIncreasingStateValue')" :step="0.01"/>
                </div>
                <div>
                    <InputComponent :label="'Difficulty'" :type="'select'" :value="newEnemy.difficulty.value" 
                            v-model="newEnemy.difficulty.value" :showError="newEnemy.difficulty.showError" 
                            :error="newEnemy.difficulty.error"
                            :readonly="manageSkills"
                            :options="difficulties"
                            @valueChanged="onChangeInput($event, 'difficulty')" />
                </div>
                <div>
                    <InputComponent :label="'Category'" :type="'select'" :value="newEnemy.category.value" 
                            v-model="newEnemy.category.value" :showError="newEnemy.category.showError" 
                            :error="newEnemy.category.error"
                            :readonly="manageSkills"
                            :options="categories"
                            @valueChanged="onChangeInput($event, 'category')" />
                </div>
                <div class="mt-2">
                    <button type="button" :class="manageSkills ? 'btn btn-outline-secondary me-2 disabled' : 'btn btn-outline-secondary me-2'" @click="reset">
                        Reset
                    </button>
                    <button type="button" :class="manageSkills ? 'btn btn-secondary me-2 disabled' : 'btn btn-secondary me-2'" @click="cancel">
                        Cancel
                    </button>
                    <button :class="manageSkills ? 'btn btn-success disabled' : 'btn btn-success'">
                        Send
                    </button>
                </div>
            </form>
        </div>
        <div v-if="manageSkills" class="mt-2 mb-2">
            <EnemySkillsFormComponent :skills="newEnemy.skills.value" :strategiesIncreasing="strategiesIncreasing" @cancelEnemySkills="cancelEnemySkills" @saveNewEnemySkills="saveNewEnemySkills" />
        </div>
    </div>
</template>

<script>
    import InputComponent from '../Input/InputComponent.vue';
    import EnemySkillsFormComponent from './EnemySkillsFormComponent.vue';
    
    export default {
        name: 'EnemyFormComponent',
        props: {
            enemy: {
                type: Object
            },
            strategiesIncreasing: {
                type: Array
            },
            difficulties: {
                type: Array
            },
            categories: {
                type: Array
            }
        },
        components: {
            InputComponent,
            EnemySkillsFormComponent
        },
        data() {
            return {
                newEnemy: this.initEnemy(),
                manageSkills: false
            }
        },
        methods: {
            initEnemy() {
                return {
                    id: {
                        value: this.enemy?.id ?? null,
                        rules: []
                    },
                    enemyName: {
                        value: this.enemy?.enemyName ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Enemy name is required',
                            v => v.length > 0 || 'Enemy name is required',
                            v => v.length < 100 || 'Enemy name cannot be higher than 100 characters',
                            v => v.length > 2 || 'Enemy name should have at least 3 characters',
                            v => !/^\s+$/.test(v) || 'Enemy name cannot contain white spaces'
                        ]
                    },
                    baseHealth: {
                        value: this.enemy?.baseHealth?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Health is required',
                            v => v.toString().length > 0 || 'Health is required',
                            v => v >= 0 || 'Health cannot be negative'
                        ]
                    },
                    baseHealLvl: {
                        value: this.enemy?.baseHealLvl?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Heal is required',
                            v => v.toString().length > 0 || 'Heal is required',
                            v => v >= 0 || 'Heal cannot be negative'
                        ]
                    },
                    baseAttack: {
                        value: this.enemy?.baseAttack?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Attack is required',
                            v => v.toString().length > 0 || 'Attack is required',
                            v => v >= 0 || 'Attack cannot be negative'
                        ]
                    },
                    experience: {
                        value: this.enemy?.experience?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Base required experience is required',
                            v => v.toString().length > 0 || 'Base required experience is required',
                            v => v >= 0 || 'Base required experience cannot be negative'
                        ]
                    },
                    baseHealthIncreasingState: {
                        value: this.enemy?.baseHealth?.increasingState?.strategyIncreasing ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Health increasing state is required',
                            v => v.length > 0 || 'Health increasing state is required',
                        ]
                    },
                    baseHealthIncreasingStateValue: {
                        value: this.enemy?.baseHealth?.increasingState?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Health increasing state value is required',
                            v => v.toString().length > 0 || 'Health increasing state value is required',
                            v => v >= 0 || 'Health increasing state value cannot be negative'
                        ]
                    },
                    baseHealLvlIncreasingState: {
                        value: this.enemy?.baseHealLvl?.increasingState?.strategyIncreasing ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Heal increasing state is required',
                            v => v.length > 0 || 'Heal increasing state is required',
                        ]
                    },
                    baseHealLvlIncreasingStateValue: {
                        value: this.enemy?.baseHealLvl?.increasingState?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Heal increasing state value is required',
                            v => v.toString().length > 0 || 'Heal increasing state value is required',
                            v => v >= 0 || 'Heal increasing state value cannot be negative'
                        ]
                    },
                    baseAttackIncreasingState: {
                        value: this.enemy?.baseAttack?.increasingState?.strategyIncreasing ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Attack increasing state is required',
                            v => v.length > 0 || 'Attack increasing state is required',
                        ]
                    },
                    baseAttackIncreasingStateValue: {
                        value: this.enemy?.baseAttack?.increasingState?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Attack increasing state value is required',
                            v => v.toString().length > 0 || 'Attack increasing state value is required',
                            v => v >= 0 || 'Attack increasing state value cannot be negative'
                        ]
                    },
                    experienceIncreasingState: {
                        value: this.enemy?.experience?.increasingState?.strategyIncreasing  ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Base required experience increasing state is required',
                            v => v.length > 0 || 'Base required experience increasing state is required',
                        ]
                    },
                    experienceIncreasingStateValue: {
                        value: this.enemy?.experience?.increasingState?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Base required experience increasing state value is required',
                            v => v.toString().length > 0 || 'Base required experience increasing state value is required',
                            v => v >= 0 || 'Base required experience increasing state value cannot be negative'
                        ]
                    },
                    difficulty: {
                        value: this.enemy?.difficulty  ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Difficulty is required',
                            v => v.length > 0 || 'Difficulty is required',
                        ]
                    },
                    category: {
                        value: this.enemy?.category  ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Category is required',
                            v => v.length > 0 || 'Category is required',
                        ]
                    },
                    skills: {
                        value: this.enemy?.skills ?? [],
                        rules: []
                    }
                }
            },
            submit() {
                const errors = [];
                for (const field in this.newEnemy) {
                    const error = this.validate(this.newEnemy[field].value, field);
                    if (error.length > 0) {
                        errors.push(error);
                    }
                }

                if (errors.length > 0) {
                    return;
                }

                const formToSend = {
                    id: this.newEnemy.id.value,
                    enemyName: this.newEnemy.enemyName.value,
                    baseHealth: {
                        value: Number(this.newEnemy.baseHealth.value),
                        increasingState: {
                            strategyIncreasing: this.newEnemy.baseHealthIncreasingState.value,
                            value: this.newEnemy.baseHealthIncreasingStateValue.value,
                        }
                    },
                    baseAttack: {
                        value: Number(this.newEnemy.baseAttack.value),
                        increasingState: {
                            strategyIncreasing: this.newEnemy.baseAttackIncreasingState.value,
                            value: Number(this.newEnemy.baseAttackIncreasingStateValue.value),
                        }
                    },
                    baseHealLvl: {
                        value: Number(this.newEnemy.baseHealLvl.value),
                        increasingState: {
                            strategyIncreasing: this.newEnemy.baseHealLvlIncreasingState.value,
                            value: Number(this.newEnemy.baseHealLvlIncreasingStateValue.value),
                        }
                    },
                    experience: {
                        value: Number(this.newEnemy.experience.value),
                        increasingState: {
                            strategyIncreasing: this.newEnemy.experienceIncreasingState.value,
                            value: Number(this.newEnemy.experienceIncreasingStateValue.value),
                        }
                    },
                    difficulty: this.newEnemy.difficulty.value,
                    category: this.newEnemy.category.value,
                    skills: this.newEnemy.skills.value
                };
                
                if (formToSend.id === null){
                    delete formToSend.id;
                }
                this.$emit('submitForm', formToSend);
            },
            onChangeInput(value, fieldName) {
                this.validate(value, fieldName);
                this.newEnemy[fieldName].value = value;
            },
            reset() {
                this.newEnemy = this.initEnemy();
            },
            validate(value, fieldName) {
                const rules = this.newEnemy[fieldName].rules;
                this.newEnemy[fieldName].error = '';
                this.newEnemy[fieldName].showError = false;
                
                for (const rule of rules) {
                    const valid = rule(value);

                    if (valid !== true) {
                        this.newEnemy[fieldName].error = valid;
                        this.newEnemy[fieldName].showError = true;
                        return valid;
                    }
                }

                return '';
            },
            saveNewEnemySkills(skills) {
                this.manageSkills = false;
                this.newEnemy.skills.value = skills;
            },
            cancelEnemySkills() {
                this.manageSkills = false;
            },
            cancel() {
                this.$emit('cancel');
            }
        }
    }
</script>

<style>
    .enemy-form-page-position {
        display: block;
        text-align: center;
    }

    .enemy-form-position {
        display: flex;
        justify-content: center;
    }

    .enemy-form {
        flex-direction: column;
        width: 50%;
        padding-left: 5px;
        padding-right: 5px;
    }
</style>