using UnityEngine;

[CreateAssetMenu(fileName = "Ring Data", menuName = "Scriptable Object/Ring Data", order = 0)]
public class RingData : EquipmentBaseData
{
    [Header("반지")]
    [Space(10)] // 기본 정보
    public int RingATK;
    public int RingATKPercent;
    public int RingDef;
    public int RingDefPercent;
    [Space(10)] // 확률 정보
    public int RingCriticalPercent;
    public int RingCriticalResist;
    public int RingDrainPercent;
    public int RingDrainResist;
    [Space(10)] // 추가 정보
    public int RingEXPPercent;
    public int RingGoldPercent;
    public int RingSTRPercent;
    public int RingLUCPercent;
    public int RingDEXPercent;
}
