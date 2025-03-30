using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
using UnityEngine.Events;

public enum EItemID
{
    Stone = 0,
    Wood,
    Iron,
    Battery,
    Rubber,
    Drug,

    Gun = 100,
    StoneSpear,
    IronSpear,
    Flashbang,
    WoodSpear,

    HighPressureShut = 200,
    Flash,
    Web,
    OmnidirectionalFlash,
    NormalPressureShut,
}
[Flags]
public enum EItemType
{
    Resource = 1,
    Useable = 2,
    HeadArmor = 4,
    BodyArmor = 8,
    FootArmor = 16,
    Consumable = 32,
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "SO/Item/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string itemDescription;
    public Sprite itemSprite;
    public EItemID itemID;
    public EItemType itemType;
    public int maxOverlapAmount = 1;
    public int durability = 1;
    public UnityEvent onConsume;
    
    [Header("Recipe")]
    public bool isCraftable;
    public SerializedDictionary<ItemSO, int> recipeDict = new SerializedDictionary<ItemSO, int>();

    [Header("Equip")]
    public string className;
    public EquipAction equipAction;

    private void OnEnable()
    {
        if (className == "") return;
        try
        {
            Type type = Type.GetType(className);
            equipAction = Activator.CreateInstance(type) as EquipAction;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
}