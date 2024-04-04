using UnityEngine;

[CreateAssetMenu(fileName = "Belt Data", menuName = "Scriptable Object/Belt Data", order = 0)]
public class BeltData : EquipmentBaseData
{
    [Header("벨트")]
    [Space(10)] // 기본 정보
    public int BeltHP;
    public int BeltHPPercent;
    [Space(10)] // 확률 정보
    public int BeltAvoidPercent;
    [Space(10)] // 추가 정보
    public int BeltEXPPercent;
    public int BeltLUCPercent;
    public int BeltVITPercent;
}
