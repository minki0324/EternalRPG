using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentQuickSlot : MonoBehaviour
{
    [Header("인벤토리 창")]
    [SerializeField] private TMP_Text[] quickSlotText;

    [Header("이름편집 창")]
    [SerializeField] private TMP_InputField[] input;
    [SerializeField] private TMP_Text[] holderText;
    [SerializeField] private TMP_Text[] modifyText;

    private void OnEnable()
    {
        for(int i = 0; i < holderText.Length; i++)
        {
            // 초기화
            modifyText[i].text = string.Empty;
            quickSlotText[i].text = GameManager.Instance.QuickSlot[i];
            holderText[i].text = GameManager.Instance.QuickSlot[i];
        }
    }

    public void ModifyQuickSlotName(int _index)
    {
        // string 배열 초기화
        GameManager.Instance.QuickSlot[_index] = input[_index].text;

        // 바뀐 이름으로 텍스트 초기화
        quickSlotText[_index].text = GameManager.Instance.QuickSlot[_index];
        holderText[_index].text = GameManager.Instance.QuickSlot[_index];

        // 저장한 이름은 초기화
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
