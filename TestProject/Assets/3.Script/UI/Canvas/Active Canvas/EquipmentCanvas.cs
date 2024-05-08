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
    private string otherType = "전체";

    [Header("장착하기 창")]
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private TMP_Text equipNameText;
    [SerializeField] private TMP_Text equipdesText;
    [SerializeField] private Image equipImage;
    [SerializeField] private GameObject equipmentQaPanel;
    private bool isEquip = false;

    [Header("아이템 정보")]
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

    
    private void OnEnable()
    {
        EquipmentManager.Instance.ItemListSet("무기", itemListParent);
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
        if (_type == "보조")
        {
            int selectDropDown = categoryDropDown.value;
            switch (categoryDropDown.options[selectDropDown].text)
            {
                case "전체":
                    otherType = "All";
                    break;
                case "경험치":
                    otherType = "EXP";
                    break;
                case "골드 획득량":
                    otherType = "Gold";
                    break;
                case "보너스 능력치":
                    otherType = "BonusAP";
                    break;
                case "아이템 드롭률":
                    otherType = "DropRate";
                    break;
                case "스텟":
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
        // 만약 정보창이 꺼져있다면 켜주기
        if (!infomationPanel.activeSelf) infomationPanel.SetActive(true);
        // 만약 보유 개수가 10개라면 구매 버튼 비활성화
        if (DataManager.Instance.GetOwnDictionary(CurrentItem.EquipmentData)[CurrentItem.EquipmentData.ItemID] == 10) BuyButton.interactable = false;
        // 만약 상점에서 구매 불가능한 아이템이라면 버튼 비활성화
        else if (!CurrentItem.EquipmentData.isCanBuy) BuyButton.interactable = false;
        // 조건을 통과했다면 활성화
        else BuyButton.interactable = true;

        GetDataText();
        GetCompareText();
    }

    #region 장비 장착
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
                    { // 장비하기 눌렀을 때
                        for (int i = 0; i < itemListParent.childCount; i++)
                        {
                            ItemPanel itemPanel = itemListParent.GetChild(i).GetComponent<ItemPanel>();
                            if (GameManager.Instance.WeaponData != null && GameManager.Instance.WeaponData.ItemID == itemPanel.EquipmentData.ItemID)
                            { // 장착중 아이콘 표시
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break; // 찾았으면 for문 나감
                            }
                        }
                        GameManager.Instance.WeaponData = weaponData;
                        weaponIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 장착");
                    }
                    else
                    { // 해제하기 눌렀을 때
                        if(CurrentItem.ItemID == GameManager.Instance.WeaponData.ItemID)
                        { // 다른 장비를 눌렀을 때도 장착 해제되는 현상 방지
                            EquipMarkActive();
                            GameManager.Instance.WeaponData = GameManager.Instance.Punch;
                            weaponIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 해제");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"장착중이지 않은 아이템 입니다.");
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
                            { // 장착중 아이콘 표시
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break; // 찾았으면 for문 나감
                            }
                        }
                        GameManager.Instance.ArmorData = armorData;
                        armorIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 장착");
                    }
                    else
                    {
                        if (CurrentItem.ItemID == GameManager.Instance.ArmorData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.ArmorData = null;
                            armorIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 해제");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"장착중이지 않은 아이템 입니다.");
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
                            { // 장착중 아이콘 표시
                                itemPanel.SelectIcon.SetActive(true);
                                itemPanel.EquipCheckIcon.SetActive(false);
                                break; // 찾았으면 for문 나감
                            }
                        }
                        GameManager.Instance.PantsData = pantsData;
                        pantsIcon.sprite = EquipmentManager.Instance.GetEquipmentSprite(CurrentItem.EquipmentData);
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 장착");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.PantsData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.PantsData = null;
                            pantsIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 해제");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"장착중이지 않은 아이템 입니다.");
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
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 장착");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.HelmetData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.HelmetData = null;
                            helmetIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 해제");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"장착중이지 않은 아이템 입니다.");
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
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 장착");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.GloveData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.GloveData = null;
                            gloveIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 해제");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"장착중이지 않은 아이템 입니다.");
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
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 장착");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.ShoesData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.ShoesData = null;
                            shoesIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 해제");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"장착중이지 않은 아이템 입니다.");
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
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 장착");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.ClockData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.ClockData = null;
                            cloakIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 해제");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"장착중이지 않은 아이템 입니다.");
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
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 장착");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.BeltData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.BeltData = null;
                            beltIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 해제");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"장착중이지 않은 아이템 입니다.");
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
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 장착");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.ShoulderArmorData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.ShoulderArmorData = null;
                            shoulderArmorIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 해제");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"장착중이지 않은 아이템 입니다.");
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
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 장착");
                    }
                    else
                    {
                        if (CurrentItem.ItemID == GameManager.Instance.NecklessData.ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.NecklessData = null;
                            necklessIcon.sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 해제");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"장착중이지 않은 아이템 입니다.");
                        }
                    }
                break;
            case Category.Ring:
                if (CurrentItem.EquipmentData is RingData ringData)
                {
                    for (int i = 0; i < GameManager.Instance.RingDatas.Length; i++)
                    {
                        if (GameManager.Instance.RingDatas[i] != null && GameManager.Instance.RingDatas[i].ItemID == ringData.ItemID)
                        { // 중복 착용 todo
                            PrintLog.Instance.StaticLog("이미 착용중인 아이템 입니다.");
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
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 장착");
                    }
                    else
                    {
                        if(CurrentItem.ItemID == GameManager.Instance.RingDatas[(int)Slot].ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.RingDatas[(int)Slot] = null;
                            ringIcons[(int)Slot].sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 해제");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"장착중이지 않은 아이템 입니다.");
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
                        { // 중복 착용 todo
                            PrintLog.Instance.StaticLog("이미 착용중인 아이템 입니다.");
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
                        PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 장착");
                    }
                    else
                    {
                        if (CurrentItem.ItemID == GameManager.Instance.OtherDatas[(int)Slot].ItemID)
                        {
                            EquipMarkActive();
                            GameManager.Instance.OtherDatas[(int)Slot] = null;
                            otherIcons[(int)Slot].sprite = GameManager.Instance.NoneBackground;
                            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 해제");
                        }
                        else
                        {
                            PrintLog.Instance.StaticLog($"장착중이지 않은 아이템 입니다.");
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
            PrintLog.Instance.StaticLog("보유 중인 아이템이 아닙니다.");
            return;
        }
        equipmentQaPanel.SetActive(true);

        isEquip = _isEquip;

        string equipment = isEquip ? "장착하기" : "해제하기";
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
        { // 보유개수 10개 꽉찼음
            PrintLog.Instance.StaticLog("더 이상 구매할 수 없습니다.");
            return;
        }

        if (GameManager.Instance.Gold >= CurrentItem.EquipmentData.RequireCost)
        {
            GameManager.Instance.Gold -= CurrentItem.EquipmentData.RequireCost;
            // 보유 수량 딕셔너리
            int itemID = CurrentItem.EquipmentData.ItemID;

            // 구매 했으니 밸류 값 올려주기
            if (ownDictionary.ContainsKey(itemID))
            {
                ownDictionary[itemID]++;
            }

            totalGoldText.text = $"{GameManager.Instance.Gold:N0}";
            ownCountText.text = $"보유 수량 : {ownDictionary[itemID]}";
            CurrentItem.OwnCount.text = ownDictionary[itemID].ToString();
            PrintLog.Instance.StaticLog($"[{CurrentItem.EquipmentData.EquipmentName}] 구매 성공!");

            // 구매 완료했다면 뱃지 갱신
            GameManager.Instance.BadgeGrade();
            HUDCanvas.CheckBadgeCount();

            // 스텟 갱신 해줘야됨
            GameManager.Instance.RenewAbility();
        }
        else
        { // todo
            PrintLog.Instance.StaticLog("보유 중인 골드가 부족합니다.");
        }
    }

    #region 데이터 텍스트 출력 메소드
    public void GetDataText()
    {
        nameText.text = CurrentItem.EquipmentData.EquipmentName;
        if (!CurrentItem.EquipmentData.isCanBuy)
        { // 구매 불가능한 아이템
            costText.text = "Drop Only.";
        }
        else
        { // 구매 가능한 아이템은 cost 출력
            costText.text = $"{CurrentItem.EquipmentData.RequireCost:N0}";
        }
        ownCountText.text = $"보유 수량 : {DataManager.Instance.GetOwnDictionary(CurrentItem.EquipmentData)[CurrentItem.EquipmentData.ItemID]}";


        int ownCount = 0;
        switch (EquipmentManager.Instance.type)
        {
            case Category.Weapon:
                if (CurrentItem.EquipmentData is WeaponData weaponData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(weaponData));
                    AppendBasicData("공격력", weaponData.WeaponATK, ownCount);
                    AppendBasicData("공격력%", weaponData.WeaponATKPercent, ownCount);
                    AppendPercentData("연타 확률", weaponData.WeaponComboPercent, ownCount);
                    AppendPercentData("크리티컬 확률", weaponData.WeaponCriticalPercent, ownCount);
                    AppendPercentData("크리티컬 데미지", weaponData.WeaponCriticalDamage * 100, ownCount);
                    AppendPercentData("흡혈 확률", weaponData.WeaponDrainPercent, ownCount);
                    AppendPercentData("흡혈", weaponData.WeaponDrainAmount * 100, ownCount);
                    AppendAddData("추가 STR%", weaponData.WeaponSTRPercent, ownCount);
                    AppendAddData("추가 DEX%", weaponData.WeaponDEXPercent, ownCount);
                }
                break;
            case Category.Armor:
                if (CurrentItem.EquipmentData is ArmorData armorData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(armorData));
                    AppendBasicData("체력", armorData.ArmorHP, ownCount);
                    AppendBasicData("체력%", armorData.ArmorHPPercent, ownCount);
                    AppendBasicData("방어력", armorData.ArmorDef, ownCount);
                    AppendBasicData("방어력%", armorData.ArmorDefPercent, ownCount);
                    AppendPercentData("연타 저항", armorData.ArmorComboResist, ownCount);
                    AppendPercentData("크리티컬 저항", armorData.ArmorCriticalResist, ownCount);
                    AppendPercentData("흡혈 저항", armorData.ArmorDrainResist, ownCount);
                    AppendAddData("추가 VIT%", armorData.ArmorVITPercent, ownCount);
                }
                break;
            case Category.Helmet:
                if (CurrentItem.EquipmentData is HelmetData helmetData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(helmetData));
                    AppendBasicData("체력", helmetData.HelmetHP, ownCount);
                    AppendBasicData("체력%", helmetData.HelmetHPPercent, ownCount);
                    AppendBasicData("방어력", helmetData.HelmetDef, ownCount);
                    AppendBasicData("방어력%", helmetData.HelmetDefPercent, ownCount);
                    AppendPercentData("회피 확률", helmetData.HelmetAvoidPercent, ownCount);
                    AppendPercentData("흡혈 저항", helmetData.HelmetDrainResist, ownCount);
                    AppendAddData("추가 STR%", helmetData.HelmetSTRPercent, ownCount);
                    AppendAddData("추가 VIT%", helmetData.HelmetVITPercent, ownCount);
                }
                break;
            case Category.Pants:
                if (CurrentItem.EquipmentData is PantsData pantsData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(pantsData));
                    AppendBasicData("체력", pantsData.PantsHP, ownCount);
                    AppendBasicData("체력%", pantsData.PantsHPPercent, ownCount);
                    AppendBasicData("방어력", pantsData.PantsDef, ownCount);
                    AppendBasicData("방어력%", pantsData.PantsDefPercent, ownCount);
                    AppendPercentData("연타 저항", pantsData.PantsComboResist, ownCount);
                    AppendPercentData("크리티컬 저항", pantsData.PantsCriticalResist, ownCount);
                    AppendAddData("추가 VIT%", pantsData.PantsVITPercent, ownCount);
                }
                break;
            case Category.Glove:
                if (CurrentItem.EquipmentData is GloveData gloveData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(gloveData));
                    AppendBasicData("공격력", gloveData.GloveATK, ownCount);
                    AppendBasicData("공격력%", gloveData.GloveATKPercent, ownCount);
                    AppendBasicData("체력", gloveData.GloveHP, ownCount);
                    AppendBasicData("체력%", gloveData.GloveHPPercent, ownCount);
                    AppendPercentData("연타 확률", gloveData.GloveComboPercent, ownCount);
                    AppendPercentData("크리티컬 확률", gloveData.GloveCriticalPercent, ownCount);
                    AppendPercentData("크리티컬 데미지", gloveData.GloveCriticalDamage * 100, ownCount);
                    AppendAddData("추가 STR%", gloveData.GloveSTRPercent, ownCount);
                    AppendAddData("추가 DEX%", gloveData.GloveDEXPercent, ownCount);
                }
                break;
            case Category.Shoes:
                if (CurrentItem.EquipmentData is ShoesData shoesData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(shoesData));
                    AppendBasicData("체력", shoesData.ShoesHP, ownCount);
                    AppendBasicData("체력%", shoesData.ShoesHPPercent, ownCount);
                    AppendBasicData("방어력", shoesData.ShoesDef, ownCount);
                    AppendBasicData("방어력%", shoesData.ShoesDefPercent, ownCount);
                    AppendPercentData("회피 확률", shoesData.ShoesAvoidPercent, ownCount);
                    AppendPercentData("회피 저항", shoesData.ShoesAvoidResist, ownCount);
                    AppendAddData("추가 DEX%", shoesData.ShoesDEXPercent, ownCount);
                    AppendAddData("추가 VIT%", shoesData.ShoesVITPercent, ownCount);
                    AppendAddData("이동속도 증가", shoesData.ShoesMoveSpeed, ownCount);
                }
                break;
            case Category.Belt:
                if (CurrentItem.EquipmentData is BeltData beltData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(beltData));
                    AppendBasicData("체력", beltData.BeltHP, ownCount);
                    AppendBasicData("체력%", beltData.BeltHPPercent, ownCount);
                    AppendPercentData("회피 확률", beltData.BeltAvoidPercent, ownCount);
                    AppendAddData("추가 경험치%", beltData.BeltEXPPercent, ownCount);
                    AppendAddData("추가 LUC%", beltData.BeltLUCPercent, ownCount);
                    AppendAddData("추가 VIT%", beltData.BeltVITPercent, ownCount);
                }
                break;
            case Category.ShoulderArmor:
                if (CurrentItem.EquipmentData is ShoulderData shoulderData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(shoulderData));
                    AppendBasicData("방어력", shoulderData.ShoulderDef, ownCount);
                    AppendBasicData("방어력%", shoulderData.ShoulderDefPercent, ownCount);
                    AppendPercentData("크리티컬 저항", shoulderData.ShoulderCriticalResist, ownCount);
                    AppendPercentData("흡혈 저항", shoulderData.ShoulderDrainResist, ownCount);
                    AppendAddData("추가 DEX%", shoulderData.ShoulderDEXPercent, ownCount);
                    AppendAddData("추가 VIT%", shoulderData.ShoulderVITPercent, ownCount);
                }
                break;
            case Category.Ring:
                if (CurrentItem.EquipmentData is RingData ringData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(ringData));
                    AppendBasicData("공격력", ringData.RingATK, ownCount);
                    AppendBasicData("공격력%", ringData.RingATKPercent, ownCount);
                    AppendBasicData("방어력", ringData.RingDef, ownCount);
                    AppendBasicData("방어력%", ringData.RingDefPercent, ownCount);
                    AppendPercentData("크리티컬 확률", ringData.RingCriticalPercent, ownCount);
                    AppendPercentData("크리티컬 저항", ringData.RingCriticalResist, ownCount);
                    AppendPercentData("흡혈 확률", ringData.RingDrainPercent, ownCount);
                    AppendPercentData("흡혈 저항", ringData.RingDrainResist, ownCount);
                    AppendAddData("추가 경험치%", ringData.RingEXPPercent, ownCount);
                    AppendAddData("아이템 드롭률%", ringData.RingItemDropRate, ownCount);
                    AppendAddData("추가 골드%", ringData.RingGoldPercent, ownCount);
                    AppendAddData("추가 STR%", ringData.RingSTRPercent, ownCount);
                    AppendAddData("추가 LUC%", ringData.RingLUCPercent, ownCount);
                }
                break;
            case Category.Neckless:
                if (CurrentItem.EquipmentData is NecklessData necklessData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(necklessData));
                    AppendBasicData("체력", necklessData.NecklessHP, ownCount);
                    AppendBasicData("체력%", necklessData.NecklessHPPercent, ownCount);
                    AppendPercentData("연타 확률", necklessData.NecklessComboPercent, ownCount);
                    AppendPercentData("회피 확률", necklessData.NecklessAvoidResist, ownCount);
                    AppendAddData("추가 경험치%", necklessData.NecklessEXPPercent, ownCount);
                    AppendAddData("아이템 드롭률%", necklessData.NecklessItemDropRate, ownCount);
                    AppendAddData("추가 골드%", necklessData.NecklessGoldPercent, ownCount);
                    AppendAddData("추가 DEX%", necklessData.NecklessDEXPercent, ownCount);
                    AppendAddData("추가 LUC%", necklessData.NecklessLUCPercent, ownCount);
                }
                break;
            case Category.Clock:
                if (CurrentItem.EquipmentData is CloakData cloakData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(cloakData));
                    AppendBasicData("체력", cloakData.CloakHP, ownCount);
                    AppendBasicData("체력%", cloakData.CloakHPPercent, ownCount);
                    AppendBasicData("방어력", cloakData.CloakDef, ownCount);
                    AppendBasicData("방어력%", cloakData.CloakDefPercent, ownCount);
                    AppendPercentData("회피 확률", cloakData.CloakAvoidPercent, ownCount);
                    AppendPercentData("연타 저항", cloakData.CloakComboResist, ownCount);
                    AppendAddData("추가 DEX%", cloakData.CloakDEXPercent, ownCount);
                    AppendAddData("추가 LUC%", cloakData.CloakLUCPercent, ownCount);
                    AppendAddData("추가 VIT%", cloakData.CloakVITPercent, ownCount);
                }
                break;
            case Category.Other:
                if (CurrentItem.EquipmentData is OtherData otherData)
                {
                    ownCount = Mathf.Max(0, GetOwnCount(otherData));
                    AppendBasicData("공격력", otherData.OtherATK, ownCount);
                    AppendBasicData("공격력%", otherData.OtherATKPercent, ownCount);
                    AppendBasicData("체력", otherData.OtherHP, ownCount);
                    AppendBasicData("체력%", otherData.OtherHPPercent, ownCount);
                    AppendBasicData("방어력", otherData.OtherDef, ownCount);
                    AppendBasicData("방어력%", otherData.OtherDefPercent, ownCount);
                    AppendPercentData("크리티컬 확률", otherData.OtherCriticalPercent, ownCount);
                    AppendPercentData("크리티컬 저항", otherData.OtherCriticalResist, ownCount);
                    AppendPercentData("크리티컬 데미지", otherData.OtherCriticalDamage * 100, ownCount);
                    AppendPercentData("연타 확률", otherData.OtherComboPercent, ownCount);
                    AppendPercentData("연타 저항", otherData.OtherComboResist, ownCount);
                    AppendPercentData("회피 확률", otherData.OtherAvoidPercent, ownCount);
                    AppendPercentData("회피 저항", otherData.OtherAvoidResist, ownCount);
                    AppendPercentData("흡혈 확률", otherData.OtherDrainPercent, ownCount);
                    AppendPercentData("흡혈 저항", otherData.OtherDrainResist, ownCount);
                    AppendPercentData("흡혈", otherData.OtherDrainAmount, ownCount);
                    AppendAddData("추가 경험치%", otherData.OtherEXPPercent, ownCount);
                    AppendAddData("아이템 드롭률%", otherData.OtherItemDropRate, ownCount);
                    AppendAddData("추가 골드%", otherData.OtherGoldPercent, ownCount);
                    AppendAddData("추가 STR%", otherData.OtherSTRPercent, ownCount);
                    AppendAddData("추가 DEX%", otherData.OtherDEXPercent, ownCount);
                    AppendAddData("추가 LUC%", otherData.OtherLUCPercent, ownCount);
                    AppendAddData("추가 VIT%", otherData.OtherVITPercent, ownCount);
                    AppendAddData("레벨업 능력치", otherData.OtherBonusAP, ownCount);
                }
                break;
        }
        // 리스트에 있는 정보를 텍스트로 변환하여 출력
        basicDataText.text = string.Join("\n", basicDataList);
        percentDataText.text = string.Join("\n", percentDataList);
        addDataText.text = string.Join("\n", addDataList);

        // 리스트 초기화
        basicDataList.Clear();
        percentDataList.Clear();
        addDataList.Clear();
    }
    #endregion

    #region 데이터 텍스트 출력 관련
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

    #region 비교 텍스트 출력 메소드
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
                        CompareAttribute("공격력", GameManager.Instance.WeaponData.WeaponATK, weaponData.WeaponATK, ownCount, targetOwnCount);
                        CompareAttribute("공격력%", GameManager.Instance.WeaponData.WeaponATKPercent, weaponData.WeaponATKPercent, ownCount, targetOwnCount);
                        CompareAttribute("연타 확률", GameManager.Instance.WeaponData.WeaponComboPercent, weaponData.WeaponComboPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("크리티컬 확률", GameManager.Instance.WeaponData.WeaponCriticalPercent, weaponData.WeaponCriticalPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("크리티컬 데미지", GameManager.Instance.WeaponData.WeaponCriticalDamage, weaponData.WeaponCriticalDamage, ownCount, targetOwnCount, true);
                        CompareAttribute("흡혈 확률", GameManager.Instance.WeaponData.WeaponDrainPercent, weaponData.WeaponDrainPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("흡혈", GameManager.Instance.WeaponData.WeaponDrainAmount, weaponData.WeaponDrainAmount, ownCount, targetOwnCount, true);
                        CompareAddAttribute("추가 STR%", GameManager.Instance.WeaponData.WeaponSTRPercent, weaponData.WeaponSTRPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 DEX%", GameManager.Instance.WeaponData.WeaponDEXPercent, weaponData.WeaponDEXPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("공격력", 0, weaponData.WeaponATK, ownCount, targetOwnCount);
                        CompareAttribute("공격력%", 0, weaponData.WeaponATKPercent, ownCount, targetOwnCount);
                        CompareAttribute("연타 확률", 0, weaponData.WeaponComboPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("크리티컬 확률", 0, weaponData.WeaponCriticalPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("크리티컬 데미지", 0, weaponData.WeaponCriticalDamage, ownCount, targetOwnCount, true);
                        CompareAttribute("흡혈 확률", 0, weaponData.WeaponDrainPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("흡혈", 0, weaponData.WeaponDrainAmount, ownCount, targetOwnCount, true);
                        CompareAddAttribute("추가 STR%", 0, weaponData.WeaponSTRPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 DEX%", 0, weaponData.WeaponDEXPercent, ownCount, targetOwnCount);
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
                        CompareAttribute("체력", GameManager.Instance.ArmorData.ArmorHP, armorData.ArmorHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", GameManager.Instance.ArmorData.ArmorHPPercent, armorData.ArmorHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("방어력", GameManager.Instance.ArmorData.ArmorDef, armorData.ArmorDef, ownCount, targetOwnCount);
                        CompareAttribute("방어력%", GameManager.Instance.ArmorData.ArmorDefPercent, armorData.ArmorDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("연타 저항", GameManager.Instance.ArmorData.ArmorComboResist, armorData.ArmorComboResist, ownCount, targetOwnCount, false);
                        CompareAttribute("크리티컬 저항", GameManager.Instance.ArmorData.ArmorCriticalResist, armorData.ArmorCriticalResist, ownCount, targetOwnCount, false);
                        CompareAttribute("흡혈 저항", GameManager.Instance.ArmorData.ArmorDrainResist, armorData.ArmorDrainResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.ArmorData.ArmorVITPercent, armorData.ArmorVITPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, armorData.ArmorHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", 0, armorData.ArmorHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("방어력", 0, armorData.ArmorDef, ownCount, targetOwnCount);
                        CompareAttribute("방어력%", 0, armorData.ArmorDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("연타 저항", 0, armorData.ArmorComboResist, ownCount, targetOwnCount, false);
                        CompareAttribute("크리티컬 저항", 0, armorData.ArmorCriticalResist, ownCount, targetOwnCount, false);
                        CompareAttribute("흡혈 저항", 0, armorData.ArmorDrainResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 VIT%", 0, armorData.ArmorVITPercent, ownCount, targetOwnCount);
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
                        CompareAttribute("체력", GameManager.Instance.HelmetData.HelmetHP, helmetData.HelmetHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", GameManager.Instance.HelmetData.HelmetHPPercent, helmetData.HelmetHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("방어력", GameManager.Instance.HelmetData.HelmetDef, helmetData.HelmetDef, ownCount, targetOwnCount);
                        CompareAttribute("방어력%", GameManager.Instance.HelmetData.HelmetDefPercent, helmetData.HelmetDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("회피 확률", GameManager.Instance.HelmetData.HelmetAvoidPercent, helmetData.HelmetAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("흡혈 저항", GameManager.Instance.HelmetData.HelmetDrainResist, helmetData.HelmetDrainResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 STR%", GameManager.Instance.HelmetData.HelmetSTRPercent, helmetData.HelmetSTRPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.HelmetData.HelmetVITPercent, helmetData.HelmetVITPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, helmetData.HelmetHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", 0, helmetData.HelmetHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("방어력", 0, helmetData.HelmetDef, ownCount, targetOwnCount);
                        CompareAttribute("방어력%", 0, helmetData.HelmetDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("회피 확률", 0, helmetData.HelmetAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("흡혈 저항", 0, helmetData.HelmetDrainResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 STR%", 0, helmetData.HelmetSTRPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 VIT%", 0, helmetData.HelmetVITPercent, ownCount, targetOwnCount);
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
                        CompareAttribute("체력", GameManager.Instance.PantsData.PantsHP, pantsData.PantsHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", GameManager.Instance.PantsData.PantsHPPercent, pantsData.PantsHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("방어력", GameManager.Instance.PantsData.PantsDef, pantsData.PantsDef, ownCount, targetOwnCount);
                        CompareAttribute("방어력%", GameManager.Instance.PantsData.PantsDefPercent, pantsData.PantsDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("연타 저항", GameManager.Instance.PantsData.PantsComboResist, pantsData.PantsComboResist, ownCount, targetOwnCount, false);
                        CompareAttribute("크리티컬 저항", GameManager.Instance.PantsData.PantsCriticalResist, pantsData.PantsCriticalResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.PantsData.PantsVITPercent, pantsData.PantsVITPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, pantsData.PantsHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", 0, pantsData.PantsHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("방어력", 0, pantsData.PantsDef, ownCount, targetOwnCount);
                        CompareAttribute("방어력%", 0, pantsData.PantsDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("연타 저항", 0, pantsData.PantsComboResist, ownCount, targetOwnCount, false);
                        CompareAttribute("크리티컬 저항", 0, pantsData.PantsCriticalResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 VIT%", 0, pantsData.PantsVITPercent, ownCount, targetOwnCount);
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
                        CompareAttribute("공격력", GameManager.Instance.GloveData.GloveATK, gloveData.GloveATK, ownCount, targetOwnCount);
                        CompareAttribute("공격력%", GameManager.Instance.GloveData.GloveATKPercent, gloveData.GloveATKPercent, ownCount, targetOwnCount);
                        CompareAttribute("체력", GameManager.Instance.GloveData.GloveHP, gloveData.GloveHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", GameManager.Instance.GloveData.GloveHPPercent, gloveData.GloveHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("연타 확률", GameManager.Instance.GloveData.GloveComboPercent, gloveData.GloveComboPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("크리티컬 확률", GameManager.Instance.GloveData.GloveCriticalPercent, gloveData.GloveCriticalPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("크리티컬 데미지", GameManager.Instance.GloveData.GloveCriticalDamage, gloveData.GloveCriticalDamage, ownCount, targetOwnCount, true);
                        CompareAddAttribute("추가 STR%", GameManager.Instance.GloveData.GloveSTRPercent, gloveData.GloveSTRPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 DEX%", GameManager.Instance.GloveData.GloveDEXPercent, gloveData.GloveDEXPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("공격력", 0, gloveData.GloveATK, ownCount, targetOwnCount);
                        CompareAttribute("공격력%", 0, gloveData.GloveATKPercent, ownCount, targetOwnCount);
                        CompareAttribute("체력", 0, gloveData.GloveHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", 0, gloveData.GloveHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("연타 확률", 0, gloveData.GloveComboPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("크리티컬 확률", 0, gloveData.GloveCriticalPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("크리티컬 데미지", 0, gloveData.GloveCriticalDamage, ownCount, targetOwnCount, true);
                        CompareAddAttribute("추가 STR%", 0, gloveData.GloveSTRPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 DEX%", 0, gloveData.GloveDEXPercent, ownCount, targetOwnCount);
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
                        CompareAttribute("체력", GameManager.Instance.ShoesData.ShoesHP, shoesData.ShoesHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", GameManager.Instance.ShoesData.ShoesHPPercent, shoesData.ShoesHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("방어력", GameManager.Instance.ShoesData.ShoesDef, shoesData.ShoesDef, ownCount, targetOwnCount);
                        CompareAttribute("방어력%", GameManager.Instance.ShoesData.ShoesDefPercent, shoesData.ShoesDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("회피 확률", GameManager.Instance.ShoesData.ShoesAvoidPercent, shoesData.ShoesAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("회피 저항", GameManager.Instance.ShoesData.ShoesAvoidResist, shoesData.ShoesAvoidResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 DEX%", GameManager.Instance.ShoesData.ShoesDEXPercent, shoesData.ShoesDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.ShoesData.ShoesVITPercent, shoesData.ShoesVITPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("이동속도", GameManager.Instance.ShoesData.ShoesMoveSpeed, shoesData.ShoesMoveSpeed, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, shoesData.ShoesHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", 0, shoesData.ShoesHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("방어력", 0, shoesData.ShoesDef, ownCount, targetOwnCount);
                        CompareAttribute("방어력%", 0, shoesData.ShoesDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("회피 확률", 0, shoesData.ShoesAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("회피 저항", 0, shoesData.ShoesAvoidResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 DEX%", 0, shoesData.ShoesDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 VIT%", 0, shoesData.ShoesVITPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("이동속도", 0, shoesData.ShoesMoveSpeed, ownCount, targetOwnCount);
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
                        CompareAttribute("체력", GameManager.Instance.BeltData.BeltHP, beltData.BeltHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", GameManager.Instance.BeltData.BeltHPPercent, beltData.BeltHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("회피 확률", GameManager.Instance.BeltData.BeltAvoidPercent, beltData.BeltAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 경험치%", GameManager.Instance.BeltData.BeltEXPPercent, beltData.BeltEXPPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 LUC%", GameManager.Instance.BeltData.BeltLUCPercent, beltData.BeltLUCPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.BeltData.BeltVITPercent, beltData.BeltVITPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, beltData.BeltHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", 0, beltData.BeltHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("회피 확률", 0, beltData.BeltAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 경험치%", 0, beltData.BeltEXPPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 LUC%", 0, beltData.BeltLUCPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 VIT%", 0, beltData.BeltVITPercent, ownCount, targetOwnCount);
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
                        CompareAttribute("방어력", GameManager.Instance.ShoulderArmorData.ShoulderDef, shoulderData.ShoulderDef, ownCount, targetOwnCount);
                        CompareAttribute("방어력%", GameManager.Instance.ShoulderArmorData.ShoulderDefPercent, shoulderData.ShoulderDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("크리티컬 저항", GameManager.Instance.ShoulderArmorData.ShoulderCriticalResist, shoulderData.ShoulderCriticalResist, ownCount, targetOwnCount, false);
                        CompareAttribute("흡혈 저항", GameManager.Instance.ShoulderArmorData.ShoulderDrainResist, shoulderData.ShoulderDrainResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 DEX%", GameManager.Instance.ShoulderArmorData.ShoulderDEXPercent, shoulderData.ShoulderDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.ShoulderArmorData.ShoulderVITPercent, shoulderData.ShoulderVITPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("방어력", 0, shoulderData.ShoulderDef, ownCount, targetOwnCount);
                        CompareAttribute("방어력%", 0, shoulderData.ShoulderDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("크리티컬 저항", 0, shoulderData.ShoulderCriticalResist, ownCount, targetOwnCount, false);
                        CompareAttribute("흡혈 저항", 0, shoulderData.ShoulderDrainResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 DEX%", 0, shoulderData.ShoulderDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 VIT%", 0, shoulderData.ShoulderVITPercent, ownCount, targetOwnCount);
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
                        CompareAttribute("체력", GameManager.Instance.NecklessData.NecklessHP, necklessData.NecklessHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", GameManager.Instance.NecklessData.NecklessHPPercent, necklessData.NecklessHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("연타 확률", GameManager.Instance.NecklessData.NecklessComboPercent, necklessData.NecklessComboPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("회피 확률", GameManager.Instance.NecklessData.NecklessAvoidResist, necklessData.NecklessAvoidResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 경험치%", GameManager.Instance.NecklessData.NecklessEXPPercent, necklessData.NecklessEXPPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("아이템 드롭률%", GameManager.Instance.NecklessData.NecklessItemDropRate, necklessData.NecklessItemDropRate, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 골드%", GameManager.Instance.NecklessData.NecklessGoldPercent, necklessData.NecklessGoldPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 DEX%", GameManager.Instance.NecklessData.NecklessDEXPercent, necklessData.NecklessDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 LUC%", GameManager.Instance.NecklessData.NecklessLUCPercent, necklessData.NecklessLUCPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, necklessData.NecklessHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", 0, necklessData.NecklessHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("연타 확률", 0, necklessData.NecklessComboPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("회피 확률", 0, necklessData.NecklessAvoidResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 경험치%", 0, necklessData.NecklessEXPPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("아이템 드롭률%", 0, necklessData.NecklessItemDropRate, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 골드%", 0, necklessData.NecklessGoldPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 DEX%", 0, necklessData.NecklessDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 LUC%", 0, necklessData.NecklessLUCPercent, ownCount, targetOwnCount);
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
                        CompareAttribute("체력", GameManager.Instance.ClockData.CloakHP, cloakData.CloakHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", GameManager.Instance.ClockData.CloakHPPercent, cloakData.CloakHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("방어력", GameManager.Instance.ClockData.CloakDef, cloakData.CloakDef, ownCount, targetOwnCount);
                        CompareAttribute("방어력%", GameManager.Instance.ClockData.CloakDefPercent, cloakData.CloakDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("회피 확률", GameManager.Instance.ClockData.CloakAvoidPercent, cloakData.CloakAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("연타 저항", GameManager.Instance.ClockData.CloakComboResist, cloakData.CloakComboResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 DEX%", GameManager.Instance.ClockData.CloakDEXPercent, cloakData.CloakDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 LUC%", GameManager.Instance.ClockData.CloakLUCPercent, cloakData.CloakLUCPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.ClockData.CloakVITPercent, cloakData.CloakVITPercent, ownCount, targetOwnCount);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, cloakData.CloakHP, ownCount, targetOwnCount);
                        CompareAttribute("체력%", 0, cloakData.CloakHPPercent, ownCount, targetOwnCount);
                        CompareAttribute("방어력", 0, cloakData.CloakDef, ownCount, targetOwnCount);
                        CompareAttribute("방어력%", 0, cloakData.CloakDefPercent, ownCount, targetOwnCount);
                        CompareAttribute("회피 확률", 0, cloakData.CloakAvoidPercent, ownCount, targetOwnCount, false);
                        CompareAttribute("연타 저항", 0, cloakData.CloakComboResist, ownCount, targetOwnCount, false);
                        CompareAddAttribute("추가 DEX%", 0, cloakData.CloakDEXPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 LUC%", 0, cloakData.CloakLUCPercent, ownCount, targetOwnCount);
                        CompareAddAttribute("추가 VIT%", 0, cloakData.CloakVITPercent, ownCount, targetOwnCount);
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

    #region 비교 텍스트 출력 관련

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
            CompareAttribute("공격력", GameManager.Instance.OtherDatas[(int)slot].OtherATK, otherData.OtherATK, ownCount, targetOwnCount);
            CompareAttribute("공격력%", GameManager.Instance.OtherDatas[(int)slot].OtherATKPercent, otherData.OtherATKPercent, ownCount, targetOwnCount);
            CompareAttribute("체력", GameManager.Instance.OtherDatas[(int)slot].OtherHP, otherData.OtherHP, ownCount, targetOwnCount);
            CompareAttribute("체력%", GameManager.Instance.OtherDatas[(int)slot].OtherHPPercent, otherData.OtherHPPercent, ownCount, targetOwnCount);
            CompareAttribute("방어력", GameManager.Instance.OtherDatas[(int)slot].OtherDef, otherData.OtherDef, ownCount, targetOwnCount);
            CompareAttribute("방어력%", GameManager.Instance.OtherDatas[(int)slot].OtherDefPercent, otherData.OtherDefPercent, ownCount, targetOwnCount);
            CompareAttribute("크리티컬 확률", GameManager.Instance.OtherDatas[(int)slot].OtherCriticalPercent, otherData.OtherCriticalPercent, ownCount, targetOwnCount, false);
            CompareAttribute("크리티컬 저항", GameManager.Instance.OtherDatas[(int)slot].OtherCriticalResist, otherData.OtherCriticalResist, ownCount, targetOwnCount, false);
            CompareAttribute("크리티컬 데미지", GameManager.Instance.OtherDatas[(int)slot].OtherCriticalDamage, otherData.OtherCriticalDamage, ownCount, targetOwnCount, true);
            CompareAttribute("흡혈 확률", GameManager.Instance.OtherDatas[(int)slot].OtherDrainPercent, otherData.OtherDrainPercent, ownCount, targetOwnCount, false);
            CompareAttribute("흡혈 저항", GameManager.Instance.OtherDatas[(int)slot].OtherDrainResist, otherData.OtherDrainResist, ownCount, targetOwnCount, false);
            CompareAttribute("흡혈", GameManager.Instance.OtherDatas[(int)slot].OtherDrainAmount, otherData.OtherDrainAmount, ownCount, targetOwnCount, true);
            CompareAttribute("연타 확률", GameManager.Instance.OtherDatas[(int)slot].OtherComboPercent, otherData.OtherComboPercent, ownCount, targetOwnCount, false);
            CompareAttribute("연타 저항", GameManager.Instance.OtherDatas[(int)slot].OtherComboResist, otherData.OtherComboResist, ownCount, targetOwnCount, false);
            CompareAttribute("회피 확률", GameManager.Instance.OtherDatas[(int)slot].OtherAvoidPercent, otherData.OtherAvoidPercent, ownCount, targetOwnCount, false);
            CompareAttribute("회피 저항", GameManager.Instance.OtherDatas[(int)slot].OtherAvoidResist, otherData.OtherAvoidResist, ownCount, targetOwnCount, false);
            CompareAddAttribute("추가 경험치%", GameManager.Instance.OtherDatas[(int)slot].OtherEXPPercent, otherData.OtherEXPPercent, ownCount, targetOwnCount);
            CompareAddAttribute("아이템 드롭률%", GameManager.Instance.OtherDatas[(int)slot].OtherItemDropRate, otherData.OtherItemDropRate, ownCount, targetOwnCount);
            CompareAddAttribute("추가 골드%", GameManager.Instance.OtherDatas[(int)slot].OtherGoldPercent, otherData.OtherGoldPercent, ownCount, targetOwnCount);
            CompareAddAttribute("추가 STR%", GameManager.Instance.OtherDatas[(int)slot].OtherSTRPercent, otherData.OtherSTRPercent, ownCount, targetOwnCount);
            CompareAddAttribute("추가 DEX%", GameManager.Instance.OtherDatas[(int)slot].OtherDEXPercent, otherData.OtherDEXPercent, ownCount, targetOwnCount);
            CompareAddAttribute("추가 LUC%", GameManager.Instance.OtherDatas[(int)slot].OtherLUCPercent, otherData.OtherLUCPercent, ownCount, targetOwnCount);
            CompareAddAttribute("추가 VIT%", GameManager.Instance.OtherDatas[(int)slot].OtherVITPercent, otherData.OtherVITPercent, ownCount, targetOwnCount);
            CompareAddAttribute("레벨업 능력치", GameManager.Instance.OtherDatas[(int)slot].OtherBonusAP, otherData.OtherBonusAP, ownCount, targetOwnCount);
        }
        else
        {
            // 기본 정보
            CompareAttribute("공격력", 0, otherData.OtherATK, 0, targetOwnCount);
            CompareAttribute("공격력%", 0, otherData.OtherATKPercent, 0, targetOwnCount);
            CompareAttribute("체력", 0, otherData.OtherHP, 0, targetOwnCount);
            CompareAttribute("체력%", 0, otherData.OtherHPPercent, 0, targetOwnCount);
            CompareAttribute("방어력", 0, otherData.OtherDef, 0, targetOwnCount);
            CompareAttribute("방어력%", 0, otherData.OtherDefPercent, 0, targetOwnCount);
            // 확률
            CompareAttribute("크리티컬 확률", 0, otherData.OtherCriticalPercent, 0, targetOwnCount, false);
            CompareAttribute("크리티컬 저항", 0, otherData.OtherCriticalResist, 0, targetOwnCount, false);
            CompareAttribute("크리티컬 데미지", 0, otherData.OtherCriticalDamage, 0, targetOwnCount, true);
            CompareAttribute("흡혈 확률", 0, otherData.OtherDrainPercent, 0, targetOwnCount, false);
            CompareAttribute("흡혈 저항", 0, otherData.OtherDrainResist, 0, targetOwnCount, false);
            CompareAttribute("흡혈", 0, otherData.OtherDrainAmount, 0, targetOwnCount, true);
            CompareAttribute("연타 확률", 0, otherData.OtherComboPercent, 0, targetOwnCount, false);
            CompareAttribute("연타 저항", 0, otherData.OtherComboResist, 0, targetOwnCount, false);
            CompareAttribute("회피 확률", 0, otherData.OtherAvoidPercent, 0, targetOwnCount, false);
            CompareAttribute("회피 저항", 0, otherData.OtherAvoidResist, 0, targetOwnCount, false);
            CompareAddAttribute("추가 경험치%", 0, otherData.OtherEXPPercent, 0, targetOwnCount);
            CompareAddAttribute("아이템 드롭률%", 0, otherData.OtherItemDropRate, 0, targetOwnCount);
            CompareAddAttribute("추가 골드%", 0, otherData.OtherGoldPercent, 0, targetOwnCount);
            CompareAddAttribute("추가 STR%", 0, otherData.OtherSTRPercent, 0, targetOwnCount);
            CompareAddAttribute("추가 DEX%", 0, otherData.OtherDEXPercent, 0, targetOwnCount);
            CompareAddAttribute("추가 LUC%", 0, otherData.OtherLUCPercent, 0, targetOwnCount);
            CompareAddAttribute("추가 VIT%", 0, otherData.OtherVITPercent, 0, targetOwnCount);
            CompareAddAttribute("레벨업 능력치", 0, otherData.OtherBonusAP, 0, targetOwnCount);
        }
    }

    private void CompareRingAttribute(CompareSlot slot, RingData ringData)
    {
        int targetOwnCount = Mathf.Max(0, GetOwnCount(ringData) - 1);
        if (GameManager.Instance.RingDatas[(int)slot] != null)
        {
            int ownCount = GetOwnCount(GameManager.Instance.RingDatas[(int)slot]) - 1;
            CompareAttribute("공격력", GameManager.Instance.RingDatas[(int)slot].RingATK, ringData.RingATK, ownCount, targetOwnCount);
            CompareAttribute("공격력%", GameManager.Instance.RingDatas[(int)slot].RingATKPercent, ringData.RingATKPercent, ownCount, targetOwnCount);
            CompareAttribute("방어력", GameManager.Instance.RingDatas[(int)slot].RingDef, ringData.RingDef, ownCount, targetOwnCount);
            CompareAttribute("방어력%", GameManager.Instance.RingDatas[(int)slot].RingDefPercent, ringData.RingDefPercent, ownCount, targetOwnCount);
            CompareAttribute("크리티컬 확률", GameManager.Instance.RingDatas[(int)slot].RingCriticalPercent, ringData.RingCriticalPercent, ownCount, targetOwnCount, false);
            CompareAttribute("크리티컬 저항", GameManager.Instance.RingDatas[(int)slot].RingCriticalResist, ringData.RingCriticalResist, ownCount, targetOwnCount, false);
            CompareAttribute("흡혈 확률", GameManager.Instance.RingDatas[(int)slot].RingDrainPercent, ringData.RingDrainPercent, ownCount, targetOwnCount, false);
            CompareAttribute("흡혈 저항", GameManager.Instance.RingDatas[(int)slot].RingDrainResist, ringData.RingDrainResist, ownCount, targetOwnCount, false);
            CompareAddAttribute("추가 경험치%", GameManager.Instance.RingDatas[(int)slot].RingEXPPercent, ringData.RingEXPPercent, ownCount, targetOwnCount);
            CompareAddAttribute("아이템 드롭률%", GameManager.Instance.RingDatas[(int)slot].RingItemDropRate, ringData.RingItemDropRate, ownCount, targetOwnCount);
            CompareAddAttribute("추가 골드%", GameManager.Instance.RingDatas[(int)slot].RingGoldPercent, ringData.RingGoldPercent, ownCount, targetOwnCount);
            CompareAddAttribute("추가 STR%", GameManager.Instance.RingDatas[(int)slot].RingSTRPercent, ringData.RingSTRPercent, ownCount, targetOwnCount);
            CompareAddAttribute("추가 LUC%", GameManager.Instance.RingDatas[(int)slot].RingLUCPercent, ringData.RingLUCPercent, ownCount, targetOwnCount);
            CompareAddAttribute("추가 DEX%", GameManager.Instance.RingDatas[(int)slot].RingDEXPercent, ringData.RingDEXPercent, ownCount, targetOwnCount);
        }
        else
        {
            CompareAttribute("공격력", 0, ringData.RingATK, 0, targetOwnCount);
            CompareAttribute("공격력%", 0, ringData.RingATKPercent, 0, targetOwnCount);
            CompareAttribute("방어력", 0, ringData.RingDef, 0, targetOwnCount);
            CompareAttribute("방어력%", 0, ringData.RingDefPercent, 0, targetOwnCount);
            CompareAttribute("크리티컬 확률", 0, ringData.RingCriticalPercent, 0, targetOwnCount, false);
            CompareAttribute("크리티컬 저항", 0, ringData.RingCriticalResist, 0, targetOwnCount, false);
            CompareAttribute("흡혈 확률", 0, ringData.RingDrainPercent, 0, targetOwnCount, false);
            CompareAttribute("흡혈 저항", 0, ringData.RingDrainResist, 0, targetOwnCount, false);
            CompareAddAttribute("추가 경험치%", 0, ringData.RingEXPPercent, 0, targetOwnCount);
            CompareAddAttribute("아이템 드롭률%", 0, ringData.RingItemDropRate, 0, targetOwnCount);
            CompareAddAttribute("추가 골드%", 0, ringData.RingGoldPercent, 0, targetOwnCount);
            CompareAddAttribute("추가 STR%", 0, ringData.RingSTRPercent, 0, targetOwnCount);
            CompareAddAttribute("추가 LUC%", 0, ringData.RingLUCPercent, 0, targetOwnCount);
            CompareAddAttribute("추가 DEX%", 0, ringData.RingDEXPercent, 0, targetOwnCount);
        }
    }

    private void CompareBasicData(string labelText, int value)
    {
        if (value != 0)
        {
            string colorCode = value > 0 ? "#00FF00" : "#FF0000";
            string sign = value > 0 ? "+" : ""; // 양수일 때 '+' 기호 추가
            compareBasicDataText.text += $"{labelText} : <color={colorCode}>{sign}{value:N0}</color>\n";
        }
    }

    private void ComparePercentData(string labelText, float value)
    {
        if (value != 0)
        {
            string colorCode = value > 0 ? "#00FF00" : "#FF0000";
            string sign = value > 0 ? "+" : ""; // 양수일 때 '+' 기호 추가
            comparePercentDataText.text += $"{labelText} : <color={colorCode}>{sign}{value:N0}%</color>\n";
        }
    }

    private void CompareAddData(string labelText, float value)
    {
        if (value != 0)
        {
            string colorCode = value > 0 ? "#00FF00" : "#FF0000";
            string sign = value > 0 ? "+" : ""; // 양수일 때 '+' 기호 추가
            compareAddDataText.text += $"{labelText} : <color={colorCode}>{sign}{value:N0}</color>\n";
        }
    }
    #endregion

    private int GetOwnCount(EquipmentBaseData equipmentBaseData)
    {
        // 보유 아이템 개수 가져오기
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
