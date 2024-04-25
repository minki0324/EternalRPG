using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text infomationText;
    [SerializeField] private Image transitionImage;
    [SerializeField] private GameObject transitionObj;
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private GameObject resetWarningPanel;
    [SerializeField] private GameObject cardBuffPanel;
    [SerializeField] private GameObject cardBuffWarningPanel;

    public void TitleStartButton()
    {
        infomationText.text = $"���� : {GameManager.Instance.PlayerLevel:N0}\n" +
                                          $"������ : {GameManager.Instance.Power:N0}\n" +
                                          $"��� : {GameManager.Instance.Gold:N0}\n" +
                                          $"���� ������ : {GameManager.Instance.CurrentEnergy}\n" +
                                          $"ĳ���� ��ġ : {GameManager.Instance.CurrentMapName}\n" +
                                          $"�÷��� ȸ�� : {GameManager.Instance.PlayCount:N0}";
    }

    public void StartButton(bool _isReStart)
    {
        if (!GameManager.Instance.FirstConnect) GameManager.Instance.FirstConnect = true;
        if(_isReStart)
        { // �� ���� ������ �� ��� ����
            warningPanel.SetActive(true);
        }
        else
        { // ���� �ϴ� �� �̾��ϱ�
            if(GameManager.Instance.CardBuff == CardBuffEnum.None)
            {
                cardBuffPanel.SetActive(true);
                warningPanel.SetActive(false);
            }
            else
            {
                StartCoroutine(StartButton_Co());
            }
        }
    }

    public void CardBuffPanel()
    {
        GameManager.Instance.ResetRound();
        TitleStartButton();
        cardBuffPanel.SetActive(true);
        warningPanel.SetActive(false);
    }

    public void CardBuffSelected(int _buffType)
    {
        GameManager.Instance.CardBuff = (CardBuffEnum)_buffType;
    }

    public void ReStartButton()
    {
        if (GameManager.Instance.CardBuff == CardBuffEnum.None)
        { // �Ȱ�����Ƿ� ī�� ����� ��� ����
            cardBuffWarningPanel.SetActive(true);
        }
        else
        {
            GameManager.Instance.RenewAbility();
            StartCoroutine(StartButton_Co());
            warningPanel.SetActive(false);
        }
    }

    private IEnumerator StartButton_Co()
    {
        cardBuffPanel.SetActive(false);
        transitionObj.SetActive(true);
        if (TransitionFade.instance.FadeCoroutine != null)
        {
            StopCoroutine(TransitionFade.instance.FadeCoroutine);
        }
        TransitionFade.instance.FadeCoroutine = StartCoroutine(TransitionFade.instance.fade_out(transitionImage, true));

        // Loading.isLoading�� false�� �� ������ ���
        while (TransitionFade.instance.isLoading)
        {
            yield return null;
        }

        SceneManager.LoadScene(1);
    }

    public void ResetButton()
    {
        if(!GameManager.Instance.FirstConnect)
        {
            resetWarningPanel.SetActive(true);
            return;
        }
        // ��� �ʱ�ȭ
        GameManager.Instance.CardBuff = CardBuffEnum.None;
        GameManager.Instance.WeaponData = GameManager.Instance.Punch;
        GameManager.Instance.ArmorData = null;
        GameManager.Instance.HelmetData = null;
        GameManager.Instance.ShoesData = null;
        GameManager.Instance.GloveData = null;
        GameManager.Instance.PantsData = null;
        GameManager.Instance.ShoulderArmorData = null;
        GameManager.Instance.BeltData = null;
        GameManager.Instance.ClockData = null;
        GameManager.Instance.NecklessData = null;
        for(int i = 0; i < GameManager.Instance.OtherDatas.Length; i++)
        {
            GameManager.Instance.OtherDatas[i] = null;
        }
        for(int i = 0; i < GameManager.Instance.RingDatas.Length; i++)
        {
            GameManager.Instance.RingDatas[i] = null;
        }

        // ����Ʈ ���� �ʱ�ȭ
        DataManager.Instance.EliteMonsterDic.Clear();

        // ��� �������� �ʱ�ȭ
        DataManager.Instance.OwnCountReset();

        // �� ���� �ʱ�ȭ
        DataManager.Instance.QuickSlotReset();

        // ���� ���� �ʱ�ȭ
        GameManager.Instance.DeadMonsterList.Clear();

        // ��ġ �ʱ�ȭ
        GameManager.Instance.StartPos = Vector3.zero;
        GameManager.Instance.LastPos = Vector3.zero;

        // �� �ʱ�ȭ
        GameManager.Instance.RuneHashSet.Clear();

        // ���� �ʱ�ȭ
        GameManager.Instance.BattleSpeed = 1;
        GameManager.Instance.PlayerLevel = 1;
        GameManager.Instance.Gold = 0;
        GameManager.Instance.CurrentEnergy = 25;
        GameManager.Instance.BonusEnergy = 0;
        GameManager.Instance.CurrentEXP = 0;
        GameManager.Instance.RequireEXP = 50;
        GameManager.Instance.PlayCount = 0;
        GameManager.Instance.CurrentAP = 0;
        GameManager.Instance.BonusAP = 0;
        GameManager.Instance.APSTR = 0;
        GameManager.Instance.APDEX = 0;
        GameManager.Instance.APVIT = 0;
        GameManager.Instance.APLUC = 0;
        GameManager.Instance.AutoSTR = 0;
        GameManager.Instance.AutoDEX = 0;
        GameManager.Instance.AutoLUC = 0;
        GameManager.Instance.AutoVIT = 0;
        GameManager.Instance.isAPBook = false;
        GameManager.Instance.isClover = false;
        GameManager.Instance.isFood = false;
        GameManager.Instance.isGoldPack = false;
        GameManager.Instance.CurrentMapName = string.Empty;
        GameManager.Instance.FirstConnect = false;

        // ���� ����
        GameManager.Instance.RenewAbility();
        GameManager.Instance.PlayerCurHP = GameManager.Instance.PlayerMaxHP;
        TitleStartButton();
    }
}
