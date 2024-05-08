using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropItem : MonoBehaviour
{
    public Image DropItemImage;
    public TMP_Text DropRateText;
    public TMP_Text OwnCountText;
    public GameObject TooltipPanel;

    public void ToolTipActive()
    {
        bool tooltipActive = TooltipPanel.activeSelf;

        TooltipPanel.SetActive(!tooltipActive);
    }
}
