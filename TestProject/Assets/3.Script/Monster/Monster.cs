using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private TMP_Text levelTxt;

    private void Awake()
    {
        InitData();
    }

    public void InitData()
    {
        MonsterMaxHP = monsterData.MonsterMaxHP;
        MonsterCurHP = monsterData.MonsterCurHP;
        MonsterATK = monsterData.MonsterATK;
        if(monsterData.isElite)
        {
            levelTxt.text = $"Lv. {monsterData.MonsterLevel:N0}\n엘리트 몬스터" ;

        }
        else
        {
            levelTxt.text = $"Lv. {monsterData.MonsterLevel:N0}";
        }
        RenewMonsterPower();
        if(GameManager.Instance.DeadMonsterList.Contains(monsterData.MonsterID))
        { // 리스트에 담겨있으면 죽은 몹
            isDead = true;
            animator.enabled = false;
            reviveMonster.areaSprite.color = Color.gray;
            for (int i = 0; i < sprites.Length; i++)
            {
                // 몬스터의 스프라이트 알파값을 100으로 설정
                Color spriteColor = sprites[i].color;
                spriteColor.a = 0.4f; // 알파값을 0.5로 설정 (100% 투명도)
                sprites[i].color = spriteColor;
            }
        }
    }

    public void RenewMonsterPower()
    {
        /*
         전투력 공식
        전투력 1당 수치들
        1. 공격력 / 50
        2. 체력 / 200
        3. 방어력 / 10
        4. 콤보 확률 / 3, 콤보 저항 / 2
        5. 크리 확률 / 2, 크리 저항 / 2, 크리 데미지 * 10
        6. 회피 확률, 회피 저항
        7. 흡혈 확률 / 3, 흡혈 저항 / 2, 흡혈 * 5
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
