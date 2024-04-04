using UnityEngine;

[CreateAssetMenu(fileName = "Glove Data", menuName = "Scriptable Object/Glove Data", order = 0)]
public class GloveData : EquipmentBaseData
{
    [Header("장갑")]
    [Space(10)] // 기본 정보
    public int GloveATK;
    public int GloveATKPercent;
    public int GloveHP;
    public int GloveHPPercent;
    [Space(10)] // 확률 정보
    public int GloveComboPercent;
    public int GloveCriticalPercent;
    public float GloveCriticalDamage;
    [Space(10)] // 추가 정보
    public int GloveSTRPercent;
    public int GloveDEXPercent;
}
