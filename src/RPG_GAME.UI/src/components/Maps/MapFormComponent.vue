<template>
    <div class="map-form-page-position">
        <div class="map-enemies-table-position">
            <TableComponent :data="enemies" :fields="enemiesFields" :filterBy="enemyFilter" :elementsPerPage="5" sortable  @markedElement="markedElement" />
        </div>
        <div class="map-add-enemy-position">
            <div class="map-add-container">
                <div>
                    <InputComponent :label="'Id'" :type="'text'" :value="markedEnemy?.id" :readonly="true" :class="'map-add-enemt-input'"/>
                </div>
                <div>
                    <InputComponent :label="'Quantity'" :type="'number'" :value="enemyQuantity.value" :class="'map-add-enemt-input'"
                                v-model="enemyQuantity.value" :showError="enemyQuantity.showError" 
                                :error="enemyQuantity.error"
                                @valueChanged="onChangeEnemyQantity($event)" />
                </div>
                <div class="mt-2 mb-2">
                    <button class="btn btn-success" type="button" @click="addEnemy">Add Enemy</button>
                </div>
            </div>
        </div>
        <div class="map-form-position">
            <form class="map-form" @submit.prevent="submit">
                <div>
                    <InputComponent :label="'Map Name'" :type="'text'" :value="newMap.name.value" 
                            v-model="newMap.name.value" :showError="newMap.name.showError" 
                            :error="newMap.name.error"
                            @valueChanged="onChangeInput($event, 'name')"/>
                </div>
                <div>
                    <InputComponent :label="'Difficulty'" :type="'select'" :value="newMap.difficulty.value" 
                            v-model="newMap.difficulty.value" :showError="newMap.difficulty.showError" 
                            :error="newMap.difficulty.error"
                            :options="difficulties" 
                            @valueChanged="onChangeInput($event, 'difficulty')"/>
                </div>
                <div v-if="newMap.enemies.value.length > 0" class="mt-2 mb-2">
                    <h4>Skills</h4>
                    <div>
                        <table class="table table-hover table-striped table-fit table-bordered">
                            <thead>
                                <tr class="table-dark">
                                    <th> 
                                        Id
                                    </th>
                                    <th> 
                                        Quantity
                                    </th>
                                    <th> 
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody class="table-group-divider">
                                <tr v-for="enemy in newMap.enemies.value" :key="enemy.enemyId" class="table-light">
                                    <td>{{ enemy.enemyId }}</td>
                                    <td>
                                        <div class="d-flex">
                                            <div class="icon" @click="() => addOneEnemy(enemy)">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-plus-circle" viewBox="0 0 16 16">
                                                    <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                                                    <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z"/>
                                                </svg>
                                            </div>
                                            {{ enemy.quantity }}
                                            <div class="icon" @click="() => deleteOneEnemy(enemy)">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-dash-circle" viewBox="0 0 16 16">
                                                    <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                                                    <path d="M4 8a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7A.5.5 0 0 1 4 8z"/>
                                                </svg>
                                            </div>
                                        </div>
                                    </td>
                                    <td><button class="btn btn-danger" @click="() => deleteEnemy(enemy)">Delete</button></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div>
                    <div class="mt-2">
                        <button type="button" class="btn btn-outline-secondary me-2" @click="reset">
                            Reset
                        </button>
                        <button type="button" class="btn btn-secondary me-2" @click="cancel">
                            Cancel
                        </button>
                        <button class="btn btn-success">
                            Send
                        </button>
                    </div>
                </div>
            </form>
        </div>
    </div>
</template>
  
<script>
import InputComponent from '@/components/Input/InputComponent.vue';
import TableComponent from '../Tables/TableComponent.vue';
  
export default {
    name: 'MapFormComponent',
    props: {
        map: {
            type: Object
        },
        enemies: {
            type: Array,
            required: true
        },
        difficulties: {
            type: Array,
            required: true
        },
        enemiesFields: {
            type: Array,
            required: true
        },
        enemyFilter: {
            type: String,
            required: true
        }
    },
    components: {
        InputComponent,
        TableComponent
    },
    data() {
        return {
            newMap: this.initMap(),
            markedEnemy: null,
            enemyQuantity: {
                value: null,
                showError: false,
                error: '',
                rules: [
                    v => v !== null || 'Quantity is required',
                    v => v.toString().length > 0 || 'Quantity is required',
                    v => v >= 0 || 'Quantity cannot be negative'
                ]
            }
        }
    },
    methods: {
        initMap() {
            return {
                id: {
                    value: this.map?.id ?? null,
                    rules: []
                },
                name: {
                    value: this.map?.name ?? null,
                    showError: false,
                    error: '',
                    rules: [
                        v => v !== null || 'Map name is required',
                        v => v.length > 0 || 'Map name is required',
                        v => v.length < 100 || 'Map name cannot be higher than 100 characters',
                        v => v.length > 2 || 'Map name should have at least 3 characters',
                        v => !/^\s+$/.test(v) || 'Map name cannot contain white spaces'
                    ]
                },
                difficulty: {
                    value: this.map?.difficulty ?? null,
                    showError: false,
                    error: '',
                    rules: [
                        v => v !== null || 'Difficulty is required',
                        v => v.length > 0 || 'Difficulty is required',
                    ]
                },
                enemies: {
                    value: this.map?.enemies ?? [],
                    rules: []
                }
            }
        },
        markedElement(element) {
            this.markedEnemy = element;

            if (this.markedEnemy === null) {
                this.enemyQuantity.value = null;
            }
        },
        validateEnemyQuantity(value) {
            this.enemyQuantity.error = '';
            this.enemyQuantity.showError = false;

            if (!this.markedEnemy) {
                this.enemyQuantity.error = 'Mark enemy to add';
                this.enemyQuantity.showError = true;
                this.enemyQuantity.value = null;
                return false;
            }

            const rules = this.enemyQuantity.rules;
            for (const rule of rules) {
                const valid = rule(value);

                if (valid !== true) {
                    this.enemyQuantity.error = valid;
                    this.enemyQuantity.showError = true;
                    return false;
                }
            }

            return true;
        },
        onChangeEnemyQantity(value) {
            const isValid = this.validateEnemyQuantity(value);
            if (!isValid) {
                return;
            }

            this.enemyQuantity.value = Number(value);
        },
        addEnemy() {
            const isValid = this.validateEnemyQuantity(this.enemyQuantity.value);

            if (!isValid) {
                return;
            }

            const enemyExists = this.newMap.enemies.value.find(e => e.enemyId === this.markedEnemy.id);
            
            if (enemyExists) {
                enemyExists.quantity += this.enemyQuantity.value;
                return;
            }

            this.newMap.enemies.value.push({ enemyId: this.markedEnemy.id, quantity: this.enemyQuantity.value });
        },
        addOneEnemy(enemy) {
            const enemyExists = this.newMap.enemies.value.find(e => e.enemyId === enemy.enemyId);
            
            if (!enemyExists) {
                throw new Error(`Enemy with id ${enemy.enemyId} doesnt exists`);
            }

            enemyExists.quantity += 1;
        },
        deleteOneEnemy(enemy) {
            const enemyExists = this.newMap.enemies.value.find(e => e.enemyId === enemy.enemyId);
            
            if (!enemyExists) {
                throw new Error(`Enemy with id ${enemy.enemyId} doesnt exists`);
            }

            if (enemyExists.quantity === 1) {
                this.deleteEnemy(enemy);
                return;
            }

            enemyExists.quantity -= 1;
        },
        deleteEnemy(enemy) {
            this.newMap.enemies.value = this.newMap.enemies.value.filter(e => e.enemyId !== enemy.enemyId);
        },
        onChangeInput(value, fieldName) {
            this.validate(value, fieldName);
            this.newMap[fieldName].value = value;
        },
        validate(value, fieldName) {
            const rules = this.newMap[fieldName].rules;
            this.newMap[fieldName].error = '';
            this.newMap[fieldName].showError = false;
            
            for (const rule of rules) {
                const valid = rule(value);

                if (valid !== true) {
                    this.newMap[fieldName].error = valid;
                    this.newMap[fieldName].showError = true;
                    return valid;
                }
            }

            return '';
        },
        submit() {
            const errors = [];
            for (const field in this.newMap) {
                const error = this.validate(this.newMap[field].value, field);
                if (error.length > 0) {
                    errors.push(error);
                }
            }

            if (errors.length > 0) {
                return;
            }

            const formToSend = {
                id: this.newMap.id.value,
                name: this.newMap.name.value,
                difficulty: this.newMap.difficulty.value,
                enemies: this.newMap.enemies.value
            };
            
            if (formToSend.id === null){
                delete formToSend.id;
            }
            
            this.$emit('submitForm', formToSend);
        },
        reset() {
            this.newMap = this.initMap();
        },
        cancel() {
            this.$emit('cancel');
        }
    },
    emits: ['cancel', 'submitForm']
}
</script>
  
<style>
    .map-form-page-position {
        display: block;
        text-align: center;
    }

    .map-form-position {
        display: flex;
        justify-content: center;
    }

    .map-form {
        flex-direction: column;
        width: 50%;
        padding-left: 5px;
        padding-right: 5px;
    }

    .map-enemies-table-position {
        padding-left: 10%;
        padding-right: 10%;
    }

    .map-add-enemy-position{
        padding-left: 10%;
        padding-right: 10%;
        display: flex;
        justify-content: center;
    }

    .map-add-enemt-input {
        flex: 1;
        flex-direction: row;
    }

    .map-add-container {
        width: 50%;
    }

    .icon {
        cursor: pointer;
        margin-left: 0.5rem !important;
        margin-right: 0.5rem !important;
        color: blue;
    }
</style>