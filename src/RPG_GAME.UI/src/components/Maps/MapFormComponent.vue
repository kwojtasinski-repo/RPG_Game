<template>
    <div class="map-form-page-position">
        <div class="map-enemies-table-position">
            <TableComponent :data="enemies" :fields="enemiesFields" :filterBy="enemyFilter" :elementsPerPage="5" sortable />
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
            newMap: this.initMap()
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
                }
            }
        },
        submit() {

        }
    }
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
</style>