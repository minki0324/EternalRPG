using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class HUDCanvas : MonoBehaviour
{
    [SerializeField] private Image transitionImage;
    [SerializeField] private GameObject transitionObj;
    [SerializeField] private GameObject activeCanvas;

    [Header("정보")]
    public TMP_Text mapText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text gemText;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private Slider HPSlider;

    [Header("카드 버프")]
    [SerializeField] private Sprite[] cardBuffSprite;
    [SerializeField] private Image cardBuffImage;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardDesText;

    [Header("영혼 수집상자")]
    public GameObject soulChestObj;
    public TMP_Text soulDesText;

    [Header("젬 천장")]
    [SerializeField] private GameObject buyingGemPanel;
    private int buyingGemCount;
    [SerializeField] private TMP_Text gemCountText;
    [SerializeField] private TMP_Text goldCostText;

    [Header("뱃지 이펙트")]
    [SerializeField] private GameObject badgeEffectPanel;
    [SerializeField] private GameObject badgeInfoPanel;
    [SerializeField] private Image badgeImage;
    [SerializeField] private TMP_Text badgeText;
    [SerializeField] private Animator badgeEffect;

    private void Start()
    {
        StartGame();
    }

    private void LateUpdate()
    {
        levelText.text = $"{GameManager.Instance.PlayerLevel:N0}Lv";
        goldText.text = $"{GameManager.Instance.Gold:N0}";
        energyText.text = $"{GameManager.Instance.CurrentEnergy:N0}";
        gemText.text = $"{GameManager.Instance.Gem:N0}";
        HPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
        HPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP:N0}";
    }

    private void StartGame()
    {
        activeCanvas.SetActive(true);
        transitionObj.SetActive(true);
        if (DataManager.Instance.EliteMonsterDic.Count > 0)
        {
            soulChestObj.SetActive(true);
            soulDesText.text = $"처치한 엘리트 몬스터 : {DataManager.Instance.EliteMonsterDic.Count(kv => kv.Value == true)}\n" +
                                          $"추가 흡혈 확률&저항 : {DataManager.Instance.EliteMonsterDic.Count(kv => kv.Value == true) * 3}%";
        }

        GameManager.Instance.LastPos = GameManager.Instance.StartPos;

        cardBuffImage.sprite = cardBuffSprite[(int)GameManager.Instance.CardBuff];
        CardBuffToolTip();
        transitionObj.SetActive(false);
        activeCanvas.SetActive(false);
    }

    private void CardBuffToolTip()
    {
        switch (GameManager.Instance.CardBuff)
        {
            case CardBuffEnum.None:
                break;
            case CardBuffEnum.RuneBuff:
                cardNameText.text = "룬의 부적";
                cardDesText.text = "이번 회차 룬 드랍율이 50% 증가합니다.";
                break;
            case CardBuffEnum.BonusAPBuff:
                cardNameText.text = "능력의 부적";
                cardDesText.text = "이번 회차 레벨업당 추가 능력치가 1 증가합니다.";
                break;
            case CardBuffEnum.EXPandGoldBuff:
                cardNameText.text = "경험의 부적";
                cardDesText.text = "이번 회차 경험치 및 골드 획득량이 20% 증가합니다.";
                break;
            case CardBuffEnum.DropBuff:
                cardNameText.text = "행운의 부적";
                cardDesText.text = "이번 회차 고정 아이템 드랍율이 3% 증가합니다.";
                break;
        }
    }

    public void LackOfEnergy()
    {
        if (GameManager.Instance.CurrentEnergy <= 0)
        {
            StartCoroutine(LackOfEnergy_Co());
        }
    }

    private IEnumerator LackOfEnergy_Co()
    {
       GameManager.Instance.ResetRound();

        activeCanvas.SetActive(true);
        transitionObj.SetActive(true);
        if (TransitionFade.instance.FadeCoroutine != null)
        {
            StopCoroutine(TransitionFade.instance.FadeCoroutine);
        }
        TransitionFade.instance.FadeCoroutine = StartCoroutine(TransitionFade.instance.fade_out(transitionImage, true));

        while (TransitionFade.instance.isLoading)
        {
            yield return null;
        }

        SceneManager.LoadScene(0);
    }

    public void MainSceneLoad()
    {
        StartCoroutine(MainSceneLoad_Co());
    }

    private IEnumerator MainSceneLoad_Co()
    {
        activeCanvas.SetActive(true);
        transitionObj.SetActive(true);
        if (TransitionFade.instance.FadeCoroutine != null)
        {
            StopCoroutine(TransitionFade.instance.FadeCoroutine);
        }
        TransitionFade.instance.FadeCoroutine = StartCoroutine(TransitionFade.instance.fade_out(transitionImage, true));

        while (TransitionFade.instance.isLoading)
        {
            yield return null;
        }

        SceneManager.LoadScene(0);
    }

    public void GemPanelOpen()
    {
        gemCountText.text = $"{GameManager.Instance.GemCount} / 100";
        goldCostText.text = $"100,000";
        buyingGemPanel.SetActive(true);
    }

    public void GemBuyButton()
    {
        switch(buyingGemCount)
        {
            case 0:
                BuyGemCal(100000, 4800000);
                break;
            case 1:
                BuyGemCal(4800000, 53000000);
                break;
            case 2:
                BuyGemCal(53000000, 145000000);
                break;
            case 3:
                BuyGemCal(145000000, 1200000000);
                break;
            case 4:
                BuyGemCal(1200000000, 0);
                break;
            default:
                BuyGemCal(0, 0);
                break;
        }
    }

    private void BuyGemCal(int _requireCost, int _nextCost)
    {
        if (buyingGemCount >= 5)
        {
            PrintLog.Instance.StaticLog("더 이상 구매가 불가능합니다.");
            return;
        }

        if (GameManager.Instance.Gold >= _requireCost)
        {
            GameManager.Instance.GemCount++;
            if (GameManager.Instance.GemCount == 100)
            { // 젬 추가해주기
                PrintLog.Instance.StaticLog("카운트가 100이 되어 젬을 1개 획득합니다.");
                GameManager.Instance.GemCount -= 100;
                GameManager.Instance.Gem++;
            }
            else
            { // 카운트 증가 로그 띄우기
                PrintLog.Instance.StaticLog("젬 카운트가 증가합니다.");
            }
            gemCountText.text = $"{GameManager.Instance.GemCount} / 100";
            buyingGemCount++;
            if(buyingGemCount >= 5)
            {
                goldCostText.text = $"더 이상 구매가 불가능합니다.";
            }
            else
            {
                goldCostText.text = $"{_nextCost:N0}";
            }
        }
        else
        {
            PrintLog.Instance.StaticLog("골드가 부족합니다.");
        }
    }

    public void CheckBadgeCount()
    {
        int totalOwnCount = DataManager.Instance.GetOwnCount();

        if (totalOwnCount == 150 || totalOwnCount == 300 || totalOwnCount == 450 || totalOwnCount == 600 || totalOwnCount == 750 || totalOwnCount == 900 || totalOwnCount == 1050 || totalOwnCount == 1200) StartCoroutine(BadgeEffect(totalOwnCount));
        else return;
    }

    public IEnumerator BadgeEffect(int totalOwnCount)
    {
        badgeEffectPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        badgeEffect.SetTrigger("Badge");
        yield return new WaitForSeconds(1.2f);
        switch(totalOwnCount)
        {
            case 150: // 구리
                badgeImage.sprite = EquipmentManager.Instance.GetBadgeSprite(DataManager.Instance.badgeDatas[1]);
                badgeText.text = "구리 등급";
                badgeText.color = new Color(217, 162, 134);
                break;
            case 300: // 실버
                badgeImage.sprite = EquipmentManager.Instance.GetBadgeSprite(DataManager.Instance.badgeDatas[2]);
                badgeText.text = "실버 등급";
                badgeText.color = new Color(178, 188, 195);
                break;
            case 450: // 골드
                badgeImage.sprite = EquipmentManager.Instance.GetBadgeSprite(DataManager.Instance.badgeDatas[3]);
                badgeText.text = "골드 등급";
                badgeText.color = new Color(253, 186, 65);
                break;
            case 600: // 플래티넘
                badgeImage.sprite = EquipmentManager.Instance.GetBadgeSprite(DataManager.Instance.badgeDatas[4]);
                badgeText.text = "플래티넘 등급";
                badgeText.color = new Color(181, 173, 212);
                break;
            case 750: // 유니크
                badgeImage.sprite = EquipmentManager.Instance.GetBadgeSprite(DataManager.Instance.badgeDatas[5]);
                badgeText.text = "유니크 등급";
                badgeText.color = new Color(166, 117, 218);
                break;
            case 900: // 레전더리
                badgeImage.sprite = EquipmentManager.Instance.GetBadgeSprite(DataManager.Instance.badgeDatas[6]);
                badgeText.text = "레전더리 등급";
                badgeText.color = new Color(170, 249, 180);
                break;
            case 1050: // 루나틱
                badgeImage.sprite = EquipmentManager.Instance.GetBadgeSprite(DataManager.Instance.badgeDatas[7]);
                badgeText.text = "루나틱 등급";
                badgeText.color = new Color(252, 147, 189);
                break;
            case 1200: // 이터널
                badgeImage.sprite = EquipmentManager.Instance.GetBadgeSprite(DataManager.Instance.badgeDatas[8]);
                badgeText.text = "이터널 등급";
                badgeText.color = new Color(202, 231, 249);
                break;
        }
        badgeInfoPanel.SetActive(true);
    }

    public void Tooltip(GameObject target)
    {
        bool active = target.activeSelf;

        target.SetActive(!active);
    }
}
