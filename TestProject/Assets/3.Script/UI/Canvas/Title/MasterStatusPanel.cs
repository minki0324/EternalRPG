using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum MasterStat
{
    Rune,
    BonusAP,
    Drop,
    Move,
    Energy,
    Gem
}

public class MasterStatusPanel : MonoBehaviour
{
    public MasterStat masterStat;
    public TMP_Text StatText;
    public MasterData data;

    private void OnEnable()
    {
        RenewMasterStat();
    }

    public void RenewMasterStat()
    {
        switch (masterStat)
        {
            case MasterStat.Rune:
                StatText.text = $": {GameManager.Instance.MasterRunePoint}";
                break;
            case MasterStat.BonusAP:
                StatText.text = $": {GameManager.Instance.MasterBonusAPPoint}";
                break;
            case MasterStat.Drop:
                StatText.text = $": {GameManager.Instance.MasterDropPoint}";
                break;
            case MasterStat.Move:
                StatText.text = $": {GameManager.Instance.MasterMovePoint}";
                break;
            case MasterStat.Energy:
                StatText.text = $": {GameManager.Instance.MasterEnergyPoint}";
                break;
            case MasterStat.Gem:
                StatText.text = $": {GameManager.Instance.MasterGemPoint}";
                break;
        }
    }

    public void StatButton(string _type)
    {
        switch(_type)
        {
            case "Rune":
                if(GameManager.Instance.MasterCurrentAP >= 200 && GameManager.Instance.MasterRunePoint < 1)
                {
                    GameManager.Instance.MasterRunePoint++;
                    GameManager.Instance.MasterCurrentAP -= 200;
                }
                else
                { // 포인트 부족 or 최대치

                }
                break;
            case "BonusAP":
                if (GameManager.Instance.MasterCurrentAP >= 100 && GameManager.Instance.MasterBonusAPPoint < 3)
                {
                    GameManager.Instance.MasterBonusAPPoint++;
                    GameManager.Instance.MasterCurrentAP -= 100;
                }
                else
                { // 포인트 부족 or 최대치

                }
                break;
            case "Drop":
                if (GameManager.Instance.MasterCurrentAP >= 1 && GameManager.Instance.MasterDropPoint < 200)
                {
                    GameManager.Instance.MasterDropPoint++;
                    GameManager.Instance.MasterCurrentAP --;
                }
                else
                { // 포인트 부족 or 최대치

                }
                break;
            case "Move":
                if (GameManager.Instance.MasterCurrentAP >= 10 && GameManager.Instance.MasterMovePoint < 10)
                {
                    GameManager.Instance.MasterMovePoint++;
                    GameManager.Instance.MasterCurrentAP -= 10;
                }
                else
                { // 포인트 부족 or 최대치

                }
                break;
            case "Energy":
                if (GameManager.Instance.MasterCurrentAP >= 30 && GameManager.Instance.MasterEnergyPoint < 10)
                {
                    GameManager.Instance.MasterEnergyPoint++;
                    GameManager.Instance.BonusEnergy++;
                    GameManager.Instance.MasterCurrentAP -= 30;
                }
                else
                { // 포인트 부족 or 최대치

                }
                break;
            case "Gem":
                if (GameManager.Instance.MasterCurrentAP >= 10 && GameManager.Instance.MasterGemPoint < 10)
                {
                    GameManager.Instance.MasterGemPoint++;
                    GameManager.Instance.MasterCurrentAP -= 10;
                }
                else
                { // 포인트 부족 or 최대치

                }
                break;
        }
        data.InitData();
        RenewMasterStat();
    }
}