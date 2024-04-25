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
        infomationText.text = $"레벨 : {GameManager.Instance.PlayerLevel:N0}\n" +
                                          $"전투력 : {GameManager.Instance.Power:N0}\n" +
                                          $"골드 : {GameManager.Instance.Gold:N0}\n" +
                                          $"남은 에너지 : {GameManager.Instance.CurrentEnergy}\n" +
                                          $"캐릭터 위치 : {GameManager.Instance.CurrentMapName}\n" +
                                          $"플레이 회수 : {GameManager.Instance.PlayCount:N0}";
    }

    public void StartButton(bool _isReStart)
    {
        if (!GameManager.Instance.FirstConnect) GameManager.Instance.FirstConnect = true;
        if(_isReStart)
        { // 새 게임 눌렀을 때 경고문 띄우기
            warningPanel.SetActive(true);
        }
        else
        { // 원래 하던 거 이어하기
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
        { // 안골랐으므로 카드 고르라는 경고 띄우기
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

        // Loading.isLoading이 false가 될 때까지 대기
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
        // 장비 초기화
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

        // 엘리트 몬스터 초기화
        DataManager.Instance.EliteMonsterDic.Clear();

        // 장비 보유개수 초기화
        DataManager.Instance.OwnCountReset();

        // 퀵 슬롯 초기화
        DataManager.Instance.QuickSlotReset();

        // 잡은 몬스터 초기화
        GameManager.Instance.DeadMonsterList.Clear();

        // 위치 초기화
        GameManager.Instance.StartPos = Vector3.zero;
        GameManager.Instance.LastPos = Vector3.zero;

        // 룬 초기화
        GameManager.Instance.RuneHashSet.Clear();

        // 상태 초기화
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

        // 정보 갱신
        GameManager.Instance.RenewAbility();
        GameManager.Instance.PlayerCurHP = GameManager.Instance.PlayerMaxHP;
        TitleStartButton();
    }
}
