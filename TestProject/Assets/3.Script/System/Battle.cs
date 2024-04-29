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
    private int fakePlayerHP = 0;
    private int fakeMonsterHP = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text monsterHPText;
    [SerializeField] private Slider monsterHPSlider;
    [SerializeField] private TMP_Text monsterNameText;
    [SerializeField] private TMP_Text monsterLevelText;
    [SerializeField] private TMP_Text oddsText;
    [SerializeField] private Image monsterImage;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text playerHPText;
    [SerializeField] private Slider playerHPSlider;

    [Header("Drop Item")]
    [SerializeField] private GameObject dropItemObject;
    [SerializeField] private Transform dropItemTransform;
    [Space(10)]
    [SerializeField] private GameObject runeItemObject;

    [Header("Effect")]
    [SerializeField] private RuntimeAnimatorController[] weaponAnimators;
    [SerializeField] private RuntimeAnimatorController runtimeEffectAnimator;
    [SerializeField] private Animator effectAnimator;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private Animator damageAnimator;
    [SerializeField] private TMP_Text monsterDamageText;
    private bool isCritical = false;

    private int zeroCountCri = 0;
    private int zeroCountNormal = 0;

    public float tempWinRate = 0;
    private Coroutine battleCoroutine;
    [Header("룬")]
    private bool isDefenceRune = false;
    private int runeHPRegen;
    private bool isPoisonRune = false;

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
        monsterImage.SetNativeSize();

        // 룬 회복량 체크
        runeHPRegen = GameManager.Instance.RuneHashSet.Contains("재생의 룬") ? Mathf.RoundToInt(GameManager.Instance.PlayerMaxHP / 200f) : 0;

        // 드롭 아이템 출력
        ItemDropRate(mon);

        // 레어 아이템 출력
        if (mon.monsterData.isElite)
        {
            RuneDropRate(mon);
        }

        // 승산 텍스트 출력
        CalculrateWinRate(mon);

        // 룬 체크
        RuneCheck();

        if (DataManager.Instance.EliteMonsterDic.ContainsKey(mon.monsterData.MonsterID))
        {
            energyText.text = $"1";
        }
        else
        {
            energyText.text = $"{mon.monsterData.RequireEnergy}";
        }
        playerHPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP:N0}";
        playerHPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
        PrintLog.Instance.BattleLog("하단의 버튼을 눌러 행동을 정해주세요.");
    }

    public void BattleButton()
    {
        if (GameManager.Instance.PlayerCurHP == 0)
        {
            PrintLog.Instance.StaticLog("체력이 없어 싸울 수 없다.");
            return;
        }
        ignoreRay.SetActive(true);
        if (battleCoroutine != null)
        {
            StopCoroutine(battleCoroutine);
            battleCoroutine = null;
        }
        battleCoroutine = StartCoroutine(StartBattle_Co(false));
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

    public void SkipBattle(bool isSkip)
    {
        if (battleCoroutine != null)
        {
            StopCoroutine(battleCoroutine);
            battleCoroutine = null;
        }
        int battleCount = 0;

        while (true)
        {
            battleCount++;
            if(battleCount == 500)
            {
                DefeatBattle();
                break;
            }
            int comboCount = ComboCount(GameManager.Instance.ComboPercent, mon.monsterData.ComboResist);
            for (int i = 1; i < comboCount + 1; i++)
            {
                PlayerTurn(i, isSkip);

                if (mon.MonsterCurHP <= 0)
                {
                    break; // 몬스터의 체력이 0 이하이면 for 루프를 빠져나옴
                }
            }
            if (mon.MonsterCurHP <= 0)
            {
                break; // 몬스터의 체력이 0 이하이면 while 루프를 빠져나옴
            }

            comboCount = ComboCount(mon.monsterData.ComboPercent, GameManager.Instance.ComboResist);
            for (int i = 1; i < comboCount + 1; i++)
            {
                monsterTurn(i, isSkip);

                if (GameManager.Instance.PlayerCurHP <= 0)
                {
                    break; // 플레이어의 체력이 0이하라면 for 루프를 빠져나옴
                }

            }

            if (GameManager.Instance.PlayerCurHP <= 0)
            {
                break; // 플레이어의 체력이 0이하라면 while 루프를 빠져나옴
            }
        }
        Time.timeScale = 1f;
        ignoreRay.SetActive(false);
        for (int i = 0; i < dropItemTransform.childCount; i++)
        {
            Destroy(dropItemTransform.GetChild(i).gameObject);
        }

        playerHPText.color = Color.white;
        monsterHPText.color = Color.white;
        result.gameObject.SetActive(true);

        damageText.gameObject.SetActive(false);
        monsterDamageText.gameObject.SetActive(false);
        effectAnimator.gameObject.SetActive(false);
        damageAnimator.gameObject.SetActive(false);
    }

    private IEnumerator StartBattle_Co(bool isSkip)
    {
        Time.timeScale = GameManager.Instance.BattleSpeed;
        PoisonRune();

        damageText.gameObject.SetActive(true);
        effectAnimator.gameObject.SetActive(true);
        damageAnimator.gameObject.SetActive(true);

        while (true)
        {
            int maxCridamage = Mathf.RoundToInt(GameManager.Instance.PlayerATK * 1.05f * GameManager.Instance.CriticalDamage);
            if (mon.monsterData.MonsterDef > maxCridamage)
            { // 최대 크리데미지가 몬스터의 방어를 뚫지 못하는 경우
                PrintLog.Instance.StaticLog("혼신의 힘을 다해 일격을 날렸지만, 적의 방어를 뚫지 못한다.");
                DefeatBattle();
                break;
            }

            int comboCount = ComboCount(GameManager.Instance.ComboPercent, mon.monsterData.ComboResist);
            for (int i = 1; i < comboCount + 1; i++)
            {
                PlayerTurn(i, isSkip);

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
                monsterTurn(i, isSkip);

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
        Time.timeScale = 1f;
        ignoreRay.SetActive(false);
        for (int i = 0; i < dropItemTransform.childCount; i++)
        {
            Destroy(dropItemTransform.GetChild(i).gameObject);
        }

        playerHPText.color = Color.white;
        monsterHPText.color = Color.white;
        result.gameObject.SetActive(true);
        battleCoroutine = null;

        damageText.gameObject.SetActive(false);
        effectAnimator.gameObject.SetActive(false);
        damageAnimator.gameObject.SetActive(false);
    }

    #region 플레이어
    private void PlayerTurn(int comboCount, bool isSkip)
    {
        if (Avoid(mon.monsterData.AvoidPercent, GameManager.Instance.AvoidResist))
        { // 회피 성공했을 때
            if (!isSkip)
            {
                damageText.text = $"회피 !";
                damageText.color = Color.cyan;
                PrintLog.Instance.BattleLog($"몬스터의 회피 !");
            }
        }
        else
        {
            PlayerAttack(comboCount, isSkip);
            if (!isSkip)
            {
                effectAnimator.SetTrigger(GetTrigger(comboCount));
            }
            GameManager.Instance.PlayerCurHP += runeHPRegen;
        }

        if (mon.MonsterCurHP <= 0)
        { // 승리
            result.isWin = true;
            if (mon.monsterData.isElite)
            { // 엘리트 몬스터는 에너지 1만 차감
                GameManager.Instance.CurrentEnergy--;
            }
            else
            {
                GameManager.Instance.CurrentEnergy = Mathf.Max(0, GameManager.Instance.CurrentEnergy - mon.monsterData.RequireEnergy);
            }
        }
    }
    private void PlayerAttack(int _comboCount, bool isSkip)
    {
        int damage = DamageCalculate(GameManager.Instance.PlayerATK, GameManager.Instance.CriticalPercant, GameManager.Instance.CriticalDamage, mon.monsterData.CriticalResist, mon.monsterData.MonsterDef);
        int drainAmount = 0;

        // 크리티컬 확인
        if (!isSkip)
        {
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
        }
        mon.MonsterCurHP = Mathf.Max(0, mon.MonsterCurHP - damage);

        // 흡혈 확인
        drainAmount = Drain(GameManager.Instance.DrainPercent, mon.monsterData.DrainResist, damage, GameManager.Instance.DrainAmount);
        if (drainAmount != 0)
        {
            GameManager.Instance.PlayerCurHP = Mathf.Min(GameManager.Instance.PlayerMaxHP, GameManager.Instance.PlayerCurHP + drainAmount);
            if (!isSkip)
            {
                PrintLog.Instance.BattleLog($"플레이어의 흡혈 {drainAmount:N0}만큼 체력을 회복합니다.");
                playerHPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP:N0}";
                playerHPText.color = Color.green;
                playerHPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
            }
        }

        // 콤보 관련 텍스트 띄워주기
        if (!isSkip)
        {
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
    }
    #endregion

    #region 몬스터
    private void monsterTurn(int comboCount, bool isSkip)
    {
        if (Avoid(GameManager.Instance.AvoidPercent, mon.monsterData.AvoidResist))
        { // 회피 성공했을 때
            PrintLog.Instance.BattleLog($"플레이어의 회피 !");
        }
        else
        {
            MonsterAttack(comboCount, isSkip);
            playerHPText.color = new Color(1f, 0f, 0f); // 빨간색
        }

        if (GameManager.Instance.PlayerCurHP <= 0)
        { // 패배
            result.isWin = false;
            if (mon.monsterData.isElite)
            {
                GameManager.Instance.CurrentEnergy -= mon.monsterData.RequireEnergy;
            }
            else
            {
                GameManager.Instance.CurrentEnergy -= mon.monsterData.RequireEnergy * 2;
            }
        }
    }


    private void MonsterAttack(int _comboCount, bool isSkip)
    {
        int damage = DamageCalculate(mon.MonsterATK, mon.monsterData.CriticalPercant, mon.monsterData.CriticalDamage, GameManager.Instance.CriticalResist, GameManager.Instance.PlayerDef);

        // 방어의 룬 절대 방어 50
        damage = isDefenceRune ? damage - 50 : damage;
        if (!isSkip)
        {
            monsterDamageText.gameObject.SetActive(true);
            monsterDamageText.text = $"-{damage:N0}";
            monsterDamageText.gameObject.GetComponent<Animator>().SetTrigger("Hit");
        }
        int drainAmount = 0;

        GameManager.Instance.PlayerCurHP = Mathf.Max(0, GameManager.Instance.PlayerCurHP - damage);
        drainAmount = Drain(mon.monsterData.DrainPercent, GameManager.Instance.DrainResist, damage, mon.monsterData.DrainAmount);
        if (drainAmount != 0)
        { // 흡혈 성공 텍스트 띄워줘야함 todo
            mon.MonsterCurHP = Mathf.Min(mon.MonsterMaxHP, mon.MonsterCurHP + drainAmount);
            if (!isSkip)
            {
                PrintLog.Instance.BattleLog($"몬스터의 흡혈 {drainAmount:N0}만큼 몬스터의 체력이 회복됩니다.");
                // 몬스터 체력바 관리
                monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
                monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;
            }
        }

        if (!isSkip)
        {
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
    }
    #endregion

    #region 전투 변수 계산
    private void PoisonRune()
    {
        if(isPoisonRune)
        {
            int poisonDamage = Mathf.RoundToInt(mon.MonsterMaxHP * 0.05f);
            mon.MonsterCurHP -= poisonDamage;
            PrintLog.Instance.BattleLog($"독성의 룬 효과로 몬스터의 체력이 {poisonDamage:N0}만큼 감소.");
        }
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
    #endregion

    #region 리워드 드랍
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
            int ownCount = owndictionary.ContainsKey(mon.monsterData.RewardItem[i].ItemID) ? owndictionary[mon.monsterData.RewardItem[i].ItemID] : 1;
            float dropRate = 0;
            int cardBuff = GameManager.Instance.CardBuff == CardBuffEnum.DropBuff ? 3 : 0;
            float masterBuff = GameManager.Instance.MasterDropPoint == 0 ? 0 : GameManager.Instance.MasterDropPoint / 100f;


            if (ownCount < 10)
            {
                float quickSlotDrop = GameManager.Instance.isClover ? 70 : 0;
                float addDropRate = (float)(mon.monsterData.RewardItem[i].DropRate * (1 + (float)(GameManager.Instance.ItemDropRate / 100f) + (float)(quickSlotDrop / 100f)));
                dropRate = (float)(addDropRate / (1 + ownCount) + cardBuff + masterBuff + GameManager.Instance.BadgeData.BadgeItemDropRate); // 보유하지 않은 아이템 개수로 나누어 드랍 확률 계산
            }
            drop.DropRateText.text = $"{dropRate:F2}%";
        }
    }

    private void RuneDropRate(Monster mon)
    {
        GameObject runeOBJ = Instantiate(runeItemObject);
        runeOBJ.transform.SetParent(dropItemTransform);
        RunePanel rune = runeOBJ.GetComponent<RunePanel>();

        if (GameManager.Instance.RuneHashSet == null)
        {
            GameManager.Instance.RuneHashSet = new HashSet<string>();
        }
        if (GameManager.Instance.RuneHashSet.Contains(mon.monsterData.RewardRune.EquipmentName))
        { // 이미 획득한 아이템 아이콘 출력
            rune.QuestionMark.SetActive(false);
            rune.ItemIcon.SetActive(true);
            rune.IconSprite.sprite = EquipmentManager.Instance.GetEquipmentSprite(mon.monsterData.RewardRune);
        }
        else
        { // 아직 획득하지 못한 아이템 물음표 출력
            rune.QuestionMark.SetActive(true);
            rune.ItemIcon.SetActive(false);
        }
    }
    #endregion

    #region 기타
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
            case WeaponType.Hammer:
                con = weaponAnimators[2];
                return con;
            case WeaponType.Bow:
                con = weaponAnimators[3];
                return con;
            case WeaponType.Spear:
                con = weaponAnimators[4];
                return con;
            case WeaponType.Staff:
                con = weaponAnimators[5];
                return con;
            case WeaponType.Stone:
                con = weaponAnimators[6];
                return con;
            default:
                return null;
        }
    }

    private void RuneCheck()
    {
        isDefenceRune = GameManager.Instance.RuneHashSet.Contains("방어의 룬");
        isPoisonRune = GameManager.Instance.RuneHashSet.Contains("독성의 룬");
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

    private void DefeatBattle()
    {
        result.isWin = false;
        GameManager.Instance.PlayerCurHP = 0;
        if (mon.monsterData.isElite)
        {
            GameManager.Instance.CurrentEnergy -= mon.monsterData.RequireEnergy;
        }
        else
        {
            GameManager.Instance.CurrentEnergy -= mon.monsterData.RequireEnergy * 2;
        }
    }
    #endregion

    #region 승률
    private void CalculrateWinRate(Monster mon)
    {
        int winCount = 0;

        for (int i = 0; i < 20; i++)
        {
            if (FakeBattle(mon))
            { // true 값을 받게되면 승리
                winCount++;
            }
        }

        float PlayerWinPercent = winCount * 5f;

        if (PlayerWinPercent <= 0)
        {
            oddsText.text = "불가능에 가까운 상대입니다.";
            oddsText.color = Color.red;
        }
        else if (20 >= PlayerWinPercent && PlayerWinPercent > 0)
        {
            oddsText.text = "상대하기 어려운 상대입니다.";
            oddsText.color = Color.magenta;
        }
        else if (40 >= PlayerWinPercent && PlayerWinPercent > 20)
        {
            oddsText.text = "제압해볼 수 있는 상대입니다.";
            oddsText.color = Color.gray;
        }
        else if (60 >= PlayerWinPercent && PlayerWinPercent > 40)
        {
            oddsText.text = "나와 비슷한 상대입니다.";
            oddsText.color = Color.white;
        }
        else if (80 >= PlayerWinPercent && PlayerWinPercent > 60)
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
    private bool FakeBattle(Monster mon)
    {
        int maxCridamage = Mathf.RoundToInt(GameManager.Instance.PlayerATK * 1.05f * GameManager.Instance.CriticalDamage);
        if (mon.monsterData.MonsterDef > maxCridamage)
        { // 최대 크리데미지가 몬스터의 방어를 뚫지 못하는 경우
            return false;
        }

        fakeMonsterHP = mon.MonsterCurHP;
        fakePlayerHP = GameManager.Instance.PlayerCurHP;

        for (int i = 0; i < 500; i++)
        {
            FakePlayerTurn(mon);
            if (fakeMonsterHP <= 0)
            { // 몬스터의 체력이 0보다 작을 경우 승리
                return true;
            }
            FakeMonsterTurn(mon);
            if(fakePlayerHP <= 0)
            { // 플레이어의 체력이 0보다 작을 경우 패배
                return false;
            }
        }
        // 500턴이 넘어가면 자동 패배
        return false;
    }

    private void FakePlayerTurn(Monster mon)
    {
        // 콤보 계산 해야됨
        int comboCount = ComboCount(GameManager.Instance.ComboPercent, mon.monsterData.ComboResist);
        int damage = 0;
        int drainAmount = 0;

        for (int i = 1; i < comboCount + 1; i++)
        {
            if (Avoid(mon.monsterData.AvoidPercent, GameManager.Instance.AvoidResist))
            { // 몬스터가 회피 성공했을 때

            }
            else
            {
                damage = DamageCalculate(GameManager.Instance.PlayerATK, GameManager.Instance.CriticalPercant, GameManager.Instance.CriticalDamage, mon.monsterData.CriticalResist, mon.monsterData.MonsterDef);
                fakeMonsterHP = Mathf.Max(0, fakeMonsterHP - damage);

                if (fakeMonsterHP <= 0)
                { // 승리
                    return;
                }

                drainAmount = Drain(GameManager.Instance.DrainPercent, mon.monsterData.DrainResist, damage, GameManager.Instance.DrainAmount);
                if(drainAmount != 0)
                {
                    fakePlayerHP = Mathf.Min(GameManager.Instance.PlayerMaxHP, fakePlayerHP + drainAmount);
                }

                // 리젠 룬
                fakePlayerHP += runeHPRegen;
            }
        }
    }

    private void FakeMonsterTurn(Monster mon)
    {
        int damage = 0;
        int drainAmount = 0;
        int comboCount = ComboCount(mon.monsterData.ComboPercent, GameManager.Instance.ComboResist);

        for (int i = 1; i < comboCount + 1; i++)
        {
            if(Avoid(GameManager.Instance.AvoidPercent, mon.monsterData.ComboPercent))
            { // 플레이어가 회피 성공했을 때

            }
            else
            {
                damage = DamageCalculate(mon.MonsterATK, mon.monsterData.CriticalPercant, mon.monsterData.CriticalDamage, GameManager.Instance.CriticalResist, GameManager.Instance.PlayerDef);

                // 방어의 룬 절대 방어 50
                damage = isDefenceRune ? damage - 50 : damage;

                fakePlayerHP = Mathf.Max(0, fakePlayerHP - damage);

                if (fakePlayerHP <= 0)
                { // 패배
                    return;
                }

                drainAmount = Drain(mon.monsterData.DrainPercent, GameManager.Instance.DrainResist, damage, mon.monsterData.DrainAmount);
                if (drainAmount != 0)
                {
                    fakeMonsterHP = Mathf.Min(mon.MonsterMaxHP, fakeMonsterHP + drainAmount);
                }
            }
        }
    }
#endregion
}
