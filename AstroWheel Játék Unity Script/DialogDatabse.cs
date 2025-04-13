using System.Collections.Generic;
using UnityEngine;

public static class DialogDatabase {
    public static Dictionary<int, string[]> IslandDialogs = new Dictionary<int, string[]>
    {
        { 1, new string[] { "Welcome, young one! You are about to embark on a year-long journey.", 
                            "If you can solve every task, you will become a fully accredited Time Witch!",
                            "However, you must first prove your understanding of the yearly astrological cycle.",
                            "Solve this puzzle and place the missing pieces where they belong!" } },
        { 2, new string[] { "Welcome to the domain of Capricorn! As a Time Witch you must craft potions later.",
                            "But before that, you need to show me that you can distinguish between different plant materials.",
                            "Fail, and you will be trapped in limbo—just as I am, caught between two states of being.",
                            "Capricorn gestures with his hooves, demonstrating his predicament."} },
        { 3, new string[] { "Welcome to the realm of Aquarius!", "To prove your worth, you must catch what falls from the sky—be swift, " +
                            "be precise, and do not let distractions break your focus.",
                            "You must follow where the wind takes you, adapting with every moment.",
                            "Show me your mastery, and I shall grant you passage!"} },
        { 4, new string[] { "Drifting through the tides, you have reached my domain—welcome to the waters of Pisces.",
                            "You must prove your wisdom by matching names to forms,  just as a true alchemist deciphers the hidden language of ingredients.",
                            "A name without a form is an echo. A form without a name is a shadow.",
                            "As I am both fish and maiden, so too must you unite knowledge and perception.",
                            "Show me your insight, and I shall guide you further along your path."} },
        { 5, new string[] { "You stand before the forge of Aries—the first flame, the spark that ignites all things.",
                            "Here, memory is fire: fleeting, yet powerful. Only those who can seize its light and shape will master true transformation.",
                            "Watch closely, remember well, and repeat what you have seen.",
                            "Prove your strength, and you shall earn the right to reshape what is already yours."} },
        { 6, new string[] { "You walk upon the sacred land of Taurus—the keeper of ancient riches buried deep within the earth.",
                            "Patience and keen sight are the keys to true wealth. What is hidden does not wish to be found by the careless.",
                            "Look closely at the markings left by those before you. Gather what you find, for these materials are the lifeblood of creation itself.",
                            "Show me your perseverance, and the earth shall reward you."} },
        { 7, new string[] { "Castor: Ah, a traveler! Welcome to the ever-shifting winds of Gemini!",
                            "Pollux: You have entered a place of duality, where fortune and wit walk hand in hand.",
                            "Castor: Your mind must be sharp, your memory keen—only those who can recall what was hidden shall reap the rewards.",
                            "Pollux: But beware! The game you play is one of risk and gain. Succeed, and your chosen bounty shall double.",
                            "Castor: Fail, and the winds shall scatter all that you have gathered, leaving you with nothing.",
                            "Pollux: Now, let us see if your mind is as quick as the breeze!"} },
        { 8, new string[] { "You set foot upon the tides of Cancer.",
                            "I am the guardian of these shores, a warrior bound to the ebb and flow of the ocean.",
                            "Here, knowledge is not found—it is uncovered. What seems whole may be made into many, if your hands are steady and your choices wise",
                            "Show me your skill, and the currents shall yield their treasures to you."} },
        { 9, new string[] { "You stand before the blazing domain of Leo, where only the worthy may pass.",
                            "I am the guardian of this realm, a warrior forged in fire and crowned by the sun.",
                            "Strength alone will not grant you entry—only those who soar through the trials and prove their worth shall be allowed to step forward.",
                            "Show me the finest materials you possess, for power demands tribute.",
                            "Then, and only then, shall the gates open before you."} },
        { 10, new string[] { "You tread upon the sacred grounds of Virgo—the land of precision, and quiet perfection.",
                             "Nothing in nature thrives without harmony. Only those with patience and clarity of mind shall uncover its hidden treasures.",
                             "Show me your skill in the trials of alignment. If you can bring order to chaos, the land shall grant you its finest gifts." } },
        { 11, new string[] { "You step onto the shores of Libra, where the living and the departed stand in delicate balance.",
                             "I am the Ferryman of Souls, guiding those who seek passage across the unseen veil.",
                             "Nothing escapes the eyes of the dead. If you wish to claim the gifts hidden in this realm, you must sharpen your sight and see what others overlook.",
                             "Search carefully, for in this place, every object holds a story—and not all wish to be found."} },
        { 12, new string[] { "You have stepped into the shadows of Scorpio, where truth is veiled and knowledge is power.",
                             "Only those who truly understand the essence of matter may claim the rarest of treasures.",
                             "Choose wisely, for every question is a blade—one that can cut through illusion or turn against the unworthy.",
                             "Show me your insight, and the hidden paths of alchemy shall open before you."} }
        
    };

    public static string[] GetDialogForIsland(int islandIndex)
    {
        if (IslandDialogs.ContainsKey(islandIndex))
        {
            return IslandDialogs[islandIndex];
        }
        return new string[] { "Ismeretlen sziget.", "Nincs elérhetõ párbeszéd." };
    }
}
