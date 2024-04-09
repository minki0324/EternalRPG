using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoPanel : MonoBehaviour
{
    public Stat stat;
    [SerializeField] private TMP_Text AutoNumText;
    public int AutoNum;

    private void OnEnable()
    {
        switch (stat)
        {
            case Stat.STR:
                AutoNumText.text = GameManager.Instance.AutoSTR.ToString();
                break;
            case Stat.DEX:
                AutoNumText.text = GameManager.Instance.AutoDEX.ToString();
                break;
            case Stat.LUC:
                AutoNumText.text = GameManager.Instance.AutoLUC.ToString();
                break;
            case Stat.VIT:
                AutoNumText.text = GameManager.Instance.AutoVIT.ToString();
                break;
        }
    }

        public void IncreaseButton()
    {
        AutoNum = Mathf.Min(20, AutoNum + 1);
        AutoNumText.text = AutoNum.ToString();
        switch (stat)
        {
            case Stat.STR:
                GameManager.Instance.AutoSTR = AutoNum;
                break;
            case Stat.DEX:
                GameManager.Instance.AutoDEX = AutoNum;
                break;
            case Stat.LUC:
                GameManager.Instance.AutoLUC = AutoNum;
                break;
            case Stat.VIT:
                GameManager.Instance.AutoVIT = AutoNum;
                break;
        }
    }

        public void DecreaseButton()
    {
        AutoNum = Mathf.Max(0, AutoNum - 1);
        AutoNumText.text = AutoNum.ToString();
        switch (stat)
        {
            case Stat.STR:
                GameManager.Instance.AutoSTR = AutoNum;
                break;
            case Stat.DEX:
                GameManager.Instance.AutoDEX = AutoNum;
                break;
            case Stat.LUC:
                GameManager.Instance.AutoLUC = AutoNum;
                break;
            case Stat.VIT:
                GameManager.Instance.AutoVIT = AutoNum;
                break;
        }
    }
}