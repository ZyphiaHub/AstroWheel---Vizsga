<script>
    import { page } from "$app/stores";
    let logoSrc = "/logo.webp"; // A static mappában van
</script>

<svelte:head>
    <link rel="icon" type="image/webp" href="/logo.webp" />
</svelte:head>

<main>
    {#if $page.url.pathname === "/login"}
        <!--Csak a bejelentkezési oldalon jelenik meg -->
        <img class="logo" src={logoSrc} alt="AstroWheel Logo" />
    {/if}

    <slot />
</main>

<footer class="footer">Made by: Witches Brew Games 2025</footer>

<style>
    main {
        min-height: 100vh;
        width: 100vw;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        margin: 0;
        padding-bottom: 0px;
        background: url("/background.webp") no-repeat center center fixed;
        background-size: cover;
        font-family: Arial, sans-serif;
        color: white;
        text-align: center;
        z-index: -1;
    }

    .logo {
        width: 370px;
        height: auto;
        margin-top: -60px;
    }

    .footer {
        position: absolute;
        bottom: 0;
        width: 100vw;
        background: rgba(0, 0, 0, 0.6);
        padding: 10px 0;
        color: white;
        text-align: center;
    }
</style>
