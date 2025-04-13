<script>
    import CardContainer from "../CardContainer.svelte";

    let images = [
        {
            src: "/npc Sagittarius.webp",
            alt: "Sagittarius",
            info1: "/Sagittarius_info1.webp",
            info2: "/Sagittarius_info2.webp",
        },
        {
            src: "/npc Capricorn.webp",
            alt: "Capricorn",
            info1: "/Capricorn_info1.webp",
            info2: "/Capricorn_info2.webp",
        },
        {
            src: "/npc Aquarius.webp",
            alt: "Aquarius",
            info1: "/Aquarius_info1.webp",
            info2: "/Aquarius_info2.webp",
        },
        {
            src: "/npc Pisces.webp",
            alt: "Pisces",
            info1: "/Pisces_info1.webp",
            info2: "/Pisces_info2.webp",
        },
        {
            src: "/npc Aries.webp",
            alt: "Aries",
            info1: "/Aries_info1.webp",
            info2: "/Aries_info2.webp",
        },
        {
            src: "/npc Taurus.webp",
            alt: "Taurus",
            info1: "/Taurus_info1.webp",
            info2: "/Taurus_info2.webp",
        },
        {
            src: "/npc Gemini.webp",
            alt: "Gemini",
            info1: "/Gemini_info1.webp",
            info2: "/Gemini_info2.webp",
        },
        {
            src: "/npc Cancer.webp",
            alt: "Cancer",
            info1: "/Cancer_info1.webp",
            info2: "/Cancer_info2.webp",
        },
        {
            src: "/npc Leo.webp",
            alt: "Leo",
            info1: "/Leo_info1.webp",
            info2: "/Leo_info2.webp",
        },
        {
            src: "/npc Virgo.webp",
            alt: "Virgo",
            info1: "/Virgo_info1.webp",
            info2: "/Virgo_info2.webp",
        },
        {
            src: "/npc Libra.webp",
            alt: "Libra",
            info1: "/Libra_info1.webp",
            info2: "/Libra_info2.webp",
        },
        {
            src: "/npc Scorpio.webp",
            alt: "Scorpio",
            info1: "/Scorpio_info1.webp",
            info2: "/Scorpio_info2.webp",
        },
    ];
</script>

<div class="home-container">
    <div class="content">
        <h1>CHARACTERS</h1>
    </div>
</div>

<CardContainer bind:images />

<style>
    .home-container {
        margin-left: 300px;
        display: flex;
        align-items: center;
    }

    .content {
        flex: 1;
        padding: 4px;
        background: url("/background3.webp") no-repeat center center fixed;
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
