using UnityEngine;

[CreateAssetMenu(fileName = "Shoulder Data", menuName = "Scriptable Object/Shoulder Data", order = 0)]
public class ShoulderData : EquipmentBaseData
{
    [Header("����")]
    [Space(10)] // �⺻ ����
    public int ShoulderDef;
    public int ShoulderDefPercent;
    [Space(10)] // Ȯ�� ����
    public int ShoulderCriticalResist;
    public int ShoulderDrainResist;
    [Space(10)] // �߰� ����
    public int ShoulderDEXPercent;
    public int ShoulderVITPercent;
}
