using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentQuickSlot : MonoBehaviour
{
    [Header("�κ��丮 â")]
    [SerializeField] private TMP_Text[] quickSlotText;

    [Header("�̸����� â")]
    [SerializeField] private TMP_InputField[] input;
    [SerializeField] private TMP_Text[] holderText;
    [SerializeField] private TMP_Text[] modifyText;

    private void OnEnable()
    {
        for(int i = 0; i < holderText.Length; i++)
        {
            // �ʱ�ȭ
            modifyText[i].text = string.Empty;
            quickSlotText[i].text = GameManager.Instance.QuickSlot[i];
            holderText[i].text = GameManager.Instance.QuickSlot[i];
        }
    }

    public void ModifyQuickSlotName(int _index)
    {
        // string �迭 �ʱ�ȭ
        GameManager.Instance.QuickSlot[_index] = input[_index].text;

        // �ٲ� �̸����� �ؽ�Ʈ �ʱ�ȭ
        quickSlotText[_index].text = GameManager.Instance.QuickSlot[_index];
        holderText[_index].text = GameManager.Instance.QuickSlot[_index];

        // ������ �̸��� �ʱ�ȭ
        modifyText[_index].text = string.Empty;
        input[_index].text = string.Empty;
    }

    public void ChangeEquipmentQuickSlot(int _index)
    {
        DataManager.Instance.LoadEquipSet(_index);
    }

    public void SaveEquipmentQuickSlot(int _index)
    {
        DataManager.Instance.SaveEquipSet(_index);
    }
}
