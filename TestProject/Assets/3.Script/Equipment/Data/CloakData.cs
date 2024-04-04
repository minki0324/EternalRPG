using UnityEngine;

[CreateAssetMenu(fileName = "Cloak Data", menuName = "Scriptable Object/Cloak Data", order = 0)]
public class CloakData : EquipmentBaseData
{
    [Header("����")]
    [Space(10)] // �⺻ ����
    public int CloakHP;
    public int CloakHPPercent;
    public int CloakDef;
    public int CloakDefPercent;
    [Space(10)] // Ȯ�� ����
    public int CloakAvoidPercent;
    public int CloakComboResist;
    [Space(10)]
    public int CloakDEXPercent;
    public int CloakLUCPercent;
    public int CloakVITPercent;
}
