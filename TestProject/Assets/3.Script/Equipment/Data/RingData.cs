using UnityEngine;

[CreateAssetMenu(fileName = "Ring Data", menuName = "Scriptable Object/Ring Data", order = 0)]
public class RingData : EquipmentBaseData
{
    [Header("����")]
    [Space(10)] // �⺻ ����
    public int RingATK;
    public int RingATKPercent;
    public int RingDef;
    public int RingDefPercent;
    [Space(10)] // Ȯ�� ����
    public int RingCriticalPercent;
    public int RingCriticalResist;
    public int RingDrainPercent;
    public int RingDrainResist;
    [Space(10)] // �߰� ����
    public int RingEXPPercent;
    public int RingGoldPercent;
    public int RingSTRPercent;
    public int RingLUCPercent;
    public int RingDEXPercent;
}
