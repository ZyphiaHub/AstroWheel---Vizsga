<script>
    import { onMount } from "svelte";
    import { sessionStore } from "$lib/stores/sessionStore";
    import { PUBLIC_SERVER_URL } from "$env/static/public";

    let scores = [];

    onMount(async () => {
        const response = await fetch(PUBLIC_SERVER_URL + "/api/inventory", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${sessionStore.getToken()}`,
            },
        });
        scores = await response.json();
    });
</script>

<div class="home-container">
    <div class="content">
        <h1>HIGHSCORES</h1>
    </div>
</div>

{#if scores.length}
    <div style="width: 100%; display: flex; justify-content: center;">
        <table
            style="
            border-collapse: collapse; 
            width: 100%; 
            max-width: 40%; 
            background: rgba(255, 255, 255, 0.95); 
            text-align: center;
            margin-top: 30px;
            margin-left: 270px;
        "
        >
            <thead>
                <tr
                    style="background: #001F3F; color: white; font-weight: bold;"
                >
                    <th
                        style="border: 1px solid black; padding: 8px; width: 5%;"
                        >#</th
                    >
                    <th
                        style="border: 1px solid black; padding: 8px; width: 66%;"
                        >Player Name</th
                    >
                    <th
                        style="border: 1px solid black; padding: 8px; width: 34%;"
                        >Total Score</th
                    >
                </tr>
            </thead>
            <tbody>
                {#each scores as score, i}
                    <tr>
                        <td
                            style="border: 1px solid black; padding: 8px; width: 5%;"
                            >{i + 1}</td
                        >
                        <td
                            style="border: 1px solid black; padding: 8px; width: 66%;"
                            >{score.playerName}</td
                        >
                        <td
                            style="border: 1px solid black; padding: 8px; width: 34%;"
                            >{score.totalScore}</td
                        >
                    </tr>
                {/each}
            </tbody>
        </table>
    </div>
{:else}
    <p style="text-align: center;">Loading scores...</p>
{/if}

<style>
    .home-container {
        margin-left: 300px;
        display: flex;
    }

    .content {
        flex: 1;
        padding: 4px;
        background: url("/background5.webp") no-repeat center center fixed;
        justify-content: right;
        text-shadow:
            -1.5px 0 black,
            0 1.5px black,
            1.5px 0 black,
            0 -1.5px black;
        color: white;
        align-items: right;
        font-size: 24px;
    }
</style>
