<template>
    <div v-if="loading">
        <LoadingIconComponent />
    </div>
    <div v-else>
        <MenuComponent :menuItems="menuItems()" />
    </div>
</template>

<script>
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';
import MenuComponent from '@/components/Menu/MenuComponent.vue';
import { GetCurrentBattlesRequest } from '@/grpc-client/battle_pb.js';
import grpcClient from '@/grpc-client/grpc-client-setup';
import { mapGetters } from 'vuex';

  export default {
    name: 'BattleStartPage',
    components: { MenuComponent, LoadingIconComponent },
    data() {
      return {
        loading: true,
        battles: []
      }
    },
    methods: {
      menuItems() {
        let activeBattleRoute = { text: 'Resume', disabled: true };
        if (this.battles.length > 0) {
            activeBattleRoute = { text: 'Resume', action: () => this.$router.push({ name: 'battle-resume' }) };
        }
        return [
          { text: 'Start fight', action: () => this.$router.push({ name: 'fight-start' }) },
          activeBattleRoute,
          { text: 'Back to menu', action: () => this.$router.push('/') },
        ];
      }
    },
    async created() {
      const request = new GetCurrentBattlesRequest([this.user.id]);
      try {
        const response = await grpcClient.getBattleState(request);
        this.battles = response.data; // TODO: check how to get data
      } catch (exception) {
        console.error(exception);
      }
      this.loading = false;
    },
    computed: {
      ...mapGetters(['user'])
    },
 }
</script>

<style>
</style>