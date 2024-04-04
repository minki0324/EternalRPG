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
                MonsterAttack(i);
                playerHPText.color = new Color(1f, 0f, 0f); // ������
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
        if(isCritical)
        {
            damageText.text = $"ġ��Ÿ !\n{damage:N0} !";
        }
        else
        {
            damageText.text = $"{damage:N0} !";
        }
        mon.MonsterCurHP = Mathf.Max(0, mon.MonsterCurHP - damage);
        if (_comboCount != 1)
        {
            PrintLog.Instance.BattleLog($"{_comboCount}���� ! {mon.monsterData.MonsterName}���� {damage:N0}�� ���ظ� �������ϴ�.");
        }
        else
        {
            PrintLog.Instance.BattleLog($"{mon.monsterData.MonsterName}���� {damage:N0}�� ���ظ� �������ϴ�.");
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

        damage = damage - _EnemyDef;

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
