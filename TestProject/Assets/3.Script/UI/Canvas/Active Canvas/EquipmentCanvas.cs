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
        // ���� ����â�� �����ִٸ� ���ֱ�
        if (!infomationPanel.activeSelf) infomationPanel.SetActive(true);
        // ���� ���� ������ 10����� ���� ��ư ��Ȱ��ȭ
        if (CurrentItem.EquipmentData.OwnCount == 10) BuyButton.interactable = false;
        // ���� �������� ���� �Ұ����� �������̶�� ��ư ��Ȱ��ȭ
        else if (!CurrentItem.EquipmentData.isCanBuy) BuyButton.interactable = false;
        // ������ ����ߴٸ� Ȱ��ȭ
        else BuyButton.interactable = true;

        GetDataText();
        GetCompareText();
    }

    public void EquipItem(bool _isEquip)
    {
        if (CurrentItem.EquipmentData.OwnCount == 0)
        {
            Debug.Log("���� ���� �������� �ƴմϴ�.");
            return;
        }

        switch (CurrentItem.EquipmentData.EquipmentType)
        {
            case Category.Weapon:
                if (CurrentItem.EquipmentData is WeaponData weaponData)
                    if (_isEquip)
                    { // ����ϱ� ������ ��
                        GameManager.Instance.WeaponData = weaponData;
                        weaponIcon.sprite = CurrentItem.EquipmentData.EquipmentSprite;
                    }
                    else
                    { // �����ϱ� ������ ��
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
                        { // �ߺ� ���� todo
                            Debug.Log("�ߺ� ���� �α� ����");
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
                        { // �ߺ� ���� todo
                            Debug.Log("�ߺ� ���� �α� ����");
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
            ownCountText.text = $"���� ���� : {CurrentItem.EquipmentData.OwnCount}";
            CurrentItem.OwnCount.text = CurrentItem.EquipmentData.OwnCount.ToString();
        }
        else
        { // todo
            Debug.Log("��� ���� ���߿� �α� ����");
        }
    }

    #region ������ �ؽ�Ʈ ��� �޼ҵ�
    public void GetDataText()
    {
        nameText.text = CurrentItem.EquipmentData.EquipmentName;
        if(!CurrentItem.EquipmentData.isCanBuy)
        { // ���� �Ұ����� ������
            costText.text = "Drop Only.";
        }
        else
        { // ���� ������ �������� cost ���
            costText.text = $"{CurrentItem.EquipmentData.RequireCost.ToString():N0}";
        }
        ownCountText.text = $"���� ���� : {CurrentItem.EquipmentData.OwnCount}";

        switch (EquipmentManager.Instance.type)
        {
            case Category.Weapon:
                if (CurrentItem.EquipmentData is WeaponData weaponData)
                {
                    AppendBasicData("���ݷ�", weaponData.WeaponATK);
                    AppendBasicData("���ݷ�%", weaponData.WeaponATKPercent);
                    AppendPercentData("��Ÿ Ȯ��", weaponData.WeaponComboPercent);
                    AppendPercentData("ũ��Ƽ�� Ȯ��", weaponData.WeaponCriticalPercent);
                    AppendPercentData("ũ��Ƽ�� ������", (weaponData.WeaponCriticalDamage - 1) * 100);
                    AppendPercentData("���� Ȯ��", weaponData.WeaponDrainPercent);
                    AppendPercentData("����", weaponData.WeaponDrainAmount);
                }
                break;
            case Category.Armor:
                if (CurrentItem.EquipmentData is ArmorData armorData)
                {
                    AppendBasicData("ü��", armorData.ArmorHP);
                    AppendBasicData("ü��%", armorData.ArmorHPPercent);
                    AppendBasicData("����", armorData.ArmorDef);
                    AppendBasicData("����%", armorData.ArmorDefPercent);
                    AppendPercentData("��Ÿ ����", armorData.ArmorComboResist);
                    AppendPercentData("ũ��Ƽ�� ����", armorData.ArmorCriticalResist);
                    AppendPercentData("���� ����", armorData.ArmorDrainResist);
                }
                break;
            case Category.Helmet:
                if (CurrentItem.EquipmentData is HelmetData helmetData)
                {
                    AppendBasicData("ü��", helmetData.HelmetHP);
                    AppendBasicData("ü��%", helmetData.HelmetHPPercent);
                    AppendBasicData("����", helmetData.HelmetDef);
                    AppendBasicData("����%", helmetData.HelmetDefPercent);
                    AppendPercentData("ȸ�� Ȯ��", helmetData.HelmetAvoidPercent);
                    AppendPercentData("���� ����", helmetData.HelmetDrainResist);
                }
                break;
            case Category.Pants:
                if (CurrentItem.EquipmentData is PantsData pantsData)
                {
                    AppendBasicData("ü��", pantsData.PantsHP);
                    AppendBasicData("ü��%", pantsData.PantsHPPercent);
                    AppendBasicData("����", pantsData.PantsDef);
                    AppendBasicData("����%", pantsData.PantsDefPercent);
                    AppendPercentData("��Ÿ ����", pantsData.PantsComboResist);
                    AppendPercentData("ũ��Ƽ�� ����", pantsData.PantsCriticalResist);
                }
                break;
            case Category.Glove:
                if (CurrentItem.EquipmentData is GloveData gloveData)
                {
                    AppendBasicData("���ݷ�", gloveData.GloveATK);
                    AppendBasicData("���ݷ�%", gloveData.GloveATKPercent);
                    AppendBasicData("ü��", gloveData.GloveHP);
                    AppendBasicData("ü��%", gloveData.GloveHPPercent);
                    AppendPercentData("��Ÿ Ȯ��", gloveData.GloveComboPercent);
                    AppendPercentData("ũ��Ƽ�� Ȯ��", gloveData.GloveCriticalPercent);
                    AppendPercentData("ũ��Ƽ�� ������", (gloveData.GloveCriticalDamage - 1) * 100);
                }
                break;
            case Category.Shoes:
                if (CurrentItem.EquipmentData is ShoesData shoesData)
                {
                    AppendBasicData("ü��", shoesData.ShoesHP);
                    AppendBasicData("ü��%", shoesData.ShoesHPPercent);
                    AppendBasicData("����", shoesData.ShoesDef);
                    AppendBasicData("����%", shoesData.ShoesDefPercent);
                    AppendPercentData("ȸ�� Ȯ��", shoesData.ShoesAvoidPercent);
                    AppendPercentData("ȸ�� ����", shoesData.ShoesAvoidResist);
                }
                break;
            case Category.Belt:
                if (CurrentItem.EquipmentData is BeltData beltData)
                {
                    AppendBasicData("ü��", beltData.BeltHP);
                    AppendBasicData("ü��%", beltData.BeltHPPercent);
                    AppendPercentData("ȸ�� Ȯ��", beltData.BeltAvoidPercent);
                    AppendAddData("�߰� ����ġ%", beltData.BeltEXPPercent);
                }
                break;
            case Category.ShoulderArmor:
                if (CurrentItem.EquipmentData is ShoulderData shoulderData)
                {
                    AppendBasicData("����", shoulderData.ShoulderDef);
                    AppendBasicData("����%", shoulderData.ShoulderDefPercent);
                    AppendPercentData("ũ��Ƽ�� ����", shoulderData.ShoulderCriticalResist);
                    AppendPercentData("���� ����", shoulderData.ShoulderDrainResist);
                }
                break;
            case Category.Ring:
                if (CurrentItem.EquipmentData is RingData ringData)
                {
                    AppendBasicData("���ݷ�", ringData.RingATK);
                    AppendBasicData("���ݷ�%", ringData.RingATKPercent);
                    AppendBasicData("����", ringData.RingDef);
                    AppendBasicData("����%", ringData.RingDefPercent);
                    AppendPercentData("ũ��Ƽ�� Ȯ��", ringData.RingCriticalPercent);
                    AppendPercentData("ũ��Ƽ�� ����", ringData.RingCriticalResist);
                    AppendPercentData("���� Ȯ��", ringData.RingDrainPercent);
                    AppendPercentData("���� ����", ringData.RingDrainResist);
                    AppendAddData("�߰� ����ġ%", ringData.RingEXPPercent);
                    AppendAddData("�߰� ���%", ringData.RingGoldPercent);
                }
                break;
            case Category.Neckless:
                if (CurrentItem.EquipmentData is NecklessData necklessData)
                {
                    AppendBasicData("ü��", necklessData.NecklessHP);
                    AppendBasicData("ü��%", necklessData.NecklessHPPercent);
                    AppendPercentData("��Ÿ Ȯ��", necklessData.NecklessComboPercent);
                    AppendPercentData("ȸ�� Ȯ��", necklessData.NecklessAvoidPercent);
                    AppendAddData("�߰� ����ġ%", necklessData.NecklessEXPPercent);
                    AppendAddData("�߰� ���%", necklessData.NecklessGoldPercent);
                }
                break;
            case Category.Clock:
                if (CurrentItem.EquipmentData is CloakData cloakData)
                {
                    AppendBasicData("ü��", cloakData.CloakHP);
                    AppendBasicData("ü��%", cloakData.CloakHPPercent);
                    AppendBasicData("����", cloakData.CloakDef);
                    AppendBasicData("����%", cloakData.CloakDefPercent);
                    AppendPercentData("ȸ�� Ȯ��", cloakData.CloakAvoidPercent);
                    AppendPercentData("��Ÿ ����", cloakData.CloakComboResist);
                }
                break;
            case Category.Other:
                if (CurrentItem.EquipmentData is OtherData otherData)
                {
                    AppendAddData("�߰� ����ġ%", otherData.OtherEXPPercent);
                    AppendAddData("�߰� ���%", otherData.OtherGoldPercent);
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

    #region �� �ؽ�Ʈ ��� �޼ҵ�
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
                        CompareAttribute("���ݷ�", GameManager.Instance.WeaponData.WeaponATK, weaponData.WeaponATK);
                        CompareAttribute("���ݷ�%", GameManager.Instance.WeaponData.WeaponATKPercent, weaponData.WeaponATKPercent);
                        CompareAttribute("��Ÿ Ȯ��", GameManager.Instance.WeaponData.WeaponComboPercent, weaponData.WeaponComboPercent, false);
                        CompareAttribute("ũ��Ƽ�� Ȯ��", GameManager.Instance.WeaponData.WeaponCriticalPercent, weaponData.WeaponCriticalPercent, false);
                        CompareAttribute("ũ��Ƽ�� ������", GameManager.Instance.WeaponData.WeaponCriticalDamage, weaponData.WeaponCriticalDamage, true);
                        CompareAttribute("���� Ȯ��", GameManager.Instance.WeaponData.WeaponDrainPercent, weaponData.WeaponDrainPercent, false);
                        CompareAttribute("����", GameManager.Instance.WeaponData.WeaponDrainAmount, weaponData.WeaponDrainAmount, true);
                        CompareAddAttribute("�߰� STR%", GameManager.Instance.WeaponData.WeaponSTRPercent, weaponData.WeaponSTRPercent);
                        CompareAddAttribute("�߰� DEX%", GameManager.Instance.WeaponData.WeaponDEXPercent, weaponData.WeaponDEXPercent);
                    }
                    else
                    {
                        CompareAttribute("���ݷ�", 0, weaponData.WeaponATK);
                        CompareAttribute("���ݷ�%", 0, weaponData.WeaponATKPercent);
                        CompareAttribute("��Ÿ Ȯ��", 0, weaponData.WeaponComboPercent, false);
                        CompareAttribute("ũ��Ƽ�� Ȯ��", 0, weaponData.WeaponCriticalPercent, false);
                        CompareAttribute("ũ��Ƽ�� ������", 0, weaponData.WeaponCriticalDamage, true);
                        CompareAttribute("���� Ȯ��", 0, weaponData.WeaponDrainPercent, false);
                        CompareAttribute("����", 0, weaponData.WeaponDrainAmount, true);
                        CompareAddAttribute("�߰� STR%", 0, weaponData.WeaponSTRPercent);
                        CompareAddAttribute("�߰� DEX%", 0, weaponData.WeaponDEXPercent);
                    }
                }
                break;
            case Category.Armor:
                if (CurrentItem.EquipmentData is ArmorData armorData)
                {
                    if (GameManager.Instance.ArmorData != null)
                    {
                        CompareAttribute("ü��", GameManager.Instance.ArmorData.ArmorHP, armorData.ArmorHP);
                        CompareAttribute("ü��%", GameManager.Instance.ArmorData.ArmorHPPercent, armorData.ArmorHPPercent);
                        CompareAttribute("����", GameManager.Instance.ArmorData.ArmorDef, armorData.ArmorDef);
                        CompareAttribute("����%", GameManager.Instance.ArmorData.ArmorDefPercent, armorData.ArmorDefPercent);
                        CompareAttribute("��Ÿ ����", GameManager.Instance.ArmorData.ArmorComboResist, armorData.ArmorComboResist, false);
                        CompareAttribute("ũ��Ƽ�� ����", GameManager.Instance.ArmorData.ArmorCriticalResist, armorData.ArmorCriticalResist, false);
                        CompareAttribute("���� ����", GameManager.Instance.ArmorData.ArmorDrainResist, armorData.ArmorDrainResist, false);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.ArmorData.ArmorVITPercent, armorData.ArmorVITPercent);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, armorData.ArmorHP);
                        CompareAttribute("ü��%", 0, armorData.ArmorHPPercent);
                        CompareAttribute("����", 0, armorData.ArmorDef);
                        CompareAttribute("����%", 0, armorData.ArmorDefPercent);
                        CompareAttribute("��Ÿ ����", 0, armorData.ArmorComboResist, false);
                        CompareAttribute("ũ��Ƽ�� ����", 0, armorData.ArmorCriticalResist, false);
                        CompareAttribute("���� ����", 0, armorData.ArmorDrainResist, false);
                        CompareAddAttribute("�߰� VIT%", 0, armorData.ArmorVITPercent);
                    }
                }
                break;
            case Category.Helmet:
                if (CurrentItem.EquipmentData is HelmetData helmetData)
                {
                    if (GameManager.Instance.HelmetData != null)
                    {
                        CompareAttribute("ü��", GameManager.Instance.HelmetData.HelmetHP, helmetData.HelmetHP);
                        CompareAttribute("ü��%", GameManager.Instance.HelmetData.HelmetHPPercent, helmetData.HelmetHPPercent);
                        CompareAttribute("����", GameManager.Instance.HelmetData.HelmetDef, helmetData.HelmetDef);
                        CompareAttribute("����%", GameManager.Instance.HelmetData.HelmetDefPercent, helmetData.HelmetDefPercent);
                        CompareAttribute("ȸ�� Ȯ��", GameManager.Instance.HelmetData.HelmetAvoidPercent, helmetData.HelmetAvoidPercent, false);
                        CompareAttribute("���� ����", GameManager.Instance.HelmetData.HelmetDrainResist, helmetData.HelmetDrainResist, false);
                        CompareAddAttribute("�߰� STR%", GameManager.Instance.HelmetData.HelmetSTRPercent, helmetData.HelmetSTRPercent);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.HelmetData.HelmetVITPercent, helmetData.HelmetVITPercent);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, helmetData.HelmetHP);
                        CompareAttribute("ü��%", 0, helmetData.HelmetHPPercent);
                        CompareAttribute("����", 0, helmetData.HelmetDef);
                        CompareAttribute("����%", 0, helmetData.HelmetDefPercent);
                        CompareAttribute("ȸ�� Ȯ��", 0, helmetData.HelmetAvoidPercent, false);
                        CompareAttribute("���� ����", 0, helmetData.HelmetDrainResist, false);
                        CompareAddAttribute("�߰� STR%", 0, helmetData.HelmetSTRPercent);
                        CompareAddAttribute("�߰� VIT%", 0, helmetData.HelmetVITPercent);
                    }
                }
                break;
            case Category.Pants:
                if (CurrentItem.EquipmentData is PantsData pantsData)
                {
                    if (GameManager.Instance.PantsData != null)
                    {
                        CompareAttribute("ü��", GameManager.Instance.PantsData.PantsHP, pantsData.PantsHP);
                        CompareAttribute("ü��%", GameManager.Instance.PantsData.PantsHPPercent, pantsData.PantsHPPercent);
                        CompareAttribute("����", GameManager.Instance.PantsData.PantsDef, pantsData.PantsDef);
                        CompareAttribute("����%", GameManager.Instance.PantsData.PantsDefPercent, pantsData.PantsDefPercent);
                        CompareAttribute("��Ÿ ����", GameManager.Instance.PantsData.PantsComboResist, pantsData.PantsComboResist, false);
                        CompareAttribute("ũ��Ƽ�� ����", GameManager.Instance.PantsData.PantsCriticalResist, pantsData.PantsCriticalResist, false);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.PantsData.PantsVITPercent, pantsData.PantsVITPercent);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, pantsData.PantsHP);
                        CompareAttribute("ü��%", 0, pantsData.PantsHPPercent);
                        CompareAttribute("����", 0, pantsData.PantsDef);
                        CompareAttribute("����%", 0, pantsData.PantsDefPercent);
                        CompareAttribute("��Ÿ ����", 0, pantsData.PantsComboResist, false);
                        CompareAttribute("ũ��Ƽ�� ����", 0, pantsData.PantsCriticalResist, false);
                        CompareAddAttribute("�߰� VIT%", 0, pantsData.PantsVITPercent);
                    }
                }
                break;
            case Category.Glove:
                if (CurrentItem.EquipmentData is GloveData gloveData)
                {
                    if (GameManager.Instance.GloveData != null)
                    {
                        CompareAttribute("���ݷ�", GameManager.Instance.GloveData.GloveATK, gloveData.GloveATK);
                        CompareAttribute("���ݷ�%", GameManager.Instance.GloveData.GloveATKPercent, gloveData.GloveATKPercent);
                        CompareAttribute("ü��", GameManager.Instance.GloveData.GloveHP, gloveData.GloveHP);
                        CompareAttribute("ü��%", GameManager.Instance.GloveData.GloveHPPercent, gloveData.GloveHPPercent);
                        CompareAttribute("��Ÿ Ȯ��", GameManager.Instance.GloveData.GloveComboPercent, gloveData.GloveComboPercent, false);
                        CompareAttribute("ũ��Ƽ�� Ȯ��", GameManager.Instance.GloveData.GloveCriticalPercent, gloveData.GloveCriticalPercent, false);
                        CompareAttribute("ũ��Ƽ�� ������", GameManager.Instance.GloveData.GloveCriticalDamage, gloveData.GloveCriticalDamage, true);
                        CompareAddAttribute("�߰� STR%", GameManager.Instance.GloveData.GloveSTRPercent, gloveData.GloveSTRPercent);
                        CompareAddAttribute("�߰� DEX%", GameManager.Instance.GloveData.GloveDEXPercent, gloveData.GloveDEXPercent);
                    }
                    else
                    {
                        CompareAttribute("���ݷ�", 0, gloveData.GloveATK);
                        CompareAttribute("���ݷ�%", 0, gloveData.GloveATKPercent);
                        CompareAttribute("ü��", 0, gloveData.GloveHP);
                        CompareAttribute("ü��%", 0, gloveData.GloveHPPercent);
                        CompareAttribute("��Ÿ Ȯ��", 0, gloveData.GloveComboPercent, false);
                        CompareAttribute("ũ��Ƽ�� Ȯ��", 0, gloveData.GloveCriticalPercent, false);
                        CompareAttribute("ũ��Ƽ�� ������", 0, gloveData.GloveCriticalDamage, true);
                        CompareAddAttribute("�߰� STR%", 0, gloveData.GloveSTRPercent);
                        CompareAddAttribute("�߰� DEX%", 0, gloveData.GloveDEXPercent);
                    }
                }
                break;
            case Category.Shoes:
                if (CurrentItem.EquipmentData is ShoesData shoesData)
                {
                    if (GameManager.Instance.ShoesData != null)
                    {
                        CompareAttribute("ü��", GameManager.Instance.ShoesData.ShoesHP, shoesData.ShoesHP);
                        CompareAttribute("ü��%", GameManager.Instance.ShoesData.ShoesHPPercent, shoesData.ShoesHPPercent);
                        CompareAttribute("����", GameManager.Instance.ShoesData.ShoesDef, shoesData.ShoesDef);
                        CompareAttribute("����%", GameManager.Instance.ShoesData.ShoesDefPercent, shoesData.ShoesDefPercent);
                        CompareAttribute("ȸ�� Ȯ��", GameManager.Instance.ShoesData.ShoesAvoidPercent, shoesData.ShoesAvoidPercent, false);
                        CompareAttribute("ȸ�� ����", GameManager.Instance.ShoesData.ShoesAvoidResist, shoesData.ShoesAvoidResist, false);
                        CompareAddAttribute("�߰� DEX%", GameManager.Instance.ShoesData.ShoesDEXPercent, shoesData.ShoesDEXPercent);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.ShoesData.ShoesVITPercent, shoesData.ShoesVITPercent);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, shoesData.ShoesHP);
                        CompareAttribute("ü��%", 0, shoesData.ShoesHPPercent);
                        CompareAttribute("����", 0, shoesData.ShoesDef);
                        CompareAttribute("����%", 0, shoesData.ShoesDefPercent);
                        CompareAttribute("ȸ�� Ȯ��", 0, shoesData.ShoesAvoidPercent, false);
                        CompareAttribute("ȸ�� ����", 0, shoesData.ShoesAvoidResist, false);
                        CompareAddAttribute("�߰� DEX%", 0, shoesData.ShoesDEXPercent);
                        CompareAddAttribute("�߰� VIT%", 0, shoesData.ShoesVITPercent);
                    }
                }
                break;
            case Category.Belt:
                if (CurrentItem.EquipmentData is BeltData beltData)
                {
                    if (GameManager.Instance.BeltData != null)
                    {
                        CompareAttribute("ü��", GameManager.Instance.BeltData.BeltHP, beltData.BeltHP);
                        CompareAttribute("ü��%", GameManager.Instance.BeltData.BeltHPPercent, beltData.BeltHPPercent);
                        CompareAttribute("ȸ�� Ȯ��", GameManager.Instance.BeltData.BeltAvoidPercent, beltData.BeltAvoidPercent, false);
                        CompareAddAttribute("�߰� ����ġ%", GameManager.Instance.BeltData.BeltEXPPercent, beltData.BeltEXPPercent);
                        CompareAddAttribute("�߰� LUC%", GameManager.Instance.BeltData.BeltLUCPercent, beltData.BeltLUCPercent);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.BeltData.BeltVITPercent, beltData.BeltVITPercent);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, beltData.BeltHP);
                        CompareAttribute("ü��%", 0, beltData.BeltHPPercent);
                        CompareAttribute("ȸ�� Ȯ��", 0, beltData.BeltAvoidPercent, false);
                        CompareAddAttribute("�߰� ����ġ%", 0, beltData.BeltEXPPercent);
                        CompareAddAttribute("�߰� LUC%", 0, beltData.BeltLUCPercent);
                        CompareAddAttribute("�߰� VIT%", 0, beltData.BeltVITPercent);
                    }
                }
                break;
            case Category.ShoulderArmor:
                if (CurrentItem.EquipmentData is ShoulderData shoulderData)
                {
                    if (GameManager.Instance.ShoulderArmorData != null)
                    {
                        CompareAttribute("����", GameManager.Instance.ShoulderArmorData.ShoulderDef, shoulderData.ShoulderDef);
                        CompareAttribute("����%", GameManager.Instance.ShoulderArmorData.ShoulderDefPercent, shoulderData.ShoulderDefPercent);
                        CompareAttribute("ũ��Ƽ�� ����", GameManager.Instance.ShoulderArmorData.ShoulderCriticalResist, shoulderData.ShoulderCriticalResist, false);
                        CompareAttribute("���� ����", GameManager.Instance.ShoulderArmorData.ShoulderDrainResist, shoulderData.ShoulderDrainResist, false);
                        CompareAddAttribute("�߰� DEX%", GameManager.Instance.ShoulderArmorData.ShoulderDEXPercent, shoulderData.ShoulderDEXPercent);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.ShoulderArmorData.ShoulderVITPercent, shoulderData.ShoulderVITPercent);
                    }
                    else
                    {
                        CompareAttribute("����", 0, shoulderData.ShoulderDef);
                        CompareAttribute("����%", 0, shoulderData.ShoulderDefPercent);
                        CompareAttribute("ũ��Ƽ�� ����", 0, shoulderData.ShoulderCriticalResist, false);
                        CompareAttribute("���� ����", 0, shoulderData.ShoulderDrainResist, false);
                        CompareAddAttribute("�߰� DEX%", 0, shoulderData.ShoulderDEXPercent);
                        CompareAddAttribute("�߰� VIT%", 0, shoulderData.ShoulderVITPercent);
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
                        CompareAttribute("ü��", GameManager.Instance.NecklessData.NecklessHP, necklessData.NecklessHP);
                        CompareAttribute("ü��%", GameManager.Instance.NecklessData.NecklessHPPercent, necklessData.NecklessHPPercent);
                        CompareAttribute("��Ÿ Ȯ��", GameManager.Instance.NecklessData.NecklessComboPercent, necklessData.NecklessComboPercent, false);
                        CompareAttribute("ȸ�� Ȯ��", GameManager.Instance.NecklessData.NecklessAvoidPercent, necklessData.NecklessAvoidPercent, false);
                        CompareAddAttribute("�߰� ����ġ%", GameManager.Instance.NecklessData.NecklessEXPPercent, necklessData.NecklessEXPPercent);
                        CompareAddAttribute("�߰� ���%", GameManager.Instance.NecklessData.NecklessGoldPercent, necklessData.NecklessGoldPercent);
                        CompareAddAttribute("�߰� DEX%", GameManager.Instance.NecklessData.NecklessDEXPercent, necklessData.NecklessDEXPercent);
                        CompareAddAttribute("�߰� LUC%", GameManager.Instance.NecklessData.NecklessLUCPercent, necklessData.NecklessLUCPercent);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, necklessData.NecklessHP);
                        CompareAttribute("ü��%", 0, necklessData.NecklessHPPercent);
                        CompareAttribute("��Ÿ Ȯ��", 0, necklessData.NecklessComboPercent, false);
                        CompareAttribute("ȸ�� Ȯ��", 0, necklessData.NecklessAvoidPercent, false);
                        CompareAddAttribute("�߰� ����ġ%", 0, necklessData.NecklessEXPPercent);
                        CompareAddAttribute("�߰� ���%", 0, necklessData.NecklessGoldPercent);
                        CompareAddAttribute("�߰� DEX%", 0, necklessData.NecklessDEXPercent);
                        CompareAddAttribute("�߰� LUC%", 0, necklessData.NecklessLUCPercent);
                    }
                }
                break;
            case Category.Clock:
                if (CurrentItem.EquipmentData is CloakData cloakData)
                {
                    if (GameManager.Instance.ClockData != null)
                    {
                        CompareAttribute("ü��", GameManager.Instance.ClockData.CloakHP, cloakData.CloakHP);
                        CompareAttribute("ü��%", GameManager.Instance.ClockData.CloakHPPercent, cloakData.CloakHPPercent);
                        CompareAttribute("����", GameManager.Instance.ClockData.CloakDef, cloakData.CloakDef);
                        CompareAttribute("����%", GameManager.Instance.ClockData.CloakDefPercent, cloakData.CloakDefPercent);
                        CompareAttribute("ȸ�� Ȯ��", GameManager.Instance.ClockData.CloakAvoidPercent, cloakData.CloakAvoidPercent, false);
                        CompareAttribute("��Ÿ ����", GameManager.Instance.ClockData.CloakComboResist, cloakData.CloakComboResist, false);
                        CompareAddAttribute("�߰� DEX%", GameManager.Instance.ClockData.CloakDEXPercent, cloakData.CloakDEXPercent);
                        CompareAddAttribute("�߰� LUC%", GameManager.Instance.ClockData.CloakLUCPercent, cloakData.CloakLUCPercent);
                        CompareAddAttribute("�߰� VIT%", GameManager.Instance.ClockData.CloakVITPercent, cloakData.CloakVITPercent);
                    }
                    else
                    {
                        CompareAttribute("ü��", 0, cloakData.CloakHP);
                        CompareAttribute("ü��%", 0, cloakData.CloakHPPercent);
                        CompareAttribute("����", 0, cloakData.CloakDef);
                        CompareAttribute("����%", 0, cloakData.CloakDefPercent);
                        CompareAttribute("ȸ�� Ȯ��", 0, cloakData.CloakAvoidPercent, false);
                        CompareAttribute("��Ÿ ����", 0, cloakData.CloakComboResist, false);
                        CompareAddAttribute("�߰� DEX%", 0, cloakData.CloakDEXPercent);
                        CompareAddAttribute("�߰� LUC%", 0, cloakData.CloakLUCPercent);
                        CompareAddAttribute("�߰� VIT%", 0, cloakData.CloakVITPercent);
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

    #region �� �ؽ�Ʈ ��� ����

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
            CompareAttribute("���ݷ�", GameManager.Instance.OtherDatas[(int)slot].OtherATK, otherData.OtherATK);
            CompareAttribute("���ݷ�%", GameManager.Instance.OtherDatas[(int)slot].OtherATKPercent, otherData.OtherATKPercent);
            CompareAttribute("ü��", GameManager.Instance.OtherDatas[(int)slot].OtherHP, otherData.OtherHP);
            CompareAttribute("ü��%", GameManager.Instance.OtherDatas[(int)slot].OtherHPPercent, otherData.OtherHPPercent);
            CompareAttribute("����", GameManager.Instance.OtherDatas[(int)slot].OtherDef, otherData.OtherDef);
            CompareAttribute("����%", GameManager.Instance.OtherDatas[(int)slot].OtherDefPercent, otherData.OtherDefPercent);
            CompareAttribute("ũ��Ƽ�� Ȯ��", GameManager.Instance.OtherDatas[(int)slot].OtherCriticalPercent, otherData.OtherDefPercent, false);
            CompareAttribute("ũ��Ƽ�� ����", GameManager.Instance.OtherDatas[(int)slot].OtherCriticalResist, otherData.OtherDefPercent, false);
            CompareAttribute("ũ��Ƽ�� ������", GameManager.Instance.OtherDatas[(int)slot].OtherCriticalDamage, otherData.OtherDefPercent, true);
            CompareAttribute("���� Ȯ��", GameManager.Instance.OtherDatas[(int)slot].OtherDrainPercent, otherData.OtherDefPercent, false);
            CompareAttribute("���� ����", GameManager.Instance.OtherDatas[(int)slot].OtherDrainResist, otherData.OtherDefPercent, false);
            CompareAttribute("����", GameManager.Instance.OtherDatas[(int)slot].OtherDrainAmount, otherData.OtherDefPercent, true);
            CompareAttribute("��Ÿ Ȯ��", GameManager.Instance.OtherDatas[(int)slot].OtherComboPercent, otherData.OtherDefPercent, false);
            CompareAttribute("��Ÿ ����", GameManager.Instance.OtherDatas[(int)slot].OtherComboResist, otherData.OtherDefPercent, false);
            CompareAttribute("ȸ�� Ȯ��", GameManager.Instance.OtherDatas[(int)slot].OtherAvoidPercent, otherData.OtherDefPercent, false);
            CompareAttribute("ȸ�� ����", GameManager.Instance.OtherDatas[(int)slot].OtherAvoidResist, otherData.OtherDefPercent, false);
            CompareAddAttribute("�߰� ����ġ%", GameManager.Instance.OtherDatas[(int)slot].OtherEXPPercent, otherData.OtherEXPPercent);
            CompareAddAttribute("�߰� ���%", GameManager.Instance.OtherDatas[(int)slot].OtherEXPPercent, otherData.OtherGoldPercent);
            CompareAddAttribute("�߰� STR%", GameManager.Instance.OtherDatas[(int)slot].OtherSTRPercent, otherData.OtherSTRPercent);
            CompareAddAttribute("�߰� DEX%", GameManager.Instance.OtherDatas[(int)slot].OtherDEXPercent, otherData.OtherDEXPercent);
            CompareAddAttribute("�߰� LUC%", GameManager.Instance.OtherDatas[(int)slot].OtherLUCPercent, otherData.OtherLUCPercent);
            CompareAddAttribute("�߰� VIT%", GameManager.Instance.OtherDatas[(int)slot].OtherVITPercent, otherData.OtherVITPercent);
        }
        else
        {
            // �⺻ ����
            CompareAttribute("���ݷ�", 0, otherData.OtherATK);
            CompareAttribute("���ݷ�%", 0, otherData.OtherATKPercent);
            CompareAttribute("ü��", 0, otherData.OtherHP);
            CompareAttribute("ü��%", 0, otherData.OtherHPPercent);
            CompareAttribute("����", 0, otherData.OtherDef);
            CompareAttribute("����%", 0, otherData.OtherDefPercent);
            // Ȯ��
            CompareAttribute("ũ��Ƽ�� Ȯ��", 0, otherData.OtherDefPercent, false);
            CompareAttribute("ũ��Ƽ�� ����", 0, otherData.OtherDefPercent, false);
            CompareAttribute("ũ��Ƽ�� ������", 0, otherData.OtherDefPercent, true);
            CompareAttribute("���� Ȯ��", 0, otherData.OtherDefPercent, false);
            CompareAttribute("���� ����", 0, otherData.OtherDefPercent, false);
            CompareAttribute("����", 0, otherData.OtherDefPercent, true);
            CompareAttribute("��Ÿ Ȯ��", 0, otherData.OtherDefPercent, false);
            CompareAttribute("��Ÿ ����", 0, otherData.OtherDefPercent, false);
            CompareAttribute("ȸ�� Ȯ��", 0, otherData.OtherDefPercent, false);
            CompareAttribute("ȸ�� ����", 0, otherData.OtherDefPercent, false);
            CompareAddAttribute("�߰� ����ġ%", 0, otherData.OtherEXPPercent);
            CompareAddAttribute("�߰� ���%", 0, otherData.OtherGoldPercent);
            CompareAddAttribute("�߰� STR%", 0, otherData.OtherSTRPercent);
            CompareAddAttribute("�߰� DEX%", 0, otherData.OtherDEXPercent);
            CompareAddAttribute("�߰� LUC%", 0, otherData.OtherLUCPercent);
            CompareAddAttribute("�߰� VIT%", 0, otherData.OtherVITPercent);
        }
    }

    private void CompareRingAttribute(CompareSlot slot, RingData ringData)
    {
        if (GameManager.Instance.RingDatas[(int)slot] != null)
        {
            CompareAttribute("���ݷ�", GameManager.Instance.RingDatas[(int)slot].RingATK, ringData.RingATK);
            CompareAttribute("���ݷ�%", GameManager.Instance.RingDatas[(int)slot].RingATKPercent, ringData.RingATKPercent);
            CompareAttribute("����", GameManager.Instance.RingDatas[(int)slot].RingDef, ringData.RingDef);
            CompareAttribute("����%", GameManager.Instance.RingDatas[(int)slot].RingDefPercent, ringData.RingDefPercent);
            CompareAttribute("ũ��Ƽ�� Ȯ��", GameManager.Instance.RingDatas[(int)slot].RingCriticalPercent, ringData.RingCriticalPercent, false);
            CompareAttribute("ũ��Ƽ�� ����", GameManager.Instance.RingDatas[(int)slot].RingCriticalResist, ringData.RingCriticalResist, false);
            CompareAttribute("���� Ȯ��", GameManager.Instance.RingDatas[(int)slot].RingDrainPercent, ringData.RingDrainPercent, false);
            CompareAttribute("���� ����", GameManager.Instance.RingDatas[(int)slot].RingDrainResist, ringData.RingDrainResist, false);
            CompareAddAttribute("�߰� ����ġ%", GameManager.Instance.RingDatas[(int)slot].RingEXPPercent, ringData.RingEXPPercent);
            CompareAddAttribute("�߰� ���%", GameManager.Instance.RingDatas[(int)slot].RingGoldPercent, ringData.RingGoldPercent);
            CompareAddAttribute("�߰� STR%", GameManager.Instance.RingDatas[(int)slot].RingSTRPercent, ringData.RingSTRPercent);
            CompareAddAttribute("�߰� LUC%", GameManager.Instance.RingDatas[(int)slot].RingLUCPercent, ringData.RingLUCPercent);
            CompareAddAttribute("�߰� DEX%", GameManager.Instance.RingDatas[(int)slot].RingDEXPercent, ringData.RingDEXPercent);
        }
        else
        {
            CompareAttribute("���ݷ�", 0, ringData.RingATK);
            CompareAttribute("���ݷ�%", 0, ringData.RingATKPercent);
            CompareAttribute("����", 0, ringData.RingDef);
            CompareAttribute("����%", 0, ringData.RingDefPercent);
            CompareAttribute("ũ��Ƽ�� Ȯ��", 0, ringData.RingCriticalPercent, false);
            CompareAttribute("ũ��Ƽ�� ����", 0, ringData.RingCriticalResist, false);
            CompareAttribute("���� Ȯ��", 0, ringData.RingDrainPercent, false);
            CompareAttribute("���� ����", 0, ringData.RingDrainResist, false);
            CompareAddAttribute("�߰� ����ġ%", 0, ringData.RingEXPPercent);
            CompareAddAttribute("�߰� ���%", 0, ringData.RingGoldPercent);
            CompareAddAttribute("�߰� STR%", 0, ringData.RingSTRPercent);
            CompareAddAttribute("�߰� LUC%", 0, ringData.RingLUCPercent);
            CompareAddAttribute("�߰� DEX%", 0, ringData.RingDEXPercent);
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

    private void ComparePercentData(string labelText, int value)
    {
        if (value != 0)
        {
            string colorCode = value > 0 ? "#00FF00" : "#FF0000";
            string sign = value > 0 ? "+" : ""; // ����� �� '+' ��ȣ �߰�
            comparePercentDataText.text += $"{labelText} : <color={colorCode}>{sign}{value:N0}%</color>\n";
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
}
