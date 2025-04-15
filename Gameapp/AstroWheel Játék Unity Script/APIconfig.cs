using UnityEngine;

[CreateAssetMenu(fileName = "APIConfig", menuName = "ScriptableObjects/APIConfig", order = 4)]
public class APIConfig : ScriptableObject {
    [Header("Player Controller")]
    public string playersGetUrl = "https://astrowheelapi.onrender.com/api/players";
    public string playerGetByIdUrl = "https://astrowheelapi.onrender.com/api/players/{0}";
    public string playerMeGetUrl = "https://astrowheelapi.onrender.com/api/players/me";
    public string playerMaterialsGetUrl = "https://astrowheelapi.onrender.com/api/players/{0}/materials";
    public string playerPutUrl = "https://astrowheelapi.onrender.com/api/players/{0}";
    public string playerDeleteUrl = "https://astrowheelapi.onrender.com/api/players/{0}";

    [Header("Character Controller")]
    public string characterGetUrl = "https://astrowheelapi.onrender.com/api/character";
    public string characterGetByIdUrl = "https://astrowheelapi.onrender.com/api/character/{0}";
    public string characterPostUrl = "https://astrowheelapi.onrender.com/api/character";
    public string characterPutUrl = "https://astrowheelapi.onrender.com/api/character/{0}";
    public string characterDeleteUrl = "https://astrowheelapi.onrender.com/api/character/{0}";

    [Header("Inventory Controller")]
    public string inventoriesGetUrl = "https://astrowheelapi.onrender.com/api/Inventory";
    public string inventoryGetByIdUrl = "https://astrowheelapi.onrender.com/api/Inventory/{0}";
    public string inventoryPostUrl = "https://astrowheelapi.onrender.com/api/Inventory";
    public string inventoryPutUrl = "https://astrowheelapi.onrender.com/api/Inventory/{0}";
    public string inventoryDeleteUrl = "https://astrowheelapi.onrender.com/api/Inventory/{0}";

    [Header("InventoryMaterial Controller")]
    public string inventoryMaterialsGetUrl = "https://astrowheelapi.onrender.com/api/inventoryMaterials";
    public string inventoryMaterialGetByIdUrl = "https://astrowheelapi.onrender.com/api/inventoryMaterials/{0}/{1}";
    public string inventoryMaterialPostUrl = "https://astrowheelapi.onrender.com/api/inventoryMaterials";
    public string inventoryMaterialPutUrl = "https://astrowheelapi.onrender.com/api/inventoryMaterials/{0}/{1}";
    public string inventoryMaterialDeleteUrl = "https://astrowheelapi.onrender.com/api/inventoryMaterials/{0}/{1}";

    [Header("Material Controller")]
    public string materialsGetUrl = "https://astrowheelapi.onrender.com/api/Material";
    public string materialGetByIdUrl = "https://astrowheelapi.onrender.com/api/Material/{0}";
    public string materialPostUrl = "https://astrowheelapi.onrender.com/api/Material";
    public string materialPutUrl = "https://astrowheelapi.onrender.com/api/Material/{0}";
    public string materialDeleteUrl = "https://astrowheelapi.onrender.com/api/Material/{0}";

    [Header("TotalScore Controller")]
    public string totalScoreGetByPlayerIdUrl = "https://astrowheelapi.onrender.com/api/TotalScore/{0}";
}