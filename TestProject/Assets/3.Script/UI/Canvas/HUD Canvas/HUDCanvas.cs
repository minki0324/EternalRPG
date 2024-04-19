using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDCanvas : MonoBehaviour
{
    [SerializeField] private ScreenTransitionSimpleSoft loading;
    [SerializeField] private GameObject transitionObj;
    [SerializeField] private GameObject activeCanvas;

    [Header("����")]
    public TMP_Text mapText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text gemText;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private Slider HPSlider;

    [Header("ī�� ����")]
    [SerializeField] private Sprite[] cardBuffSprite;
    [SerializeField] private Image cardBuffImage;

    [Header("�� õ��")]
    [SerializeField] private GameObject buyingGemPanel;
    private int buyingGemCount;
    [SerializeField] private TMP_Text gemCountText;
    [SerializeField] private TMP_Text goldCostText;

    private void Start()
    {
        StartCoroutine(StartGame());
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

    private IEnumerator StartGame()
    {
        activeCanvas.SetActive(true);
        transitionObj.SetActive(true);
        loading.SetTransitioning(true);
        StartCoroutine(loading.StartLoadingDark());

        // Loading.isLoading�� false�� �� ������ ���
        while (loading.isLoading)
        {
            yield return null;
        }

        cardBuffImage.sprite = cardBuffSprite[(int)GameManager.Instance.CardBuff];
        transitionObj.SetActive(false);
        activeCanvas.SetActive(false);
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
        loading.SetTransitioning(true);
        StartCoroutine(loading.StartLoadingLight());

        while (loading.isLoading)
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
        loading.SetTransitioning(true);
        StartCoroutine(loading.StartLoadingLight());

        while (loading.isLoading)
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
            PrintLog.Instance.StaticLog("�� �̻� ���Ű� �Ұ����մϴ�.");
            return;
        }

        if (GameManager.Instance.Gold >= _requireCost)
        {
            GameManager.Instance.GemCount++;
            if (GameManager.Instance.GemCount == 100)
            { // �� �߰����ֱ�
                PrintLog.Instance.StaticLog("ī��Ʈ�� 100�� �Ǿ� ���� 1�� ȹ���մϴ�.");
                GameManager.Instance.GemCount -= 100;
                GameManager.Instance.Gem++;
            }
            else
            { // ī��Ʈ ���� �α� ����
                PrintLog.Instance.StaticLog("�� ī��Ʈ�� �����մϴ�.");
            }
            gemCountText.text = $"{GameManager.Instance.GemCount} / 100";
            buyingGemCount++;
            if(buyingGemCount >= 5)
            {
                goldCostText.text = $"�� �̻� ���Ű� �Ұ����մϴ�.";
            }
            else
            {
                goldCostText.text = $"{_nextCost:N0}";
            }
        }
        else
        {
            PrintLog.Instance.StaticLog("��尡 �����մϴ�.");
        }
    }
}
