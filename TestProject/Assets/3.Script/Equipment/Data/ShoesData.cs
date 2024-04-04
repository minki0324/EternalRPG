using UnityEngine;

[CreateAssetMenu(fileName = "Shoes Data", menuName = "Scriptable Object/Shoes Data", order = 0)]
public class ShoesData : EquipmentBaseData
{
    [Header("신발")]
    [Space(10)] // 기본 정보
    public int ShoesHP;
    public int ShoesHPPercent;
    public int ShoesDef;
    public int ShoesDefPercent;
    [Space(10)] // 확률 정보
    public int ShoesAvoidPercent;
    public int ShoesAvoidResist;
    [Space(10)] // 추가 정보
    public int ShoesDEXPercent;
    public int ShoesVITPercent;
}
