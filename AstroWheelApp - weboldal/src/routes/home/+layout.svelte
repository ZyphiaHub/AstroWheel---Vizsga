<script>
  import Sidebar from "./Menu.svelte";

  let { children } = $props();
  import { sessionStore } from "$lib/stores/sessionStore";
</script>

{#if sessionStore.getToken()}
  <main>
    <Sidebar />
    {@render children()}
  </main>
{:else}
  Unauthorised 403
{/if}

<style>
  main {
    flex-direction: column;
    overflow: hidden;
    text-align: center;
    z-index: -1;
  }
</style>
