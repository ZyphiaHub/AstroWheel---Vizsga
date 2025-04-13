<script>
    export let image;
    export let showModal;
    export let info1;
    export let info2;
    export let isMultiPage;
    export let quantity;
    export let roundedSquare;
</script>

<div
    class="card"
    onclick={() => {
        info1 = image.info1;
        info2 = image.info2;
        isMultiPage = image.info2 !== undefined;
        showModal = true;
    }}
>
    {#if roundedSquare}
        <img src={image.src} alt={image.alt} class="npc_pic roundedSquare" />
    {:else}
        <img src={image.src} alt={image.alt} class="npc_pic" />
    {/if}

    {#if quantity}
        <div>{image.alt} - {quantity}</div>
    {:else}
        <div>{image.alt}</div>
    {/if}
</div>

<style>
    .card {
        padding: 20px;
    }
    .npc_pic {
        height: 200px;
        width: 200px;
        object-fit: cover;
        margin-bottom: 8px;
    }
    .npc_pic:hover {
        transform: scale(1.05);
        box-shadow: 0 0 5px 10px rgba(94, 92, 91, 0.5);
        cursor: pointer;
    }
    .roundedSquare {
        border-radius: 75px 25px;
    }
</style>
