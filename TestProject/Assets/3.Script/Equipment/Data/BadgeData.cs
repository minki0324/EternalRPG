using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum MasterBadge
{
    Bronze,
    Cooper,
    Silver,
    Gold,
    Platinum,
    Unique,
    Legendary,
    Lunatic,
    Eternal
}

[CreateAssetMenu(fileName = "Badge Data", menuName = "Scriptable Object/Badge Data", order = 0)]
public class BadgeData : ScriptableObject
{
    public string BadgeSprite;
    public string BadgeName;
    public int NextGrade;
    [Space(10)]
    public int BadgeATKPercent;
    [Space(10)]
    public int BadgeMoveSpeed = 2;
    public int BadgeBonusEnergy;
    public int BadgeBonusAP;
    [Space(10)]
    public int BadgeSTRPercent = 1;
    public int BadgeDEXPercent = 1;
    public int BadgeLUCPercent = 1;
    public int BadgeVITPercent = 1;
    [Space(10)]
    public int BadgeAvoidResist = 5;
    [Space(10)]
    public int BadgeEXPPercent = 5;
    public int BadgeGoldPercent = 5;
    public int BadgeItemDropRate;
    public float BadgeRuneDrop;
}
