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
            // todo 레벨업 시스템 넣어야됨
            levelText.text = "0";
            APText.text = "0";
            goldText.text = "0";
            totalGoldText.text = $"{GameManager.Instance.Gold:N0}";
        }
    }

    private void AccountBattle_Co()
    { // 전투 결산
        // 경험치
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

        // 스텟 자동 분배
        APDistribute();

        // 골드
        int gold = Mathf.RoundToInt(UnityEngine.Random.Range(mon.monsterData.RewordGold * 0.95f, mon.monsterData.RewordGold * 1.05f));
        GameManager.Instance.Gold += gold;

        // 아이템
        PrintDropItem(mon);

        // 결산 내용 프린트
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
        // 전투 후에는 몬스터에 저장된 위치만큼 물러나서 시작
        player.gameObject.transform.position = new Vector2(mon.gameObject.transform.position.x + mon.monsterData.ReturnPos.x,
                                                                                                                        mon.gameObject.transform.position.y + mon.monsterData.ReturnPos.y);

        if(isWin)
        { // todo 전투에서 승리 했을 시 알파값 낮춰주는거 개발해야함
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
            { // 드랍율이 100% 이상이거나 랜덤값 보다 높다면 드랍
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
            // 각각의 자동 능력치에 대해 나눠주는 비율을 계산하여 적용합니다.
            float strRatio = (float)GameManager.Instance.AutoSTR / sumAutoAP;
            float dexRatio = (float)GameManager.Instance.AutoDEX / sumAutoAP;
            float lucRatio = (float)GameManager.Instance.AutoLUC / sumAutoAP;
            float vitRatio = (float)GameManager.Instance.AutoVIT / sumAutoAP;

            // 각 능력치에 비율을 적용하여 분배합니다.
            int distributedSTR = (int)(GameManager.Instance.CurrentAP * strRatio);
            int distributedDEX = (int)(GameManager.Instance.CurrentAP * dexRatio);
            int distributedLUC = (int)(GameManager.Instance.CurrentAP * lucRatio);
            int distributedVIT = (int)(GameManager.Instance.CurrentAP * vitRatio);

            // 각 능력치를 적용합니다.
            GameManager.Instance.APSTR += distributedSTR;
            GameManager.Instance.APDEX += distributedDEX;
            GameManager.Instance.APLUC += distributedLUC;
            GameManager.Instance.APVIT += distributedVIT;

            // 남은 능력치를 갱신합니다.
            GameManager.Instance.CurrentAP -= distributedSTR + distributedDEX + distributedLUC + distributedVIT;
        }
    }
}
