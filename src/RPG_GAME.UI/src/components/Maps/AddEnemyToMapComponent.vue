<template>
    <div class="map-add-container">
        <div>
            <InputComponent :label="'Id'" :type="'text'" :value="markedEnemy?.id" :readonly="true" :class="'add-enemy-input'" />
        </div>
        <div>
            <InputComponent :label="'Quantity'" :type="'number'" :value="enemyQuantity.value" :class="'add-enemy-input'"
                        v-model="enemyQuantity.value" :showError="enemyQuantity.showError" 
                        :error="enemyQuantity.error"
                        @valueChanged="onChangeEnemyQantity($event)" />
        </div>
        <div class="mt-2 mb-2">
            <button class="btn btn-success" type="button" @click="addEnemy">Add Enemy</button>
        </div>
    </div>
</template>

<script>
import InputComponent from '@/components/Input/InputComponent.vue';
  
export default {
    name: 'MapFormComponent',
    props: {
        markedEnemy: {
            type: Object
        },
    },
    components: {
        InputComponent
    },
    data() {
        return {
            enemyQuantity: this.initEnemyQuantity()
        }
    },
    methods: {
        initEnemyQuantity() {
            return {
                value: null,
                showError: false,
                error: '',
                rules: [
                    v => v !== null || 'Quantity is required',
                    v => v.toString().length > 0 || 'Quantity is required',
                    v => v >= 0 || 'Quantity cannot be negative'
                ]
            };
        },
        reset() {
            this.enemyQuantity = this.initEnemyQuantity()
        },
        onChangeEnemyQantity(value) {
            const isValid = this.validateEnemyQuantity(value);
            if (!isValid) {
                return;
            }

            this.enemyQuantity.value = Number(value);
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
        addEnemy() {
            const isValid = this.validateEnemyQuantity(this.enemyQuantity.value);

            if (!isValid) {
                return;
            }

            this.$emit('enemyToAdd', this.markedEnemy, this.enemyQuantity.value);
        },
    },
    emits: ['enemyToAdd']
}
</script>

<style>
    .add-enemy-input {
        flex: 1;
        flex-direction: row;
    }
</style>