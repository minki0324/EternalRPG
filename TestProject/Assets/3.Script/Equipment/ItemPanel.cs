using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPanel : MonoBehaviour
{
    public EquipmentBaseData EquipmentData;
    public Image ItemIcon;
    public TMP_Text OwnCount;
    public int ItemID;

    private void Start()
    {
        ItemIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(EquipmentData);
        OwnCount.text = DataManager.Instance.GetOwnDictionary(EquipmentData)[EquipmentData.ItemID].ToString();
        ItemID = EquipmentData.ItemID;
    }

    public void itemButton()
    {
        EquipmentCanvas canvas = FindObjectOfType<EquipmentCanvas>();
        canvas.CurrentItem = this;
        canvas.PrintItemInfomation();
    }
}
