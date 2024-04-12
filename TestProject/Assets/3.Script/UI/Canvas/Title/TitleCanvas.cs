using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text infomationText;
    [SerializeField] private ScreenTransitionSimpleSoft loading;
    [SerializeField] private GameObject transitionObj;
    public void TitleStartButton()
    {
        infomationText.text = $"레벨 : {GameManager.Instance.PlayerLevel:N0}\n" +
                                          $"전투력 : {GameManager.Instance.Power:N0}\n" +
                                          $"골드 : {GameManager.Instance.Gold:N0}\n" +
                                          $"남은 에너지 : {GameManager.Instance.CurrentEnergy}\n" +
                                          $"캐릭터 위치 : \n" +
                                          $"플레이 회수 : {GameManager.Instance.PlayCount:N0}";

        
    }

    public void StartButton()
    {
        StartCoroutine(StartButton_Co());
    }

    private IEnumerator StartButton_Co()
    {
        transitionObj.SetActive(true);
        StartCoroutine(loading.StartLoadingLight());

        // Loading.isLoading이 false가 될 때까지 대기
        while (loading.isLoading)
        {
            yield return null;
        }

        SceneManager.LoadScene(1);
    }

    public void ResetButton()
    {
        // 장비 초기화
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

        // 상태 초기화
        GameManager.Instance.PlayerLevel = 1;
        GameManager.Instance.Gold = 0;
        GameManager.Instance.CurrentEnergy = 25;
        GameManager.Instance.BonusEnergy = 0;
        GameManager.Instance.CurrentEXP = 0;
        GameManager.Instance.RequireEXP = 50;
        GameManager.Instance.PlayCount = 0;
        GameManager.Instance.APSTR = 0;
        GameManager.Instance.APDEX = 0;
        GameManager.Instance.APVIT = 0;
        GameManager.Instance.APLUC = 0;
        GameManager.Instance.isAPBook = false;
        GameManager.Instance.isClover = false;
        GameManager.Instance.isFood = false;
        GameManager.Instance.isGoldPack = false;

        // 정보 갱신
        GameManager.Instance.RenewAbility();
        GameManager.Instance.PlayerCurHP = GameManager.Instance.PlayerMaxHP;
        TitleStartButton();
    }
}
