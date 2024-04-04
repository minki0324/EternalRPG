using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    [Header("�⺻ ������")]
    public string MonsterName;
    public string MonsterType;
    public int MonsterLevel;
    public int MonsterMaxHP;
    public int MonsterCurHP;
    public int MonsterDef;
    public int MonsterATK;
    public Sprite MonsterSprite;
    public Vector2 ReturnPos;
    public int RequireEnergy;


    [Header("Ȯ��")]
    // ũ��Ƽ��
    public float CriticalPercant = 5f;
    public float CriticalResist = 5f;
    public float CriticalDamage = 1.2f;
    // ��Ÿ
    public float ComboPercent = 5f;
    public float ComboResist = 5f;

    [Header("Reword")]
    public float RewordEXP;
    public float RewordGold;
}
