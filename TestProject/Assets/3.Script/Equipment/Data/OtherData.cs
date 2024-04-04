using UnityEngine;

[CreateAssetMenu(fileName = "Other Data", menuName = "Scriptable Object/Other Data", order = 0)]
public class OtherData : EquipmentBaseData
{
    [Header("기타")]
    [Space(10)] // 기본 정보
    public int OtherATK;
    public int OtherATKPercent;
    public int OtherHP;
    public int OtherHPPercent;
    public int OtherDef;
    public int OtherDefPercent;
    [Space(10)] // 확률 정보
    public int OtherCriticalPercent;
    public int OtherCriticalResist;
    public float OtherCriticalDamage;
    public int OtherComboPercent;
    public int OtherComboResist;
    public int OtherAvoidPercent;
    public int OtherAvoidResist;
    public int OtherDrainPercent;
    public int OtherDrainResist;
    public float OtherDrainAmount;

    [Space(10)] // 추가 정보
    public int OtherEXPPercent;
    public int OtherGoldPercent;
    public int OtherSTRPercent;
    public int OtherDEXPercent;
    public int OtherLUCPercent;
    public int OtherVITPercent;
}
