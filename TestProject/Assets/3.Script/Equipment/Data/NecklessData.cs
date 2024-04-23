using UnityEngine;

[CreateAssetMenu(fileName = "Neckless Data", menuName = "Scriptable Object/Neckless Data", order = 0)]
public class NecklessData : EquipmentBaseData
{
    [Header("목걸이")]
    [Space(10)] // 기본 정보
    public int NecklessHP;
    public int NecklessHPPercent;
    [Space(10)] // 확률 정보
    public int NecklessComboPercent;
    public int NecklessAvoidResist;
    [Space(10)] // 추가 정보
    public int NecklessEXPPercent;
    public int NecklessGoldPercent;
    public int NecklessDEXPercent;
    public int NecklessLUCPercent;
    public int NecklessItemDropRate;
}
