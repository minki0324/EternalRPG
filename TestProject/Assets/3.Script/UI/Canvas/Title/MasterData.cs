using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MasterData : MonoBehaviour
{
    [SerializeField] private TMP_Text MasterInfomationText;
    [SerializeField] private GameObject WarningPanel;
    [SerializeField] private MasterStatusPanel[] panels;
    [SerializeField] private Sprite[] badgeSprites;

    private void OnEnable()
    {
        InitData();
    }

    public void InitData()
    {
        MasterInfomationText.text = $": {GameManager.Instance.MasterLevel}\n" +
                                                             $": {GameManager.Instance.MasterCurrentEXP:N0} / {GameManager.Instance.MasterRequireEXP:N0}\n" +
                                                             $": {GameManager.Instance.MasterCurrentAP:N0}";
    }

    public void PointReset()
    {
        GameManager.Instance.MasterCurrentAP += GameManager.Instance.MasterRunePoint * 200;
        GameManager.Instance.MasterCurrentAP += GameManager.Instance.MasterBonusAPPoint * 100;
        GameManager.Instance.MasterCurrentAP += GameManager.Instance.MasterDropPoint;
        GameManager.Instance.MasterCurrentAP += GameManager.Instance.MasterMovePoint * 10;
        GameManager.Instance.MasterCurrentAP += GameManager.Instance.MasterEnergyPoint * 30;
        GameManager.Instance.MasterCurrentAP += GameManager.Instance.MasterGemPoint * 10;

        GameManager.Instance.MasterRunePoint = 0;
        GameManager.Instance.MasterBonusAPPoint = 0;
        GameManager.Instance.MasterDropPoint = 0;
        GameManager.Instance.MasterMovePoint = 0;
        GameManager.Instance.BonusEnergy -= GameManager.Instance.MasterEnergyPoint;
        GameManager.Instance.MasterEnergyPoint = 0;
        GameManager.Instance.MasterGemPoint = 0;

        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].RenewMasterStat();
        }
        InitData();
        WarningPanel.SetActive(false);
    }
}
