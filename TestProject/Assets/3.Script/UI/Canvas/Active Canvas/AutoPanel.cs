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

    public void IncreaseButton()
    {
        AutoNum = Mathf.Min(20, AutoNum + 1);
        AutoNumText.text = AutoNum.ToString();
    }

    public void DecreaseButton()
    {
        AutoNum = Mathf.Max(0, AutoNum - 1);
        AutoNumText.text = AutoNum.ToString();
    }
}
