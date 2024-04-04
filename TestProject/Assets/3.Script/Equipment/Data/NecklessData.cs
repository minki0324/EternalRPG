using UnityEngine;

[CreateAssetMenu(fileName = "Neckless Data", menuName = "Scriptable Object/Neckless Data", order = 0)]
public class NecklessData : EquipmentBaseData
{
    [Header("�����")]
    [Space(10)] // �⺻ ����
    public int NecklessHP;
    public int NecklessHPPercent;
    [Space(10)] // Ȯ�� ����
    public int NecklessComboPercent;
    public int NecklessAvoidPercent;
    [Space(10)] // �߰� ����
    public int NecklessEXPPercent;
    public int NecklessGoldPercent;
    public int NecklessDEXPercent;
    public int NecklessLUCPercent;
}
