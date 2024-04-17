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
    [SerializeField] private GameObject warningPanel;
    public void TitleStartButton()
    {
        infomationText.text = $"���� : {GameManager.Instance.PlayerLevel:N0}\n" +
                                          $"������ : {GameManager.Instance.Power:N0}\n" +
                                          $"��� : {GameManager.Instance.Gold:N0}\n" +
                                          $"���� ������ : {GameManager.Instance.CurrentEnergy}\n" +
                                          $"ĳ���� ��ġ : \n" +
                                          $"�÷��� ȸ�� : {GameManager.Instance.PlayCount:N0}";
    }

    public void StartButton(bool _isReStart)
    {
        if(_isReStart)
        { // �� ���� ������ �� ��� ����
            warningPanel.SetActive(true);
        }
        else
        { // ���� �ϴ� �� �̾��ϱ�
            StartCoroutine(StartButton_Co(_isReStart));
        }
    }

    public void ReStartButton(bool _isRestart)
    {
        StartCoroutine(StartButton_Co(_isRestart));
        warningPanel.SetActive(false);
    }

    private IEnumerator StartButton_Co(bool _isReStart)
    {
        transitionObj.SetActive(true);
        StartCoroutine(loading.StartLoadingLight());

        // �� ���� ��ư�� ���
        if (_isReStart) GameManager.Instance.ResetRound();

        // Loading.isLoading�� false�� �� ������ ���
        while (loading.isLoading)
        {
            yield return null;
        }

        SceneManager.LoadScene(1);
    }

    public void ResetButton()
    {
        // ��� �ʱ�ȭ
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

        // ���� ����
        GameManager.Instance.RenewAbility();
        GameManager.Instance.PlayerCurHP = GameManager.Instance.PlayerMaxHP;
        TitleStartButton();
    }
}
