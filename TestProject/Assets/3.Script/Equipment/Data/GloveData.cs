using UnityEngine;

[CreateAssetMenu(fileName = "Glove Data", menuName = "Scriptable Object/Glove Data", order = 0)]
public class GloveData : EquipmentBaseData
{
    [Header("�尩")]
    [Space(10)] // �⺻ ����
    public int GloveATK;
    public int GloveATKPercent;
    public int GloveHP;
    public int GloveHPPercent;
    [Space(10)] // Ȯ�� ����
    public int GloveComboPercent;
    public int GloveCriticalPercent;
    public float GloveCriticalDamage;
    [Space(10)] // �߰� ����
    public int GloveSTRPercent;
    public int GloveDEXPercent;
}
