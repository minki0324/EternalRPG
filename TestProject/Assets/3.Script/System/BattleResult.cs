using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleResult : MonoBehaviour
{
    [SerializeField] private ActiveCanvas activeCanvas;
    [SerializeField] private GameObject battlePanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Player player;
    [SerializeField] private ResultPanelTitle resultPanelTitle;
    public Monster mon;
    public bool isWin = false;

    [Header("UI")]
    [SerializeField] private TMP_Text expText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text APText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text totalGoldText;
    [SerializeField] private TMP_Text energyText;
    public GameObject DataPanel = null;
    [SerializeField] private GameObject gemPanel;
    [SerializeField] private GameObject overEnergyWarningPanel;

    [Header("������")]
    [SerializeField] private GameObject dropResultItem;
    [SerializeField] private Transform dropResultItemParent;

    [Header("��")]
    [SerializeField] private GameObject runeDropObj;
    [SerializeField] private Transform runeDropParent;
    [SerializeField] private GameObject runeDropEffectPanel;
    [SerializeField] private Animator effectAnimator;
    [SerializeField] private GameObject runeUIPanel;
    [SerializeField] private Image runeImage;
    [SerializeField] private TMP_Text runeNameTxt;
    [SerializeField] private TMP_Text runeDesTxt;

    [Header("�� õ��")]
    [SerializeField] private GameObject buyingGemButton;
    private int gemDrop = 2000;
    private void OnEnable()
    {
        if(GameManager.Instance.CurrentEnergy<=0)
        {
            MasterLevelCal();
        }
        InitData();
    }

    private void InitData()
    {
        if (isWin)
        {
            resultPanelTitle.Result = true;
            AccountBattle_Co();
            GameManager.Instance.DeadMonsterList.Add(mon.monsterData.MonsterID);
        }
        else if (!isWin)
        {
            resultPanelTitle.Result = false;
            expText.text = ": 0";
            levelText.text = ": 0";
            APText.text = ": 0";
            goldText.text = ": 0";
            totalGoldText.text = $": {GameManager.Instance.Gold:N0}";
            if (mon.monsterData.isElite)
            {
                energyText.text = $": {mon.monsterData.RewordEnergy}";
            }
            else
            {
                energyText.text = $": {mon.monsterData.RewordEnergy * 2}";
            }
        }
        AfterBattleMonster(mon, isWin);
    }

    private void AccountBattle_Co()
    { // ���� ���
        // ����ġ
        long EXP = (long)Mathf.Round((float)(mon.monsterData.RewordEXP * (float)(GameManager.Instance.EXPPercent / 100f)));
        EXP = (long)Mathf.Round(Random.Range(EXP * 0.95f, EXP * 1.05f));
        GameManager.Instance.CurrentEXP += EXP;
        int levelup = 0;
        double requireEXP = 0f;
        
        int totalAP = GameManager.Instance.BonusAP;
        while (GameManager.Instance.RequireEXP < GameManager.Instance.CurrentEXP)
        {
            // ������ ��� 
            requireEXP = (long)Mathf.Round(GameManager.Instance.RequireEXP * 1.001f) == GameManager.Instance.RequireEXP ?
                                                        GameManager.Instance.RequireEXP + 1 : (long)Mathf.Round(GameManager.Instance.RequireEXP * 1.001f);
            GameManager.Instance.CurrentEXP -= GameManager.Instance.RequireEXP;
            GameManager.Instance.RequireEXP = (long)Mathf.Round((float)requireEXP);
            GameManager.Instance.PlayerLevel++;
            GameManager.Instance.CurrentAP += totalAP;
            levelup++;
        }

        // ���� �ڵ� �й�
        APDistribute();

        // ���
        long gold = (long)Mathf.Round(UnityEngine.Random.Range(mon.monsterData.RewordGold * 0.95f, mon.monsterData.RewordGold * 1.05f));
        float quickSlotGold = GameManager.Instance.isGoldPack ? 30 : 0;
        long totalGold = (long)Mathf.Round(gold * ((GameManager.Instance.GoldPercent / 100f) + (quickSlotGold / 100f)));
        GameManager.Instance.Gold += totalGold;

        // ������
        PrintDropItem(mon);

        // ��
        if(mon.monsterData.isElite) StartCoroutine(PrintRuneDrop(mon));

        // ��
        GemCalculate();

        // ���� ����
        GameManager.Instance.RenewAbility();

        // ��� ���� ����Ʈ
        expText.text = $": {EXP:N0}";
        levelText.text = $": {levelup:N0} UP";
        APText.text = $": +{totalAP * levelup}";
        goldText.text = $": {totalGold:N0}";
        totalGoldText.text = $": {GameManager.Instance.Gold:N0}";
        if (mon.monsterData.isElite)
        { // ����Ʈ ���Ͷ�� ������ ������ ���
            if (DataManager.Instance.EliteMonsterDic.ContainsKey(mon.monsterData.MonsterID))
            {
                energyText.text = $": -1";
            }
            else
            {
                energyText.text = $": +{mon.monsterData.RewordEnergy}";
            }
        }
        else
        { // �Ϲ� ���̶�� �Ҹ� �������� ���
            energyText.text = $": -{mon.monsterData.RequireEnergy}";
        }
        DataPanel.SetActive(true);
    }

    public void BackButton()
    {
        if (GameManager.Instance.CurrentEnergy <= 0)
        {
            // ������ ��� ���� ����� �� �ʱ�ȭ
            gameObject.SetActive(false);
            battlePanel.SetActive(false);
            activeCanvas.gameObject.SetActive(true);
            inventoryPanel.SetActive(true);
            overEnergyWarningPanel.SetActive(true);
            buyingGemButton.SetActive(true);
        }
        else
        { // �������� �������� ��
            activeCanvas.resultPanel.SetActive(false);
            activeCanvas.versusPanel.SetActive(false);
            activeCanvas.gameObject.SetActive(false);

            player.playerMove.isFight = false;
            // ���� �Ŀ��� ���Ϳ� ����� ��ġ��ŭ �������� ����
            player.gameObject.transform.position = new Vector2(mon.gameObject.transform.position.x + mon.monsterData.ReturnPos.x,
                                                                                                                            mon.gameObject.transform.position.y + mon.monsterData.ReturnPos.y);

            if (isWin)
            {
                mon.isDead = true;
                mon.animator.enabled = false;
                mon.reviveMonster.areaSprite.color = Color.gray;
            }
            else
            {
                mon.isDead = false;
            }

            for (int i = 0; i < dropResultItemParent.childCount; i++)
            { // ��� ��� �ʱ�ȭ
                Destroy(dropResultItemParent.GetChild(i).gameObject);
            }

            resultPanelTitle.TitleText.gameObject.SetActive(false);
            gemPanel.SetActive(false);
            DataPanel.SetActive(false);
            PrintLog.Instance.BattleLogClear();
        }
    }

    private void PrintDropItem(Monster mon)
    {
        for (int i = 0; i < mon.monsterData.RewardItem.Length; i++)
        {
            Dictionary<int, int> owndictionary = DataManager.Instance.GetOwnDictionary(mon.monsterData.RewardItem[i]);
            int ownCount = owndictionary.ContainsKey(mon.monsterData.RewardItem[i].ItemID) ? owndictionary[mon.monsterData.RewardItem[i].ItemID] : 1;

            if (ownCount == 10) continue; // ���� ������ 10���� �� �̻� ��� x
            float masterBuff = GameManager.Instance.MasterDropPoint == 0 ? 0 : GameManager.Instance.MasterDropPoint / 100f;
            int cardBuff = GameManager.Instance.CardBuff == CardBuffEnum.DropBuff ? 3 : 0;
            float quickSlotDrop = GameManager.Instance.isClover ? 70 : 0;
            float addDropRate = (float)(mon.monsterData.RewardItem[i].DropRate * (1 + (float)(GameManager.Instance.ItemDropRate / 100) + (float)(quickSlotDrop / 100)));
            float dropRate = (float)(addDropRate / (1 + ownCount) + cardBuff + masterBuff);
            float randomValue = UnityEngine.Random.Range(0f, 100f);

            if (dropRate >= 100 || dropRate > randomValue)
            { // ������� 100% �̻��̰ų� ������ ���� ���ٸ� ���
                if (owndictionary.ContainsKey(mon.monsterData.RewardItem[i].ItemID))
                {
                    owndictionary[mon.monsterData.RewardItem[i].ItemID]++;
                }
                GameObject dropItem = Instantiate(dropResultItem);
                dropItem.transform.SetParent(dropResultItemParent);
                dropItem.GetComponent<DropItem>().DropItemImage.sprite = EquipmentManager.Instance.GetEquipmentSprite(mon.monsterData.RewardItem[i]);
            }
        }
    }

    private IEnumerator PrintRuneDrop(Monster mon)
    {
        float randomValue = Random.Range(0f, 100f);
        float cardbuff = GameManager.Instance.CardBuff == CardBuffEnum.RuneBuff ? 0.5f : 0;
        int masterBuff = GameManager.Instance.MasterRunePoint != 0 ? 1 : 0;
        float runeDropRate = GameManager.Instance.RuneHashSet.Contains("����� ��") ? 1.5f + cardbuff + masterBuff : 1f + cardbuff + masterBuff;

        if (!GameManager.Instance.RuneHashSet.Contains(mon.monsterData.RewardRune.EquipmentName))
        {
            if (runeDropRate >= randomValue)
            {
                runeDropEffectPanel.SetActive(true);
                effectAnimator.SetTrigger("Impact");

                // �ִϸ��̼��� ���� ���� ���� ���
                yield return new WaitForSeconds(1.5f);

                runeUIPanel.SetActive(true);

                GameManager.Instance.RuneHashSet.Add(mon.monsterData.RewardRune.EquipmentName);
                runeNameTxt.text = mon.monsterData.RewardRune.EquipmentName;
                runeImage.sprite = EquipmentManager.Instance.GetEquipmentSprite(mon.monsterData.RewardRune);
                runeDesTxt.text = mon.monsterData.RewardRune.EquipmentDes;
            }
        }
    }

    private void APDistribute()
    {
        int sumAutoAP = GameManager.Instance.AutoSTR + GameManager.Instance.AutoDEX + GameManager.Instance.AutoLUC + GameManager.Instance.AutoVIT;

        if (sumAutoAP > 0 && GameManager.Instance.CurrentAP >= sumAutoAP)
        {
            // ������ �ڵ� �ɷ�ġ�� ���� �����ִ� ������ ����Ͽ� �����մϴ�.
            float strRatio = (float)GameManager.Instance.AutoSTR / sumAutoAP;
            float dexRatio = (float)GameManager.Instance.AutoDEX / sumAutoAP;
            float lucRatio = (float)GameManager.Instance.AutoLUC / sumAutoAP;
            float vitRatio = (float)GameManager.Instance.AutoVIT / sumAutoAP;

            // �� �ɷ�ġ�� ������ �����Ͽ� �й��մϴ�.
            int distributedSTR = (int)(GameManager.Instance.CurrentAP * strRatio);
            int distributedDEX = (int)(GameManager.Instance.CurrentAP * dexRatio);
            int distributedLUC = (int)(GameManager.Instance.CurrentAP * lucRatio);
            int distributedVIT = (int)(GameManager.Instance.CurrentAP * vitRatio);

            // �� �ɷ�ġ�� �����մϴ�.
            GameManager.Instance.APSTR += distributedSTR;
            GameManager.Instance.APDEX += distributedDEX;
            GameManager.Instance.APLUC += distributedLUC;
            GameManager.Instance.APVIT += distributedVIT;

            // ���� �ɷ�ġ�� �����մϴ�.
            GameManager.Instance.CurrentAP -= distributedSTR + distributedDEX + distributedLUC + distributedVIT;
        }
    }

    private void AfterBattleMonster(Monster mon, bool _isWin)
    {
        if (_isWin)
        {
            for (int i = 0; i < mon.sprites.Length; i++)
            {
                // ������ ��������Ʈ ���İ��� 100���� ����
                Color spriteColor = mon.sprites[i].color;
                spriteColor.a = 0.4f; // ���İ��� 0.5�� ���� (100% ����)
                mon.sprites[i].color = spriteColor;
            }

            if (DataManager.Instance.EliteMonsterDic.ContainsKey(mon.monsterData.MonsterID))
            { // ��ƺô� ����Ʈ ������
                return;
            }
            else
            { // ���� ����Ʈ ���� ��ųʸ��� �ش� Ű�� ���ٸ� (ó�� ��Ҵٸ�)
                if (mon.monsterData.isElite)
                { // ����Ʈ ���Ͷ��
                    DataManager.Instance.EliteMonsterDic.Add(mon.monsterData.MonsterID, true);
                }
                GameManager.Instance.BonusEnergy += mon.monsterData.RewordEnergy;
                GameManager.Instance.CurrentEnergy += mon.monsterData.RewordEnergy;
            }
        }
        else
        {
            mon.MonsterCurHP = mon.MonsterMaxHP;
        }
    }

    private void GemCalculate()
    {
        int masterBuff = GameManager.Instance.MasterGemPoint == 0 ? 0 : GameManager.Instance.MasterGemPoint * 100;
        gemDrop -= masterBuff;
        int randomValue = Random.Range(0, gemDrop);
        if (randomValue == 77)
        {
            gemPanel.SetActive(true);
            GameManager.Instance.Gem++;
        }
    }

    private void MasterLevelCal()
    {
        int masterExp = GameManager.Instance.PlayerLevel / 1000;

        GameManager.Instance.MasterCurrentEXP += masterExp;
        if(GameManager.Instance.MasterCurrentEXP >= GameManager.Instance.MasterRequireEXP)
        {
            GameManager.Instance.MasterLevel++;
            GameManager.Instance.MasterCurrentEXP -= GameManager.Instance.MasterRequireEXP;
            GameManager.Instance.MasterRequireEXP = GameManager.Instance.GetRequireEXPForLevel(GameManager.Instance.MasterLevel);
            PrintLog.Instance.StaticLog("������ ������ ����߽��ϴ�!");
            MasterAP(GameManager.Instance.MasterLevel);
        }
    }

    private void MasterAP(int level)
    {
        switch(level)
        {
            case 2:
                GameManager.Instance.MasterCurrentAP += 10;
                break;
            case 3:
                GameManager.Instance.MasterCurrentAP += 10;
                break;
            case 4:
                GameManager.Instance.MasterCurrentAP += 10;
                break;
            case 5:
                GameManager.Instance.MasterCurrentAP += 15;
                break;
            case 6:
                GameManager.Instance.MasterCurrentAP += 10;
                break;
            case 7:
                GameManager.Instance.MasterCurrentAP += 10;
                break;
            case 8:
                GameManager.Instance.MasterCurrentAP += 10;
                break;
            case 9:
                GameManager.Instance.MasterCurrentAP += 10;
                break;
            case 10:
                GameManager.Instance.MasterCurrentAP += 10;
                break;
            case 11:
                GameManager.Instance.MasterCurrentAP += 11;
                break;
            case 12:
                GameManager.Instance.MasterCurrentAP += 12;
                break;
            case 13:
                GameManager.Instance.MasterCurrentAP += 13;
                break;
            case 14:
                GameManager.Instance.MasterCurrentAP += 14;
                break;
            case 15:
                GameManager.Instance.MasterCurrentAP += 15;
                break;
            case 16:
                GameManager.Instance.MasterCurrentAP += 16;
                break;
            case 17:
                GameManager.Instance.MasterCurrentAP += 17;
                break;
            case 18:
                GameManager.Instance.MasterCurrentAP += 18;
                break;
            case 19:
                GameManager.Instance.MasterCurrentAP += 19;
                break;
            case 20:
                GameManager.Instance.MasterCurrentAP += 20;
                break;
            case 21:
                GameManager.Instance.MasterCurrentAP += 20;
                break;
            case 22:
                GameManager.Instance.MasterCurrentAP += 20;
                break;
            case 23:
                GameManager.Instance.MasterCurrentAP += 20;
                break;
            case 24:
                GameManager.Instance.MasterCurrentAP += 25;
                break;
            case 25:
                GameManager.Instance.MasterCurrentAP += 25;
                break;
            case 26:
                GameManager.Instance.MasterCurrentAP += 25;
                break;
            case 27:
                GameManager.Instance.MasterCurrentAP += 25;
                break;
            case 28:
                GameManager.Instance.MasterCurrentAP += 25;
                break;
            case 29:
                GameManager.Instance.MasterCurrentAP += 30;
                break;
            case 30:
                GameManager.Instance.MasterCurrentAP += 30;
                break;
        }
    }
}
