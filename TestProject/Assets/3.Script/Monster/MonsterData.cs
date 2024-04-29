using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    [Header("기본 데이터")]
    public string MonsterName;
    public string MonsterLocation;
    public int MonsterID;
    public string MonsterType;
    public int MonsterLevel;
    public int MonsterMaxHP;
    public int MonsterCurHP;
    public int MonsterDef;
    public int MonsterATK;
    public Sprite MonsterSprite;
    public Vector2 ReturnPos;
    public int RequireEnergy;
    public bool isElite;
    public int EliteID;

    [Header("확률")]
    [Space(10)] // 크리티컬
    public int CriticalPercant;
    public int CriticalResist;
    public float CriticalDamage;
    [Space(10)] // 연타
    public int ComboPercent;
    public int ComboResist;
    [Space(10)] // 회피
    public int AvoidPercent;
    public int AvoidResist;
    [Space(10)] // 흡혈
    public int DrainPercent;
    public int DrainResist;
    public float DrainAmount;

    [Header("Reword")]
    public long RewordEXP;
    public long RewordGold;
    public int RewordEnergy;
    public EquipmentBaseData[] RewardItem;
    public RuneData RewardRune;
}
