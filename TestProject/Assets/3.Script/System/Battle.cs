using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battle : MonoBehaviour
{
    [SerializeField] private ActiveCanvas activeCanvas = null;
    [SerializeField] private Player player = null;
    [SerializeField] private BattleResult result = null;
    [SerializeField] private GameObject ignoreRay = null;
    public Monster mon = null;
    private WaitForSeconds battleDelay = new WaitForSeconds(0.7f);

    [Header("UI")]
    [SerializeField] private TMP_Text monsterHPText;
    [SerializeField] private Slider monsterHPSlider;
    [SerializeField] private TMP_Text monsterNameText;
    [SerializeField] private TMP_Text monsterLevelText;
    [SerializeField] private TMP_Text oddsText;
    [SerializeField] private Image monsterImage;
    [Space(10)]
    [SerializeField] private TMP_Text playerHPText;
    [SerializeField] private Slider playerHPSlider;

    [Header("Effect")]
    [SerializeField] private RuntimeAnimatorController runtimeEffectAnimator;
    [SerializeField] private Animator effectAnimator;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private Animator damageAnimator;
    private bool isCritical = false;

    private void OnEnable()
    {
        InitData();
    }

    public void InitData()
    {
        result.mon = mon;
        monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
        monsterNameText.text = mon.monsterData.MonsterName;
        monsterLevelText.text = $"레벨 {mon.monsterData.MonsterLevel:N0} - {mon.monsterData.MonsterType}";
        monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;
        monsterImage.sprite = mon.monsterData.MonsterSprite;
        // todo 승산 텍스트 구현해야됨

        // ============================ 몬스터 데이터 입력
        playerHPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP}";
        playerHPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
        PrintLog.Instance.BattleLog("하단의 버튼을 눌러 행동을 정해주세요.");
    }

    public void BattleButton()
    {
        if(GameManager.Instance.PlayerCurHP == 0)
        { // todo 피통 0으로 전투 불가 메세지 출력
            return;
        }
        ignoreRay.SetActive(true);
        StartCoroutine(StartBattle_Co());
    }

    public void BackButton()
    {
        player.playerMove.isFight = false;
        // 전투 후에는 몬스터에 저장된 위치만큼 물러나서 시작
        player.gameObject.transform.position = new Vector2(mon.gameObject.transform.position.x + mon.monsterData.ReturnPos.x,
                                                                                                              mon.gameObject.transform.position.y + mon.monsterData.ReturnPos.y);
        activeCanvas.gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    private IEnumerator StartBattle_Co()
    {
        Time.timeScale = GameManager.Instance.BattleSpeed;
        
        while (true)
        {
            int comboCount = ComboCount(GameManager.Instance.ComboPercent, mon.monsterData.ComboResist);
            for (int i = 1; i < comboCount + 1; i++)
            {
                PlayerAttack(i);
                effectAnimator.SetTrigger(GetTrigger(i));

                if (mon.MonsterCurHP <= 0)
                { // 승리
                    result.isWin = true;
                    GameManager.Instance.Energy -= mon.monsterData.RequireEnergy;
                    // for문 이탈
                    break;
                }
                yield return battleDelay;
            }
            if (mon.MonsterCurHP <= 0)
            {
                break; // 몬스터의 체력이 0 이하이면 while 루프를 빠져나옴
            }

            comboCount = 1;
            comboCount = ComboCount(mon.monsterData.ComboPercent, GameManager.Instance.ComboResist);
            for (int i = 1; i < comboCount + 1; i++)
            {
                MonsterAttack(i);
                playerHPText.color = new Color(1f, 0f, 0f); // 빨간색
                if (GameManager.Instance.PlayerCurHP <= 0)
                { // 패배
                    result.isWin = false;
                    GameManager.Instance.Energy -= mon.monsterData.RequireEnergy * 2;
                    //for문 이탈
                    break;
                }
                yield return battleDelay;
                playerHPText.color = new Color(1f, 1f, 1f); // 흰색
            }
            if (GameManager.Instance.PlayerCurHP <= 0)
            {
                break; // 플레이어의 체력이 0이하라면 while 루프를 빠져나옴
            }
        }
        result.gameObject.SetActive(true);
        ignoreRay.SetActive(false);
        Time.timeScale = 1f;
    }

    private void PlayerAttack(int _comboCount)
    {
        int damage = DamageCalculate(GameManager.Instance.PlayerATK, GameManager.Instance.CriticalPercant, GameManager.Instance.CriticalDamage, mon.monsterData.CriticalResist, mon.monsterData.MonsterDef);
        if(isCritical)
        {
            damageText.text = $"치명타 !\n{damage:N0} !";
        }
        else
        {
            damageText.text = $"{damage:N0} !";
        }
        mon.MonsterCurHP = Mathf.Max(0, mon.MonsterCurHP - damage);
        if (_comboCount != 1)
        {
            PrintLog.Instance.BattleLog($"{_comboCount}연격 ! {mon.monsterData.MonsterName}에게 {damage:N0}의 피해를 입혔습니다.");
        }
        else
        {
            PrintLog.Instance.BattleLog($"{mon.monsterData.MonsterName}에게 {damage:N0}의 피해를 입혔습니다.");
        }
        monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
        monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;

        runtimeEffectAnimator = GameManager.Instance.WeaponData.animator;
        effectAnimator.runtimeAnimatorController = runtimeEffectAnimator;
        damageAnimator.SetTrigger("Damage");
    }

    private void MonsterAttack(int _comboCount)
    {
        int damage = DamageCalculate(mon.MonsterATK, mon.monsterData.CriticalPercant, mon.monsterData.CriticalDamage, GameManager.Instance.CriticalResist, GameManager.Instance.PlayerDef);
        GameManager.Instance.PlayerCurHP = Mathf.Max(0, GameManager.Instance.PlayerCurHP - damage);
        if(_comboCount != 1)
        {
            PrintLog.Instance.BattleLog($"몬스터의 {_comboCount}연격 ! 플레이어는 {damage:N0}의 피해를 입혔습니다.");
        }
        else
        {
            PrintLog.Instance.BattleLog($"플레이어는 {damage:N0}의 피해를 입혔습니다.");
        }
        playerHPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP:N0}";
        playerHPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
    }

    private int DamageCalculate(float _Objdamage, float _ObjCriPercent, float _ObjCriDamage, float _EnemyCriResist, int _EnemyDef)
    {
        float damage = Random.Range(_Objdamage * 0.95f, _Objdamage * 1.05f);
        float adjustedCriPercent = _ObjCriPercent - _EnemyCriResist;
        float randomValue = Random.Range(0f, 100f);

        if (adjustedCriPercent >= 100f || randomValue <= adjustedCriPercent)
        { // 크리티컬 보정한 값이 100%가 넘거나, 확률에 성공했을 때
            damage *= _ObjCriDamage;
            isCritical = true;
        }
        else
        {
            isCritical = false;
        }

        damage = damage - _EnemyDef;

        int roundedDamage = Mathf.RoundToInt(damage);

        return roundedDamage;
    }

    private int ComboCount(float _ObjComboPercent, float _EnemyComboResist)
    {
        // 콤보 저항을 고려하여 콤보 퍼센트를 조정합니다.
        float adjustedComboPercent = _ObjComboPercent - _EnemyComboResist;
        int comboCount = 1;

        // 콤보 저항을 뺀 확률이 100 초과라면 무조건 연타
        while (adjustedComboPercent >= 100f)
        {
            comboCount++;
            adjustedComboPercent -= 100f;
        }

        // 남은 확률에 따라 콤보 확률 계산
        float randomValue = Random.Range(0f, 100f);

        // 생성된 랜덤 값이 조정된 콤보 퍼센트보다 작은 경우
        if (randomValue <= adjustedComboPercent)
        {
            // 콤보 카운트를 1 증가.
            comboCount++;
        }

        return comboCount;
    }

    private string GetTrigger(int comboCount)
    {
        switch (comboCount)
        {
            case 1: return "Single";
            case 2:  return "Double";
            case 3: return "Tripple";
            case 4: return "Four";
            case 5: return "Five";
            default: return "Six";
        }
    }
}
