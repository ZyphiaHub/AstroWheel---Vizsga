using UnityEditor;
using UnityEngine;

public class PlantInitializer : MonoBehaviour {
    public PlantDatabase plantDatabase;

    void Start()
    {
        
        plantDatabase.items = new PlantDatabase.Item[]
        {
            new PlantDatabase.Item
            {
                plantId = 12,
                englishName = "Jade Orchid",
                witchName = "Squirrel's ear",
                latinName = "Goodyera repens",
                description = "Material for  Crystals.",
                icon = LoadSprite("Assets/Art/Matik/Avarvirág.png")
            },
            new PlantDatabase.Item
            {
                plantId = 13,
                englishName = "Velvet Bean",
                witchName = "Donkey's eye",
                latinName = "Mucuna pruriens",
                description = "Herbal drug used for the management of male infertility, nervous disorders, and also as an aphrodisiac.",
                icon = LoadSprite("Assets/Art/Matik/Bengáli bársonybab.png")
            },

            new PlantDatabase.Item
            {
                plantId = 14,
                englishName = "Horseweed",
                witchName = "Colt's tail",
                latinName = "Erigeron canadensis",
                description = "Treat for sore throat and dysentery.",
                icon = LoadSprite("Assets/Art/Matik/Betyárkóró.png")
            },
            new PlantDatabase.Item
            {
                plantId = 15,
                englishName = "Starflower Pincushions",
                witchName = "Cat's eye",
                latinName = "Scabiosa stellata",
                description = "It is known in ancient times to treat scurvy.",
                icon = LoadSprite("Assets/Art/Matik/Csillag ördögszem.png")
            },
            new PlantDatabase.Item
            {
                plantId = 16,
                englishName = "Spotted Cranesbill",
                witchName = "Crowfoot",
                latinName = "Geranium maculatum",
                description = "Egy erõteljes kristály, amely varázslatokban használható.",
                icon = LoadSprite("Assets/Art/Matik/Foltos gólyaorr.png")
            },
            
            new PlantDatabase.Item
            {
                plantId = 17,
                englishName = "Bulbous Buttercup",
                witchName = "Frog leg",
                latinName = "Ranunculus bulbosus",
                description = "Used for skin diseases, arthritis, nerve pain, flu. Can cause irritation.",
                icon = LoadSprite("Assets/Art/Matik/Hagymás boglárka.png")
            },
            new PlantDatabase.Item
            {
                plantId = 18,
                englishName = "Canadian Snakeroot",
                witchName = "Cat's paw",
                latinName = "Asarum canadense",
                description = "Used for bronchitis, bronchial spasms, and bronchial asthma.",
                icon = LoadSprite("Assets/Art/Matik/Kanadai kapotnyak.png")
            },
            
            new PlantDatabase.Item
            {
                plantId = 19,
                englishName = "White Turtlehead",
                witchName = "Snake's head",
                latinName = "Chelone glabra",
                description = "It has been used as a method of birth control.",
                icon = LoadSprite("Assets/Art/Matik/Kopasz gerlefej.png")
            },
            new PlantDatabase.Item
            {
                plantId = 20,
                englishName = "Coral-root",
                witchName = "Dragon's claw",
                latinName = "Corallorhiza odontorrhiza",
                description = "Might help cause sweating, reduce fever, and promote drowsiness.",
                icon = LoadSprite("Assets/Art/Matik/Korallgyökér.png")
            } ,  
            new PlantDatabase.Item
            {
                plantId = 21,
                englishName = "Stiff Slubmoss",
                witchName = "Wolf's claw",
                latinName = "Lycopodium annotinum",
                description = "Used to treat wounds and skin diseases.",
                icon = LoadSprite("Assets/Art/Matik/Korpafû.png")
            },
            
            new PlantDatabase.Item
            {
                plantId = 22,
                englishName = "Calvary Clover",
                witchName = "Hedgehog",
                latinName = "Medicago intertexta",
                description = "Used in conditions like colds, diabetes, and skin infections.",
                icon = LoadSprite("Assets/Art/Matik/Krisztuskorona.png")
            },
            new PlantDatabase.Item
            {
                plantId = 23,
                englishName = "Houndstongue",
                witchName = "Dog's tongue",
                latinName = "Cynoglossum officinale",
                description = "Used in piles, lung diseases, persistent coughs, baldness, sores, and ulcers.",
                icon = LoadSprite("Assets/Art/Matik/Közönséges ebnyelvûfû.png")
            },
            
            new PlantDatabase.Item
            {
                plantId = 24,
                englishName = "Common Toadflax",
                witchName = "Toad",
                latinName = "Linaria vulgaris",
                description = "Astringent, hepatic and detergent - cleanses toxins from the tissue.",
                icon = LoadSprite("Assets/Art/Matik/Közönséges gyújtoványfû.png")
            },
            new PlantDatabase.Item
            {
                plantId = 25,
                englishName = "Narrowleaf Plantain",
                witchName = "Lamb's tongue",
                latinName = "Plantago lanceolata",
                description = "Effective treatment for bleeding, it quickly staunches blood flow and encourages the repair of damaged tissue.",
                icon = LoadSprite("Assets/Art/Matik/Lándzsás utifû.png")
            },
            new PlantDatabase.Item
            {
                plantId = 26,
                englishName = "Red Clover",
                witchName = "Rabbit's foot",
                latinName = "Trifolium arvense",
                description = "A remedy for menopause symptoms, asthma, whooping cough, arthritis.",
                icon = LoadSprite("Assets/Art/Matik/Tarlóhere.png")
            },
            new PlantDatabase.Item
            {
                plantId = 27,
                englishName = "Wild Spurge",
                witchName = "Snake's milk",
                latinName = "Euphorbia corollata",
                description = "The plant has irritating and uncertain qualities and so is seldom used in herbal medicine.",
                icon = LoadSprite("Assets/Art/Matik/Viruló kutyatej.png")
            }

            
        };

       
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(plantDatabase);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }
    private Sprite LoadSprite(string path)
    {
#if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
#else
        return null;
#endif
    }
}