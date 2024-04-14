using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class BattleResult : MonoBehaviour
{
    [SerializeField] private ActiveCanvas activeCanvas;
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
            expText.text = ": 0";
            // todo 레벨업 시스템 넣어야됨
            levelText.text = ": 0";
            APText.text = ": 0";
            goldText.text = ": 0";
            totalGoldText.text = $": {GameManager.Instance.Gold:N0}";
            if(mon.monsterData.isElite)
            {
                energyText.text = $": {mon.monsterData.RewordEnergy}";
            }
            else
            {
                energyText.text = $": {mon.monsterData.RewordEnergy*2}";
            }
        }
        AfterBattleMonster(mon, isWin);
    }

    private void AccountBattle_Co()
    { // 전투 결산
        // 경험치
        int EXP = Mathf.RoundToInt((float)(mon.monsterData.RewordEXP * (1 + (float)(GameManager.Instance.EXPPercent/ 100))));
        EXP = Mathf.RoundToInt(Random.Range(EXP * 0.95f, EXP * 1.05f));
        GameManager.Instance.CurrentEXP += EXP;
        int levelup = 0;
        float requireEXP = 0f;
        int quickSlotBook = GameManager.Instance.isAPBook ? 1 : 0;
        while (GameManager.Instance.RequireEXP < GameManager.Instance.CurrentEXP)
        {
            // 곱연산 계산 
            requireEXP = Mathf.RoundToInt((float)(GameManager.Instance.RequireEXP * 1.001f)) == GameManager.Instance.RequireEXP ?
                                                        GameManager.Instance.RequireEXP + 1  : Mathf.RoundToInt((float)(GameManager.Instance.RequireEXP * 1.001f));
            GameManager.Instance.CurrentEXP -= GameManager.Instance.RequireEXP;
            GameManager.Instance.RequireEXP = Mathf.RoundToInt(requireEXP);
            GameManager.Instance.PlayerLevel++;
            GameManager.Instance.CurrentAP += 5 + quickSlotBook + GameManager.Instance.BonusAP;
            levelup++;
        }

        // 스텟 자동 분배
        APDistribute();

        // 골드
        int gold = Mathf.RoundToInt(UnityEngine.Random.Range(mon.monsterData.RewordGold * 0.95f, mon.monsterData.RewordGold * 1.05f));
        float quickSlotGold = GameManager.Instance.isGoldPack ? 30 : 0;
        int totalGold = Mathf.RoundToInt((float)(gold * (1 + ((float)(GameManager.Instance.GoldPercent / 100) + (float)(quickSlotGold / 100)))));
        GameManager.Instance.Gold += totalGold;

        // 아이템
        PrintDropItem(mon);

        // 젬
        GemCalculate();

        // 스텟 갱신
        GameManager.Instance.RenewAbility();

        // 결산 내용 프린트
        expText.text = $": {EXP:N0}";
        levelText.text = $": {levelup:N0} UP";
        APText.text = $": +{levelup * 5}";
        goldText.text = $": {totalGold:N0}";
        totalGoldText.text = $": {GameManager.Instance.Gold:N0}";
        if(mon.monsterData.isElite)
        { // 엘리트 몬스터라면 리워드 에너지 출력
            energyText.text = $": +{mon.monsterData.RewordEnergy}";
        }
        else
        { // 일반 몹이라면 소모 에너지만 출력
            energyText.text = $": {mon.monsterData.RequireEnergy}";
        }
        DataPanel.SetActive(true);
    }

    public void BackButton()
    {
        if (GameManager.Instance.CurrentEnergy <= 0)
        {
            // 에너지 없어서 라운드 종료될 때 초기화
            gameObject.SetActive(false);
            activeCanvas.gameObject.SetActive(true);
            inventoryPanel.SetActive(true);
        }
        else
        { // 에너지가 남아있을 때
            activeCanvas.resultPanel.SetActive(false);
            activeCanvas.versusPanel.SetActive(false);
            activeCanvas.gameObject.SetActive(false);

            player.playerMove.isFight = false;
            // 전투 후에는 몬스터에 저장된 위치만큼 물러나서 시작
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

            for(int i = 0; i < dropResultItemParent.childCount; i++)
            { // 드랍 장비 초기화
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
            int ownCount = owndictionary.Values.Sum() == 0 ? 1 : owndictionary.Values.Sum();
            float quickSlotDrop = GameManager.Instance.isClover ? 70 : 0;
            float addDropRate = (float)(mon.monsterData.RewardItem[i].DropRate * (1 + (float)(GameManager.Instance.ItemDropRate / 100) + (float)(quickSlotDrop / 100)));
            float dropRate = (float)((mon.monsterData.RewardItem[i].DropRate + addDropRate) / ownCount);
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

    private void AfterBattleMonster(Monster mon, bool _isWin)
    {
        if (_isWin)
        {
            for (int i = 0; i < mon.sprites.Length; i++)
            {
                // 몬스터의 스프라이트 알파값을 100으로 설정
                Color spriteColor = mon.sprites[i].color;
                spriteColor.a = 0.4f; // 알파값을 0.5로 설정 (100% 투명도)
                mon.sprites[i].color = spriteColor;
            }

            if (DataManager.Instance.EliteMonsterDic.ContainsKey(mon.monsterData.MonsterID))
            { // 잡아봤던 엘리트 몬스터임
                return;
            }
            else
            { // 만약 엘리트 몬스터 딕셔너리에 해당 키가 없다면 (처음 잡았다면)
                if (mon.monsterData.isElite)
                { // 엘리트 몬스터라면
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

    public void LackOfEnergy()
    {
        if(GameManager.Instance.CurrentEnergy <= 0)
        {
            GameManager.Instance.isAPBook = false;
            GameManager.Instance.isClover = false;
            GameManager.Instance.isFood = false;
            GameManager.Instance.isGoldPack = false;

            GameManager.Instance.PlayCount++;
            GameManager.Instance.APDEX = 0;
            GameManager.Instance.APLUC = 0;
            GameManager.Instance.APSTR = 0;
            GameManager.Instance.APVIT = 0;
            GameManager.Instance.Gold = 0;
            GameManager.Instance.PlayerLevel = 1;
            GameManager.Instance.CurrentEXP = 0;
            GameManager.Instance.RequireEXP = 50;
            GameManager.Instance.CurrentEnergy = 25 + GameManager.Instance.BonusEnergy;
            GameManager.Instance.RenewAbility();

            SceneManager.LoadScene(0);
        }
    }

    private void GemCalculate()
    {
        int randomValue = Random.Range(0, 2000);
        if(randomValue == 77)
        {
            gemPanel.SetActive(true);
            GameManager.Instance.Gem++;
        }
    }
}
