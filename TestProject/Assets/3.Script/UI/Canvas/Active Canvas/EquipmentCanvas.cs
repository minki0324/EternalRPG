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
    [SerializeField] private TMP_Text categoryName;
    [SerializeField] private Transform itemListParent;
    [SerializeField] private TMP_Text totalGoldText;

    List<string> basicDataList = new List<string>();
    List<string> percentDataList = new List<string>();
    List<string> addDataList = new List<string>();

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
        EquipmentManager.Instance.ItemListSet(_type, itemListParent);
    }

    public void SlotSetting(int _slot)
    {
        Slot = (CompareSlot)_slot;
    }

    public void PrintItemInfomation()
    {
        // 만약 정보창이 꺼져있다면 켜주기
        if (!infomationPanel.activeSelf) infomationPanel.SetActive(true);
        // 만약 보유 개수가 10개라면 구매 버튼 비활성화
        if (CurrentItem.EquipmentData.OwnCount == 10) BuyButton.interactable = false;
        // 만약 상점에서 구매 불가능한 아이템이라면 버튼 비활성화
        else if (!CurrentItem.EquipmentData.isCanBuy) BuyButton.interactable = false;
        // 조건을 통과했다면 활성화
        else BuyButton.interactable = true;

        GetDataText();
        GetCompareText();
    }

    public void EquipItem(bool _isEquip)
    {
        if (CurrentItem.EquipmentData.OwnCount == 0)
        {
            Debug.Log("보유 중인 아이템이 아닙니다.");
            return;
        }

        switch (CurrentItem.EquipmentData.EquipmentType)
        {
            case Category.Weapon:
                if (CurrentItem.EquipmentData is WeaponData weaponData)
                    if (_isEquip)
                    { // 장비하기 눌렀을 때
                        GameManager.Instance.WeaponData = weaponData;
                        weaponIcon.sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    { // 해제하기 눌렀을 때
                        GameManager.Instance.WeaponData = GameManager.Instance.Punch;
                        weaponIcon.sprite = GameManager.Instance.NoneBackground;
                    }
                break;
            case Category.Armor:
                if (CurrentItem.EquipmentData is ArmorData armorData)
                    if (_isEquip)
                    {
                        GameManager.Instance.ArmorData = armorData;
                        armorIcon.sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    {
                        GameManager.Instance.ArmorData = null;
                        armorIcon.sprite = GameManager.Instance.NoneBackground;
                    }
                break;
            case Category.Pants:
                if (CurrentItem.EquipmentData is PantsData pantsData)
                    if (_isEquip)
                    {
                        GameManager.Instance.PantsData = pantsData;
                        pantsIcon.sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    {
                        GameManager.Instance.PantsData = null;
                        pantsIcon.sprite = GameManager.Instance.NoneBackground;
                    }
                break;
            case Category.Helmet:
                if (CurrentItem.EquipmentData is HelmetData helmetData)
                    if (_isEquip)
                    {
                        GameManager.Instance.HelmetData = helmetData;
                        helmetIcon.sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    {
                        GameManager.Instance.HelmetData = null;
                        helmetIcon.sprite = GameManager.Instance.NoneBackground;
                    }
                break;
            case Category.Glove:
                if (CurrentItem.EquipmentData is GloveData gloveData)
                    if (_isEquip)
                    {
                        GameManager.Instance.GloveData = gloveData;
                        gloveIcon.sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    {
                        GameManager.Instance.GloveData = null;
                        gloveIcon.sprite = GameManager.Instance.NoneBackground;
                    }
                break;
            case Category.Shoes:
                if (CurrentItem.EquipmentData is ShoesData shoesData)
                    if (_isEquip)
                    {
                        GameManager.Instance.ShoesData = shoesData;
                        shoesIcon.sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    {
                        GameManager.Instance.ShoesData = null;
                        shoesIcon.sprite = GameManager.Instance.NoneBackground;
                    }
                break;
            case Category.Clock:
                if (CurrentItem.EquipmentData is CloakData cloakData)
                    if (_isEquip)
                    {
                        GameManager.Instance.ClockData = cloakData;
                        cloakIcon.sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    {
                        GameManager.Instance.ClockData = null;
                        cloakIcon.sprite = GameManager.Instance.NoneBackground;
                    }
                break;
            case Category.Belt:
                if (CurrentItem.EquipmentData is BeltData beltData)
                    if (_isEquip)
                    {
                        GameManager.Instance.BeltData = beltData;
                        beltIcon.sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    {
                        GameManager.Instance.BeltData = null;
                        beltIcon.sprite = GameManager.Instance.NoneBackground;
                    }
                break;
            case Category.ShoulderArmor:
                if (CurrentItem.EquipmentData is ShoulderData shoulderData)
                    if (_isEquip)
                    {
                        GameManager.Instance.ShoulderArmorData = shoulderData;
                        shoulderArmorIcon.sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    {
                        GameManager.Instance.ShoulderArmorData = null;
                        shoulderArmorIcon.sprite = GameManager.Instance.NoneBackground;
                    }
                break;
            case Category.Neckless:
                if (CurrentItem.EquipmentData is NecklessData necklessData)
                    if (_isEquip)
                    {
                        GameManager.Instance.NecklessData = necklessData;
                        necklessIcon.sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    {
                        GameManager.Instance.NecklessData = null;
                        necklessIcon.sprite = GameManager.Instance.NoneBackground;
                    }
                break;
            case Category.Ring:
                if (CurrentItem.EquipmentData is RingData ringData)
                {
                    for(int i = 0; i < GameManager.Instance.RingDatas.Length; i++)
                    {
                        if(GameManager.Instance.RingDatas[i] == ringData)
                        { // 중복 착용 todo
                            Debug.Log("중복 착용 로그 띄우기");
                            return;
                        }
                    }
                    if (_isEquip)
                    {
                        GameManager.Instance.RingDatas[(int)Slot] = ringData;
                        ringIcons[(int)Slot].sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    {
                        GameManager.Instance.RingDatas[(int)Slot] = null;
                        ringIcons[(int)Slot].sprite = GameManager.Instance.NoneBackground;
                    }
                }
                    
                break;
            case Category.Other:
                if (CurrentItem.EquipmentData is OtherData otherData)
                {
                    for(int i = 0; i < GameManager.Instance.OtherDatas.Length; i++)
                    {
                        if(GameManager.Instance.OtherDatas[i] == otherData)
                        { // 중복 착용 todo
                            Debug.Log("중복 착용 로그 띄우기");
                            return;
                        }
                    }
                    if (_isEquip)
                    {
                        GameManager.Instance.OtherDatas[(int)Slot] = otherData;
                        otherIcons[(int)Slot].sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    {
                        GameManager.Instance.OtherDatas[(int)Slot] = null;
                        otherIcons[(int)Slot].sprite = GameManager.Instance.NoneBackground;
                    }
                }
                break;
        }
        GameManager.Instance.RenewAbility();
    }

    public void BuyItemButton()
    {
        if (GameManager.Instance.Gold >= CurrentItem.EquipmentData.RequireCost)
        {
            GameManager.Instance.Gold -= CurrentItem.EquipmentData.RequireCost;
            CurrentItem.EquipmentData.OwnCount++;
            totalGoldText.text = $"{GameManager.Instance.Gold:N0}";
            ownCountText.text = $"보유 수량 : {CurrentItem.EquipmentData.OwnCount}";
            CurrentItem.OwnCount.text = CurrentItem.EquipmentData.OwnCount.ToString();
        }
        else
        { // todo
            Debug.Log("골드 부족 나중에 로그 띄우기");
        }
    }

    #region 데이터 텍스트 출력 메소드
    public void GetDataText()
    {
        nameText.text = CurrentItem.EquipmentData.EquipmentName;
        if(!CurrentItem.EquipmentData.isCanBuy)
        { // 구매 불가능한 아이템
            costText.text = "Drop Only.";
        }
        else
        { // 구매 가능한 아이템은 cost 출력
            costText.text = $"{CurrentItem.EquipmentData.RequireCost.ToString():N0}";
        }
        ownCountText.text = $"보유 수량 : {CurrentItem.EquipmentData.OwnCount}";

        switch (EquipmentManager.Instance.type)
        {
            case Category.Weapon:
                if (CurrentItem.EquipmentData is WeaponData weaponData)
                {
                    AppendBasicData("공격력", weaponData.WeaponATK);
                    AppendBasicData("공격력%", weaponData.WeaponATKPercent);
                    AppendPercentData("연타 확률", weaponData.WeaponComboPercent);
                    AppendPercentData("크리티컬 확률", weaponData.WeaponCriticalPercent);
                    AppendPercentData("크리티컬 데미지", (weaponData.WeaponCriticalDamage - 1) * 100);
                    AppendPercentData("흡혈 확률", weaponData.WeaponDrainPercent);
                    AppendPercentData("흡혈", weaponData.WeaponDrainAmount);
                }
                break;
            case Category.Armor:
                if (CurrentItem.EquipmentData is ArmorData armorData)
                {
                    AppendBasicData("체력", armorData.ArmorHP);
                    AppendBasicData("체력%", armorData.ArmorHPPercent);
                    AppendBasicData("방어력", armorData.ArmorDef);
                    AppendBasicData("방어력%", armorData.ArmorDefPercent);
                    AppendPercentData("연타 저항", armorData.ArmorComboResist);
                    AppendPercentData("크리티컬 저항", armorData.ArmorCriticalResist);
                    AppendPercentData("흡혈 저항", armorData.ArmorDrainResist);
                }
                break;
            case Category.Helmet:
                if (CurrentItem.EquipmentData is HelmetData helmetData)
                {
                    AppendBasicData("체력", helmetData.HelmetHP);
                    AppendBasicData("체력%", helmetData.HelmetHPPercent);
                    AppendBasicData("방어력", helmetData.HelmetDef);
                    AppendBasicData("방어력%", helmetData.HelmetDefPercent);
                    AppendPercentData("회피 확률", helmetData.HelmetAvoidPercent);
                    AppendPercentData("흡혈 저항", helmetData.HelmetDrainResist);
                }
                break;
            case Category.Pants:
                if (CurrentItem.EquipmentData is PantsData pantsData)
                {
                    AppendBasicData("체력", pantsData.PantsHP);
                    AppendBasicData("체력%", pantsData.PantsHPPercent);
                    AppendBasicData("방어력", pantsData.PantsDef);
                    AppendBasicData("방어력%", pantsData.PantsDefPercent);
                    AppendPercentData("연타 저항", pantsData.PantsComboResist);
                    AppendPercentData("크리티컬 저항", pantsData.PantsCriticalResist);
                }
                break;
            case Category.Glove:
                if (CurrentItem.EquipmentData is GloveData gloveData)
                {
                    AppendBasicData("공격력", gloveData.GloveATK);
                    AppendBasicData("공격력%", gloveData.GloveATKPercent);
                    AppendBasicData("체력", gloveData.GloveHP);
                    AppendBasicData("체력%", gloveData.GloveHPPercent);
                    AppendPercentData("연타 확률", gloveData.GloveComboPercent);
                    AppendPercentData("크리티컬 확률", gloveData.GloveCriticalPercent);
                    AppendPercentData("크리티컬 데미지", (gloveData.GloveCriticalDamage - 1) * 100);
                }
                break;
            case Category.Shoes:
                if (CurrentItem.EquipmentData is ShoesData shoesData)
                {
                    AppendBasicData("체력", shoesData.ShoesHP);
                    AppendBasicData("체력%", shoesData.ShoesHPPercent);
                    AppendBasicData("방어력", shoesData.ShoesDef);
                    AppendBasicData("방어력%", shoesData.ShoesDefPercent);
                    AppendPercentData("회피 확률", shoesData.ShoesAvoidPercent);
                    AppendPercentData("회피 저항", shoesData.ShoesAvoidResist);
                }
                break;
            case Category.Belt:
                if (CurrentItem.EquipmentData is BeltData beltData)
                {
                    AppendBasicData("체력", beltData.BeltHP);
                    AppendBasicData("체력%", beltData.BeltHPPercent);
                    AppendPercentData("회피 확률", beltData.BeltAvoidPercent);
                    AppendAddData("추가 경험치%", beltData.BeltEXPPercent);
                }
                break;
            case Category.ShoulderArmor:
                if (CurrentItem.EquipmentData is ShoulderData shoulderData)
                {
                    AppendBasicData("방어력", shoulderData.ShoulderDef);
                    AppendBasicData("방어력%", shoulderData.ShoulderDefPercent);
                    AppendPercentData("크리티컬 저항", shoulderData.ShoulderCriticalResist);
                    AppendPercentData("흡혈 저항", shoulderData.ShoulderDrainResist);
                }
                break;
            case Category.Ring:
                if (CurrentItem.EquipmentData is RingData ringData)
                {
                    AppendBasicData("공격력", ringData.RingATK);
                    AppendBasicData("공격력%", ringData.RingATKPercent);
                    AppendBasicData("방어력", ringData.RingDef);
                    AppendBasicData("방어력%", ringData.RingDefPercent);
                    AppendPercentData("크리티컬 확률", ringData.RingCriticalPercent);
                    AppendPercentData("크리티컬 저항", ringData.RingCriticalResist);
                    AppendPercentData("흡혈 확률", ringData.RingDrainPercent);
                    AppendPercentData("흡혈 저항", ringData.RingDrainResist);
                    AppendAddData("추가 경험치%", ringData.RingEXPPercent);
                    AppendAddData("추가 골드%", ringData.RingGoldPercent);
                }
                break;
            case Category.Neckless:
                if (CurrentItem.EquipmentData is NecklessData necklessData)
                {
                    AppendBasicData("체력", necklessData.NecklessHP);
                    AppendBasicData("체력%", necklessData.NecklessHPPercent);
                    AppendPercentData("연타 확률", necklessData.NecklessComboPercent);
                    AppendPercentData("회피 확률", necklessData.NecklessAvoidPercent);
                    AppendAddData("추가 경험치%", necklessData.NecklessEXPPercent);
                    AppendAddData("추가 골드%", necklessData.NecklessGoldPercent);
                }
                break;
            case Category.Clock:
                if (CurrentItem.EquipmentData is CloakData cloakData)
                {
                    AppendBasicData("체력", cloakData.CloakHP);
                    AppendBasicData("체력%", cloakData.CloakHPPercent);
                    AppendBasicData("방어력", cloakData.CloakDef);
                    AppendBasicData("방어력%", cloakData.CloakDefPercent);
                    AppendPercentData("회피 확률", cloakData.CloakAvoidPercent);
                    AppendPercentData("연타 저항", cloakData.CloakComboResist);
                }
                break;
            case Category.Other:
                if (CurrentItem.EquipmentData is OtherData otherData)
                {
                    AppendAddData("추가 경험치%", otherData.OtherEXPPercent);
                    AppendAddData("추가 골드%", otherData.OtherGoldPercent);
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
    private void AppendBasicData(string labelText, int value)
    {
        if (value != 0)
        {
            basicDataList.Add($"{labelText} : {value:N0}");
        }
    }

    private void AppendPercentData(string labelText, int value)
    {
        if (value != 0)
        {
            percentDataList.Add($"{labelText} : {value:N0}%");
        }
    }

    private void AppendPercentData(string labelText, float value)
    {
        if (value != 0)
        {
            percentDataList.Add($"{labelText} : {value:N0}%");
        }
    }

    private void AppendAddData(string labelText, float value)
    {
        if (value != 0)
        {
            addDataList.Add($"{labelText} : {value:N0}");
        }
    }
    #endregion

    #region 비교 텍스트 출력 메소드
    public void GetCompareText()
    {
        comparePercentDataText.text = string.Empty;
        compareBasicDataText.text = string.Empty;
        compareAddDataText.text = string.Empty;

        switch (EquipmentManager.Instance.type)
        {
            case Category.Weapon:
                if (CurrentItem.EquipmentData is WeaponData weaponData)
                {
                    if (GameManager.Instance.WeaponData != null)
                    {
                        CompareAttribute("공격력", GameManager.Instance.WeaponData.WeaponATK, weaponData.WeaponATK);
                        CompareAttribute("공격력%", GameManager.Instance.WeaponData.WeaponATKPercent, weaponData.WeaponATKPercent);
                        CompareAttribute("연타 확률", GameManager.Instance.WeaponData.WeaponComboPercent, weaponData.WeaponComboPercent, false);
                        CompareAttribute("크리티컬 확률", GameManager.Instance.WeaponData.WeaponCriticalPercent, weaponData.WeaponCriticalPercent, false);
                        CompareAttribute("크리티컬 데미지", GameManager.Instance.WeaponData.WeaponCriticalDamage, weaponData.WeaponCriticalDamage, true);
                        CompareAttribute("흡혈 확률", GameManager.Instance.WeaponData.WeaponDrainPercent, weaponData.WeaponDrainPercent, false);
                        CompareAttribute("흡혈", GameManager.Instance.WeaponData.WeaponDrainAmount, weaponData.WeaponDrainAmount, true);
                        CompareAddAttribute("추가 STR%", GameManager.Instance.WeaponData.WeaponSTRPercent, weaponData.WeaponSTRPercent);
                        CompareAddAttribute("추가 DEX%", GameManager.Instance.WeaponData.WeaponDEXPercent, weaponData.WeaponDEXPercent);
                    }
                    else
                    {
                        CompareAttribute("공격력", 0, weaponData.WeaponATK);
                        CompareAttribute("공격력%", 0, weaponData.WeaponATKPercent);
                        CompareAttribute("연타 확률", 0, weaponData.WeaponComboPercent, false);
                        CompareAttribute("크리티컬 확률", 0, weaponData.WeaponCriticalPercent, false);
                        CompareAttribute("크리티컬 데미지", 0, weaponData.WeaponCriticalDamage, true);
                        CompareAttribute("흡혈 확률", 0, weaponData.WeaponDrainPercent, false);
                        CompareAttribute("흡혈", 0, weaponData.WeaponDrainAmount, true);
                        CompareAddAttribute("추가 STR%", 0, weaponData.WeaponSTRPercent);
                        CompareAddAttribute("추가 DEX%", 0, weaponData.WeaponDEXPercent);
                    }
                }
                break;
            case Category.Armor:
                if (CurrentItem.EquipmentData is ArmorData armorData)
                {
                    if (GameManager.Instance.ArmorData != null)
                    {
                        CompareAttribute("체력", GameManager.Instance.ArmorData.ArmorHP, armorData.ArmorHP);
                        CompareAttribute("체력%", GameManager.Instance.ArmorData.ArmorHPPercent, armorData.ArmorHPPercent);
                        CompareAttribute("방어력", GameManager.Instance.ArmorData.ArmorDef, armorData.ArmorDef);
                        CompareAttribute("방어력%", GameManager.Instance.ArmorData.ArmorDefPercent, armorData.ArmorDefPercent);
                        CompareAttribute("연타 저항", GameManager.Instance.ArmorData.ArmorComboResist, armorData.ArmorComboResist, false);
                        CompareAttribute("크리티컬 저항", GameManager.Instance.ArmorData.ArmorCriticalResist, armorData.ArmorCriticalResist, false);
                        CompareAttribute("흡혈 저항", GameManager.Instance.ArmorData.ArmorDrainResist, armorData.ArmorDrainResist, false);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.ArmorData.ArmorVITPercent, armorData.ArmorVITPercent);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, armorData.ArmorHP);
                        CompareAttribute("체력%", 0, armorData.ArmorHPPercent);
                        CompareAttribute("방어력", 0, armorData.ArmorDef);
                        CompareAttribute("방어력%", 0, armorData.ArmorDefPercent);
                        CompareAttribute("연타 저항", 0, armorData.ArmorComboResist, false);
                        CompareAttribute("크리티컬 저항", 0, armorData.ArmorCriticalResist, false);
                        CompareAttribute("흡혈 저항", 0, armorData.ArmorDrainResist, false);
                        CompareAddAttribute("추가 VIT%", 0, armorData.ArmorVITPercent);
                    }
                }
                break;
            case Category.Helmet:
                if (CurrentItem.EquipmentData is HelmetData helmetData)
                {
                    if (GameManager.Instance.HelmetData != null)
                    {
                        CompareAttribute("체력", GameManager.Instance.HelmetData.HelmetHP, helmetData.HelmetHP);
                        CompareAttribute("체력%", GameManager.Instance.HelmetData.HelmetHPPercent, helmetData.HelmetHPPercent);
                        CompareAttribute("방어력", GameManager.Instance.HelmetData.HelmetDef, helmetData.HelmetDef);
                        CompareAttribute("방어력%", GameManager.Instance.HelmetData.HelmetDefPercent, helmetData.HelmetDefPercent);
                        CompareAttribute("회피 확률", GameManager.Instance.HelmetData.HelmetAvoidPercent, helmetData.HelmetAvoidPercent, false);
                        CompareAttribute("흡혈 저항", GameManager.Instance.HelmetData.HelmetDrainResist, helmetData.HelmetDrainResist, false);
                        CompareAddAttribute("추가 STR%", GameManager.Instance.HelmetData.HelmetSTRPercent, helmetData.HelmetSTRPercent);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.HelmetData.HelmetVITPercent, helmetData.HelmetVITPercent);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, helmetData.HelmetHP);
                        CompareAttribute("체력%", 0, helmetData.HelmetHPPercent);
                        CompareAttribute("방어력", 0, helmetData.HelmetDef);
                        CompareAttribute("방어력%", 0, helmetData.HelmetDefPercent);
                        CompareAttribute("회피 확률", 0, helmetData.HelmetAvoidPercent, false);
                        CompareAttribute("흡혈 저항", 0, helmetData.HelmetDrainResist, false);
                        CompareAddAttribute("추가 STR%", 0, helmetData.HelmetSTRPercent);
                        CompareAddAttribute("추가 VIT%", 0, helmetData.HelmetVITPercent);
                    }
                }
                break;
            case Category.Pants:
                if (CurrentItem.EquipmentData is PantsData pantsData)
                {
                    if (GameManager.Instance.PantsData != null)
                    {
                        CompareAttribute("체력", GameManager.Instance.PantsData.PantsHP, pantsData.PantsHP);
                        CompareAttribute("체력%", GameManager.Instance.PantsData.PantsHPPercent, pantsData.PantsHPPercent);
                        CompareAttribute("방어력", GameManager.Instance.PantsData.PantsDef, pantsData.PantsDef);
                        CompareAttribute("방어력%", GameManager.Instance.PantsData.PantsDefPercent, pantsData.PantsDefPercent);
                        CompareAttribute("연타 저항", GameManager.Instance.PantsData.PantsComboResist, pantsData.PantsComboResist, false);
                        CompareAttribute("크리티컬 저항", GameManager.Instance.PantsData.PantsCriticalResist, pantsData.PantsCriticalResist, false);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.PantsData.PantsVITPercent, pantsData.PantsVITPercent);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, pantsData.PantsHP);
                        CompareAttribute("체력%", 0, pantsData.PantsHPPercent);
                        CompareAttribute("방어력", 0, pantsData.PantsDef);
                        CompareAttribute("방어력%", 0, pantsData.PantsDefPercent);
                        CompareAttribute("연타 저항", 0, pantsData.PantsComboResist, false);
                        CompareAttribute("크리티컬 저항", 0, pantsData.PantsCriticalResist, false);
                        CompareAddAttribute("추가 VIT%", 0, pantsData.PantsVITPercent);
                    }
                }
                break;
            case Category.Glove:
                if (CurrentItem.EquipmentData is GloveData gloveData)
                {
                    if (GameManager.Instance.GloveData != null)
                    {
                        CompareAttribute("공격력", GameManager.Instance.GloveData.GloveATK, gloveData.GloveATK);
                        CompareAttribute("공격력%", GameManager.Instance.GloveData.GloveATKPercent, gloveData.GloveATKPercent);
                        CompareAttribute("체력", GameManager.Instance.GloveData.GloveHP, gloveData.GloveHP);
                        CompareAttribute("체력%", GameManager.Instance.GloveData.GloveHPPercent, gloveData.GloveHPPercent);
                        CompareAttribute("연타 확률", GameManager.Instance.GloveData.GloveComboPercent, gloveData.GloveComboPercent, false);
                        CompareAttribute("크리티컬 확률", GameManager.Instance.GloveData.GloveCriticalPercent, gloveData.GloveCriticalPercent, false);
                        CompareAttribute("크리티컬 데미지", GameManager.Instance.GloveData.GloveCriticalDamage, gloveData.GloveCriticalDamage, true);
                        CompareAddAttribute("추가 STR%", GameManager.Instance.GloveData.GloveSTRPercent, gloveData.GloveSTRPercent);
                        CompareAddAttribute("추가 DEX%", GameManager.Instance.GloveData.GloveDEXPercent, gloveData.GloveDEXPercent);
                    }
                    else
                    {
                        CompareAttribute("공격력", 0, gloveData.GloveATK);
                        CompareAttribute("공격력%", 0, gloveData.GloveATKPercent);
                        CompareAttribute("체력", 0, gloveData.GloveHP);
                        CompareAttribute("체력%", 0, gloveData.GloveHPPercent);
                        CompareAttribute("연타 확률", 0, gloveData.GloveComboPercent, false);
                        CompareAttribute("크리티컬 확률", 0, gloveData.GloveCriticalPercent, false);
                        CompareAttribute("크리티컬 데미지", 0, gloveData.GloveCriticalDamage, true);
                        CompareAddAttribute("추가 STR%", 0, gloveData.GloveSTRPercent);
                        CompareAddAttribute("추가 DEX%", 0, gloveData.GloveDEXPercent);
                    }
                }
                break;
            case Category.Shoes:
                if (CurrentItem.EquipmentData is ShoesData shoesData)
                {
                    if (GameManager.Instance.ShoesData != null)
                    {
                        CompareAttribute("체력", GameManager.Instance.ShoesData.ShoesHP, shoesData.ShoesHP);
                        CompareAttribute("체력%", GameManager.Instance.ShoesData.ShoesHPPercent, shoesData.ShoesHPPercent);
                        CompareAttribute("방어력", GameManager.Instance.ShoesData.ShoesDef, shoesData.ShoesDef);
                        CompareAttribute("방어력%", GameManager.Instance.ShoesData.ShoesDefPercent, shoesData.ShoesDefPercent);
                        CompareAttribute("회피 확률", GameManager.Instance.ShoesData.ShoesAvoidPercent, shoesData.ShoesAvoidPercent, false);
                        CompareAttribute("회피 저항", GameManager.Instance.ShoesData.ShoesAvoidResist, shoesData.ShoesAvoidResist, false);
                        CompareAddAttribute("추가 DEX%", GameManager.Instance.ShoesData.ShoesDEXPercent, shoesData.ShoesDEXPercent);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.ShoesData.ShoesVITPercent, shoesData.ShoesVITPercent);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, shoesData.ShoesHP);
                        CompareAttribute("체력%", 0, shoesData.ShoesHPPercent);
                        CompareAttribute("방어력", 0, shoesData.ShoesDef);
                        CompareAttribute("방어력%", 0, shoesData.ShoesDefPercent);
                        CompareAttribute("회피 확률", 0, shoesData.ShoesAvoidPercent, false);
                        CompareAttribute("회피 저항", 0, shoesData.ShoesAvoidResist, false);
                        CompareAddAttribute("추가 DEX%", 0, shoesData.ShoesDEXPercent);
                        CompareAddAttribute("추가 VIT%", 0, shoesData.ShoesVITPercent);
                    }
                }
                break;
            case Category.Belt:
                if (CurrentItem.EquipmentData is BeltData beltData)
                {
                    if (GameManager.Instance.BeltData != null)
                    {
                        CompareAttribute("체력", GameManager.Instance.BeltData.BeltHP, beltData.BeltHP);
                        CompareAttribute("체력%", GameManager.Instance.BeltData.BeltHPPercent, beltData.BeltHPPercent);
                        CompareAttribute("회피 확률", GameManager.Instance.BeltData.BeltAvoidPercent, beltData.BeltAvoidPercent, false);
                        CompareAddAttribute("추가 경험치%", GameManager.Instance.BeltData.BeltEXPPercent, beltData.BeltEXPPercent);
                        CompareAddAttribute("추가 LUC%", GameManager.Instance.BeltData.BeltLUCPercent, beltData.BeltLUCPercent);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.BeltData.BeltVITPercent, beltData.BeltVITPercent);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, beltData.BeltHP);
                        CompareAttribute("체력%", 0, beltData.BeltHPPercent);
                        CompareAttribute("회피 확률", 0, beltData.BeltAvoidPercent, false);
                        CompareAddAttribute("추가 경험치%", 0, beltData.BeltEXPPercent);
                        CompareAddAttribute("추가 LUC%", 0, beltData.BeltLUCPercent);
                        CompareAddAttribute("추가 VIT%", 0, beltData.BeltVITPercent);
                    }
                }
                break;
            case Category.ShoulderArmor:
                if (CurrentItem.EquipmentData is ShoulderData shoulderData)
                {
                    if (GameManager.Instance.ShoulderArmorData != null)
                    {
                        CompareAttribute("방어력", GameManager.Instance.ShoulderArmorData.ShoulderDef, shoulderData.ShoulderDef);
                        CompareAttribute("방어력%", GameManager.Instance.ShoulderArmorData.ShoulderDefPercent, shoulderData.ShoulderDefPercent);
                        CompareAttribute("크리티컬 저항", GameManager.Instance.ShoulderArmorData.ShoulderCriticalResist, shoulderData.ShoulderCriticalResist, false);
                        CompareAttribute("흡혈 저항", GameManager.Instance.ShoulderArmorData.ShoulderDrainResist, shoulderData.ShoulderDrainResist, false);
                        CompareAddAttribute("추가 DEX%", GameManager.Instance.ShoulderArmorData.ShoulderDEXPercent, shoulderData.ShoulderDEXPercent);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.ShoulderArmorData.ShoulderVITPercent, shoulderData.ShoulderVITPercent);
                    }
                    else
                    {
                        CompareAttribute("방어력", 0, shoulderData.ShoulderDef);
                        CompareAttribute("방어력%", 0, shoulderData.ShoulderDefPercent);
                        CompareAttribute("크리티컬 저항", 0, shoulderData.ShoulderCriticalResist, false);
                        CompareAttribute("흡혈 저항", 0, shoulderData.ShoulderDrainResist, false);
                        CompareAddAttribute("추가 DEX%", 0, shoulderData.ShoulderDEXPercent);
                        CompareAddAttribute("추가 VIT%", 0, shoulderData.ShoulderVITPercent);
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
                    if (GameManager.Instance.NecklessData != null)
                    {
                        CompareAttribute("체력", GameManager.Instance.NecklessData.NecklessHP, necklessData.NecklessHP);
                        CompareAttribute("체력%", GameManager.Instance.NecklessData.NecklessHPPercent, necklessData.NecklessHPPercent);
                        CompareAttribute("연타 확률", GameManager.Instance.NecklessData.NecklessComboPercent, necklessData.NecklessComboPercent, false);
                        CompareAttribute("회피 확률", GameManager.Instance.NecklessData.NecklessAvoidPercent, necklessData.NecklessAvoidPercent, false);
                        CompareAddAttribute("추가 경험치%", GameManager.Instance.NecklessData.NecklessEXPPercent, necklessData.NecklessEXPPercent);
                        CompareAddAttribute("추가 골드%", GameManager.Instance.NecklessData.NecklessGoldPercent, necklessData.NecklessGoldPercent);
                        CompareAddAttribute("추가 DEX%", GameManager.Instance.NecklessData.NecklessDEXPercent, necklessData.NecklessDEXPercent);
                        CompareAddAttribute("추가 LUC%", GameManager.Instance.NecklessData.NecklessLUCPercent, necklessData.NecklessLUCPercent);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, necklessData.NecklessHP);
                        CompareAttribute("체력%", 0, necklessData.NecklessHPPercent);
                        CompareAttribute("연타 확률", 0, necklessData.NecklessComboPercent, false);
                        CompareAttribute("회피 확률", 0, necklessData.NecklessAvoidPercent, false);
                        CompareAddAttribute("추가 경험치%", 0, necklessData.NecklessEXPPercent);
                        CompareAddAttribute("추가 골드%", 0, necklessData.NecklessGoldPercent);
                        CompareAddAttribute("추가 DEX%", 0, necklessData.NecklessDEXPercent);
                        CompareAddAttribute("추가 LUC%", 0, necklessData.NecklessLUCPercent);
                    }
                }
                break;
            case Category.Clock:
                if (CurrentItem.EquipmentData is CloakData cloakData)
                {
                    if (GameManager.Instance.ClockData != null)
                    {
                        CompareAttribute("체력", GameManager.Instance.ClockData.CloakHP, cloakData.CloakHP);
                        CompareAttribute("체력%", GameManager.Instance.ClockData.CloakHPPercent, cloakData.CloakHPPercent);
                        CompareAttribute("방어력", GameManager.Instance.ClockData.CloakDef, cloakData.CloakDef);
                        CompareAttribute("방어력%", GameManager.Instance.ClockData.CloakDefPercent, cloakData.CloakDefPercent);
                        CompareAttribute("회피 확률", GameManager.Instance.ClockData.CloakAvoidPercent, cloakData.CloakAvoidPercent, false);
                        CompareAttribute("연타 저항", GameManager.Instance.ClockData.CloakComboResist, cloakData.CloakComboResist, false);
                        CompareAddAttribute("추가 DEX%", GameManager.Instance.ClockData.CloakDEXPercent, cloakData.CloakDEXPercent);
                        CompareAddAttribute("추가 LUC%", GameManager.Instance.ClockData.CloakLUCPercent, cloakData.CloakLUCPercent);
                        CompareAddAttribute("추가 VIT%", GameManager.Instance.ClockData.CloakVITPercent, cloakData.CloakVITPercent);
                    }
                    else
                    {
                        CompareAttribute("체력", 0, cloakData.CloakHP);
                        CompareAttribute("체력%", 0, cloakData.CloakHPPercent);
                        CompareAttribute("방어력", 0, cloakData.CloakDef);
                        CompareAttribute("방어력%", 0, cloakData.CloakDefPercent);
                        CompareAttribute("회피 확률", 0, cloakData.CloakAvoidPercent, false);
                        CompareAttribute("연타 저항", 0, cloakData.CloakComboResist, false);
                        CompareAddAttribute("추가 DEX%", 0, cloakData.CloakDEXPercent);
                        CompareAddAttribute("추가 LUC%", 0, cloakData.CloakLUCPercent);
                        CompareAddAttribute("추가 VIT%", 0, cloakData.CloakVITPercent);
                    }
                }
                break;
            case Category.Other:
                if(CurrentItem.EquipmentData is OtherData otherData)
                {
                    CompareOtherAttribute(Slot, otherData);
                }
                break;
        }
    }
    #endregion

    #region 비교 텍스트 출력 관련

    private void CompareAttribute(string labelText, int equipValue, int dataValue)
    {
        int compareValue = dataValue - equipValue;
        CompareBasicData(labelText, compareValue);
    }

    private void CompareAttribute(string labelText, float equipValue, float dataValue, bool isPercentage = false)
    {
        float compareValue = dataValue - equipValue;
        if (isPercentage)
        {
            compareValue = (compareValue - 1) * 100;
        }
        ComparePercentData(labelText, compareValue);
    }

    private void CompareAddAttribute(string labelText, int equipValue, int dataValue)
    {
        int compareValue = dataValue - equipValue;
        CompareAddData(labelText, compareValue);
    }

    private void CompareOtherAttribute(CompareSlot slot, OtherData otherData)
    {
        if (GameManager.Instance.OtherDatas[(int)slot] != null)
        {
            CompareAttribute("공격력", GameManager.Instance.OtherDatas[(int)slot].OtherATK, otherData.OtherATK);
            CompareAttribute("공격력%", GameManager.Instance.OtherDatas[(int)slot].OtherATKPercent, otherData.OtherATKPercent);
            CompareAttribute("체력", GameManager.Instance.OtherDatas[(int)slot].OtherHP, otherData.OtherHP);
            CompareAttribute("체력%", GameManager.Instance.OtherDatas[(int)slot].OtherHPPercent, otherData.OtherHPPercent);
            CompareAttribute("방어력", GameManager.Instance.OtherDatas[(int)slot].OtherDef, otherData.OtherDef);
            CompareAttribute("방어력%", GameManager.Instance.OtherDatas[(int)slot].OtherDefPercent, otherData.OtherDefPercent);
            CompareAttribute("크리티컬 확률", GameManager.Instance.OtherDatas[(int)slot].OtherCriticalPercent, otherData.OtherDefPercent, false);
            CompareAttribute("크리티컬 저항", GameManager.Instance.OtherDatas[(int)slot].OtherCriticalResist, otherData.OtherDefPercent, false);
            CompareAttribute("크리티컬 데미지", GameManager.Instance.OtherDatas[(int)slot].OtherCriticalDamage, otherData.OtherDefPercent, true);
            CompareAttribute("흡혈 확률", GameManager.Instance.OtherDatas[(int)slot].OtherDrainPercent, otherData.OtherDefPercent, false);
            CompareAttribute("흡혈 저항", GameManager.Instance.OtherDatas[(int)slot].OtherDrainResist, otherData.OtherDefPercent, false);
            CompareAttribute("흡혈", GameManager.Instance.OtherDatas[(int)slot].OtherDrainAmount, otherData.OtherDefPercent, true);
            CompareAttribute("연타 확률", GameManager.Instance.OtherDatas[(int)slot].OtherComboPercent, otherData.OtherDefPercent, false);
            CompareAttribute("연타 저항", GameManager.Instance.OtherDatas[(int)slot].OtherComboResist, otherData.OtherDefPercent, false);
            CompareAttribute("회피 확률", GameManager.Instance.OtherDatas[(int)slot].OtherAvoidPercent, otherData.OtherDefPercent, false);
            CompareAttribute("회피 저항", GameManager.Instance.OtherDatas[(int)slot].OtherAvoidResist, otherData.OtherDefPercent, false);
            CompareAddAttribute("추가 경험치%", GameManager.Instance.OtherDatas[(int)slot].OtherEXPPercent, otherData.OtherEXPPercent);
            CompareAddAttribute("추가 골드%", GameManager.Instance.OtherDatas[(int)slot].OtherEXPPercent, otherData.OtherGoldPercent);
            CompareAddAttribute("추가 STR%", GameManager.Instance.OtherDatas[(int)slot].OtherSTRPercent, otherData.OtherSTRPercent);
            CompareAddAttribute("추가 DEX%", GameManager.Instance.OtherDatas[(int)slot].OtherDEXPercent, otherData.OtherDEXPercent);
            CompareAddAttribute("추가 LUC%", GameManager.Instance.OtherDatas[(int)slot].OtherLUCPercent, otherData.OtherLUCPercent);
            CompareAddAttribute("추가 VIT%", GameManager.Instance.OtherDatas[(int)slot].OtherVITPercent, otherData.OtherVITPercent);
        }
        else
        {
            // 기본 정보
            CompareAttribute("공격력", 0, otherData.OtherATK);
            CompareAttribute("공격력%", 0, otherData.OtherATKPercent);
            CompareAttribute("체력", 0, otherData.OtherHP);
            CompareAttribute("체력%", 0, otherData.OtherHPPercent);
            CompareAttribute("방어력", 0, otherData.OtherDef);
            CompareAttribute("방어력%", 0, otherData.OtherDefPercent);
            // 확률
            CompareAttribute("크리티컬 확률", 0, otherData.OtherDefPercent, false);
            CompareAttribute("크리티컬 저항", 0, otherData.OtherDefPercent, false);
            CompareAttribute("크리티컬 데미지", 0, otherData.OtherDefPercent, true);
            CompareAttribute("흡혈 확률", 0, otherData.OtherDefPercent, false);
            CompareAttribute("흡혈 저항", 0, otherData.OtherDefPercent, false);
            CompareAttribute("흡혈", 0, otherData.OtherDefPercent, true);
            CompareAttribute("연타 확률", 0, otherData.OtherDefPercent, false);
            CompareAttribute("연타 저항", 0, otherData.OtherDefPercent, false);
            CompareAttribute("회피 확률", 0, otherData.OtherDefPercent, false);
            CompareAttribute("회피 저항", 0, otherData.OtherDefPercent, false);
            CompareAddAttribute("추가 경험치%", 0, otherData.OtherEXPPercent);
            CompareAddAttribute("추가 골드%", 0, otherData.OtherGoldPercent);
            CompareAddAttribute("추가 STR%", 0, otherData.OtherSTRPercent);
            CompareAddAttribute("추가 DEX%", 0, otherData.OtherDEXPercent);
            CompareAddAttribute("추가 LUC%", 0, otherData.OtherLUCPercent);
            CompareAddAttribute("추가 VIT%", 0, otherData.OtherVITPercent);
        }
    }

    private void CompareRingAttribute(CompareSlot slot, RingData ringData)
    {
        if (GameManager.Instance.RingDatas[(int)slot] != null)
        {
            CompareAttribute("공격력", GameManager.Instance.RingDatas[(int)slot].RingATK, ringData.RingATK);
            CompareAttribute("공격력%", GameManager.Instance.RingDatas[(int)slot].RingATKPercent, ringData.RingATKPercent);
            CompareAttribute("방어력", GameManager.Instance.RingDatas[(int)slot].RingDef, ringData.RingDef);
            CompareAttribute("방어력%", GameManager.Instance.RingDatas[(int)slot].RingDefPercent, ringData.RingDefPercent);
            CompareAttribute("크리티컬 확률", GameManager.Instance.RingDatas[(int)slot].RingCriticalPercent, ringData.RingCriticalPercent, false);
            CompareAttribute("크리티컬 저항", GameManager.Instance.RingDatas[(int)slot].RingCriticalResist, ringData.RingCriticalResist, false);
            CompareAttribute("흡혈 확률", GameManager.Instance.RingDatas[(int)slot].RingDrainPercent, ringData.RingDrainPercent, false);
            CompareAttribute("흡혈 저항", GameManager.Instance.RingDatas[(int)slot].RingDrainResist, ringData.RingDrainResist, false);
            CompareAddAttribute("추가 경험치%", GameManager.Instance.RingDatas[(int)slot].RingEXPPercent, ringData.RingEXPPercent);
            CompareAddAttribute("추가 골드%", GameManager.Instance.RingDatas[(int)slot].RingGoldPercent, ringData.RingGoldPercent);
            CompareAddAttribute("추가 STR%", GameManager.Instance.RingDatas[(int)slot].RingSTRPercent, ringData.RingSTRPercent);
            CompareAddAttribute("추가 LUC%", GameManager.Instance.RingDatas[(int)slot].RingLUCPercent, ringData.RingLUCPercent);
            CompareAddAttribute("추가 DEX%", GameManager.Instance.RingDatas[(int)slot].RingDEXPercent, ringData.RingDEXPercent);
        }
        else
        {
            CompareAttribute("공격력", 0, ringData.RingATK);
            CompareAttribute("공격력%", 0, ringData.RingATKPercent);
            CompareAttribute("방어력", 0, ringData.RingDef);
            CompareAttribute("방어력%", 0, ringData.RingDefPercent);
            CompareAttribute("크리티컬 확률", 0, ringData.RingCriticalPercent, false);
            CompareAttribute("크리티컬 저항", 0, ringData.RingCriticalResist, false);
            CompareAttribute("흡혈 확률", 0, ringData.RingDrainPercent, false);
            CompareAttribute("흡혈 저항", 0, ringData.RingDrainResist, false);
            CompareAddAttribute("추가 경험치%", 0, ringData.RingEXPPercent);
            CompareAddAttribute("추가 골드%", 0, ringData.RingGoldPercent);
            CompareAddAttribute("추가 STR%", 0, ringData.RingSTRPercent);
            CompareAddAttribute("추가 LUC%", 0, ringData.RingLUCPercent);
            CompareAddAttribute("추가 DEX%", 0, ringData.RingDEXPercent);
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

    private void ComparePercentData(string labelText, int value)
    {
        if (value != 0)
        {
            string colorCode = value > 0 ? "#00FF00" : "#FF0000";
            string sign = value > 0 ? "+" : ""; // 양수일 때 '+' 기호 추가
            comparePercentDataText.text += $"{labelText} : <color={colorCode}>{sign}{value:N0}%</color>\n";
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
}
