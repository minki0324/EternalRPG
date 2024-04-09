using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    [SerializeField] private GameObject dropItemObject;
    [SerializeField] private Transform dropItemTransform;
    [Space(10)]
    [SerializeField] private TMP_Text playerHPText;
    [SerializeField] private Slider playerHPSlider;

    [Header("Effect")]
    [SerializeField] private RuntimeAnimatorController[] weaponAnimators;
    [SerializeField] private RuntimeAnimatorController runtimeEffectAnimator;
    [SerializeField] private Animator effectAnimator;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private Animator damageAnimator;
    [SerializeField] private TMP_Text monsterDamageText;
    private bool isCritical = false;

    public float tempWinRate = 0;

    private void OnEnable()
    {
        InitData();
    }

    private void OnDisable()
    {
        PrintLog.Instance.BattleLogClear();
    }

    public void InitData()
    {
        // ============================ 몬스터 데이터 입력
        result.mon = mon;
        monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
        monsterNameText.text = mon.monsterData.MonsterName;
        monsterLevelText.text = $"레벨 {mon.monsterData.MonsterLevel:N0} - {mon.monsterData.MonsterType}";
        monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;
        monsterImage.sprite = mon.monsterData.MonsterSprite;

        // 드롭 아이템 출력
        ItemDropRate(mon);

        // 승산 텍스트 출력
        CalculrateWinRate(mon);

        playerHPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP}";
        playerHPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
        PrintLog.Instance.BattleLog("하단의 버튼을 눌러 행동을 정해주세요.");
    }

    public void BattleButton()
    {
        if (GameManager.Instance.PlayerCurHP == 0)
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
        for (int i = 0; i < dropItemTransform.childCount; i++)
        {
            Destroy(dropItemTransform.GetChild(i).gameObject);
        }
        activeCanvas.gameObject.SetActive(false);
        activeCanvas.versusPanel.SetActive(false);
    }

    private IEnumerator StartBattle_Co()
    {
        Time.timeScale = GameManager.Instance.BattleSpeed;

        while (true)
        {
            int comboCount = ComboCount(GameManager.Instance.ComboPercent, mon.monsterData.ComboResist);
            for (int i = 1; i < comboCount + 1; i++)
            {
                PlayerTurn(i);

                if (mon.MonsterCurHP <= 0)
                {
                    break; // 몬스터의 체력이 0 이하이면 for 루프를 빠져나옴
                }
                yield return battleDelay;
                playerHPText.color = Color.white;
            }
            if (mon.MonsterCurHP <= 0)
            {
                break; // 몬스터의 체력이 0 이하이면 while 루프를 빠져나옴
            }

            comboCount = ComboCount(mon.monsterData.ComboPercent, GameManager.Instance.ComboResist);
            for (int i = 1; i < comboCount + 1; i++)
            {
                monsterTurn(i);

                if (GameManager.Instance.PlayerCurHP <= 0)
                {
                    break; // 플레이어의 체력이 0이하라면 for 루프를 빠져나옴
                }

                yield return battleDelay;
                playerHPText.color = new Color(1f, 1f, 1f); // 흰색
                monsterDamageText.gameObject.SetActive(false);
            }
            if (GameManager.Instance.PlayerCurHP <= 0)
            {
                break; // 플레이어의 체력이 0이하라면 while 루프를 빠져나옴
            }
        }
        for(int i = 0; i < dropItemTransform.childCount; i++)
        {
            Destroy(dropItemTransform.GetChild(i).gameObject);
        }

        playerHPText.color = Color.white;
        monsterHPText.color = Color.white;
        result.gameObject.SetActive(true);
        ignoreRay.SetActive(false);
        Time.timeScale = 1f;
    }

    private void PlayerTurn(int comboCount)
    {
        if (Avoid(mon.monsterData.AvoidPercent, GameManager.Instance.AvoidResist))
        { // 회피 성공했을 때
            damageText.text = $"회피 !";
            damageText.color = Color.cyan;
            PrintLog.Instance.BattleLog($"몬스터의 회피 !");
        }
        else
        {
            PlayerAttack(comboCount);
            effectAnimator.SetTrigger(GetTrigger(comboCount));
        }

        if (mon.MonsterCurHP <= 0)
        { // 승리
            result.isWin = true;
            GameManager.Instance.CurrentEnergy -= mon.monsterData.RequireEnergy;
        }
    }

    private void monsterTurn(int comboCount)
    {
        if (Avoid(GameManager.Instance.AvoidPercent, mon.monsterData.AvoidResist))
        { // 회피 성공했을 때
            PrintLog.Instance.BattleLog($"플레이어의 회피 !");
        }
        else
        {
            MonsterAttack(comboCount);
            playerHPText.color = new Color(1f, 0f, 0f); // 빨간색
        }

        if (GameManager.Instance.PlayerCurHP <= 0)
        { // 패배
            result.isWin = false;
            GameManager.Instance.CurrentEnergy -= mon.monsterData.RequireEnergy * 2;
        }
    }

    private void PlayerAttack(int _comboCount)
    {
        int damage = DamageCalculate(GameManager.Instance.PlayerATK, GameManager.Instance.CriticalPercant, GameManager.Instance.CriticalDamage, mon.monsterData.CriticalResist, mon.monsterData.MonsterDef);
        int drainAmount = 0;

        // 크리티컬 확인
        if (isCritical)
        {
            damageText.text = $"치명타 !\n{damage:N0} !";
            damageText.color = Color.yellow;
        }
        else
        {
            damageText.text = $"{damage:N0} !";
            damageText.color = Color.white;
        }
        mon.MonsterCurHP = Mathf.Max(0, mon.MonsterCurHP - damage);

        // 흡혈 확인
        drainAmount = Drain(GameManager.Instance.DrainPercent, mon.monsterData.DrainResist, damage, GameManager.Instance.DrainAmount);
        if (drainAmount != 0)
        {
            PrintLog.Instance.BattleLog($"플레이어의 흡혈 {drainAmount:N0}만큼 체력을 회복합니다.");
            GameManager.Instance.PlayerCurHP = Mathf.Min(GameManager.Instance.PlayerMaxHP, GameManager.Instance.PlayerCurHP + drainAmount);
            playerHPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP:N0}";
            playerHPText.color = Color.green;
            playerHPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
        }

        // 콤보 관련 텍스트 띄워주기
        if (_comboCount != 1)
        {
            PrintLog.Instance.BattleLog($"{_comboCount}연격 ! {mon.monsterData.MonsterName}에게 {damage:N0}의 피해를 입혔습니다.");
        }
        else
        {
            PrintLog.Instance.BattleLog($"{mon.monsterData.MonsterName}에게 {damage:N0}의 피해를 입혔습니다.");
        }

        // 몬스터 체력바 관리
        monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
        monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;

        // 데미지 이펙트 
        runtimeEffectAnimator = GetAnimator();
        effectAnimator.runtimeAnimatorController = runtimeEffectAnimator;
        damageAnimator.SetTrigger("Damage");
    }

    private RuntimeAnimatorController GetAnimator()
    {
        RuntimeAnimatorController con;
        switch (GameManager.Instance.WeaponData.WeaponType)
        {
            case WeaponType.Punch:
                con = weaponAnimators[0];
                return con;
            case WeaponType.Sword:
                con = weaponAnimators[1];
                return con;
            default:
                return null;
        }
    }

    private void MonsterAttack(int _comboCount)
    {
        int damage = DamageCalculate(mon.MonsterATK, mon.monsterData.CriticalPercant, mon.monsterData.CriticalDamage, GameManager.Instance.CriticalResist, GameManager.Instance.PlayerDef);
        monsterDamageText.gameObject.SetActive(true);
        monsterDamageText.text = $"-{damage:N0}";
        monsterDamageText.gameObject.GetComponent<Animator>().SetTrigger("Hit");
        int drainAmount = 0;
        GameManager.Instance.PlayerCurHP = Mathf.Max(0, GameManager.Instance.PlayerCurHP - damage);
        drainAmount = Drain(mon.monsterData.DrainPercent, GameManager.Instance.DrainResist, damage, mon.monsterData.DrainAmount);
        if (drainAmount != 0)
        { // 흡혈 성공 텍스트 띄워줘야함 todo
            mon.MonsterCurHP = Mathf.Min(mon.MonsterMaxHP, mon.MonsterCurHP + drainAmount);
            PrintLog.Instance.BattleLog($"몬스터의 흡혈 {drainAmount:N0}만큼 몬스터의 체력이 회복됩니다.");
            // 몬스터 체력바 관리
            monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
            monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;
        }

        if (_comboCount != 1)
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

        damage = Mathf.Max(0, damage - _EnemyDef);

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

    private int Drain(int _ObjDrainPercent, int _EnemyDrainResist, int damage, float ObjDrainAmount)
    {
        int adjustedDrainPercent = _ObjDrainPercent - _EnemyDrainResist;
        float randomValue = Random.Range(0f, 100f);
        int drain = 0;

        if (adjustedDrainPercent > 100 || adjustedDrainPercent >= randomValue)
        {
            drain = Mathf.RoundToInt(damage * ObjDrainAmount);

        }
        return drain;
    }

    private bool Avoid(int _ObjAvoidChance, int _EnemyAvoidResist)
    {
        int adjustedAvoidPercent = _ObjAvoidChance - _EnemyAvoidResist;
        float randomValue = Random.Range(0f, 100f);

        if (adjustedAvoidPercent > 100 || adjustedAvoidPercent >= randomValue)
        { // 회피 성공
            return true;
        }
        else
        {
            return false;
        }
    }

    private string GetTrigger(int comboCount)
    {
        switch (comboCount)
        {
            case 1: return "Single";
            case 2: return "Double";
            case 3: return "Tripple";
            case 4: return "Four";
            case 5: return "Five";
            default: return "Six";
        }
    }

    private void CalculrateWinRate(Monster mon)
    {
        string winRateString = string.Empty;
        float PlayerWinPercent = 0f;
        int playerPower = GameManager.Instance.Power;
        int monsterPower = mon.MonsterPower;

        if(playerPower >= monsterPower*2)
        { // 전투력 2배 이상이기 때문에 승리
            PlayerWinPercent = 100f;
        }
        else if(monsterPower >= playerPower*2)
        { // 몬스터 전투력이 2배 이상이기 때문에 패배
            PlayerWinPercent = 0f;
        }
        else
        {
            float tempPower = (float)playerPower - (float)monsterPower;
            if(tempPower >= 0)
            { // 플레이어가 더 강하거나 같은 경우
                PlayerWinPercent = 100 - (((float)monsterPower - ((float)playerPower / 2)) / ((float)playerPower / 200) / 2);
            }
            else
            { // 몬스터가 더 강한 경우
                PlayerWinPercent = 50 - (((float)monsterPower - (float)playerPower) / (((float)playerPower * 2) / 100));
            }
        }

        if (PlayerWinPercent <= 0)
        {
            oddsText.text = "불가능에 가까운 상대입니다.";
            oddsText.color = Color.red;
        }
        else if (20 > PlayerWinPercent && PlayerWinPercent > 0)
        {
            oddsText.text = "상대하기 어려운 상대입니다.";
            oddsText.color = Color.magenta;
        }
        else if (40 > PlayerWinPercent && PlayerWinPercent > 20)
        {
            oddsText.text = "제압해볼 수 있는 상대입니다.";
            oddsText.color = Color.gray;
        }
        else if (60 > PlayerWinPercent && PlayerWinPercent > 40)
        {
            oddsText.text = "나와 비슷한 상대입니다.";
            oddsText.color = Color.blue;
        }
        else if (80 > PlayerWinPercent && PlayerWinPercent > 60)
        {
            oddsText.text = "비교적 손쉬운 상대입니다.";
            oddsText.color = Color.cyan;
        }
        else
        {
            oddsText.text = "압도할 수 있는 상대입니다.";
            oddsText.color = Color.green;
        }
    }

    private void ItemDropRate(Monster mon)
    {
        for (int i = 0; i < mon.monsterData.RewardItem.Length; i++)
        {
            // 오브젝트 생성
            GameObject item = Instantiate(dropItemObject);
            item.transform.SetParent(dropItemTransform);

            // 오브젝트의 이미지 설정
            DropItem drop = item.GetComponent<DropItem>();
            drop.DropItemImage.sprite = EquipmentManager.Instance.GetEquipmentSprite(mon.monsterData.RewardItem[i]);

            // 아이템 드랍율 표기
            Dictionary<int, int> owndictionary = DataManager.Instance.GetOwnDictionary(mon.monsterData.RewardItem[i]);
            int ownCount = owndictionary.Values.Sum() == 0 ? 1 : owndictionary.Values.Sum();
            float quickSlotDrop = GameManager.Instance.isClover ? 70 : 0;
            float addDropRate = (float)(mon.monsterData.RewardItem[i].DropRate * (1 + (float)(GameManager.Instance.ItemDropRate / 100) + (float)(quickSlotDrop / 100)));
            float dropRate = (float)((mon.monsterData.RewardItem[i].DropRate + addDropRate) / ownCount);
            drop.DropRateText.text = $"{dropRate:F2}%";
        }
    }
}