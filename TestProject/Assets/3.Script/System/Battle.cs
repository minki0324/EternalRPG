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
    [SerializeField] private RuntimeAnimatorController[] weaponAnimators;
    [SerializeField] private RuntimeAnimatorController runtimeEffectAnimator;
    [SerializeField] private Animator effectAnimator;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private Animator damageAnimator;
    private bool isCritical = false;

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
        result.mon = mon;
        monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
        monsterNameText.text = mon.monsterData.MonsterName;
        monsterLevelText.text = $"���� {mon.monsterData.MonsterLevel:N0} - {mon.monsterData.MonsterType}";
        monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;
        monsterImage.sprite = mon.monsterData.MonsterSprite;
        // todo �»� �ؽ�Ʈ �����ؾߵ�

        // ============================ ���� ������ �Է�
        playerHPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP}";
        playerHPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
        PrintLog.Instance.BattleLog("�ϴ��� ��ư�� ���� �ൿ�� �����ּ���.");
    }

    public void BattleButton()
    {
        if(GameManager.Instance.PlayerCurHP == 0)
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
                if (Avoid(mon.monsterData.AvoidPercent, GameManager.Instance.AvoidResist))
                { // ȸ�� �������� ��
                    damageText.text = $"ȸ�� !";
                    damageText.color = Color.cyan;
                    Debug.Log("���Ͱ� ȸ��");
                }
                else
                {
                    PlayerAttack(i);
                    effectAnimator.SetTrigger(GetTrigger(i));
                }

                if (mon.MonsterCurHP <= 0)
                { // �¸�
                    result.isWin = true;
                    GameManager.Instance.Energy -= mon.monsterData.RequireEnergy;
                    // for�� ��Ż
                    break;
                }

                yield return battleDelay;
            }
            if (mon.MonsterCurHP <= 0)
            {
                break; // ������ ü���� 0 �����̸� while ������ ��������
            }

            comboCount = 1;
            comboCount = ComboCount(mon.monsterData.ComboPercent, GameManager.Instance.ComboResist);
            for (int i = 1; i < comboCount + 1; i++)
            {
                if(Avoid(GameManager.Instance.AvoidPercent, mon.monsterData.AvoidResist))
                { // ȸ�� �������� ��
                    Debug.Log("�÷��̾ ȸ��");
                }
                else
                {
                    MonsterAttack(i);
                    playerHPText.color = new Color(1f, 0f, 0f); // ������
                }
                
                if (GameManager.Instance.PlayerCurHP <= 0)
                { // �й�
                    result.isWin = false;
                    GameManager.Instance.Energy -= mon.monsterData.RequireEnergy * 2;
                    //for�� ��Ż
                    break;
                }
                yield return battleDelay;
                playerHPText.color = new Color(1f, 1f, 1f); // ���
            }
            if (GameManager.Instance.PlayerCurHP <= 0)
            {
                break; // �÷��̾��� ü���� 0���϶�� while ������ ��������
            }
        }
        result.gameObject.SetActive(true);
        ignoreRay.SetActive(false);
        Time.timeScale = 1f;
    }

    private void PlayerAttack(int _comboCount)
    {
        int damage = DamageCalculate(GameManager.Instance.PlayerATK, GameManager.Instance.CriticalPercant, GameManager.Instance.CriticalDamage, mon.monsterData.CriticalResist, mon.monsterData.MonsterDef);
        int drainAmount = 0;

        // ũ��Ƽ�� Ȯ��
        if(isCritical)
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
        if(drainAmount != 0)
        { // ���� ���� �ؽ�Ʈ �������� todo
            GameManager.Instance.PlayerCurHP = Mathf.Min(GameManager.Instance.PlayerMaxHP, GameManager.Instance.PlayerCurHP + drainAmount);
            Debug.Log($"�÷��̾ ���� : {Mathf.Min(GameManager.Instance.PlayerMaxHP, GameManager.Instance.PlayerCurHP + drainAmount)}");
            playerHPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP:N0}";
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

        // 
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
        int drainAmount = 0;
        GameManager.Instance.PlayerCurHP = Mathf.Max(0, GameManager.Instance.PlayerCurHP - damage);
        drainAmount = Drain(mon.monsterData.DrainPercent, GameManager.Instance.DrainResist, damage, mon.monsterData.DrainAmount);
        if(drainAmount != 0)
        { // ���� ���� �ؽ�Ʈ �������� todo
            mon.MonsterCurHP = Mathf.Min(mon.MonsterMaxHP, mon.MonsterCurHP + drainAmount);
            Debug.Log("���Ͱ� ����");
            // ���� ü�¹� ����
            monsterHPText.text = $"{mon.MonsterCurHP:N0} / {mon.MonsterMaxHP:N0}";
            monsterHPSlider.value = (float)mon.MonsterCurHP / mon.MonsterMaxHP;
        }

        if(_comboCount != 1)
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

        if(adjustedDrainPercent > 100 || adjustedDrainPercent >= randomValue)
        {
            drain = Mathf.RoundToInt(damage * ObjDrainAmount);
            
        }
        return drain;
    }

    private bool Avoid(int _ObjAvoidChance, int _EnemyAvoidResist)
    {
        int adjustedAvoidPercent = _ObjAvoidChance - _EnemyAvoidResist;
        float randomValue = Random.Range(0f, 100f);

        if(adjustedAvoidPercent > 100 || adjustedAvoidPercent >= randomValue)
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
            case 2:  return "Double";
            case 3: return "Tripple";
            case 4: return "Four";
            case 5: return "Five";
            default: return "Six";
        }
    }
}