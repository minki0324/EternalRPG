using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Object/Weapon Data", order = 0)]
public   class WeaponData : EquipmentBaseData
{
    [Header("무기")]
    [Space(10)] // 기본 정보
    public RuntimeAnimatorController animator;
    public int WeaponATK;
    public int WeaponATKPercent;
    [Space(10)] // 확률 정보
    public int WeaponComboPercent;
    public int WeaponCriticalPercent;
    public float WeaponCriticalDamage;
    public int WeaponDrainPercent;
    public float WeaponDrainAmount;
    [Space(10)] // 추가 정보
    public int WeaponSTRPercent;
    public int WeaponDEXPercent;
}
