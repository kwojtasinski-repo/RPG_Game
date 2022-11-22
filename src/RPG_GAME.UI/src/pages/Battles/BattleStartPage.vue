<template>
  <div v-if="loading"><LoadingIconComponent /></div>
  <div v-else>
    <h3>BattleStartPage</h3>
    <div v-if="error" className="alert alert-danger mt-2 mb-2">
      {{error}}
    </div>
    <div v-else class="mt-2 mb-2">
      <BattleComponent :enemies="battle.map.enemies" :enemiesKilled="[]" />
    </div>
    <RouterButtonComponent url="/" buttonText="Back to menu" />
  </div>
</template>

<script>
import BattleComponent from '@/components/Battles/BattleComponent.vue';
import LoadingIconComponent from '@/components/LoadingIcon/LoadingIconComponent.vue';
import RouterButtonComponent from '@/components/RouterButton/RouterButtonComponent.vue';
import { BattleRequest, GetBattleStateRequest } from "@/grpc-client/battle_pb.js";
import grpcClient from "@/grpc-client/grpc-client-setup";
import { mapGetters } from 'vuex';

  export default {
    name: 'BattleStartPage',
    components: {
      BattleComponent,
      LoadingIconComponent,
      RouterButtonComponent
    },
    data() {
      return {
        loading: true,
        error: '',
        battleState: null
      }
    },
    methods: {
      async startBattle() {
        try {
          const request = new BattleRequest([this.$route.params.battleId, this.user.id]);
          await grpcClient.startBattle(request);
          return true;
        } catch(exception) {
          if (!exception.message) {
            this.error = 'There was something wrong, please try again later';
          } else {
            this.error = exception.message;
          }
          console.error(exception);
          return false;
        }
      },
      async getBattleState() {
        try {
          const request = new GetBattleStateRequest([this.$route.params.battleId]);
          const response = await grpcClient.getBattleState(request);
          this.battleState = {
            id: response.getId(),
            battleId: response.getBattleid(),
            battleStatus: response.getBattlestatus(),
            player: {
              id: response.getPlayer().getId(),
              name: response.getPlayer().getName(),
              level: response.getPlayer().getLevel(),
              currentExp: Number(response.getPlayer().getCurrentexp().getUnits() + '.' + response.getPlayer().getCurrentexp().getNanos()),
              requiredExp: Number(response.getPlayer().getRequiredexp().getUnits() + '.' + response.getPlayer().getRequiredexp().getNanos()),
              hero: {
                id: response.getPlayer().getHero().getId(),
                heroName: response.getPlayer().getHero().getHeroname(),
                health: response.getPlayer().getHero().getHealth(),
                healLvl: response.getPlayer().getHero().getHeallvl(),
                attack: response.getPlayer().getHero().getAttack(),
                skills: response.getPlayer().getHero().getSkillsList().map(s => ({
                  id: s.getId(),
                  name: s.getName(),
                  attack: s.getAttack()
                }))
              },
              userId: response.getPlayer().getUserid()
            },
            created: new Date(response.getCreated() * 1000),
            modified: response.getModified() ? new Date(response.getModified() * 1000) : null
          }
          this.$store.dispatch('player', this.battleState.player);
          // eslint-disable-next-line
          debugger;
        } catch(exception) {
          if (!exception.message) {
            this.error = 'There was something wrong, please try again later';
          } else {
            this.error = exception.message;
          }
          console.error(exception);
        }
      }
    },
    async created() {
      const isStarted = await this.startBattle();
      if (!isStarted) {
        this.loading = false;
        return;
      }
      await this.getBattleState();
      this.loading = false;
    },
    computed: {
      ...mapGetters(['user', 'battle'])
    }
  }
</script>

<style>
</style>