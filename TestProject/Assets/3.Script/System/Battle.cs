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
    [Header("��")]
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
        // ============================ ���� ������ �Է�
        result.mon = mon;
        monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
        monsterNameText.text = mon.monsterData.MonsterName;
        monsterLevelText.text = $"���� {mon.monsterData.MonsterLevel:N0} - {mon.monsterData.MonsterType}";
        monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;
        monsterImage.sprite = mon.monsterData.MonsterSprite;
        monsterImage.SetNativeSize();

        // �� ȸ���� üũ
        runeHPRegen = GameManager.Instance.RuneHashSet.Contains("����� ��") ? Mathf.RoundToInt(GameManager.Instance.PlayerMaxHP / 200f) : 0;

        // ��� ������ ���
        ItemDropRate(mon);

        // ���� ������ ���
        if (mon.monsterData.isElite)
        {
            RuneDropRate(mon);
        }

        // �»� �ؽ�Ʈ ���
        CalculrateWinRate(mon);

        // �� üũ
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
        PrintLog.Instance.BattleLog("�ϴ��� ��ư�� ���� �ൿ�� �����ּ���.");
    }

    public void BattleButton()
    {
        if (GameManager.Instance.PlayerCurHP == 0)
        {
            PrintLog.Instance.StaticLog("ü���� ���� �ο� �� ����.");
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
        // ���� �Ŀ��� ���Ϳ� ����� ��ġ��ŭ �������� ����
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
                    break; // ������ ü���� 0 �����̸� for ������ ��������
                }
            }
            if (mon.MonsterCurHP <= 0)
            {
                break; // ������ ü���� 0 �����̸� while ������ ��������
            }

            comboCount = ComboCount(mon.monsterData.ComboPercent, GameManager.Instance.ComboResist);
            for (int i = 1; i < comboCount + 1; i++)
            {
                monsterTurn(i, isSkip);

                if (GameManager.Instance.PlayerCurHP <= 0)
                {
                    break; // �÷��̾��� ü���� 0���϶�� for ������ ��������
                }

            }

            if (GameManager.Instance.PlayerCurHP <= 0)
            {
                break; // �÷��̾��� ü���� 0���϶�� while ������ ��������
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
            { // �ִ� ũ���������� ������ �� ���� ���ϴ� ���
                PrintLog.Instance.StaticLog("ȥ���� ���� ���� �ϰ��� ��������, ���� �� ���� ���Ѵ�.");
                DefeatBattle();
                break;
            }

            int comboCount = ComboCount(GameManager.Instance.ComboPercent, mon.monsterData.ComboResist);
            for (int i = 1; i < comboCount + 1; i++)
            {
                PlayerTurn(i, isSkip);

                if (mon.MonsterCurHP <= 0)
                {
                    break; // ������ ü���� 0 �����̸� for ������ ��������
                }
                yield return battleDelay;
                playerHPText.color = Color.white;
            }
            if (mon.MonsterCurHP <= 0)
            {
                break; // ������ ü���� 0 �����̸� while ������ ��������
            }

            comboCount = ComboCount(mon.monsterData.ComboPercent, GameManager.Instance.ComboResist);
            for (int i = 1; i < comboCount + 1; i++)
            {
                monsterTurn(i, isSkip);

                if (GameManager.Instance.PlayerCurHP <= 0)
                {
                    break; // �÷��̾��� ü���� 0���϶�� for ������ ��������
                }

                yield return battleDelay;
                playerHPText.color = new Color(1f, 1f, 1f); // ���
                monsterDamageText.gameObject.SetActive(false);
            }

            if (GameManager.Instance.PlayerCurHP <= 0)
            {
                break; // �÷��̾��� ü���� 0���϶�� while ������ ��������
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

    #region �÷��̾�
    private void PlayerTurn(int comboCount, bool isSkip)
    {
        if (Avoid(mon.monsterData.AvoidPercent, GameManager.Instance.AvoidResist))
        { // ȸ�� �������� ��
            if (!isSkip)
            {
                damageText.text = $"ȸ�� !";
                damageText.color = Color.cyan;
                PrintLog.Instance.BattleLog($"������ ȸ�� !");
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
        { // �¸�
            result.isWin = true;
            if (mon.monsterData.isElite)
            { // ����Ʈ ���ʹ� ������ 1�� ����
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

        // ũ��Ƽ�� Ȯ��
        if (!isSkip)
        {
            if (isCritical)
            {
                damageText.text = $"ġ��Ÿ !\n{damage:N0} !";
                damageText.color = Color.yellow;
            }
            else
            {
                damageText.text = $"{damage:N0} !";
                damageText.color = Color.white;
            }
        }
        mon.MonsterCurHP = Mathf.Max(0, mon.MonsterCurHP - damage);

        // ���� Ȯ��
        drainAmount = Drain(GameManager.Instance.DrainPercent, mon.monsterData.DrainResist, damage, GameManager.Instance.DrainAmount);
        if (drainAmount != 0)
        {
            GameManager.Instance.PlayerCurHP = Mathf.Min(GameManager.Instance.PlayerMaxHP, GameManager.Instance.PlayerCurHP + drainAmount);
            if (!isSkip)
            {
                PrintLog.Instance.BattleLog($"�÷��̾��� ���� {drainAmount:N0}��ŭ ü���� ȸ���մϴ�.");
                playerHPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP:N0}";
                playerHPText.color = Color.green;
                playerHPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
            }
        }

        // �޺� ���� �ؽ�Ʈ ����ֱ�
        if (!isSkip)
        {
            if (_comboCount != 1)
            {
                PrintLog.Instance.BattleLog($"{_comboCount}���� ! {mon.monsterData.MonsterName}���� {damage:N0}�� ���ظ� �������ϴ�.");
            }
            else
            {
                PrintLog.Instance.BattleLog($"{mon.monsterData.MonsterName}���� {damage:N0}�� ���ظ� �������ϴ�.");
            }

            // ���� ü�¹� ����
            monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
            monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;

            // ������ ����Ʈ 
            runtimeEffectAnimator = GetAnimator();
            effectAnimator.runtimeAnimatorController = runtimeEffectAnimator;
            damageAnimator.SetTrigger("Damage");
        }
    }
    #endregion

    #region ����
    private void monsterTurn(int comboCount, bool isSkip)
    {
        if (Avoid(GameManager.Instance.AvoidPercent, mon.monsterData.AvoidResist))
        { // ȸ�� �������� ��
            PrintLog.Instance.BattleLog($"�÷��̾��� ȸ�� !");
        }
        else
        {
            MonsterAttack(comboCount, isSkip);
            playerHPText.color = new Color(1f, 0f, 0f); // ������
        }

        if (GameManager.Instance.PlayerCurHP <= 0)
        { // �й�
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

        // ����� �� ���� ��� 50
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
        { // ���� ���� �ؽ�Ʈ �������� todo
            mon.MonsterCurHP = Mathf.Min(mon.MonsterMaxHP, mon.MonsterCurHP + drainAmount);
            if (!isSkip)
            {
                PrintLog.Instance.BattleLog($"������ ���� {drainAmount:N0}��ŭ ������ ü���� ȸ���˴ϴ�.");
                // ���� ü�¹� ����
                monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
                monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;
            }
        }

        if (!isSkip)
        {
            if (_comboCount != 1)
            {
                PrintLog.Instance.BattleLog($"������ {_comboCount}���� ! �÷��̾�� {damage:N0}�� ���ظ� �������ϴ�.");
            }
            else
            {
                PrintLog.Instance.BattleLog($"�÷��̾�� {damage:N0}�� ���ظ� �������ϴ�.");
            }
            playerHPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP:N0}";
            playerHPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
        }
    }
    #endregion

    #region ���� ���� ���
    private void PoisonRune()
    {
        if(isPoisonRune)
        {
            int poisonDamage = Mathf.RoundToInt(mon.MonsterMaxHP * 0.05f);
            mon.MonsterCurHP -= poisonDamage;
            PrintLog.Instance.BattleLog($"������ �� ȿ���� ������ ü���� {poisonDamage:N0}��ŭ ����.");
        }
    }

    private int DamageCalculate(float _Objdamage, float _ObjCriPercent, float _ObjCriDamage, float _EnemyCriResist, int _EnemyDef)
    {
        float damage = Random.Range(_Objdamage * 0.95f, _Objdamage * 1.05f);
        float adjustedCriPercent = _ObjCriPercent - _EnemyCriResist;
        float randomValue = Random.Range(0f, 100f);

        if (adjustedCriPercent >= 100f || randomValue <= adjustedCriPercent)
        { // ũ��Ƽ�� ������ ���� 100%�� �Ѱų�, Ȯ���� �������� ��
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
        // �޺� ������ ����Ͽ� �޺� �ۼ�Ʈ�� �����մϴ�.
        float adjustedComboPercent = _ObjComboPercent - _EnemyComboResist;
        int comboCount = 1;

        // �޺� ������ �� Ȯ���� 100 �ʰ���� ������ ��Ÿ
        while (adjustedComboPercent >= 100f)
        {
            comboCount++;
            adjustedComboPercent -= 100f;
        }

        // ���� Ȯ���� ���� �޺� Ȯ�� ���
        float randomValue = Random.Range(0f, 100f);

        // ������ ���� ���� ������ �޺� �ۼ�Ʈ���� ���� ���
        if (randomValue <= adjustedComboPercent)
        {
            // �޺� ī��Ʈ�� 1 ����.
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
        { // ȸ�� ����
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region ������ ���
    private void ItemDropRate(Monster mon)
    {
        for (int i = 0; i < mon.monsterData.RewardItem.Length; i++)
        {
            // ������Ʈ ����
            GameObject item = Instantiate(dropItemObject);
            item.transform.SetParent(dropItemTransform);

            // ������Ʈ�� �̹��� ����
            DropItem drop = item.GetComponent<DropItem>();
            drop.DropItemImage.sprite = EquipmentManager.Instance.GetEquipmentSprite(mon.monsterData.RewardItem[i]);

            // ������ ����� ǥ��
            Dictionary<int, int> owndictionary = DataManager.Instance.GetOwnDictionary(mon.monsterData.RewardItem[i]);
            int ownCount = owndictionary.ContainsKey(mon.monsterData.RewardItem[i].ItemID) ? owndictionary[mon.monsterData.RewardItem[i].ItemID] : 1;
            float dropRate = 0;
            int cardBuff = GameManager.Instance.CardBuff == CardBuffEnum.DropBuff ? 3 : 0;
            float masterBuff = GameManager.Instance.MasterDropPoint == 0 ? 0 : GameManager.Instance.MasterDropPoint / 100f;


            if (ownCount < 10)
            {
                float quickSlotDrop = GameManager.Instance.isClover ? 70 : 0;
                float addDropRate = (float)(mon.monsterData.RewardItem[i].DropRate * (1 + (float)(GameManager.Instance.ItemDropRate / 100f) + (float)(quickSlotDrop / 100f)));
                dropRate = (float)(addDropRate / (1 + ownCount) + cardBuff + masterBuff + GameManager.Instance.BadgeData.BadgeItemDropRate); // �������� ���� ������ ������ ������ ��� Ȯ�� ���
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
        { // �̹� ȹ���� ������ ������ ���
            rune.QuestionMark.SetActive(false);
            rune.ItemIcon.SetActive(true);
            rune.IconSprite.sprite = EquipmentManager.Instance.GetEquipmentSprite(mon.monsterData.RewardRune);
        }
        else
        { // ���� ȹ������ ���� ������ ����ǥ ���
            rune.QuestionMark.SetActive(true);
            rune.ItemIcon.SetActive(false);
        }
    }
    #endregion

    #region ��Ÿ
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
        isDefenceRune = GameManager.Instance.RuneHashSet.Contains("����� ��");
        isPoisonRune = GameManager.Instance.RuneHashSet.Contains("������ ��");
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

    #region �·�
    private void CalculrateWinRate(Monster mon)
    {
        int winCount = 0;

        for (int i = 0; i < 20; i++)
        {
            if (FakeBattle(mon))
            { // true ���� �ްԵǸ� �¸�
                winCount++;
            }
        }

        float PlayerWinPercent = winCount * 5f;

        if (PlayerWinPercent <= 0)
        {
            oddsText.text = "�Ұ��ɿ� ����� ����Դϴ�.";
            oddsText.color = Color.red;
        }
        else if (20 >= PlayerWinPercent && PlayerWinPercent > 0)
        {
            oddsText.text = "����ϱ� ����� ����Դϴ�.";
            oddsText.color = Color.magenta;
        }
        else if (40 >= PlayerWinPercent && PlayerWinPercent > 20)
        {
            oddsText.text = "�����غ� �� �ִ� ����Դϴ�.";
            oddsText.color = Color.gray;
        }
        else if (60 >= PlayerWinPercent && PlayerWinPercent > 40)
        {
            oddsText.text = "���� ����� ����Դϴ�.";
            oddsText.color = Color.white;
        }
        else if (80 >= PlayerWinPercent && PlayerWinPercent > 60)
        {
            oddsText.text = "���� �ս��� ����Դϴ�.";
            oddsText.color = Color.cyan;
        }
        else
        {
            oddsText.text = "�е��� �� �ִ� ����Դϴ�.";
            oddsText.color = Color.green;
        }
    }
    private bool FakeBattle(Monster mon)
    {
        int maxCridamage = Mathf.RoundToInt(GameManager.Instance.PlayerATK * 1.05f * GameManager.Instance.CriticalDamage);
        if (mon.monsterData.MonsterDef > maxCridamage)
        { // �ִ� ũ���������� ������ �� ���� ���ϴ� ���
            return false;
        }

        fakeMonsterHP = mon.MonsterCurHP;
        fakePlayerHP = GameManager.Instance.PlayerCurHP;

        for (int i = 0; i < 500; i++)
        {
            FakePlayerTurn(mon);
            if (fakeMonsterHP <= 0)
            { // ������ ü���� 0���� ���� ��� �¸�
                return true;
            }
            FakeMonsterTurn(mon);
            if(fakePlayerHP <= 0)
            { // �÷��̾��� ü���� 0���� ���� ��� �й�
                return false;
            }
        }
        // 500���� �Ѿ�� �ڵ� �й�
        return false;
    }

    private void FakePlayerTurn(Monster mon)
    {
        // �޺� ��� �ؾߵ�
        int comboCount = ComboCount(GameManager.Instance.ComboPercent, mon.monsterData.ComboResist);
        int damage = 0;
        int drainAmount = 0;

        for (int i = 1; i < comboCount + 1; i++)
        {
            if (Avoid(mon.monsterData.AvoidPercent, GameManager.Instance.AvoidResist))
            { // ���Ͱ� ȸ�� �������� ��

            }
            else
            {
                damage = DamageCalculate(GameManager.Instance.PlayerATK, GameManager.Instance.CriticalPercant, GameManager.Instance.CriticalDamage, mon.monsterData.CriticalResist, mon.monsterData.MonsterDef);
                fakeMonsterHP = Mathf.Max(0, fakeMonsterHP - damage);

                if (fakeMonsterHP <= 0)
                { // �¸�
                    return;
                }

                drainAmount = Drain(GameManager.Instance.DrainPercent, mon.monsterData.DrainResist, damage, GameManager.Instance.DrainAmount);
                if(drainAmount != 0)
                {
                    fakePlayerHP = Mathf.Min(GameManager.Instance.PlayerMaxHP, fakePlayerHP + drainAmount);
                }

                // ���� ��
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
            { // �÷��̾ ȸ�� �������� ��

            }
            else
            {
                damage = DamageCalculate(mon.MonsterATK, mon.monsterData.CriticalPercant, mon.monsterData.CriticalDamage, GameManager.Instance.CriticalResist, GameManager.Instance.PlayerDef);

                // ����� �� ���� ��� 50
                damage = isDefenceRune ? damage - 50 : damage;

                fakePlayerHP = Mathf.Max(0, fakePlayerHP - damage);

                if (fakePlayerHP <= 0)
                { // �й�
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
