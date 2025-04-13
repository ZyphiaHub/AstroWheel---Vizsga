<script>
    import { onMount } from "svelte";

    let userData = {};
    import { sessionStore } from "$lib/stores/sessionStore";
    import { PUBLIC_SERVER_URL } from "$env/static/public";

    onMount(async () => {
        const response = await fetch(PUBLIC_SERVER_URL + "/api/players/me", {
            method: "GET",
            headers: {
                "content-type": "application/json",
                Authorization: `Bearer ${sessionStore.getToken()}`,
            },
        });
        userData = await response.json();
    });
</script>

<div class="home-container">
    <div class="content">
        <h1>Welcome back, {userData.playerName}!</h1>
        <p>Your last visited island: {userData.islandName}</p>
        <p>Actual Points: {userData.totalScore}</p>
    </div>
</div>

<style>
    /* Jobb oldali tartalom */
    .content {
        flex: 1;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        text-align: center;
        color: black;
        font-size: 24px;
        background: rgba(255, 255, 255, 0.85);
        padding: 20px;
        box-shadow: inset 0px 0px 10px rgba(0, 0, 0, 0.2);
        border-radius: 10px;
        margin: 20px;
    }

    .content h1 {
        font-size: 32px;
        font-weight: bold;
        color: black;
        margin-bottom: 10px;
    }

    .content p {
        font-size: 20px;
        margin: 5px 0;
    }
    .home-container {
        margin-left: 300px;
        display: flex;
    }
</style>
