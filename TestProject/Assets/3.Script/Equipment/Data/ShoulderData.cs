using UnityEngine;

[CreateAssetMenu(fileName = "Shoulder Data", menuName = "Scriptable Object/Shoulder Data", order = 0)]
public class ShoulderData : EquipmentBaseData
{
    [Header("견장")]
    [Space(10)] // 기본 정보
    public int ShoulderDef;
    public int ShoulderDefPercent;
    [Space(10)] // 확률 정보
    public int ShoulderCriticalResist;
    public int ShoulderDrainResist;
    [Space(10)] // 추가 정보
    public int ShoulderDEXPercent;
    public int ShoulderVITPercent;
}
