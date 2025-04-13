<script>
  let { children } = $props();
</script>

<main>
  {@render children()}
</main>

<style>
  :global(body) {
    margin: 0;
    padding: 0;
    background: url("/background.webp") no-repeat center center fixed;
    background-size: cover;
    font-family: Arial, Helvetica, sans-serif;
    font-weight: bold;
  }
  main {
    flex-direction: column;
    overflow: hidden;
    text-align: center;
    z-index: -1;
  }
</style>
