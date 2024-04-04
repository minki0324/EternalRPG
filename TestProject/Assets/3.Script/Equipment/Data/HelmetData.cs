using UnityEngine;

[CreateAssetMenu(fileName = "Helmet Data", menuName = "Scriptable Object/Helmet Data", order = 0)]
public class HelmetData : EquipmentBaseData
{
    [Header("투구")]
    [Space(10)] // 기본 정보
    public int HelmetHP;
    public int HelmetHPPercent;
    public int HelmetDef;
    public int HelmetDefPercent;
    [Space(10)] // 확률 정보
    public int HelmetAvoidPercent;
    public int HelmetDrainResist;
    [Space(10)] // 추가 정보
    public int HelmetSTRPercent;
    public int HelmetVITPercent;
}
