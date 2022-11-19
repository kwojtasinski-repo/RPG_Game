<template>
  <div>
    <h3>BattleStartPage</h3>
    <div class="mt-2">
      <BattleComponent />
    </div>
  </div>
</template>

<script>
import BattleComponent from '@/components/Battles/BattleComponent.vue';
// import { GetBattleStateRequest } from "@/grpc-client/battle_pb.js"
// import grpcClient from "@/grpc-client/grpc-client-setup"

  export default {
    name: 'BattleStartPage',
    components: {
      BattleComponent,
    },
    data() {
      return {
      }
    },
    methods: {
    },
    async created() {
      // const request = new GetBattleStateRequest(['555f95a4-9a62-42fa-9241-c0a9b66d4129']);
      // const response = await grpcClient.getBattleState(request);
    }
  }
</script>

<style>
</style>