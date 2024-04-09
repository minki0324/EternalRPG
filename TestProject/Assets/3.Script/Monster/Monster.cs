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
         ������ ����
        ������ 1�� ��ġ��
        1. ���ݷ� / 20
        2. ü�� / 200
        3. ���� / 20
        4. �޺� Ȯ�� / 3, �޺� ���� / 2
        5. ũ�� Ȯ�� / 2, ũ�� ���� / 2, ũ�� ������ * 10
        6. ȸ�� Ȯ��, ȸ�� ����
        7. ���� Ȯ�� / 3, ���� ���� / 2, ���� * 5
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
