using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CompareSlot
{
    One,
    Two,
    Three,
    Four
}

public class EquipmentCanvas : MonoBehaviour
{
    private CompareSlot Slot;
    [SerializeField] private HUDCanvas HUDCanvas;
    [SerializeField] private TMP_Text categoryName;
    [SerializeField] private Transform itemListParent;
    [SerializeField] private TMP_Text totalGoldText;

    List<string> basicDataList = new List<string>();
    List<string> percentDataList = new List<string>();
    List<string> addDataList = new List<string>();

    [SerializeField] private TMP_Dropdown categoryDropDown;
    private string otherType = "��ü";

    [Header("�����ϱ� â")]
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private TMP_Text equipNameText;
    [SerializeField] private TMP_Text equipdesText;
    [SerializeField] private Image equipImage;
    [SerializeField] private GameObject equipmentQaPanel;
    private bool isEquip = false;

    [Header("������ ����")]
    public ItemPanel CurrentItem;
    [SerializeField] private GameObject infomationPanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text basicDataText;
    [SerializeField] private TMP_Text compareBasicDataText;
    [SerializeField] private TMP_Text percentDataText;
    [SerializeField] private TMP_Text comparePercentDataText;
    [SerializeField] private TMP_Text addDataText;
    [SerializeField] private TMP_Text compareAddDataText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text ownCountText;
    [SerializeField] private Button BuyButton;

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

    
    private void OnEnable()
    {
        EquipmentManager.Instance.ItemListSet("����", itemListParent);
        totalGoldText.text = $"{GameManager.Instance.Gold:N0}";
        InitImage();
    }

    private void OnDisable()
    {
        CurrentItem = null;
        Slot = CompareSlot.One;
        infomationPanel.SetActive(false);
    }

    public void CategoryButton(string _type)
    {
        categoryName.text = _type;
        infomationPanel.SetActive(false);
        if (_type == "����")
        {
            int selectDropDown = categoryDropDown.value;
            switch (categoryDropDown.options[selectDropDown].text)
            {
                case "��ü":
                    otherType = "All";
                    break;
                case "����ġ":
                    otherType = "EXP";
                    break;
                case "��� ȹ�淮":
                    otherType = "Gold";
                    break;
                case "���ʽ� �ɷ�ġ":
                    otherType = "BonusAP";
                    break;
                case "������ ��ӷ�":
                    otherType = "DropRate";
                    break;
                case "����":
                    otherType = "Offence";
                    break;
            }
            categoryDropDown.gameObject.SetActive(true);
            EquipmentManager.Instance.ItemListSet(_type, itemListParent, otherType);
        }
        else
        {
            categoryDropDown.gameObject.SetActive(false);
            EquipmentManager.Instance.ItemListSet(_type, itemListParent);
        }
    }

    public void SlotSetting(int _slot)
    {
        Slot = (CompareSlot)_slot;
    }

    public void InitImage()
    {
        weaponIcon.sprite = GameManager.Instance.WeaponData != null ? EquipmentManager.Instance.GetEquipmentSprite(GameManager.Instance.WeaponData) : GameManager.Instance.NoneBackground;
        armorIcon.sprite = GameManager.Instance.ArmorData != null ? EquipmentManager.Instance.GetEquipmentSprite(GameManager.Instance.ArmorData) : GameManager.Instance.NoneBackground;
        pantsIcon.sprite = GameManager.Instance.PantsData != null ? EquipmentManager.Instance.GetEquipmentSprite(GameManager.Instance.PantsData) : GameManager.Instance.NoneBackground;
        helmetIcon.sprite = GameManager.Instance.HelmetData != null ? EquipmentManager.Instance.GetEquipmentSprite(GameManager.Instance.HelmetData) : GameManager.Instance.NoneBackground;
        gloveIcon.sprite = GameManager.Instance.GloveData != null ? EquipmentManager.Instance.GetEquipmentSprite(GameManager.Instance.GloveData) : GameManager.Instance.NoneBackground;
        shoesIcon.sprite = GameManager.Instance.ShoesData != null ? EquipmentManager.Instance.GetEquipmentSprite(GameManager.Instance.ShoesData) : GameManager.Instance.NoneBackground;
        cloakIcon.sprite = GameManager.Instance.ClockData != null ? EquipmentManager.Instance.GetEquipmentSprite(GameManager.Instance.ClockData) : GameManager.Instance.NoneBackground;
        beltIcon.sprite = GameManager.Instance.BeltData != null ? EquipmentManager.Instance.GetEquipmentSprite(GameManager.Instance.BeltData) : GameManager.Instance.NoneBackground;
        shoulderArmorIcon.sprite = GameManager.Instance.ShoulderArmorData != null ? EquipmentManager.Instance.GetEquipmentSprite(GameManager.Instance.ShoulderArmorData) : GameManager.Instance.NoneBackground;
        necklessIcon.sprite = GameManager.Instance.NecklessData != null ? EquipmentManager.Instance.GetEquipmentSprite(GameManager.Instance.NecklessData) : GameManager.Instance.NoneBackground;
        for (int i = 0; i < ringIcons.Length; i++)
        {
            ringIcons[i].sprite = GameManager.Instance.RingDatas[i] != null ? EquipmentManager.Instance.GetEquipmentSprite(GameManager.Instance.RingDatas[i]) : GameManager.Instance.NoneBackground;
        }
        for (int i = 0; i < otherIcons.Length; i++)
        {
            otherIcons[i].sprite = GameManager.Instance.OtherDatas[i] != null ? EquipmentManager.Instance.GetEquipmentSprite(GameManager.Instance.OtherDatas[i]) : GameManager.Instance.NoneBackground;
        }
    }

    public void PrintItemInfomation()
    {
        // ���� ����â�� �����ִٸ� ���ֱ�
        if (!infomationPanel.activeSelf) infomationPanel.SetActive(true);
        // ���� ���� ������ 10����� ���� ��ư ��Ȱ��ȭ
        if (DataManager.Instance.GetOwnDictionary(CurrentItem.EquipmentData)[CurrentItem.EquipmentData.ItemID] == 10) BuyButton.interactable = false;
        // ���� �������� ���� �Ұ����� �������̶�� ��ư ��Ȱ��ȭ
        else if (!CurrentItem.EquipmentData.isCanBuy) BuyButton.interactable = false;
        // ������ ����ߴٸ� Ȱ��ȭ
        else BuyButton.interactable = true;

        GetDataText();
        GetCompareText();
    }

    #region ��� ����
    private void EquipMarkActive()
    {
        for (int i = 0; i < itemListParent.childCount; i++)
        {
            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
            itemPanel.SelectIcon.SetActive(true);
            itemPanel.EquipCheckIcon.SetActive(false);
        }
    }

    public void EquipItemCal()
    {
        switch (CurrentItem.EquipmentData.EquipmentType)
        {
            case Category.Weapon:
                if (CurrentItem.EquipmentData is WeaponData weaponData)
                {
                    if (isEquip)
                    { // ����ϱ� ������ ��
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.WeaponData != null && GameManager.Instance.WeaponData.ItemID == itemPanel.EquipmentData.ItemID)
                            { // ������ ������ ǥ��
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break; // ã������ for�� ����
                            }
                        }
                        GameManager.Instance.WeaponData = weaponData;
                        weaponIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                    }
                    else
                    { // �����ϱ� ������ ��
                        if(CurrentItem.ItemID == GameManager.Instance.WeaponData.ItemID)
                        { // �ٸ� ��� ������ ���� ���� �����Ǵ� ���� ����
                            EquipMarkActive();
                            GameManager.Instance.WeaponData = GameManager.Instance.Punch;
                            weaponIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"���������� ���� ������ �Դϴ�.");
                        }
                    }
                }
                break;
            case Category.Armor:
                if (CurrentItem.EquipmentData is ArmorData armorData)
                {
                    if (isEquip)
                    {
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.ArmorData != null && GameManager.Instance.ArmorData.ItemID == itemPanel.EquipmentData.ItemID)
                            { // ������ ������ ǥ��
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break; // ã������ for�� ����
                            }
                        }
                        GameManager.Instance.ArmorData = armorData;
                        armorIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                    }
                    else
                    {
                        if (CurrentItem.ItemID == GameManager.Instance.ArmorData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.ArmorData = null;
                            armorIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"���������� ���� ������ �Դϴ�.");
                        }
                    }
                }
                break;
            case Category.Pants:
                if (CurrentItem.EquipmentData is PantsData pantsData)
                {
                    if (isEquip)
                    {
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.PantsData != null && GameManager.Instance.PantsData.ItemID == itemPanel.EquipmentData.ItemID)
                            { // ������ ������ ǥ��
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break; // ã������ for�� ����
                            }
                        }
                        GameManager.Instance.PantsData = pantsData;
                        pantsIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.PantsData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.PantsData = null;
                            pantsIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"���������� ���� ������ �Դϴ�.");
                        }
                    }
                }
                break;
            case Category.Helmet:
                if (CurrentItem.EquipmentData is HelmetData helmetData)
                {
                    if (isEquip)
                    {
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.HelmetData != null && GameManager.Instance.HelmetData.ItemID == itemPanel.EquipmentData.ItemID)
                            {
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break;
                            }
                        }
                        GameManager.Instance.HelmetData = helmetData;
                        helmetIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.HelmetData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.HelmetData = null;
                            helmetIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"���������� ���� ������ �Դϴ�.");
                        }
                    }
                }
                break;
            case Category.Glove:
                if (CurrentItem.EquipmentData is GloveData gloveData)
                    if (isEquip)
                    {
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.GloveData != null && GameManager.Instance.GloveData.ItemID == itemPanel.EquipmentData.ItemID)
                            {
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break;
                            }
                        }
                        GameManager.Instance.GloveData = gloveData;
                        gloveIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.GloveData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.GloveData = null;
                            gloveIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"���������� ���� ������ �Դϴ�.");
                        }
                    }
                break;
            case Category.Shoes:
                if (CurrentItem.EquipmentData is ShoesData shoesData)
                    if (isEquip)
                    {
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.ShoesData != null && GameManager.Instance.ShoesData.ItemID == itemPanel.EquipmentData.ItemID)
                            {
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break;
                            }
                        }
                        GameManager.Instance.ShoesData = shoesData;
                        shoesIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.ShoesData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.ShoesData = null;
                            shoesIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"���������� ���� ������ �Դϴ�.");
                        }
                    }
                break;
            case Category.Clock:
                if (CurrentItem.EquipmentData is CloakData cloakData)
                    if (isEquip)
                    {
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.ClockData != null && GameManager.Instance.ClockData.ItemID == itemPanel.EquipmentData.ItemID)
                            {
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break;
                            }
                        }
                        GameManager.Instance.ClockData = cloakData;
                        cloakIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.ClockData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.ClockData = null;
                            cloakIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"���������� ���� ������ �Դϴ�.");
                        }
                    }
                break;
            case Category.Belt:
                if (CurrentItem.EquipmentData is BeltData beltData)
                    if (isEquip)
                    {
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.BeltData != null && GameManager.Instance.BeltData.ItemID == itemPanel.EquipmentData.ItemID)
                            {
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break;
                            }
                        }
                        GameManager.Instance.BeltData = beltData;
                        beltIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.BeltData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.BeltData = null;
                            beltIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"���������� ���� ������ �Դϴ�.");
                        }
                    }
                break;
            case Category.ShoulderArmor:
                if (CurrentItem.EquipmentData is ShoulderData shoulderData)
                    if (isEquip)
                    {
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.ShoulderArmorData != null && GameManager.Instance.ShoulderArmorData.ItemID == itemPanel.EquipmentData.ItemID)
                            {
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break;
                            }
                        }
                        GameManager.Instance.ShoulderArmorData = shoulderData;
                        shoulderArmorIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.ShoulderArmorData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.ShoulderArmorData = null;
                            shoulderArmorIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"���������� ���� ������ �Դϴ�.");
                        }
                    }
                break;
            case Category.Neckless:
                if (CurrentItem.EquipmentData is NecklessData necklessData)
                    if (isEquip)
                    {
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.NecklessData != null && GameManager.Instance.NecklessData.ItemID == itemPanel.EquipmentData.ItemID)
                            {
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break;
                            }
                        }
                        GameManager.Instance.NecklessData = necklessData;
                        necklessIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                    }
                    else
                    {
                        if (CurrentItem.ItemID == GameManager.Instance.NecklessData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.NecklessData = null;
                            necklessIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"���������� ���� ������ �Դϴ�.");
                        }
                    }
                break;
            case Category.Ring:
                if (CurrentItem.EquipmentData is RingData ringData)
                {
                    for (int i = 0; i < GameManager.Instance.RingDatas.Length; i++)
                    {
                        if (GameManager.Instance.RingDatas[i] != null && GameManager.Instance.RingDatas[i].ItemID == ringData.ItemID)
                        { // �ߺ� ���� todo
                            PrintLog.Instance.StaticLog("�̹� �������� ������ �Դϴ�.");
                            return;
                        }
                    }
                    if (isEquip)
                    {
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.RingDatas[(int)Slot] != null && GameManager.Instance.RingDatas[(int)Slot].ItemID == itemPanel.EquipmentData.ItemID)
                            {
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break;
                            }
                        }
                        GameManager.Instance.RingDatas[(int)Slot] = ringData;
                        ringIcons[(int)Slot].sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.RingDatas[(int)Slot].ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.RingDatas[(int)Slot] = null;
                            ringIcons[(int)Slot].sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"���������� ���� ������ �Դϴ�.");
                        }
                    }
                }
                break;
            case Category.Other:
                if (CurrentItem.EquipmentData is OtherData otherData)
                {
                    for (int i = 0; i < GameManager.Instance.OtherDatas.Length; i++)
                    {
                        if (GameManager.Instance.OtherDatas[i] != null && GameManager.Instance.OtherDatas[i].ItemID == otherData.ItemID && isEquip)
                        { // �ߺ� ���� todo
                            PrintLog.Instance.StaticLog("�̹� �������� ������ �Դϴ�.");
                            return;
                        }
                    }
                    if (isEquip)
                    {
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.OtherDatas[(int)Slot] != null && GameManager.Instance.OtherDatas[(int)Slot].ItemID == itemPanel.EquipmentData.ItemID)
                            {
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break;
                            }
                        }
                        GameManager.Instance.OtherDatas[(int)Slot] = otherData;
                        otherIcons[(int)Slot].sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                    }
                    else
                    {
                        if (CurrentItem.ItemID == GameManager.Instance.OtherDatas[(int)Slot].ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.OtherDatas[(int)Slot] = null;
                            otherIcons[(int)Slot].sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ����");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"���������� ���� ������ �Դϴ�.");
                        }
                    }
                }
                break;
        }
        if(isEquip)
        {
            CurrentItem.EquipCheckIcon.SetActive(true);
            CurrentItem.SelectIcon.SetActive(false);
        }
        DataManager.Instance.SaveEquipment();
        GameManager.Instance.RenewAbility();
    }
    #endregion

    public void EquipItem(bool _isEquip)
    {
        if (DataManager.Instance.GetOwnDictionary(CurrentItem.EquipmentData)[CurrentItem.EquipmentData.ItemID] == 0)
        {
            PrintLog.Instance.StaticLog("���� ���� �������� �ƴմϴ�.");
            return;
        }
        equipmentQaPanel.SetActive(true);

        isEquip = _isEquip;

        string equipment = isEquip ? "�����ϱ�" : "�����ϱ�";
        buttonText.text = equipment;
        equipNameText.text = CurrentItem.EquipmentData.EquipmentName;
        equipdesText.text = CurrentItem.EquipmentData.EquipmentDes;
        equipImage.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
        
    }

    public void BuyItemButton()
    {
        Dictionary<int, int> ownDictionary = DataManager.Instance.GetOwnDictionary(CurrentItem.EquipmentData);
        int ownCount = 0;
        if (ownDictionary.ContainsKey(CurrentItem.EquipmentData.ItemID))
        {
            ownCount = ownDictionary[CurrentItem.EquipmentData.ItemID];
        }

        if (ownCount == 10)
        { // �������� 10�� ��á��
            PrintLog.Instance.StaticLog("�� �̻� ������ �� �����ϴ�.");
            return;
        }

        if (GameManager.Instance.Gold >= CurrentItem.EquipmentData.RequireCost)
        {
            GameManager.Instance.Gold -= CurrentItem.EquipmentData.RequireCost;
            // ���� ���� ��ųʸ�
            int itemID = CurrentItem.EquipmentData.ItemID;

            // ���� ������ ��� �� �÷��ֱ�
            if (ownDictionary.ContainsKey(itemID))
            {
                ownDictionary[itemID]++;
            }

            totalGoldText.text = $"{GameManager.Instance.Gold:N0}";
            ownCountText.text = $"���� ���� : {ownDictionary[itemID]}";
            CurrentItem.OwnCount.text = ownDictionary[itemID].ToString();
            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] ���� ����!");

            // ���� �Ϸ��ߴٸ� ���� ����
            GameManager.Instance.BadgeGrade();
            HUDCanvas.CheckBadgeCount();

            // ���� ���� ����ߵ�
            GameManager.Instance.RenewAbility();
        }
        else
        { // todo
            PrintLog.Instance.StaticLog("���� ���� ��尡 �����մϴ�.");
        }
    }

    #region ������ �ؽ�Ʈ ��� �޼ҵ�
    public void GetDataText()
    {
        nameText.text = CurrentItem.EquipmentData.EquipmentName;
        if (!CurrentItem.EquipmentData.isCanBuy)
        { // ���� �Ұ����� ������
            costText.text = "Drop Only.";
        }
        else
        { // ���� ������ �������� cost ���
            costText.text = $"{CurrentItem.EquipmentData.RequireCost:N0}";
        }
        ownCountText.text = $"���� ���� : {DataManager.Instance.GetOwnDictionary(CurrentItem.EquipmentData)[CurrentItem.EquipmentData.ItemID]}";


        int ownCount = 0;
        switch (EquipmentManager.Instance.type)
        {
            case Category.Weapon:
                if (CurrentItem.EquipmentData is WeaponData weaponData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(weaponData));
                    AppendBasicData("���ݷ�", weaponData.WeaponATK, ownCount);
                    AppendBasicData("���ݷ�%", weaponData.WeaponATKPercent, ownCount);
                    AppendPercentData("��Ÿ Ȯ��", weaponData.WeaponComboPercent, ownCount);
                    AppendPercentData("ũ��Ƽ�� Ȯ��", weaponData.WeaponCriticalPercent, ownCount);
                    AppendPercentData("ũ��Ƽ�� ������", weaponData.WeaponCriticalDamage * 100, ownCount);
                    AppendPercentData("���� Ȯ��", weaponData.WeaponDrainPercent, ownCount);
                    AppendPercentData("����", weaponData.WeaponDrainAmount * 100, ownCount);
                    AppendAddData("�߰� STR%", weaponData.WeaponSTRPercent, ownCount);
                    AppendAddData("�߰� DEX%", weaponData.WeaponDEXPercent, ownCount);
                }
                break;
            case Category.Armor:
                if (CurrentItem.EquipmentData is ArmorData armorData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(armorData));
                    AppendBasicData("ü��", armorData.ArmorHP, ownCount);
                    AppendBasicData("ü��%", armorData.ArmorHPPercent, ownCount);
                    AppendBasicData("����", armorData.ArmorDef, ownCount);
                    AppendBasicData("����%", armorData.ArmorDefPercent, ownCount);
                    AppendPercentData("��Ÿ ����", armorData.ArmorComboResist, ownCount);
                    AppendPercentData("ũ��Ƽ�� ����", armorData.ArmorCriticalResist, ownCount);
                    AppendPercentData("���� ����", armorData.ArmorDrainResist, ownCount);
                    AppendAddData("�߰� VIT%", armorData.ArmorVITPercent, ownCount);
                }
                break;
            case Category.Helmet:
                if (CurrentItem.EquipmentData is HelmetData helmetData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(helmetData));
                    AppendBasicData("ü��", helmetData.HelmetHP, ownCount);
                    AppendBasicData("ü��%", helmetData.HelmetHPPercent, ownCount);
                    AppendBasicData("����", helmetData.HelmetDef, ownCount);
                    AppendBasicData("����%", helmetData.HelmetDefPercent, ownCount);
                    AppendPercentData("ȸ�� Ȯ��", helmetData.HelmetAvoidPercent, ownCount);
                    AppendPercentData("���� ����", helmetData.HelmetDrainResist, ownCount);
                    AppendAddData("�߰� STR%", helmetData.HelmetSTRPercent, ownCount);
                    AppendAddData("�߰� VIT%", helmetData.HelmetVITPercent, ownCount);
                }
                break;
            case Category.Pants:
                if (CurrentItem.EquipmentData is PantsData pantsData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(pantsData));
                    AppendBasicData("ü��", pantsData.PantsHP, ownCount);
                    AppendBasicData("ü��%", pantsData.PantsHPPercent, ownCount);
                    AppendBasicData("����", pantsData.PantsDef, ownCount);
                    AppendBasicData("����%", pantsData.PantsDefPercent, ownCount);
                    AppendPercentData("��Ÿ ����", pantsData.PantsComboResist, ownCount);
                    AppendPercentData("ũ��Ƽ�� ����", pantsData.PantsCriticalResist, ownCount);
                    AppendAddData("�߰� VIT%", pantsData.PantsVITPercent, ownCount);
                }
                break;
            case Category.Glove:
                if (CurrentItem.EquipmentData is GloveData gloveData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(gloveData));
                    AppendBasicData("���ݷ�", gloveData.GloveATK, ownCount);
                    AppendBasicData("���ݷ�%", gloveData.GloveATKPercent, ownCount);
                    AppendBasicData("ü��", gloveData.GloveHP, ownCount);
                    AppendBasicData("ü��%", gloveData.GloveHPPercent, ownCount);
                    AppendPercentData("��Ÿ Ȯ��", gloveData.GloveComboPercent, ownCount);
                    AppendPercentData("ũ��Ƽ�� Ȯ��", gloveData.GloveCriticalPercent, ownCount);
                    AppendPercentData("ũ��Ƽ�� ������", gloveData.GloveCriticalDamage * 100, ownCount);
                    AppendAddData("�߰� STR%", gloveData.GloveSTRPercent, ownCount);
                    AppendAddData("�߰� DEX%", gloveData.GloveDEXPercent, ownCount);
                }
                break;
            case Category.Shoes:
                if (CurrentItem.EquipmentData is ShoesData shoesData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(shoesData));
                    AppendBasicData("ü��", shoesData.ShoesHP, ownCount);
                    AppendBasicData("ü��%", shoesData.ShoesHPPercent, ownCount);
                    AppendBasicData("����", shoesData.ShoesDef, ownCount);
                    AppendBasicData("����%", shoesData.ShoesDefPercent, ownCount);
                    AppendPercentData("ȸ�� Ȯ��", shoesData.ShoesAvoidPercent, ownCount);
                    AppendPercentData("ȸ�� ����", shoesData.ShoesAvoidResist, ownCount);
                    AppendAddData("�߰� DEX%", shoesData.ShoesDEXPercent, ownCount);
                    AppendAddData("�߰� VIT%", shoesData.ShoesVITPercent, ownCount);
                    AppendAddData("�̵��ӵ� ����", shoesData.ShoesMoveSpeed, ownCount);
                }
                break;
            case Category.Belt:
                if (CurrentItem.EquipmentData is BeltData beltData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(beltData));
                    AppendBasicData("ü��", beltData.BeltHP, ownCount);
                    AppendBasicData("ü��%", beltData.BeltHPPercent, ownCount);
                    AppendPercentData("ȸ�� Ȯ��", beltData.BeltAvoidPercent, ownCount);
                    AppendAddData("�߰� ����ġ%", beltData.BeltEXPPercent, ownCount);
                    AppendAddData("�߰� LUC%", beltData.BeltLUCPercent, ownCount);
                    AppendAddData("�߰� VIT%", beltData.BeltVITPercent, ownCount);
                }
                break;
            case Category.ShoulderArmor:
                if (CurrentItem.EquipmentData is ShoulderData shoulderData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(shoulderData));
                    AppendBasicData("����", shoulderData.ShoulderDef, ownCount);
                    AppendBasicData("����%", shoulderData.ShoulderDefPercent, ownCount);
                    AppendPercentData("ũ��Ƽ�� ����", shoulderData.ShoulderCriticalResist, ownCount);
                    AppendPercentData("���� ����", shoulderData.ShoulderDrainResist, ownCount);
                    AppendAddData("�߰� DEX%", shoulderData.ShoulderDEXPercent, ownCount);
                    AppendAddData("�߰� VIT%", shoulderData.ShoulderVITPercent, ownCount);
                }
                break;
            case Category.Ring:
                if (CurrentItem.EquipmentData is RingData ringData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(ringData));
                    AppendBasicData("���ݷ�", ringData.RingATK, ownCount);
                    AppendBasicData("���ݷ�%", ringData.RingATKPercent, ownCount);
                    AppendBasicData("����", ringData.RingDef, ownCount);
                    AppendBasicData("����%", ringData.RingDefPercent, ownCount);
                    AppendPercentData("ũ��Ƽ�� Ȯ��", ringData.RingCriticalPercent, ownCount);
                    AppendPercentData("ũ��Ƽ�� ����", ringData.RingCriticalResist, ownCount);
                    AppendPercentData("���� Ȯ��", ringData.RingDrainPercent, ownCount);
                    AppendPercentData("���� ����", ringData.RingDrainResist, ownCount);
                    AppendAddData("�߰� ����ġ%", ringData.RingEXPPercent, ownCount);
                    AppendAddData("������ ��ӷ�%", ringData.RingItemDropRate, ownCount);
                    AppendAddData("�߰� ���%", ringData.RingGoldPercent, ownCount);
                    AppendAddData("�߰� STR%", ringData.RingSTRPercent, ownCount);
                    AppendAddData("�߰� LUC%", ringData.RingLUCPercent, ownCount);
                }
                break;
            case Category.Neckless:
                if (CurrentItem.EquipmentData is NecklessData necklessData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(necklessData));
                    AppendBasicData("ü��", necklessData.NecklessHP, ownCount);
                    AppendBasicData("ü��%", necklessData.NecklessHPPercent, ownCount);
                    AppendPercentData("��Ÿ Ȯ��", necklessData.NecklessComboPercent, ownCount);
                    AppendPercentData("ȸ�� Ȯ��", necklessData.NecklessAvoidResist, ownCount);
                    AppendAddData("�߰� ����ġ%", necklessData.NecklessEXPPercent, ownCount);
                    AppendAddData("������ ��ӷ�%", necklessData.NecklessItemDropRate, ownCount);
                    AppendAddData("�߰� ���%", necklessData.NecklessGoldPercent, ownCount);
                    AppendAddData("�߰� DEX%", necklessData.NecklessDEXPercent, ownCount);
                    AppendAddData("�߰� LUC%", necklessData.NecklessLUCPercent, ownCount);
                }
                break;
            case Category.Clock:
                if (CurrentItem.EquipmentData is CloakData cloakData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(cloakData));
                    AppendBasicData("ü��", cloakData.CloakHP, ownCount);
                    AppendBasicData("ü��%", cloakData.CloakHPPercent, ownCount);
                    AppendBasicData("����", cloakData.CloakDef, ownCount);
                    AppendBasicData("����%", cloakData.CloakDefPercent, ownCount);
                    AppendPercentData("ȸ�� Ȯ��", cloakData.CloakAvoidPercent, ownCount);
                    AppendPercentData("��Ÿ ����", cloakData.CloakComboResist, ownCount);
                    AppendAddData("�߰� DEX%", cloakData.CloakDEXPercent, ownCount);
                    AppendAddData("�߰� LUC%", cloakData.CloakLUCPercent, ownCount);
                    AppendAddData("�߰� VIT%", cloakData.CloakVITPercent, ownCount);
                }
                break;
            case Category.Other:
                if (CurrentItem.EquipmentData is OtherData otherData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(otherData));
                    AppendBasicData("���ݷ�", otherData.OtherATK, ownCount);
                    AppendBasicData("���ݷ�%", otherData.OtherATKPercent, ownCount);
                    AppendBasicData("ü��", otherData.OtherHP, ownCount);
                    AppendBasicData("ü��%", otherData.OtherHPPercent, ownCount);
                    AppendBasicData("����", otherData.OtherDef, ownCount);
                    AppendBasicData("����%", otherData.OtherDefPercent, ownCount);
                    AppendPercentData("ũ��Ƽ�� Ȯ��", otherData.OtherCriticalPercent, ownCount);
                    AppendPercentData("ũ��Ƽ�� ����", otherData.OtherCriticalResist, ownCount);
                    AppendPercentData("ũ��Ƽ�� ������", otherData.OtherCriticalDamage * 100, ownCount);
                    AppendPercentData("��Ÿ Ȯ��", otherData.OtherComboPercent, ownCount);
                    AppendPercentData("��Ÿ ����", otherData.OtherComboResist, ownCount);
                    AppendPercentData("ȸ�� Ȯ��", otherData.OtherAvoidPercent, ownCount);
                    AppendPercentData("ȸ�� ����", otherData.OtherAvoidResist, ownCount);
                    AppendPercentData("���� Ȯ��", otherData.OtherDrainPercent, ownCount);
                    AppendPercentData("���� ����", otherData.OtherDrainResist, ownCount);
                    AppendPercentData("����", otherData.OtherDrainAmount, ownCount);
                    AppendAddData("�߰� ����ġ%", otherData.OtherEXPPercent, ownCount);
                    AppendAddData("������ ��ӷ�%", otherData.OtherItemDropRate, ownCount);
                    AppendAddData("�߰� ���%", otherData.OtherGoldPercent, ownCount);
                    AppendAddData("�߰� STR%", otherData.OtherSTRPercent, ownCount);
                    AppendAddData("�߰� DEX%", otherData.OtherDEXPercent, ownCount);
                    AppendAddData("�߰� LUC%", otherData.OtherLUCPercent, ownCount);
                    AppendAddData("�߰� VIT%", otherData.OtherVITPercent, ownCount);
                    AppendAddData("������ �ɷ�ġ", otherData.OtherBonusAP, ownCount);
                }
                break;
        }
        // ����Ʈ�� �ִ� ������ �ؽ�Ʈ�� ��ȯ�Ͽ� ���
        basicDataText.text = string.Join("\n", basicDataList);
        percentDataText.text = string.Join("\n", percentDataList);
        addDataText.text = string.Join("\n", addDataList);

        // ����Ʈ �ʱ�ȭ
        basicDataList.Clear();
        percentDataList.Clear();
        addDataList.Clear();
    }
    #endregion

    #region ������ �ؽ�Ʈ ��� ����
    private void AppendBasicData(string labelText, int value, int ownCount)
    {
        if (value != 0)
        {
            if (ownCount == 0 || ownCount == 1)
            {
                basicDataList.Add($"{labelText} : {value:N0}");
            }
            else
            {
                string startColorTag = "<color=#00FFFF>";
                string endColorTag = "</color>";
                basicDataList.Add($"{labelText} : {value:N0} + {startColorTag}{GetOwnCountValue(ownCount - 1, value):N0}{endColorTag}");
            }
        }
    }

    private void AppendPercentData(string labelText, int value, int ownCount)
    {
        if (value != 0)
        {
            if (ownCount == 0 || ownCount == 1)
            {
                percentDataList.Add($"{labelText} : {value:N0}%");
            }
            else
            {
                string startColorTag = "<color=#00FFFF>";
                string endColorTag = "</color>";
                percentDataList.Add($"{labelText} : {value:N0} + {startColorTag}{GetOwnCountValue(ownCount - 1, value):N0}{endColorTag}%");
            }

        }
    }

    private void AppendPercentData(string labelText, float value, int ownCount)
    {
        if (value != 0)
        {
            if (ownCount == 0 || ownCount == 1)
            {
                percentDataList.Add($"{labelText} : {value:N0}%");
            }
            else
            {
                string startColorTag = "<color=#00FFFF>";
                string endColorTag = "</color>";
                percentDataList.Add($"{labelText} : {value:N0} + {startColorTag}{GetOwnCountValue(ownCount - 1, value):N0}{endColorTag}%");
            }
        }
    }

    private void AppendAddData(string labelText, float value, int ownCount)
    {
        if (value != 0)
        {
            if (ownCount == 0 || ownCount == 1)
            {
                addDataList.Add($"{labelText} : {value:N0}");
            }
            else
            {
                string startColorTag = "<color=#00FFFF>";
                string endColorTag = "</color>";
                addDataList.Add($"{labelText} : {value:N0} + {startColorTag}{GetOwnCountValue(ownCount - 1, value):N0}{endColorTag}");
            }
        }
    }
    #endregion

    #region �� �ؽ�Ʈ ��� �޼ҵ�
    public void GetCompareText()
    {
        comparePercentDataText.text = string.Empty;
        compareBasicDataText.text = string.Empty;
        compareAddDataText.text = string.Empty;

        int ownCount = 0;
        int targetOwnCount = 0;

        switch (EquipmentManager.Instance.type)
        {
            case Category.Weapon:
                if (CurrentItem.EquipmentData is WeaponData weaponData)
                {
                    targetOwnCount = Mathf.Max(0, GetOwnCount(weaponData) - 1);
                    if (GameManager.Instance.WeaponData != null)
                    {
                        ownCount = GetOwnCount(GameManager.Instance.WeaponData) - 1;
                        CompareAttribute("���ݷ�", GameManager.Instance.WeaponData.WeaponATK, weaponData.WeaponATK, ownCount, targetOwnCount);
                        CompareAttribute("���ݷ�%", GameManager.Instance.WeaponData.WeaponATKPercent, weaponData.WeaponATKPercent, ownCount, targetOwnCount);
                        CompareAttribute("��Ÿ Ȯ��", GameManager.Instance.WeaponData.WeaponComboPercent, weaponData.WeaponComboPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("ũ��Ƽ�� Ȯ��", GameManager.Instance.WeaponData.WeaponCriticalPercent, weaponData.WeaponCriticalPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("ũ��Ƽ�� ������", GameManager.Instance.WeaponData.WeaponCriticalDamage, weaponData.WeaponCriticalDamage, ownCount, targetOwnCount, true);
                        CompareAttribute("���� Ȯ��", GameManager.Instance.WeaponData.WeaponDrainPercent, weaponData.WeaponDrainPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("����", GameManager.Instance.WeaponData.WeaponDrainAmount, weaponData.WeaponDrainAmount, ownCount, targetOwnCount, true);
                        CompareAddAttribute("�߰� STR%", GameManager.Instance.WeaponData.WeaponSTRPercent, weaponData.WeaponSTRPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� DEX%", GameManager.Instance.WeaponData.WeaponDEXPercent, weaponData.WeaponDEXPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("���ݷ�", 0, weaponData.WeaponATK, ownCount, targetOwnCount);
                        CompareAttribute("���ݷ�%", 0, weaponData.WeaponATKPercent, ownCount, targetOwnCount);
                        CompareAttribute("��Ÿ Ȯ��", 0, weaponData.WeaponComboPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("ũ��Ƽ�� Ȯ��", 0, weaponData.WeaponCriticalPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("ũ��Ƽ�� ������", 0, weaponData.WeaponCriticalDamage, ownCount, targetOwnCount, true);
                        CompareAttribute("���� Ȯ��", 0, weaponData.WeaponDrainPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("����", 0, weaponData.WeaponDrainAmount, ownCount, targetOwnCount, true);
                        CompareAddAttribute("�߰� STR%", 0, weaponData.WeaponSTRPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� DEX%", 0, weaponData.WeaponDEXPercent, ownCount, targetOwnCount);
                    }
                }
                break;
            case Category.Armor:
                if (CurrentItem.EquipmentData is ArmorData armorData)
                {
                    targetOwnCount = Mathf.Max(0, GetOwnCount(armorData) - 1);
                    if (GameManager.Instance.ArmorData != null)
                    {
                        ownCount = GetOwnCount(GameManager.Instance.ArmorData) - 1;
                        CompareAttribute("ü��", GameManager.Instance.ArmorData.ArmorHP, armorData.ArmorHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", GameManager.Instance.ArmorData.ArmorHPPercent, armorData.ArmorHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("����", GameManager.Instance.ArmorData.ArmorDef, armorData.ArmorDef, ownCount, targetOwnCount);
                        CompareAttribute("����%", GameManager.Instance.ArmorData.ArmorDefPercent, armorData.ArmorDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("��Ÿ ����", GameManager.Instance.ArmorData.ArmorComboResist, armorData.ArmorComboResist, ownCount, targetOwnCount, false);
                        CompareAttribute("ũ��Ƽ�� ����", GameManager.Instance.ArmorData.ArmorCriticalResist, armorData.ArmorCriticalResist, ownCount, targetOwnCount, false);
                        CompareAttribute("���� ����", GameManager.Instance.ArmorData.ArmorDrainResist, armorData.ArmorDrainResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.ArmorData.ArmorVITPercent, armorData.ArmorVITPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, armorData.ArmorHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", 0, armorData.ArmorHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("����", 0, armorData.ArmorDef, ownCount, targetOwnCount);
                        CompareAttribute("����%", 0, armorData.ArmorDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("��Ÿ ����", 0, armorData.ArmorComboResist, ownCount, targetOwnCount, false);
                        CompareAttribute("ũ��Ƽ�� ����", 0, armorData.ArmorCriticalResist, ownCount, targetOwnCount, false);
                        CompareAttribute("���� ����", 0, armorData.ArmorDrainResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� VIT%", 0, armorData.ArmorVITPercent, ownCount, targetOwnCount);
                    }
                }
                break;
            case Category.Helmet:
                if (CurrentItem.EquipmentData is HelmetData helmetData)
                {
                    targetOwnCount = Mathf.Max(0, GetOwnCount(helmetData) - 1);
                    if (GameManager.Instance.HelmetData != null)
                    {
                        ownCount = GetOwnCount(GameManager.Instance.HelmetData) - 1;
                        CompareAttribute("ü��", GameManager.Instance.HelmetData.HelmetHP, helmetData.HelmetHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", GameManager.Instance.HelmetData.HelmetHPPercent, helmetData.HelmetHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("����", GameManager.Instance.HelmetData.HelmetDef, helmetData.HelmetDef, ownCount, targetOwnCount);
                        CompareAttribute("����%", GameManager.Instance.HelmetData.HelmetDefPercent, helmetData.HelmetDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("ȸ�� Ȯ��", GameManager.Instance.HelmetData.HelmetAvoidPercent, helmetData.HelmetAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("���� ����", GameManager.Instance.HelmetData.HelmetDrainResist, helmetData.HelmetDrainResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� STR%", GameManager.Instance.HelmetData.HelmetSTRPercent, helmetData.HelmetSTRPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.HelmetData.HelmetVITPercent, helmetData.HelmetVITPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, helmetData.HelmetHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", 0, helmetData.HelmetHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("����", 0, helmetData.HelmetDef, ownCount, targetOwnCount);
                        CompareAttribute("����%", 0, helmetData.HelmetDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("ȸ�� Ȯ��", 0, helmetData.HelmetAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("���� ����", 0, helmetData.HelmetDrainResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� STR%", 0, helmetData.HelmetSTRPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� VIT%", 0, helmetData.HelmetVITPercent, ownCount, targetOwnCount);
                    }
                }
                break;
            case Category.Pants:
                if (CurrentItem.EquipmentData is PantsData pantsData)
                {
                    targetOwnCount = Mathf.Max(0, GetOwnCount(pantsData) - 1);
                    if (GameManager.Instance.PantsData != null)
                    {
                        ownCount = GetOwnCount(GameManager.Instance.PantsData) - 1;
                        CompareAttribute("ü��", GameManager.Instance.PantsData.PantsHP, pantsData.PantsHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", GameManager.Instance.PantsData.PantsHPPercent, pantsData.PantsHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("����", GameManager.Instance.PantsData.PantsDef, pantsData.PantsDef, ownCount, targetOwnCount);
                        CompareAttribute("����%", GameManager.Instance.PantsData.PantsDefPercent, pantsData.PantsDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("��Ÿ ����", GameManager.Instance.PantsData.PantsComboResist, pantsData.PantsComboResist, ownCount, targetOwnCount, false);
                        CompareAttribute("ũ��Ƽ�� ����", GameManager.Instance.PantsData.PantsCriticalResist, pantsData.PantsCriticalResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.PantsData.PantsVITPercent, pantsData.PantsVITPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, pantsData.PantsHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", 0, pantsData.PantsHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("����", 0, pantsData.PantsDef, ownCount, targetOwnCount);
                        CompareAttribute("����%", 0, pantsData.PantsDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("��Ÿ ����", 0, pantsData.PantsComboResist, ownCount, targetOwnCount, false);
                        CompareAttribute("ũ��Ƽ�� ����", 0, pantsData.PantsCriticalResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� VIT%", 0, pantsData.PantsVITPercent, ownCount, targetOwnCount);
                    }
                }
                break;
            case Category.Glove:
                if (CurrentItem.EquipmentData is GloveData gloveData)
                {
                    targetOwnCount = Mathf.Max(0, GetOwnCount(gloveData) - 1);
                    if (GameManager.Instance.GloveData != null)
                    {
                        ownCount = GetOwnCount(GameManager.Instance.GloveData) - 1;
                        CompareAttribute("���ݷ�", GameManager.Instance.GloveData.GloveATK, gloveData.GloveATK, ownCount, targetOwnCount);
                        CompareAttribute("���ݷ�%", GameManager.Instance.GloveData.GloveATKPercent, gloveData.GloveATKPercent, ownCount, targetOwnCount);
                        CompareAttribute("ü��", GameManager.Instance.GloveData.GloveHP, gloveData.GloveHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", GameManager.Instance.GloveData.GloveHPPercent, gloveData.GloveHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("��Ÿ Ȯ��", GameManager.Instance.GloveData.GloveComboPercent, gloveData.GloveComboPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("ũ��Ƽ�� Ȯ��", GameManager.Instance.GloveData.GloveCriticalPercent, gloveData.GloveCriticalPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("ũ��Ƽ�� ������", GameManager.Instance.GloveData.GloveCriticalDamage, gloveData.GloveCriticalDamage, ownCount, targetOwnCount, true);
                        CompareAddAttribute("�߰� STR%", GameManager.Instance.GloveData.GloveSTRPercent, gloveData.GloveSTRPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� DEX%", GameManager.Instance.GloveData.GloveDEXPercent, gloveData.GloveDEXPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("���ݷ�", 0, gloveData.GloveATK, ownCount, targetOwnCount);
                        CompareAttribute("���ݷ�%", 0, gloveData.GloveATKPercent, ownCount, targetOwnCount);
                        CompareAttribute("ü��", 0, gloveData.GloveHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", 0, gloveData.GloveHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("��Ÿ Ȯ��", 0, gloveData.GloveComboPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("ũ��Ƽ�� Ȯ��", 0, gloveData.GloveCriticalPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("ũ��Ƽ�� ������", 0, gloveData.GloveCriticalDamage, ownCount, targetOwnCount, true);
                        CompareAddAttribute("�߰� STR%", 0, gloveData.GloveSTRPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� DEX%", 0, gloveData.GloveDEXPercent, ownCount, targetOwnCount);
                    }
                }
                break;
            case Category.Shoes:
                if (CurrentItem.EquipmentData is ShoesData shoesData)
                {
                    targetOwnCount = Mathf.Max(0, GetOwnCount(shoesData) - 1);
                    if (GameManager.Instance.ShoesData != null)
                    {
                        ownCount = GetOwnCount(GameManager.Instance.ShoesData) - 1;
                        CompareAttribute("ü��", GameManager.Instance.ShoesData.ShoesHP, shoesData.ShoesHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", GameManager.Instance.ShoesData.ShoesHPPercent, shoesData.ShoesHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("����", GameManager.Instance.ShoesData.ShoesDef, shoesData.ShoesDef, ownCount, targetOwnCount);
                        CompareAttribute("����%", GameManager.Instance.ShoesData.ShoesDefPercent, shoesData.ShoesDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("ȸ�� Ȯ��", GameManager.Instance.ShoesData.ShoesAvoidPercent, shoesData.ShoesAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("ȸ�� ����", GameManager.Instance.ShoesData.ShoesAvoidResist, shoesData.ShoesAvoidResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� DEX%", GameManager.Instance.ShoesData.ShoesDEXPercent, shoesData.ShoesDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.ShoesData.ShoesVITPercent, shoesData.ShoesVITPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�̵��ӵ�", GameManager.Instance.ShoesData.ShoesMoveSpeed, shoesData.ShoesMoveSpeed, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, shoesData.ShoesHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", 0, shoesData.ShoesHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("����", 0, shoesData.ShoesDef, ownCount, targetOwnCount);
                        CompareAttribute("����%", 0, shoesData.ShoesDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("ȸ�� Ȯ��", 0, shoesData.ShoesAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("ȸ�� ����", 0, shoesData.ShoesAvoidResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� DEX%", 0, shoesData.ShoesDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� VIT%", 0, shoesData.ShoesVITPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�̵��ӵ�", 0, shoesData.ShoesMoveSpeed, ownCount, targetOwnCount);
                    }
                }
                break;
            case Category.Belt:
                if (CurrentItem.EquipmentData is BeltData beltData)
                {
                    targetOwnCount = Mathf.Max(0, GetOwnCount(beltData) - 1);
                    if (GameManager.Instance.BeltData != null)
                    {
                        ownCount = GetOwnCount(GameManager.Instance.BeltData) - 1;
                        CompareAttribute("ü��", GameManager.Instance.BeltData.BeltHP, beltData.BeltHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", GameManager.Instance.BeltData.BeltHPPercent, beltData.BeltHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("ȸ�� Ȯ��", GameManager.Instance.BeltData.BeltAvoidPercent, beltData.BeltAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� ����ġ%", GameManager.Instance.BeltData.BeltEXPPercent, beltData.BeltEXPPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� LUC%", GameManager.Instance.BeltData.BeltLUCPercent, beltData.BeltLUCPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.BeltData.BeltVITPercent, beltData.BeltVITPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, beltData.BeltHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", 0, beltData.BeltHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("ȸ�� Ȯ��", 0, beltData.BeltAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� ����ġ%", 0, beltData.BeltEXPPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� LUC%", 0, beltData.BeltLUCPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� VIT%", 0, beltData.BeltVITPercent, ownCount, targetOwnCount);
                    }
                }
                break;
            case Category.ShoulderArmor:
                if (CurrentItem.EquipmentData is ShoulderData shoulderData)
                {
                    targetOwnCount = Mathf.Max(0, GetOwnCount(shoulderData) - 1);
                    if (GameManager.Instance.ShoulderArmorData != null)
                    {
                        ownCount = GetOwnCount(GameManager.Instance.ShoulderArmorData) - 1;
                        CompareAttribute("����", GameManager.Instance.ShoulderArmorData.ShoulderDef, shoulderData.ShoulderDef, ownCount, targetOwnCount);
                        CompareAttribute("����%", GameManager.Instance.ShoulderArmorData.ShoulderDefPercent, shoulderData.ShoulderDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("ũ��Ƽ�� ����", GameManager.Instance.ShoulderArmorData.ShoulderCriticalResist, shoulderData.ShoulderCriticalResist, ownCount, targetOwnCount, false);
                        CompareAttribute("���� ����", GameManager.Instance.ShoulderArmorData.ShoulderDrainResist, shoulderData.ShoulderDrainResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� DEX%", GameManager.Instance.ShoulderArmorData.ShoulderDEXPercent, shoulderData.ShoulderDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.ShoulderArmorData.ShoulderVITPercent, shoulderData.ShoulderVITPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("����", 0, shoulderData.ShoulderDef, ownCount, targetOwnCount);
                        CompareAttribute("����%", 0, shoulderData.ShoulderDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("ũ��Ƽ�� ����", 0, shoulderData.ShoulderCriticalResist, ownCount, targetOwnCount, false);
                        CompareAttribute("���� ����", 0, shoulderData.ShoulderDrainResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� DEX%", 0, shoulderData.ShoulderDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� VIT%", 0, shoulderData.ShoulderVITPercent, ownCount, targetOwnCount);
                    }
                }
                break;
            case Category.Ring:
                if (CurrentItem.EquipmentData is RingData ringData)
                {
                    CompareRingAttribute(Slot, ringData);
                }
                break;
            case Category.Neckless:
                if (CurrentItem.EquipmentData is NecklessData necklessData)
                {
                    targetOwnCount = Mathf.Max(0, GetOwnCount(necklessData) - 1);
                    if (GameManager.Instance.NecklessData != null)
                    {
                        ownCount = GetOwnCount(GameManager.Instance.NecklessData) - 1;
                        CompareAttribute("ü��", GameManager.Instance.NecklessData.NecklessHP, necklessData.NecklessHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", GameManager.Instance.NecklessData.NecklessHPPercent, necklessData.NecklessHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("��Ÿ Ȯ��", GameManager.Instance.NecklessData.NecklessComboPercent, necklessData.NecklessComboPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("ȸ�� Ȯ��", GameManager.Instance.NecklessData.NecklessAvoidResist, necklessData.NecklessAvoidResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� ����ġ%", GameManager.Instance.NecklessData.NecklessEXPPercent, necklessData.NecklessEXPPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("������ ��ӷ�%", GameManager.Instance.NecklessData.NecklessItemDropRate, necklessData.NecklessItemDropRate, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� ���%", GameManager.Instance.NecklessData.NecklessGoldPercent, necklessData.NecklessGoldPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� DEX%", GameManager.Instance.NecklessData.NecklessDEXPercent, necklessData.NecklessDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� LUC%", GameManager.Instance.NecklessData.NecklessLUCPercent, necklessData.NecklessLUCPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, necklessData.NecklessHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", 0, necklessData.NecklessHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("��Ÿ Ȯ��", 0, necklessData.NecklessComboPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("ȸ�� Ȯ��", 0, necklessData.NecklessAvoidResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� ����ġ%", 0, necklessData.NecklessEXPPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("������ ��ӷ�%", 0, necklessData.NecklessItemDropRate, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� ���%", 0, necklessData.NecklessGoldPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� DEX%", 0, necklessData.NecklessDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� LUC%", 0, necklessData.NecklessLUCPercent, ownCount, targetOwnCount);
                    }
                }
                break;
            case Category.Clock:
                if (CurrentItem.EquipmentData is CloakData cloakData)
                {
                    targetOwnCount = Mathf.Max(0, GetOwnCount(cloakData) - 1);
                    if (GameManager.Instance.ClockData != null)
                    {
                        ownCount = GetOwnCount(GameManager.Instance.ClockData) - 1;
                        CompareAttribute("ü��", GameManager.Instance.ClockData.CloakHP, cloakData.CloakHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", GameManager.Instance.ClockData.CloakHPPercent, cloakData.CloakHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("����", GameManager.Instance.ClockData.CloakDef, cloakData.CloakDef, ownCount, targetOwnCount);
                        CompareAttribute("����%", GameManager.Instance.ClockData.CloakDefPercent, cloakData.CloakDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("ȸ�� Ȯ��", GameManager.Instance.ClockData.CloakAvoidPercent, cloakData.CloakAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("��Ÿ ����", GameManager.Instance.ClockData.CloakComboResist, cloakData.CloakComboResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� DEX%", GameManager.Instance.ClockData.CloakDEXPercent, cloakData.CloakDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� LUC%", GameManager.Instance.ClockData.CloakLUCPercent, cloakData.CloakLUCPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.ClockData.CloakVITPercent, cloakData.CloakVITPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, cloakData.CloakHP, ownCount, targetOwnCount);
                        CompareAttribute("ü��%", 0, cloakData.CloakHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("����", 0, cloakData.CloakDef, ownCount, targetOwnCount);
                        CompareAttribute("����%", 0, cloakData.CloakDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("ȸ�� Ȯ��", 0, cloakData.CloakAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("��Ÿ ����", 0, cloakData.CloakComboResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("�߰� DEX%", 0, cloakData.CloakDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� LUC%", 0, cloakData.CloakLUCPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("�߰� VIT%", 0, cloakData.CloakVITPercent, ownCount, targetOwnCount);
                    }
                }
                break;
            case Category.Other:
                if (CurrentItem.EquipmentData is OtherData otherData)
                {
                    CompareOtherAttribute(Slot, otherData);
                }
                break;
        }
    }
    #endregion

    #region �� �ؽ�Ʈ ��� ����

    private void CompareAttribute(string labelText, int equipValue, int dataValue, int equipOwnCount, int targetOwnCount)
    {
        int compareValue = (dataValue + GetOwnCountValue(targetOwnCount, dataValue)) - (equipValue + GetOwnCountValue(equipOwnCount, equipValue));
        CompareBasicData(labelText, compareValue);
    }

    private void CompareAttribute(string labelText, float equipValue, float dataValue, int equipOwnCount, int targetOwnCount, bool isPercentage = false)
    {
        float compareValue = 0f;
        if (isPercentage)
        {
            compareValue = ((dataValue + (float)(GetOwnCountValue(targetOwnCount, dataValue))) - (equipValue + (float)GetOwnCountValue(equipOwnCount, equipValue))) * 100;
        }
        else
        {
            compareValue = (dataValue + (float)GetOwnCountValue(targetOwnCount, dataValue)) - (equipValue + (float)GetOwnCountValue(equipOwnCount, equipValue));
        }
        ComparePercentData(labelText, compareValue);
    }

    private void CompareAddAttribute(string labelText, int equipValue, int dataValue, int equipOwnCount, int targetOwnCount)
    {
        int compareValue = (dataValue + GetOwnCountValue(targetOwnCount, dataValue)) - (equipValue + GetOwnCountValue(equipOwnCount, equipValue));
        CompareAddData(labelText, compareValue);
    }

    private void CompareOtherAttribute(CompareSlot slot, OtherData otherData)
    {
        int targetOwnCount = Mathf.Max(0, GetOwnCount(otherData) - 1);
        if (GameManager.Instance.OtherDatas[(int)slot] != null)
        {
            int ownCount = GetOwnCount(GameManager.Instance.OtherDatas[(int)slot]) - 1;
            CompareAttribute("���ݷ�", GameManager.Instance.OtherDatas[(int)slot].OtherATK, otherData.OtherATK, ownCount, targetOwnCount);
            CompareAttribute("���ݷ�%", GameManager.Instance.OtherDatas[(int)slot].OtherATKPercent, otherData.OtherATKPercent, ownCount, targetOwnCount);
            CompareAttribute("ü��", GameManager.Instance.OtherDatas[(int)slot].OtherHP, otherData.OtherHP, ownCount, targetOwnCount);
            CompareAttribute("ü��%", GameManager.Instance.OtherDatas[(int)slot].OtherHPPercent, otherData.OtherHPPercent, ownCount, targetOwnCount);
            CompareAttribute("����", GameManager.Instance.OtherDatas[(int)slot].OtherDef, otherData.OtherDef, ownCount, targetOwnCount);
            CompareAttribute("����%", GameManager.Instance.OtherDatas[(int)slot].OtherDefPercent, otherData.OtherDefPercent, ownCount, targetOwnCount);
            CompareAttribute("ũ��Ƽ�� Ȯ��", GameManager.Instance.OtherDatas[(int)slot].OtherCriticalPercent, otherData.OtherCriticalPercent, ownCount, targetOwnCount, false);
            CompareAttribute("ũ��Ƽ�� ����", GameManager.Instance.OtherDatas[(int)slot].OtherCriticalResist, otherData.OtherCriticalResist, ownCount, targetOwnCount, false);
            CompareAttribute("ũ��Ƽ�� ������", GameManager.Instance.OtherDatas[(int)slot].OtherCriticalDamage, otherData.OtherCriticalDamage, ownCount, targetOwnCount, true);
            CompareAttribute("���� Ȯ��", GameManager.Instance.OtherDatas[(int)slot].OtherDrainPercent, otherData.OtherDrainPercent, ownCount, targetOwnCount, false);
            CompareAttribute("���� ����", GameManager.Instance.OtherDatas[(int)slot].OtherDrainResist, otherData.OtherDrainResist, ownCount, targetOwnCount, false);
            CompareAttribute("����", GameManager.Instance.OtherDatas[(int)slot].OtherDrainAmount, otherData.OtherDrainAmount, ownCount, targetOwnCount, true);
            CompareAttribute("��Ÿ Ȯ��", GameManager.Instance.OtherDatas[(int)slot].OtherComboPercent, otherData.OtherComboPercent, ownCount, targetOwnCount, false);
            CompareAttribute("��Ÿ ����", GameManager.Instance.OtherDatas[(int)slot].OtherComboResist, otherData.OtherComboResist, ownCount, targetOwnCount, false);
            CompareAttribute("ȸ�� Ȯ��", GameManager.Instance.OtherDatas[(int)slot].OtherAvoidPercent, otherData.OtherAvoidPercent, ownCount, targetOwnCount, false);
            CompareAttribute("ȸ�� ����", GameManager.Instance.OtherDatas[(int)slot].OtherAvoidResist, otherData.OtherAvoidResist, ownCount, targetOwnCount, false);
            CompareAddAttribute("�߰� ����ġ%", GameManager.Instance.OtherDatas[(int)slot].OtherEXPPercent, otherData.OtherEXPPercent, ownCount, targetOwnCount);
            CompareAddAttribute("������ ��ӷ�%", GameManager.Instance.OtherDatas[(int)slot].OtherItemDropRate, otherData.OtherItemDropRate, ownCount, targetOwnCount);
            CompareAddAttribute("�߰� ���%", GameManager.Instance.OtherDatas[(int)slot].OtherGoldPercent, otherData.OtherGoldPercent, ownCount, targetOwnCount);
            CompareAddAttribute("�߰� STR%", GameManager.Instance.OtherDatas[(int)slot].OtherSTRPercent, otherData.OtherSTRPercent, ownCount, targetOwnCount);
            CompareAddAttribute("�߰� DEX%", GameManager.Instance.OtherDatas[(int)slot].OtherDEXPercent, otherData.OtherDEXPercent, ownCount, targetOwnCount);
            CompareAddAttribute("�߰� LUC%", GameManager.Instance.OtherDatas[(int)slot].OtherLUCPercent, otherData.OtherLUCPercent, ownCount, targetOwnCount);
            CompareAddAttribute("�߰� VIT%", GameManager.Instance.OtherDatas[(int)slot].OtherVITPercent, otherData.OtherVITPercent, ownCount, targetOwnCount);
            CompareAddAttribute("������ �ɷ�ġ", GameManager.Instance.OtherDatas[(int)slot].OtherBonusAP, otherData.OtherBonusAP, ownCount, targetOwnCount);
        }
        else
        {
            // �⺻ ����
            CompareAttribute("���ݷ�", 0, otherData.OtherATK, 0, targetOwnCount);
            CompareAttribute("���ݷ�%", 0, otherData.OtherATKPercent, 0, targetOwnCount);
            CompareAttribute("ü��", 0, otherData.OtherHP, 0, targetOwnCount);
            CompareAttribute("ü��%", 0, otherData.OtherHPPercent, 0, targetOwnCount);
            CompareAttribute("����", 0, otherData.OtherDef, 0, targetOwnCount);
            CompareAttribute("����%", 0, otherData.OtherDefPercent, 0, targetOwnCount);
            // Ȯ��
            CompareAttribute("ũ��Ƽ�� Ȯ��", 0, otherData.OtherCriticalPercent, 0, targetOwnCount, false);
            CompareAttribute("ũ��Ƽ�� ����", 0, otherData.OtherCriticalResist, 0, targetOwnCount, false);
            CompareAttribute("ũ��Ƽ�� ������", 0, otherData.OtherCriticalDamage, 0, targetOwnCount, true);
            CompareAttribute("���� Ȯ��", 0, otherData.OtherDrainPercent, 0, targetOwnCount, false);
            CompareAttribute("���� ����", 0, otherData.OtherDrainResist, 0, targetOwnCount, false);
            CompareAttribute("����", 0, otherData.OtherDrainAmount, 0, targetOwnCount, true);
            CompareAttribute("��Ÿ Ȯ��", 0, otherData.OtherComboPercent, 0, targetOwnCount, false);
            CompareAttribute("��Ÿ ����", 0, otherData.OtherComboResist, 0, targetOwnCount, false);
            CompareAttribute("ȸ�� Ȯ��", 0, otherData.OtherAvoidPercent, 0, targetOwnCount, false);
            CompareAttribute("ȸ�� ����", 0, otherData.OtherAvoidResist, 0, targetOwnCount, false);
            CompareAddAttribute("�߰� ����ġ%", 0, otherData.OtherEXPPercent, 0, targetOwnCount);
            CompareAddAttribute("������ ��ӷ�%", 0, otherData.OtherItemDropRate, 0, targetOwnCount);
            CompareAddAttribute("�߰� ���%", 0, otherData.OtherGoldPercent, 0, targetOwnCount);
            CompareAddAttribute("�߰� STR%", 0, otherData.OtherSTRPercent, 0, targetOwnCount);
            CompareAddAttribute("�߰� DEX%", 0, otherData.OtherDEXPercent, 0, targetOwnCount);
            CompareAddAttribute("�߰� LUC%", 0, otherData.OtherLUCPercent, 0, targetOwnCount);
            CompareAddAttribute("�߰� VIT%", 0, otherData.OtherVITPercent, 0, targetOwnCount);
            CompareAddAttribute("������ �ɷ�ġ", 0, otherData.OtherBonusAP, 0, targetOwnCount);
        }
    }

    private void CompareRingAttribute(CompareSlot slot, RingData ringData)
    {
        int targetOwnCount = Mathf.Max(0, GetOwnCount(ringData) - 1);
        if (GameManager.Instance.RingDatas[(int)slot] != null)
        {
            int ownCount = GetOwnCount(GameManager.Instance.RingDatas[(int)slot]) - 1;
            CompareAttribute("���ݷ�", GameManager.Instance.RingDatas[(int)slot].RingATK, ringData.RingATK, ownCount, targetOwnCount);
            CompareAttribute("���ݷ�%", GameManager.Instance.RingDatas[(int)slot].RingATKPercent, ringData.RingATKPercent, ownCount, targetOwnCount);
            CompareAttribute("����", GameManager.Instance.RingDatas[(int)slot].RingDef, ringData.RingDef, ownCount, targetOwnCount);
            CompareAttribute("����%", GameManager.Instance.RingDatas[(int)slot].RingDefPercent, ringData.RingDefPercent, ownCount, targetOwnCount);
            CompareAttribute("ũ��Ƽ�� Ȯ��", GameManager.Instance.RingDatas[(int)slot].RingCriticalPercent, ringData.RingCriticalPercent, ownCount, targetOwnCount, false);
            CompareAttribute("ũ��Ƽ�� ����", GameManager.Instance.RingDatas[(int)slot].RingCriticalResist, ringData.RingCriticalResist, ownCount, targetOwnCount, false);
            CompareAttribute("���� Ȯ��", GameManager.Instance.RingDatas[(int)slot].RingDrainPercent, ringData.RingDrainPercent, ownCount, targetOwnCount, false);
            CompareAttribute("���� ����", GameManager.Instance.RingDatas[(int)slot].RingDrainResist, ringData.RingDrainResist, ownCount, targetOwnCount, false);
            CompareAddAttribute("�߰� ����ġ%", GameManager.Instance.RingDatas[(int)slot].RingEXPPercent, ringData.RingEXPPercent, ownCount, targetOwnCount);
            CompareAddAttribute("������ ��ӷ�%", GameManager.Instance.RingDatas[(int)slot].RingItemDropRate, ringData.RingItemDropRate, ownCount, targetOwnCount);
            CompareAddAttribute("�߰� ���%", GameManager.Instance.RingDatas[(int)slot].RingGoldPercent, ringData.RingGoldPercent, ownCount, targetOwnCount);
            CompareAddAttribute("�߰� STR%", GameManager.Instance.RingDatas[(int)slot].RingSTRPercent, ringData.RingSTRPercent, ownCount, targetOwnCount);
            CompareAddAttribute("�߰� LUC%", GameManager.Instance.RingDatas[(int)slot].RingLUCPercent, ringData.RingLUCPercent, ownCount, targetOwnCount);
            CompareAddAttribute("�߰� DEX%", GameManager.Instance.RingDatas[(int)slot].RingDEXPercent, ringData.RingDEXPercent, ownCount, targetOwnCount);
        }
        else
        {
            CompareAttribute("���ݷ�", 0, ringData.RingATK, 0, targetOwnCount);
            CompareAttribute("���ݷ�%", 0, ringData.RingATKPercent, 0, targetOwnCount);
            CompareAttribute("����", 0, ringData.RingDef, 0, targetOwnCount);
            CompareAttribute("����%", 0, ringData.RingDefPercent, 0, targetOwnCount);
            CompareAttribute("ũ��Ƽ�� Ȯ��", 0, ringData.RingCriticalPercent, 0, targetOwnCount, false);
            CompareAttribute("ũ��Ƽ�� ����", 0, ringData.RingCriticalResist, 0, targetOwnCount, false);
            CompareAttribute("���� Ȯ��", 0, ringData.RingDrainPercent, 0, targetOwnCount, false);
            CompareAttribute("���� ����", 0, ringData.RingDrainResist, 0, targetOwnCount, false);
            CompareAddAttribute("�߰� ����ġ%", 0, ringData.RingEXPPercent, 0, targetOwnCount);
            CompareAddAttribute("������ ��ӷ�%", 0, ringData.RingItemDropRate, 0, targetOwnCount);
            CompareAddAttribute("�߰� ���%", 0, ringData.RingGoldPercent, 0, targetOwnCount);
            CompareAddAttribute("�߰� STR%", 0, ringData.RingSTRPercent, 0, targetOwnCount);
            CompareAddAttribute("�߰� LUC%", 0, ringData.RingLUCPercent, 0, targetOwnCount);
            CompareAddAttribute("�߰� DEX%", 0, ringData.RingDEXPercent, 0, targetOwnCount);
        }
    }

    private void CompareBasicData(string labelText, int value)
    {
        if (value != 0)
        {
            string colorCode = value > 0 ? "#00FF00" : "#FF0000";
            string sign = value > 0 ? "+" : ""; // ����� �� '+' ��ȣ �߰�
            compareBasicDataText.text += $"{labelText} : <color={colorCode}>{sign}{value:N0}</color>\n";
        }
    }

    private void ComparePercentData(string labelText, float value)
    {
        if (value != 0)
        {
            string colorCode = value > 0 ? "#00FF00" : "#FF0000";
            string sign = value > 0 ? "+" : ""; // ����� �� '+' ��ȣ �߰�
            comparePercentDataText.text += $"{labelText} : <color={colorCode}>{sign}{value:N0}%</color>\n";
        }
    }

    private void CompareAddData(string labelText, float value)
    {
        if (value != 0)
        {
            string colorCode = value > 0 ? "#00FF00" : "#FF0000";
            string sign = value > 0 ? "+" : ""; // ����� �� '+' ��ȣ �߰�
            compareAddDataText.text += $"{labelText} : <color={colorCode}>{sign}{value:N0}</color>\n";
        }
    }
    #endregion

    private int GetOwnCount(EquipmentBaseData equipmentBaseData)
    {
        // ���� ������ ���� ��������
        Dictionary<int, int> owndictionary = DataManager.Instance.GetOwnDictionary(equipmentBaseData);
        int ownCount = owndictionary.ContainsKey(equipmentBaseData.ItemID) ? owndictionary[equipmentBaseData.ItemID] : 0;

        return ownCount;
    }
    private int GetOwnCountValue(int ownCount, int baseValue)
    {
        int ownCountValue = Mathf.RoundToInt((float)(baseValue * ((float)ownCount / 9)));
        return ownCountValue;
    }

    private float GetOwnCountValue(int ownCount, float baseValue)
    {
        float ownCountValue = ((float)(baseValue * ((float)ownCount / 9)));

        return ownCountValue;
    }
}
