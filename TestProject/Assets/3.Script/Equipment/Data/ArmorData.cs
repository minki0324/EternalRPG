using UnityEngine;

[CreateAssetMenu(fileName = "Armor Data", menuName = "Scriptable Object/Armor Data", order = 0)]
public class ArmorData : EquipmentBaseData
{
    [Header("갑옷")]
    [Space(10)] // 기본 정보
    public int ArmorHP;
    public int ArmorHPPercent;
    public int ArmorDef;
    public int ArmorDefPercent;
    [Space(10)] // 확률 정보
    public int ArmorComboResist;
    public int ArmorCriticalResist;
    public int ArmorDrainResist;
    [Space(10)] // 추가 정보
    public int ArmorVITPercent;
}
