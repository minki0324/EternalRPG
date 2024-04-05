using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Stat
{
    STR,
    DEX,
    LUC,
    VIT
}
public class StatPanel : MonoBehaviour
{
    [SerializeField] private Stat stat;
    [SerializeField] private TMP_Text basicStat;
    [SerializeField] private TMP_Text totalStat;

    private void OnEnable()
    {
        switch (stat)
        {
            case Stat.STR:
                basicStat.text = $"�⺻ : {GameManager.Instance.APSTR + 5:N0}";
                totalStat.text = $"���� : {GameManager.Instance.STR:N0}";
                break;
            case Stat.DEX:
                basicStat.text = $"�⺻ : {GameManager.Instance.APDEX + 5:N0}";
                totalStat.text = $"���� : {GameManager.Instance.DEX:N0}";
                break;
            case Stat.LUC:
                basicStat.text = $"�⺻ : {GameManager.Instance.APLUC + 5:N0}";
                totalStat.text = $"���� : {GameManager.Instance.LUC:N0}";
                break;
            case Stat.VIT:
                basicStat.text = $"�⺻ : {GameManager.Instance.APVIT + 5:N0}";
                totalStat.text = $"���� : {GameManager.Instance.VIT:N0}";
                break;
        }
    }
}