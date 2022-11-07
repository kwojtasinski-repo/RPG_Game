<template>
  <div :class="{ shake: disabled }">
    <div>BattleStartPage</div>
    <button @click="shake" class="btn btn-primary">Shake</button>
    <button @click="fire" class="btn btn-danger">Fire</button>
    <div class="mt-2">
      <BattleComponent />
    </div>
  </div>
</template>

<script>
import BattleComponent from '@/components/Battles/BattleComponent.vue';

  export default {
    name: 'BattleStartPage',
    components: {
      BattleComponent,
    },
    data() {
      return {
        disabled: false,
        fired: false
      }
    },
    methods: {
      shake() {
        this.disabled = true
        setTimeout(() => {
          this.disabled = false;
        }, 1500)
      },
      fire() {
        this.fired = true;
      }
    }
  }
</script>

<style>
.shake {
  animation: shake 0.82s cubic-bezier(0.36, 0.07, 0.19, 0.97) both;
  transform: translate3d(0, 0, 0);
}

@keyframes shake {
  10%,
  90% {
    transform: translate3d(-1px, 0, 0);
  }

  20%,
  80% {
    transform: translate3d(2px, 0, 0);
  }

  30%,
  50%,
  70% {
    transform: translate3d(-4px, 0, 0);
  }

  40%,
  60% {
    transform: translate3d(4px, 0, 0);
  }
}

@keyframes fire {
  10% {
    transform: translate(10px, 10px);
  } 
  100% {
    transform: translate(50px, 50px);
  }
}

.fire {
  animation: fire 0.82s cubic-bezier(0.36, 0.07, 0.19, 0.97) both;
}

.img-test {
  width: 100%; /* width of container */
  height: 100%; /* height of container */
  object-fit: none;
}

</style>