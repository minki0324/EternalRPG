using UnityEngine;

[CreateAssetMenu(fileName = "Shoes Data", menuName = "Scriptable Object/Shoes Data", order = 0)]
public class ShoesData : EquipmentBaseData
{
    [Header("�Ź�")]
    [Space(10)] // �⺻ ����
    public int ShoesHP;
    public int ShoesHPPercent;
    public int ShoesDef;
    public int ShoesDefPercent;
    [Space(10)] // Ȯ�� ����
    public int ShoesAvoidPercent;
    public int ShoesAvoidResist;
    [Space(10)] // �߰� ����
    public int ShoesDEXPercent;
    public int ShoesVITPercent;
}
