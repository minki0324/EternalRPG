using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentQuickSlot : MonoBehaviour
{
    [SerializeField] private GameObject activeCanvas;

    [Header("�κ��丮 â")]
    [SerializeField] private TMP_Text[] quickSlotText;

    [Header("�̸����� â")]
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

    [Header("���ĭ ������")]
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

    [Header("���ĭ �ؽ�Ʈ")]
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

    private bool isPanelOpen = false;
    private bool isMain = false;
    private Coroutine panelMoveCoroutine;

    private void OnEnable()
    {
        StartCoroutine(InitText());
    }

    private IEnumerator InitText()
    {
        yield return null;
        for (int i = 0; i < holderText.Length; i++)
        {
            // �ʱ�ȭ
            modifyText[i].text = string.Empty;
            quickSlotText[i].text = GameManager.Instance.QuickSlot[i];
            holderText[i].text = GameManager.Instance.QuickSlot[i];
            HUDquickSlotText[i].text = GameManager.Instance.QuickSlot[i];
        }
    }

    public void ModifyQuickSlotName(int _index)
    {
        // string �迭 �ʱ�ȭ
        GameManager.Instance.QuickSlot[_index] = input[_index].text;

        // �ٲ� �̸����� �ؽ�Ʈ �ʱ�ȭ
        quickSlotText[_index].text = GameManager.Instance.QuickSlot[_index];
        holderText[_index].text = GameManager.Instance.QuickSlot[_index];
        HUDquickSlotText[_index].text = GameManager.Instance.QuickSlot[_index];

        // ������ �̸��� �ʱ�ȭ
        modifyText[_index].text = string.Empty;
        input[_index].text = string.Empty;

        PrintLog.Instance.StaticLog($"{_index}��° ���� : [{GameManager.Instance.QuickSlot[_index]}] ���� �Ϸ�");
    }

    public void AskQuickSlotPanel(int _index)
    {
        if(!activeCanvas.activeSelf)
        { // ���κ��� �״���
            isMain = true;
            activeCanvas.SetActive(true);
        }
        QuickAskPanel.SetActive(true);
        GameManager.Instance.QuickSlotIndex = _index;
        QuickAskTitle.text = GameManager.Instance.QuickSlot[_index];
        PrintEquipmentData(_index);
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
        // �̹���
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

        // �ؽ�Ʈ
        if (equipmentSet.EquipWeapon == null || equipmentSet.EquipWeapon.EquipmentName == "Punch")
        {
            weaponText.text = "������";
        }
        else
        {
            weaponText.text = equipmentSet.EquipWeapon.EquipmentName;
        }
        armorText.text = equipmentSet.EquipArmor != null ? equipmentSet.EquipArmor.EquipmentName : "������";
        pantsText.text = equipmentSet.EquipPants != null ? equipmentSet.EquipPants.EquipmentName : "������";
        helmetText.text = equipmentSet.EquipHelmet != null ? equipmentSet.EquipHelmet.EquipmentName : "������";
        gloveText.text = equipmentSet.EquipGlove != null ? equipmentSet.EquipGlove.EquipmentName : "������";
        shoesText.text = equipmentSet.EquipShoes != null ? equipmentSet.EquipShoes.EquipmentName : "������";
        cloakText.text = equipmentSet.EquipCloak != null ? equipmentSet.EquipCloak.EquipmentName : "������";
        beltText.text = equipmentSet.EquipBelt != null ? equipmentSet.EquipBelt.EquipmentName : "������";
        shoulderArmorText.text = equipmentSet.EquipShoulder != null ? equipmentSet.EquipShoulder.EquipmentName : "������";
        necklessText.text = equipmentSet.EquipNeckless != null ? equipmentSet.EquipNeckless.EquipmentName : "������";
        for(int i = 0; i < ringTexts.Length; i++)
        {
            ringTexts[i].text = equipmentSet.EquipRings[i] != null ? equipmentSet.EquipRings[i].EquipmentName : "������";
        }
        for (int i = 0; i < otherTexts.Length; i++)
        {
            otherTexts[i].text = equipmentSet.EquipOther[i] != null ? equipmentSet.EquipOther[i].EquipmentName : "������";
        }
    }

    public void ChangeEquipmentQuickSlot()
    {
        DataManager.Instance.LoadEquipSet(GameManager.Instance.QuickSlotIndex);
        PrintLog.Instance.StaticLog($"{GameManager.Instance.QuickSlotIndex + 1}��° ���� : [{GameManager.Instance.QuickSlot[GameManager.Instance.QuickSlotIndex]}] ����");
    }

    public void SaveEquipmentQuickSlot(int _index)
    {
        DataManager.Instance.SaveEquipSet(_index);
        PrintLog.Instance.StaticLog($"{_index}��° ���� : [{GameManager.Instance.QuickSlot[_index]}] ���� �Ϸ�");
    }

    public void OCButton()
    {
        if (!isPanelOpen)
        {
            // �г� ���� �ڷ�ƾ ����
            if (panelMoveCoroutine != null)
                StopCoroutine(panelMoveCoroutine);
            panelMoveCoroutine = StartCoroutine(PanelMove(false));
            isPanelOpen = true;
        }
        else
        {
            // �г� �ݱ� �ڷ�ƾ ����
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

        Vector3 startPos = _isOpen ? new Vector3(0, 180, 0) : new Vector3(-220, 180, 0);
        Vector3 endPos = _isOpen ? new Vector3(-220, 180, 0) : new Vector3(0, 180, 0);

        for (int i = 0; i < HUDButton.Length; i++)
        {
            HUDButton[i].interactable = false;
        }

        RectTransform rectTransform = HUDQuickSlotPanel.GetComponent<RectTransform>();

        while (elapsedTime < duration)
        {
            // �г� ��ġ ����
            rectTransform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // ������ ��ġ ����
        rectTransform.position = endPos;

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
