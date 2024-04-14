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
        1. ���ݷ� / 50
        2. ü�� / 200
        3. ���� / 10
        4. �޺� Ȯ�� / 3, �޺� ���� / 2
        5. ũ�� Ȯ�� / 2, ũ�� ���� / 2, ũ�� ������ * 10
        6. ȸ�� Ȯ��, ȸ�� ����
        7. ���� Ȯ�� / 3, ���� ���� / 2, ���� * 5
         */
        int power = 0;
        power += Mathf.RoundToInt((float)MonsterATK / 10f);
        power += Mathf.RoundToInt((float)MonsterMaxHP / 10f);
        power += Mathf.RoundToInt((float)monsterData.MonsterDef / 2f);
        power += Mathf.RoundToInt((float)monsterData.ComboPercent / 3f);
        power += Mathf.RoundToInt((float)monsterData.ComboResist / 2f);
        power += Mathf.RoundToInt((float)monsterData.CriticalPercant / 2f);
        power += Mathf.RoundToInt((float)monsterData.CriticalResist / 2f);
        power += Mathf.RoundToInt((float)monsterData.CriticalDamage * 10f);
        power += Mathf.RoundToInt((float)monsterData.AvoidPercent);
        power += Mathf.RoundToInt((float)monsterData.AvoidResist);
        power += Mathf.RoundToInt((float)monsterData.DrainPercent / 3f);
        power += Mathf.RoundToInt((float)monsterData.DrainResist / 2f);
        power += Mathf.RoundToInt((float)monsterData.DrainAmount * 5f);

        MonsterPower = Mathf.RoundToInt(power/1.9f);
    }
}
