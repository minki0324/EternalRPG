using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentQuickSlot : MonoBehaviour
{
    [SerializeField] private GameObject activeCanvas;

    [Header("인벤토리 창")]
    [SerializeField] private TMP_Text[] quickSlotText;

    [Header("이름편집 창")]
    [SerializeField] private TMP_InputField[] input;
    [SerializeField] private TMP_Text[] holderText;
    [SerializeField] private TMP_Text[] modifyText;

    [Header("HUD")]
    [SerializeField] private Button[] HUDButton;
    [SerializeField] private TMP_Text[] HUDquickSlotText;
    [SerializeField] private Sprite[] OCButtonSprite;
    [SerializeField] private Image OCButtonImage;
    [SerializeField] private GameObject HUDQuickSlotPanel;

    [Header("Ask")]
    [SerializeField] private GameObject QuickAskPanel;
    [SerializeField] private TMP_Text QuickAskTitle;
    [SerializeField] private Transform itemListParent;
    private int quickSlotIndex;

    [Header("장비칸 아이콘")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Image armorIcon;
    [SerializeField] private Image pantsIcon;
    [SerializeField] private Image helmetIcon;
    [SerializeField] private Image gloveIcon;
    [SerializeField] private Image shoesIcon;
    [SerializeField] private Image cloakIcon;
    [SerializeField] private Image beltIcon;
    [SerializeField] private Image shoulderArmorIcon;
    [SerializeField] private Image necklessIcon;
    [SerializeField] private Image[] ringIcons;
    [SerializeField] private Image[] otherIcons;

    [Header("장비칸 텍스트")]
    [SerializeField] private TMP_Text weaponText;
    [SerializeField] private TMP_Text armorText;
    [SerializeField] private TMP_Text pantsText;
    [SerializeField] private TMP_Text helmetText;
    [SerializeField] private TMP_Text gloveText;
    [SerializeField] private TMP_Text shoesText;
    [SerializeField] private TMP_Text cloakText;
    [SerializeField] private TMP_Text beltText;
    [SerializeField] private TMP_Text shoulderArmorText;
    [SerializeField] private TMP_Text necklessText;
    [SerializeField] private TMP_Text[] ringTexts;
    [SerializeField] private TMP_Text[] otherTexts;

    private bool isPanelOpen = true;
    private bool isMain = false;
    private Coroutine panelMoveCoroutine;
    public string ListType = string.Empty;

    private void OnEnable()
    {
        StartCoroutine(InitText());
    }

    private IEnumerator InitText()
    {
        yield return null;
        for (int i = 0; i < holderText.Length; i++)
        {
            // 초기화
            modifyText[i].text = string.Empty;
            quickSlotText[i].text = GameManager.Instance.QuickSlot[i];
            holderText[i].text = GameManager.Instance.QuickSlot[i];
            HUDquickSlotText[i].text = GameManager.Instance.QuickSlot[i];
        }
    }

    public void ModifyQuickSlotName(int _index)
    {
        // string 배열 초기화
        GameManager.Instance.QuickSlot[_index] = input[_index].text;

        // 바뀐 이름으로 텍스트 초기화
        quickSlotText[_index].text = GameManager.Instance.QuickSlot[_index];
        holderText[_index].text = GameManager.Instance.QuickSlot[_index];
        HUDquickSlotText[_index].text = GameManager.Instance.QuickSlot[_index];

        // 저장한 이름은 초기화
        modifyText[_index].text = string.Empty;
        input[_index].text = string.Empty;

        PrintLog.Instance.StaticLog($"{_index}번째 슬롯 : [{GameManager.Instance.QuickSlot[_index]}] 변경 완료");
    }

    public void AskQuickSlotPanel(int _index)
    {
        if(GameManager.Instance.CurrentEnergy <= 0)
        {
            PrintLog.Instance.StaticLog($"에너지가 없어 퀵 슬롯 변경이 불가능 합니다.");
            return;
        }
        if(!activeCanvas.activeSelf)
        { // 메인부터 켰는지
            GameManager.Instance.QuickSlotIndex = _index;
            DataManager.Instance.LoadQuickSlotEquipment(GameManager.Instance.QuickSlotIndex);
            PrintLog.Instance.StaticLog($"{GameManager.Instance.QuickSlotIndex + 1}번째 슬롯 : [{GameManager.Instance.QuickSlot[GameManager.Instance.QuickSlotIndex]}] 장착");
            GameManager.Instance.RenewAbility();
        }
        else
        {
            quickSlotIndex = _index;
            QuickAskPanel.SetActive(true);
            QuickAskTitle.text = GameManager.Instance.QuickSlot[_index];
            PrintEquipmentData(_index);
        }
    }

    public void QuickBackButton()
    {
        if(isMain)
        {
            activeCanvas.SetActive(false);
            isMain = false;
        }
    }

    private void PrintEquipmentData(int _index)
    {
        EquipmentSet equipmentSet = DataManager.Instance.GetQuickSlotData(_index);
        if(equipmentSet == null)
        {
            equipmentSet = new EquipmentSet();
        }
        // 이미지
        weaponIcon.sprite = equipmentSet.EquipWeapon != null ? EquipmentManager.Instance.GetEquipmentSprite(equipmentSet.EquipWeapon) : GameManager.Instance.NoneBackground;
        armorIcon.sprite = equipmentSet.EquipArmor != null ? EquipmentManager.Instance.GetEquipmentSprite(equipmentSet.EquipArmor) : GameManager.Instance.NoneBackground;
        pantsIcon.sprite = equipmentSet.EquipPants != null ? EquipmentManager.Instance.GetEquipmentSprite(equipmentSet.EquipPants) : GameManager.Instance.NoneBackground;
        helmetIcon.sprite = equipmentSet.EquipHelmet != null ? EquipmentManager.Instance.GetEquipmentSprite(equipmentSet.EquipHelmet) : GameManager.Instance.NoneBackground;
        gloveIcon.sprite = equipmentSet.EquipGlove != null ? EquipmentManager.Instance.GetEquipmentSprite(equipmentSet.EquipGlove) : GameManager.Instance.NoneBackground;
        shoesIcon.sprite = equipmentSet.EquipShoes != null ? EquipmentManager.Instance.GetEquipmentSprite(equipmentSet.EquipShoes) : GameManager.Instance.NoneBackground;
        cloakIcon.sprite = equipmentSet.EquipCloak != null ? EquipmentManager.Instance.GetEquipmentSprite(equipmentSet.EquipCloak) : GameManager.Instance.NoneBackground;
        beltIcon.sprite = equipmentSet.EquipBelt != null ? EquipmentManager.Instance.GetEquipmentSprite(equipmentSet.EquipBelt) : GameManager.Instance.NoneBackground;
        shoulderArmorIcon.sprite = equipmentSet.EquipShoulder != null ? EquipmentManager.Instance.GetEquipmentSprite(equipmentSet.EquipShoulder) : GameManager.Instance.NoneBackground;
        necklessIcon.sprite = equipmentSet.EquipNeckless != null ? EquipmentManager.Instance.GetEquipmentSprite(equipmentSet.EquipNeckless) : GameManager.Instance.NoneBackground;
        for (int i = 0; i < ringIcons.Length; i++)
        {
            ringIcons[i].sprite = equipmentSet.EquipRings[i] != null ? EquipmentManager.Instance.GetEquipmentSprite(equipmentSet.EquipRings[i]) : GameManager.Instance.NoneBackground;
        }
        for (int i = 0; i < otherIcons.Length; i++)
        {
            otherIcons[i].sprite = equipmentSet.EquipOther[i] != null ? EquipmentManager.Instance.GetEquipmentSprite(equipmentSet.EquipOther[i]) : GameManager.Instance.NoneBackground;
        }

        // 텍스트
        if (equipmentSet.EquipWeapon == null || equipmentSet.EquipWeapon.EquipmentName == "Punch")
        {
            weaponText.text = "미장착";
        }
        else
        {
            weaponText.text = equipmentSet.EquipWeapon.EquipmentName;
        }
        armorText.text = equipmentSet.EquipArmor != null ? equipmentSet.EquipArmor.EquipmentName : "미장착";
        pantsText.text = equipmentSet.EquipPants != null ? equipmentSet.EquipPants.EquipmentName : "미장착";
        helmetText.text = equipmentSet.EquipHelmet != null ? equipmentSet.EquipHelmet.EquipmentName : "미장착";
        gloveText.text = equipmentSet.EquipGlove != null ? equipmentSet.EquipGlove.EquipmentName : "미장착";
        shoesText.text = equipmentSet.EquipShoes != null ? equipmentSet.EquipShoes.EquipmentName : "미장착";
        cloakText.text = equipmentSet.EquipCloak != null ? equipmentSet.EquipCloak.EquipmentName : "미장착";
        beltText.text = equipmentSet.EquipBelt != null ? equipmentSet.EquipBelt.EquipmentName : "미장착";
        shoulderArmorText.text = equipmentSet.EquipShoulder != null ? equipmentSet.EquipShoulder.EquipmentName : "미장착";
        necklessText.text = equipmentSet.EquipNeckless != null ? equipmentSet.EquipNeckless.EquipmentName : "미장착";
        for(int i = 0; i < ringTexts.Length; i++)
        {
            ringTexts[i].text = equipmentSet.EquipRings[i] != null ? equipmentSet.EquipRings[i].EquipmentName : "미장착";
        }
        for (int i = 0; i < otherTexts.Length; i++)
        {
            otherTexts[i].text = equipmentSet.EquipOther[i] != null ? equipmentSet.EquipOther[i].EquipmentName : "미장착";
        }
    }

    public void ChangeEquipmentQuickSlot()
    {
        if(GameManager.Instance.CurrentEnergy > 0)
        {
            GameManager.Instance.QuickSlotIndex = quickSlotIndex;
            DataManager.Instance.LoadQuickSlotEquipment(GameManager.Instance.QuickSlotIndex);
            EquipmentManager.Instance.ItemListSet(ListType, itemListParent);
            PrintLog.Instance.StaticLog($"{GameManager.Instance.QuickSlotIndex + 1}번째 슬롯 : [{GameManager.Instance.QuickSlot[GameManager.Instance.QuickSlotIndex]}] 장착");
            GameManager.Instance.RenewAbility();
        }
        else
        {
            PrintLog.Instance.StaticLog($"에너지가 없어 퀵 슬롯 변경이 불가능 합니다.");
        }
    }

    public void SaveEquipmentQuickSlot(int _index)
    {
        DataManager.Instance.SaveQuickSlotEquipment(_index);
        PrintLog.Instance.StaticLog($"{_index}번째 슬롯 : [{GameManager.Instance.QuickSlot[_index]}] 저장 완료");
    }

    public void OCButton()
    {
        if (!isPanelOpen)
        {
            // 패널 열기 코루틴 시작
            if (panelMoveCoroutine != null)
                StopCoroutine(panelMoveCoroutine);
            panelMoveCoroutine = StartCoroutine(PanelMove(false));
            isPanelOpen = true;
        }
        else
        {
            // 패널 닫기 코루틴 시작
            if (panelMoveCoroutine != null)
                StopCoroutine(panelMoveCoroutine);
            panelMoveCoroutine = StartCoroutine(PanelMove(true));
            isPanelOpen = false;
        }
    }

    private IEnumerator PanelMove(bool _isOpen)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        Vector3 startPos = _isOpen ? new Vector3(-410, -465, 0) : new Vector3(-640, -465, 0);
        Vector3 endPos = _isOpen ? new Vector3(-640, -465, 0) : new Vector3(-410, -465, 0);

        for (int i = 0; i < HUDButton.Length; i++)
        {
            HUDButton[i].interactable = false;
        }

        RectTransform rectTransform = HUDQuickSlotPanel.GetComponent<RectTransform>();

        while (elapsedTime < duration)
        {
            // 패널 위치 변경
            rectTransform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 마지막 위치 설정
        rectTransform.localPosition = endPos;

        for(int i = 0; i < HUDButton.Length; i++)
        {
            HUDButton[i].interactable = true;
        }
        if(!_isOpen)
        {
            OCButtonImage.sprite = OCButtonSprite[0];
        }
        else
        {
            OCButtonImage.sprite = OCButtonSprite[1];
        }    
    }
}
