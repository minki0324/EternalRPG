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
        defaultData.text = $"���� : {GameManager.Instance.PlayerLevel:N0}\n" +
                                            $"����ġ : {GameManager.Instance.CurrentEXP:N0} / {GameManager.Instance.RequireEXP:N0}";
        basicData1.text = $"���ݷ� : {GameManager.Instance.PlayerATK:N0}\n" +
                                         $"ü�� : {GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP:N0}\n" +
                                         $"���� : {GameManager.Instance.PlayerDef:N0}\n" +
                                         $"�̵��ӵ� : {GameManager.Instance.MoveSpeed:N0}";
        basicData2.text = $"���ݷ�% : {GameManager.Instance.PlayerATKPercent:N0}%\n" +
                                          $"ü��% : {GameManager.Instance.PlayerHPPercent:N0}%\n" +
                                          $"����% : {GameManager.Instance.PlayerDefPercent:N0}%";
        percentData1.text = $"ũ��Ƽ�� Ȯ�� : {GameManager.Instance.CriticalPercant:N0}%\n" +
                                             $"ũ��Ƽ�� ���� : {GameManager.Instance.CriticalResist:N0}%\n" +
                                             $"ũ��Ƽ�� ������ : {GameManager.Instance.CriticalDamage:N0}%\n" +
                                             $"��Ÿ Ȯ�� : {GameManager.Instance.ComboPercent:N0}%\n" +
                                             $"��Ÿ ���� : {GameManager.Instance.ComboResist:N0}%";
        percentData2.text = $"���� Ȯ�� : {GameManager.Instance.DrainPercent:N0}%\n" +
                                              $"���� ���� : {GameManager.Instance.DrainResist:N0}%\n" +
                                              $"���� : {GameManager.Instance.DrainAmount:N0}%\n" +
                                              $"ȸ�� Ȯ�� : {GameManager.Instance.AvoidPercent:N0}%\n" +
                                              $"ȸ�� ���� : {GameManager.Instance.AvoidResist:N0}%";
        AddData1.text = $"STR% : {GameManager.Instance.STRPercent:N0}%\n" +
                                        $"DEX% : {GameManager.Instance.DEXPercent:N0}%\n" +
                                        $"LUC% : {GameManager.Instance.LUCPercent:N0}%\n" +
                                        $"VIT% : {GameManager.Instance.VITPercent:N0}%";
        AddData2.text = $"�߰� ����ġ : {GameManager.Instance.EXPPercent:N0}%\n" +
                                        $"������ ��ӷ� : {GameManager.Instance.ItemDropRate:N0}%\n" +
                                        $"��� ȹ�淮 : {GameManager.Instance.GoldPercent:N0}%";
    }
}
