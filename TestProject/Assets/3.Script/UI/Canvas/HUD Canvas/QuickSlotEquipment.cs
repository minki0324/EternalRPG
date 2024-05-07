using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuickSlotEquipment : MonoBehaviour
{
    [SerializeField] private TMP_Text[] quickSlotText;

    public void ChangeEquipmentQuickSlot(int _index)
    {
        DataManager.Instance.LoadQuickSlotEquipment(_index);
    }
}
