using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text infomationText;
    public void TitleStartButton()
    {
        infomationText.text = $"���� : {GameManager.Instance.PlayerLevel:N0}\n" +
                                          $"������ : {GameManager.Instance.Power:N0}\n" +
                                          $"��� : {GameManager.Instance.Gold:N0}\n" +
                                          $"���� ������ : {GameManager.Instance.Energy}\n" +
                                          $"ĳ���� ��ġ : \n" +
                                          $"�÷��� ȸ�� : {GameManager.Instance.PlayCount:N0}";
    }

    public void StartButton()
    {
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

        // ���� �ʱ�ȭ
        GameManager.Instance.PlayerLevel = 1;
        GameManager.Instance.Gold = 0;
        GameManager.Instance.Energy = 25;
        GameManager.Instance.PlayCount = 0;
        GameManager.Instance.APSTR = 0;
        GameManager.Instance.APDEX = 0;
        GameManager.Instance.APVIT = 0;
        GameManager.Instance.APLUC = 0;

        // ���� ����
        GameManager.Instance.RenewAbility();
        TitleStartButton();
    }
}
