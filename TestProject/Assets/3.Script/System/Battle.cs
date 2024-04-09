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
        // ============================ ���� ������ �Է�
        result.mon = mon;
        monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
        monsterNameText.text = mon.monsterData.MonsterName;
        monsterLevelText.text = $"���� {mon.monsterData.MonsterLevel:N0} - {mon.monsterData.MonsterType}";
        monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;
        monsterImage.sprite = mon.monsterData.MonsterSprite;

        // ��� ������ ���
        ItemDropRate(mon);

        // �»� �ؽ�Ʈ ���
        CalculrateWinRate(mon);

        playerHPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP}";
        playerHPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
        PrintLog.Instance.BattleLog("�ϴ��� ��ư�� ���� �ൿ�� �����ּ���.");
    }

    public void BattleButton()
    {
        if (GameManager.Instance.PlayerCurHP == 0)
        { // todo ���� 0���� ���� �Ұ� �޼��� ���
            return;
        }
        ignoreRay.SetActive(true);
        StartCoroutine(StartBattle_Co());
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
                monsterTurn(i);

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
        { // ȸ�� �������� ��
            damageText.text = $"ȸ�� !";
            damageText.color = Color.cyan;
            PrintLog.Instance.BattleLog($"������ ȸ�� !");
        }
        else
        {
            PlayerAttack(comboCount);
            effectAnimator.SetTrigger(GetTrigger(comboCount));
        }

        if (mon.MonsterCurHP <= 0)
        { // �¸�
            result.isWin = true;
            GameManager.Instance.CurrentEnergy -= mon.monsterData.RequireEnergy;
        }
    }

    private void monsterTurn(int comboCount)
    {
        if (Avoid(GameManager.Instance.AvoidPercent, mon.monsterData.AvoidResist))
        { // ȸ�� �������� ��
            PrintLog.Instance.BattleLog($"�÷��̾��� ȸ�� !");
        }
        else
        {
            MonsterAttack(comboCount);
            playerHPText.color = new Color(1f, 0f, 0f); // ������
        }

        if (GameManager.Instance.PlayerCurHP <= 0)
        { // �й�
            result.isWin = false;
            GameManager.Instance.CurrentEnergy -= mon.monsterData.RequireEnergy * 2;
        }
    }

    private void PlayerAttack(int _comboCount)
    {
        int damage = DamageCalculate(GameManager.Instance.PlayerATK, GameManager.Instance.CriticalPercant, GameManager.Instance.CriticalDamage, mon.monsterData.CriticalResist, mon.monsterData.MonsterDef);
        int drainAmount = 0;

        // ũ��Ƽ�� Ȯ��
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
        mon.MonsterCurHP = Mathf.Max(0, mon.MonsterCurHP - damage);

        // ���� Ȯ��
        drainAmount = Drain(GameManager.Instance.DrainPercent, mon.monsterData.DrainResist, damage, GameManager.Instance.DrainAmount);
        if (drainAmount != 0)
        {
            PrintLog.Instance.BattleLog($"�÷��̾��� ���� {drainAmount:N0}��ŭ ü���� ȸ���մϴ�.");
            GameManager.Instance.PlayerCurHP = Mathf.Min(GameManager.Instance.PlayerMaxHP, GameManager.Instance.PlayerCurHP + drainAmount);
            playerHPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP:N0}";
            playerHPText.color = Color.green;
            playerHPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
        }

        // �޺� ���� �ؽ�Ʈ ����ֱ�
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
        { // ���� ���� �ؽ�Ʈ �������� todo
            mon.MonsterCurHP = Mathf.Min(mon.MonsterMaxHP, mon.MonsterCurHP + drainAmount);
            PrintLog.Instance.BattleLog($"������ ���� {drainAmount:N0}��ŭ ������ ü���� ȸ���˴ϴ�.");
            // ���� ü�¹� ����
            monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
            monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;
        }

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
        { // ������ 2�� �̻��̱� ������ �¸�
            PlayerWinPercent = 100f;
        }
        else if(monsterPower >= playerPower*2)
        { // ���� �������� 2�� �̻��̱� ������ �й�
            PlayerWinPercent = 0f;
        }
        else
        {
            float tempPower = (float)playerPower - (float)monsterPower;
            if(tempPower >= 0)
            { // �÷��̾ �� ���ϰų� ���� ���
                PlayerWinPercent = 100 - (((float)monsterPower - ((float)playerPower / 2)) / ((float)playerPower / 200) / 2);
            }
            else
            { // ���Ͱ� �� ���� ���
                PlayerWinPercent = 50 - (((float)monsterPower - (float)playerPower) / (((float)playerPower * 2) / 100));
            }
        }

        if (PlayerWinPercent <= 0)
        {
            oddsText.text = "�Ұ��ɿ� ����� ����Դϴ�.";
            oddsText.color = Color.red;
        }
        else if (20 > PlayerWinPercent && PlayerWinPercent > 0)
        {
            oddsText.text = "����ϱ� ����� ����Դϴ�.";
            oddsText.color = Color.magenta;
        }
        else if (40 > PlayerWinPercent && PlayerWinPercent > 20)
        {
            oddsText.text = "�����غ� �� �ִ� ����Դϴ�.";
            oddsText.color = Color.gray;
        }
        else if (60 > PlayerWinPercent && PlayerWinPercent > 40)
        {
            oddsText.text = "���� ����� ����Դϴ�.";
            oddsText.color = Color.blue;
        }
        else if (80 > PlayerWinPercent && PlayerWinPercent > 60)
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
            int ownCount = owndictionary.Values.Sum() == 0 ? 1 : owndictionary.Values.Sum();
            float quickSlotDrop = GameManager.Instance.isClover ? 70 : 0;
            float addDropRate = (float)(mon.monsterData.RewardItem[i].DropRate * (1 + (float)(GameManager.Instance.ItemDropRate / 100) + (float)(quickSlotDrop / 100)));
            float dropRate = (float)((mon.monsterData.RewardItem[i].DropRate + addDropRate) / ownCount);
            drop.DropRateText.text = $"{dropRate:F2}%";
        }
    }
}