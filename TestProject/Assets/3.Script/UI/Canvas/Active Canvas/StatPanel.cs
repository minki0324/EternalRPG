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
                basicStat.text = $"±âº» : {GameManager.Instance.APSTR + 5:N0}";
                totalStat.text = $"ÃÑÇÕ : {GameManager.Instance.STR:N0}";
                break;
            case Stat.DEX:
                basicStat.text = $"±âº» : {GameManager.Instance.APDEX + 5:N0}";
                totalStat.text = $"ÃÑÇÕ : {GameManager.Instance.DEX:N0}";
                break;
            case Stat.LUC:
                basicStat.text = $"±âº» : {GameManager.Instance.APLUC + 5:N0}";
                totalStat.text = $"ÃÑÇÕ : {GameManager.Instance.LUC:N0}";
                break;
            case Stat.VIT:
                basicStat.text = $"±âº» : {GameManager.Instance.APVIT + 5:N0}";
                totalStat.text = $"ÃÑÇÕ : {GameManager.Instance.VIT:N0}";
                break;
        }
    }
}