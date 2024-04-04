using UnityEngine;

[CreateAssetMenu(fileName = "Pants Data", menuName = "Scriptable Object/Pants Data", order = 0)]
public class PantsData : EquipmentBaseData
{
    [Header("각반")]
    [Space(10)] // 기본 정보
    public int PantsHP;
    public int PantsHPPercent;
    public int PantsDef;
    public int PantsDefPercent;
    [Space(10)] // 확률 정보
    public int PantsComboResist;
    public int PantsCriticalResist;
    [Space(10)] // 추가 정보
    public int PantsVITPercent;
}
