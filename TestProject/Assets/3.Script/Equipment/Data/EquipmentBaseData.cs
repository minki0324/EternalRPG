using UnityEngine;

public enum Category
{
    Weapon,
    Armor,
    Helmet,
    Pants,
    Glove,
    Shoes,
    Belt,
    ShoulderArmor,
    Ring,
    Neckless,
    Clock,
    Other
}

public class EquipmentBaseData : ScriptableObject
{
    [Header("기본 정보")]
    public Category EquipmentType;
    public Sprite EquipmentSprite;
    public int ItemID;
    public int OwnCount;
    public string EquipmentName;
    public int RequireCost;
    public bool isCanBuy;
}