using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public bool isDead = false;
    public Animator animator;
    public MonsterData monsterData;
    public int MonsterMaxHP = 0;
    public int MonsterCurHP = 0;
    public int MonsterATK = 0;
    public int MonsterPower = 0;
    public SpriteRenderer[] sprites;
    public ReviveMonster reviveMonster;

    private void Awake()
    {
        InitData();
    }

    public void InitData()
    {
        MonsterMaxHP = monsterData.MonsterMaxHP;
        MonsterCurHP = monsterData.MonsterCurHP;
        MonsterATK = monsterData.MonsterATK;
        RenewMonsterPower();
    }

    public void RenewMonsterPower()
    {
        /*
         전투력 공식
        전투력 1당 수치들
        1. 공격력 / 20
        2. 체력 / 200
        3. 방어력 / 20
        4. 콤보 확률 / 3, 콤보 저항 / 2
        5. 크리 확률 / 2, 크리 저항 / 2, 크리 데미지 * 10
        6. 회피 확률, 회피 저항
        7. 흡혈 확률 / 3, 흡혈 저항 / 2, 흡혈 * 5
         */
        int power = 0;
        power += Mathf.RoundToInt(MonsterATK / 20);
        power += Mathf.RoundToInt(MonsterMaxHP / 200);
        power += Mathf.RoundToInt(monsterData.MonsterDef / 20);
        power += Mathf.RoundToInt(monsterData.ComboPercent / 3);
        power += Mathf.RoundToInt(monsterData.ComboResist / 2);
        power += Mathf.RoundToInt(monsterData.CriticalPercant / 2);
        power += Mathf.RoundToInt(monsterData.CriticalResist / 2);
        power += Mathf.RoundToInt(monsterData.CriticalDamage * 10);
        power += Mathf.RoundToInt(monsterData.AvoidPercent);
        power += Mathf.RoundToInt(monsterData.AvoidResist);
        power += Mathf.RoundToInt(monsterData.DrainPercent / 3);
        power += Mathf.RoundToInt(monsterData.DrainResist / 2);
        power += Mathf.RoundToInt(monsterData.DrainAmount * 5);

        MonsterPower = power;
    }
}
