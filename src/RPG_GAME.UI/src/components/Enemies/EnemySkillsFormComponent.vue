<template>
    <div class="hero-skills-form-position">
        <form class="hero-skills-form">
            <div class="hero-new-skill" @click="addNewSkill">
                <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="currentColor" class="bi bi-plus-circle" viewBox="0 0 16 18">
                    <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                    <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z"/>
                </svg>
                Add new skill
            </div>
            <div v-for="skill in newSkills" v-bind:key="skill.uid">
                <div class="hero-skill-info mt-2">
                    Skill #{{ newSkills.indexOf(skill) + 1 }} <button class="btn btn-danger ms-2" @click="() => deleteSkill(skill.uid)">Delete skill</button>
                </div>
                <div>
                    <InputComponent :label="'Skill Name'" :type="'text'" :value="skill.name.value" 
                        v-model="skill.name.value" :showError="skill.name.showError" 
                        :error="skill.name.error"
                        @valueChanged="onChangeInput($event, 'name', skill.uid)"/>
                </div>
                <div>
                    <InputComponent :label="'Skill base attack'" :type="'number'" :value="skill.baseAttack.value" 
                            v-model="skill.baseAttack.value" :showError="skill.baseAttack.showError" 
                            :error="skill.baseAttack.error"
                            @valueChanged="onChangeInput($event, 'baseAttack', skill.uid)" :step="0.01"/>
                </div>
                <div>
                    <InputComponent :label="'Probability'" :type="'number'" :value="skill.probability.value" 
                            v-model="skill.probability.value" :showError="skill.probability.showError" 
                            :error="skill.probability.error"
                            @valueChanged="onChangeInput($event, 'probability', skill.uid)" :step="0.01"/>
                </div>
                <div>
                    <InputComponent :label="'Skill base attack strategy increasing'" :type="'select'" :value="skill.baseAttackIncreasingState.value" 
                            v-model="skill.baseAttackIncreasingState.value" :showError="skill.baseAttackIncreasingState.showError" 
                            :error="skill.baseAttackIncreasingState.error" 
                            :options="strategiesIncreasing"
                            @valueChanged="onChangeInput($event, 'baseAttackIncreasingState', skill.uid)"/>
                </div>
                <div>
                    <InputComponent :label="'Skill base attack strategy increasing value'" :type="'number'" :value="skill.baseAttackIncreasingStateValue.value" 
                            v-model="skill.baseAttackIncreasingStateValue.value" :showError="skill.baseAttackIncreasingStateValue.showError" 
                            :error="skill.baseAttackIncreasingStateValue.error"
                            @valueChanged="onChangeInput($event, 'baseAttackIncreasingStateValue', skill.uid)" :step="0.01"/>
                </div>
            </div>
            <div class="mt-2">
                <button type="button" class="btn btn-secondary me-2" @click="cancel">
                    Cancel
                </button>
                <button type="button" class="btn btn-success" @click="submit">
                    Send
                </button>
            </div>
        </form>
    </div>
</template>

<script>
    import InputComponent from '../Input/InputComponent.vue';
    
    export default {
        name: 'EnemySkillsFormComponent',
        props: {
            skills: {
                type: Array,
                required: true
            },
            strategiesIncreasing: {
                type: Array,
                required: true
            }
        },
        components: {
            InputComponent
        },
        data() {
            return {
                newSkills: this.initSkills()
            }
        },
        methods: {
            initSkills() {
                return [
                    ...this.skills?.map(s => this.initSkill(s))
                ]
            },
            initSkill(skill) {
                return {
                    uid: {
                        key: this.createKey(),
                        rules: []
                    },
                    id: {
                        value: skill?.id ?? null,
                        rules: []
                    },
                    name: {
                        value: skill?.name ?? null,
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
                    baseAttack: {
                        value: skill?.baseAttack ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Attack is required',
                            v => v.toString().length > 0 || 'Attack is required',
                            v => v >= 0 || 'Attack cannot be negative'
                        ]
                    },
                    probability: {
                        value: skill?.probability ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Probability is required',
                            v => v.toString().length > 0 || 'Probability is required',
                            v => v >= 0 || 'Probability cannot be negative',
                            v => v < 95 || 'Probability cannot be greater than 95'
                        ]
                    },
                    baseAttackIncreasingState: {
                        value: skill?.increasingState?.strategyIncreasing ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Attack increasing state is required',
                            v => v.length > 0 || 'Attack increasing state is required',
                        ]
                    },
                    baseAttackIncreasingStateValue: {
                        value: skill?.increasingState?.value ?? null,
                        showError: false,
                        error: '',
                        rules: [
                            v => v !== null || 'Attack increasing state value is required',
                            v => v.toString().length > 0 || 'Attack increasing state value is required',
                            v => v >= 0 || 'Attack increasing state value cannot be negative'
                        ]
                    }
                }
            },
            submit() {
                const errors = [];
                for (const skill of this.newSkills) {
                    for (const field in skill){
                        const error = this.validate(skill[field], skill[field].value);
                        if (error.length > 0) {
                            errors.push(error);
                        }
                    }
                }

                if (errors.length > 0) {
                    return;
                }

                const enemySkillsToSend = this.newSkills.map(
                    s => {
                        const skill = {
                            id: s.id.value,
                            name: s.name.value,
                            probability: s.probability.value,
                            baseAttack: Number(s.baseAttack.value),
                            increasingState: {
                                strategyIncreasing: s.baseAttackIncreasingState.value,
                                value: Number(s.baseAttackIncreasingStateValue.value),
                            }
                        };

                        if (skill.id === null){
                            delete skill.id;
                        }

                        return skill;
                    }
                );
                
                this.$emit('saveNewEnemySkills', enemySkillsToSend);
            },
            onChangeInput(value, fieldName, uid) {
                const skill = this.newSkills.find(s => s.uid === uid);
                this.validate(skill[fieldName], value);
                skill[fieldName].value = value;
            },
            validate(field, value) {
                const rules = field.rules;
                field.error = '';
                field.showError = false;
                
                for (const rule of rules) {
                    const valid = rule(value);

                    if (valid !== true) {
                        field.error = valid;
                        field.showError = true;
                        return valid;
                    }
                }

                return '';
            },
            createKey() {
                return new Date().getDate() + Math.random() + 1;
            },
            cancel() {
                this.$emit('cancelEnemySkills');
            },
            addNewSkill() {
                this.newSkills = [
                    ...this.newSkills,
                    this.initSkill()
                ]
            },
            deleteSkill(uid) {
                this.newSkills = this.newSkills.filter(s => s.uid !== uid);
            }
        },
        emits: ['saveNewEnemySkills', 'cancelEnemySkills']
    }
</script>

<style>
    .hero-skills-form-position {
        display: flex;
        justify-content: center;
        text-align: center;
        align-items: center;
    }

    .hero-skills-form {
        display: flex;
        flex-direction: column;
        width: 50%;
        padding-left: 5px;
        padding-right: 5px;
    }

    .hero-new-skill {
        font-size: 2rem;
        cursor: pointer;
    }

    .hero-skill-info {
        font-size: 1.5rem;
    }
</style>