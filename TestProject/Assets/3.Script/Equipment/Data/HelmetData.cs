using UnityEngine;

[CreateAssetMenu(fileName = "Helmet Data", menuName = "Scriptable Object/Helmet Data", order = 0)]
public class HelmetData : EquipmentBaseData
{
    [Header("����")]
    [Space(10)] // �⺻ ����
    public int HelmetHP;
    public int HelmetHPPercent;
    public int HelmetDef;
    public int HelmetDefPercent;
    [Space(10)] // Ȯ�� ����
    public int HelmetAvoidPercent;
    public int HelmetDrainResist;
    [Space(10)] // �߰� ����
    public int HelmetSTRPercent;
    public int HelmetVITPercent;
}
