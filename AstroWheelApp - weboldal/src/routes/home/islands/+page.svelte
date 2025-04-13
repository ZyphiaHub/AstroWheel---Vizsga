<script>
    import Card from "../Card.svelte";
    import CardContainer from "../CardContainer.svelte";

    let images = [
        {
            src: "/island Sagittarius.webp",
            alt: "Sagittarius Island",
            info1: "/Island_sagittarius_info.webp",
        },
        {
            src: "/island Capricorn.webp",
            alt: "Capricorn Island",
            info1: "/Island_capricorn_info.webp",
        },
        {
            src: "/island Aquarius.webp",
            alt: "Aquarius Island",
            info1: "/Island_aquarius_info.webp",
        },
        {
            src: "/island Pisces.webp",
            alt: "Pisces Island",
            info1: "/Island_pisces_info.webp",
        },
        {
            src: "/island Aries.webp",
            alt: "Aries Island",
            info1: "/Island_aries_info.webp",
        },
        {
            src: "/island Taurus.webp",
            alt: "Taurus Island",
            info1: "/Island_taurus_info.webp",
        },
        {
            src: "/island Gemini.webp",
            alt: "Gemini Island",
            info1: "/Island_gemini_info.webp",
        },
        {
            src: "/island Cancer.webp",
            alt: "Cancer Island",
            info1: "/Island_cancer_info.webp",
        },
        {
            src: "/island Leo.webp",
            alt: "Leo Island",
            info1: "/Island_leo_info.webp",
        },
        {
            src: "/island Virgo.webp",
            alt: "Virgo Island",
            info1: "/Island_virgo_info.webp",
        },
        {
            src: "/island Libra.webp",
            alt: "Libra Island",
            info1: "/Island_libra_info.webp",
        },
        {
            src: "/island Scorpio.webp",
            alt: "Scorpio Island",
            info1: "/Island_scorpio_info.webp",
        },
    ];
</script>

<div class="home-container">
    <div class="content">
        <h1>ISLANDS</h1>
    </div>
</div>

<CardContainer bind:images roundedSquare="True" />

<style>
    .home-container {
        margin-left: 300px;
        display: flex;
    }

    .content {
        flex: 1;
        padding: 4px;
        background: url("/background6.webp") no-repeat center center fixed;
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
