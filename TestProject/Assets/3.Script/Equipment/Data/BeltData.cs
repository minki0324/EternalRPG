using UnityEngine;

[CreateAssetMenu(fileName = "Belt Data", menuName = "Scriptable Object/Belt Data", order = 0)]
public class BeltData : EquipmentBaseData
{
    [Header("��Ʈ")]
    [Space(10)] // �⺻ ����
    public int BeltHP;
    public int BeltHPPercent;
    [Space(10)] // Ȯ�� ����
    public int BeltAvoidPercent;
    [Space(10)] // �߰� ����
    public int BeltEXPPercent;
    public int BeltLUCPercent;
    public int BeltVITPercent;
}
