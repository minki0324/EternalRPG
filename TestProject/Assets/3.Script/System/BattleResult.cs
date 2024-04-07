using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.Mathematics;

public class BattleResult : MonoBehaviour
{
    [SerializeField] private ActiveCanvas activeCanvas;
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
    public GameObject DataPanel = null;
    [SerializeField] private GameObject dropResultItem;
    [SerializeField] private Transform dropResultItemParent;

    private void OnEnable()
    {
        InitData();
    }

    private void InitData()
    {
        if (isWin)
        {
            resultPanelTitle.Result = true;
            AccountBattle_Co();
        }
        else if (!isWin)
        {
            resultPanelTitle.Result = false;
            expText.text = "0";
            // todo ������ �ý��� �־�ߵ�
            levelText.text = "0";
            APText.text = "0";
            goldText.text = "0";
            totalGoldText.text = $"{GameManager.Instance.Gold:N0}";
        }
    }

    private void AccountBattle_Co()
    { // ���� ���
        // ����ġ
        GameManager.Instance.CurrentEXP += mon.monsterData.RewordEXP;
        int levelup = 0;
        while (GameManager.Instance.RequireEXP < GameManager.Instance.CurrentEXP)
        {
            GameManager.Instance.CurrentEXP -= GameManager.Instance.RequireEXP;
            GameManager.Instance.RequireEXP = Mathf.RoundToInt(GameManager.Instance.RequireEXP * 1.02f);
            GameManager.Instance.PlayerLevel++;
            GameManager.Instance.CurrentAP += 5;
            levelup++;
        }

        // ���� �ڵ� �й�
        APDistribute();

        // ���
        int gold = Mathf.RoundToInt(UnityEngine.Random.Range(mon.monsterData.RewordGold * 0.95f, mon.monsterData.RewordGold * 1.05f));
        GameManager.Instance.Gold += gold;

        // ������
        PrintDropItem(mon);

        // ��� ���� ����Ʈ
        expText.text = $"{mon.monsterData.RewordEXP:N0}";
        levelText.text = $"{levelup} UP";
        APText.text = $"+ {levelup * 5}";
        goldText.text = $"{gold:N0}";
        totalGoldText.text = $"{GameManager.Instance.Gold:N0}";
        DataPanel.SetActive(true);
    }

    public void BackButton()
    {
        activeCanvas.resultPanel.SetActive(false);
        activeCanvas.versusPanel.SetActive(false);
        activeCanvas.gameObject.SetActive(false);

        player.playerMove.isFight = false;
        // ���� �Ŀ��� ���Ϳ� ����� ��ġ��ŭ �������� ����
        player.gameObject.transform.position = new Vector2(mon.gameObject.transform.position.x + mon.monsterData.ReturnPos.x,
                                                                                                                        mon.gameObject.transform.position.y + mon.monsterData.ReturnPos.y);

        if(isWin)
        { // todo �������� �¸� ���� �� ���İ� �����ִ°� �����ؾ���
            mon.isDead = true;
            mon.animator.enabled = false;
        }
        else
        {
            mon.isDead = false;
        }
        resultPanelTitle.TitleText.gameObject.SetActive(false);
        DataPanel.SetActive(false);
        PrintLog.Instance.BattleLogClear();
    }

    private void PrintDropItem(Monster mon)
    {
        for (int i = 0; i < mon.monsterData.RewardItem.Length; i++)
        {
            Dictionary<int, int> owndictionary = DataManager.Instance.GetOwnDictionary(mon.monsterData.RewardItem[i]);
            int ownCount = owndictionary.Values.Sum() == 0 ? 1 : owndictionary.Values.Sum();
            int dropRate = (mon.monsterData.RewardItem[i].DropRate + GameManager.Instance.ItemDropRate) / ownCount;
            int randomValue = UnityEngine.Random.Range(0, 100);

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

    private void APDistribute()
    {
        int sumAutoAP = GameManager.Instance.AutoSTR + GameManager.Instance.AutoDEX + GameManager.Instance.AutoLUC + GameManager.Instance.AutoVIT;
        
        if(sumAutoAP > 0 && GameManager.Instance.CurrentAP >=  sumAutoAP)
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
}
