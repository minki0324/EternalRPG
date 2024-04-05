using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text defaultData;
    [SerializeField] private TMP_Text basicData1;
    [SerializeField] private TMP_Text basicData2;
    [SerializeField] private TMP_Text percentData1;
    [SerializeField] private TMP_Text percentData2;
    [SerializeField] private TMP_Text AddData1;
    [SerializeField] private TMP_Text AddData2;

    private void OnEnable()
    {
        GameManager.Instance.RenewAbility();
        InitData();
    }

    private void InitData()
    {
        defaultData.text = $"레벨 : {GameManager.Instance.PlayerLevel:N0}\n" +
                                            $"경험치 : {GameManager.Instance.CurrentEXP:N0} / {GameManager.Instance.RequireEXP:N0}";
        basicData1.text = $"공격력 : {GameManager.Instance.PlayerATK:N0}\n" +
                                         $"체력 : {GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP:N0}\n" +
                                         $"방어력 : {GameManager.Instance.PlayerDef:N0}\n" +
                                         $"이동속도 : {GameManager.Instance.MoveSpeed:N0}";
        basicData2.text = $"공격력% : {GameManager.Instance.PlayerATKPercent:N0}%\n" +
                                          $"체력% : {GameManager.Instance.PlayerHPPercent:N0}%\n" +
                                          $"방어력% : {GameManager.Instance.PlayerDefPercent:N0}%";
        percentData1.text = $"크리티컬 확률 : {GameManager.Instance.CriticalPercant:N0}%\n" +
                                             $"크리티컬 저항 : {GameManager.Instance.CriticalResist:N0}%\n" +
                                             $"크리티컬 데미지 : {GameManager.Instance.CriticalDamage:N0}%\n" +
                                             $"연타 확률 : {GameManager.Instance.ComboPercent:N0}%\n" +
                                             $"연타 저항 : {GameManager.Instance.ComboResist:N0}%";
        percentData2.text = $"흡혈 확률 : {GameManager.Instance.DrainPercent:N0}%\n" +
                                              $"흡혈 저항 : {GameManager.Instance.DrainResist:N0}%\n" +
                                              $"흡혈 : {GameManager.Instance.DrainAmount:N0}%\n" +
                                              $"회피 확률 : {GameManager.Instance.AvoidPercent:N0}%\n" +
                                              $"회피 저항 : {GameManager.Instance.AvoidResist:N0}%";
        AddData1.text = $"STR% : {GameManager.Instance.STRPercent:N0}%\n" +
                                        $"DEX% : {GameManager.Instance.DEXPercent:N0}%\n" +
                                        $"LUC% : {GameManager.Instance.LUCPercent:N0}%\n" +
                                        $"VIT% : {GameManager.Instance.VITPercent:N0}%";
        AddData2.text = $"추가 경험치 : {GameManager.Instance.EXPPercent:N0}%\n" +
                                        $"아이템 드롭률 : {GameManager.Instance.ItemDropRate:N0}%\n" +
                                        $"골드 획득량 : {GameManager.Instance.GoldPercent:N0}%";
    }
}
