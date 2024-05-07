using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public Category type;

    [Header("��� SO")]
    public WeaponData[] WeaponDatas;
    public ArmorData[] ArmorDatas;
    public PantsData[] PantsDatas;
    public HelmetData[] HelmetDatas;
    public GloveData[] GloveDatas;
    public ShoesData[] ShoesDatas;
    public CloakData[] ClockDatas;
    public BeltData[] BeltDatas;
    public ShoulderData[] ShoulderArmorDatas;
    public RingData[] RingDatas;
    public NecklessData[] NecklessDatas;
    public OtherData[] OtherDatas;
    public RuneData[] RuneDatas;

    [Header("��� Sprite")]
    public SpriteAtlas spriteAtlas;

    [SerializeField] private GameObject itemPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ItemListSet(string _type, Transform parent, string _otherType = null)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }

        switch (_type)
        {
            case "����":
                CreateItemPanel(WeaponDatas, parent);
                type = Category.Weapon;
                break;
            case "����":
                CreateItemPanel(ArmorDatas, parent);
                type = Category.Armor;
                break;
            case "���":
                CreateItemPanel(HelmetDatas, parent);
                type = Category.Helmet;
                break;
            case "����":
                CreateItemPanel(PantsDatas, parent);
                type = Category.Pants;
                break;
            case "�尩":
                CreateItemPanel(GloveDatas, parent);
                type = Category.Glove;
                break;
            case "�Ź�":
                CreateItemPanel(ShoesDatas, parent);
                type = Category.Shoes;
                break;
            case "��Ʈ":
                CreateItemPanel(BeltDatas, parent);
                type = Category.Belt;
                break;
            case "����":
                CreateItemPanel(ShoulderArmorDatas, parent);
                type = Category.ShoulderArmor;
                break;
            case "����":
                CreateItemPanel(RingDatas, parent);
                type = Category.Ring;
                break;
            case "�����":
                CreateItemPanel(NecklessDatas, parent);
                type = Category.Neckless;
                break;
            case "����":
                CreateItemPanel(ClockDatas, parent);
                type = Category.Clock;
                break;
            case "����":
                CreateItemPanel(OtherDatas, parent, _otherType);
                type = Category.Other;
                break;
        }
    }

    public void CreateItemPanel(EquipmentBaseData[] _equipmentDatas, Transform parent, string _otherType = null)
    {
        for (int i = 0; i < _equipmentDatas.Length; i++)
        {
            ItemPanel itempanel;
            if (_otherType == null)
            { // ������ �ƴ� �ٸ��͵�
                GameObject item = Instantiate(itemPanel);
                itempanel = item.GetComponent<ItemPanel>();
                itempanel.EquipmentData = _equipmentDatas[i];
                item.transform.SetParent(parent);
            }
            else
            { // ����
                if (_equipmentDatas[i] is OtherData) // OtherData Ÿ������ Ȯ��
                {
                    OtherData otherData = (OtherData)_equipmentDatas[i]; // OtherData�� ĳ����
                    if (_otherType == "All")
                    { // ��ü�� �����ߴٸ� ��� ���
                        GameObject item = Instantiate(itemPanel);
                        itempanel = item.GetComponent<ItemPanel>();
                        itempanel.EquipmentData = _equipmentDatas[i];
                        item.transform.SetParent(parent);
                    }
                    else if (_otherType == otherData.type.ToString())
                    { // �ش� ���� Ÿ�԰� ��ġ�ϴ��� Ȯ��
                        GameObject item = Instantiate(itemPanel);
                        itempanel = item.GetComponent<ItemPanel>();
                        itempanel.EquipmentData = otherData;
                        item.transform.SetParent(parent);
                    }
                    else itempanel = null;
                }
                else itempanel = null;
            }
            if(itempanel != null)
            {
                if (IsEquip(itempanel.EquipmentData))
                { // true�� ���� ���̹Ƿ� ����ǥ��
                    itempanel.EquipCheckIcon.SetActive(true);
                    itempanel.SelectIcon.SetActive(false);
                }
                else
                {
                    if (itempanel.EquipCheckIcon.activeSelf)
                    {
                        itempanel.EquipCheckIcon.SetActive(false); // �����ִٸ� ���ֱ�
                        itempanel.SelectIcon.SetActive(true);
                    }
                }
            }
        }
    }

    public bool IsEquip(EquipmentBaseData _equipmentBaseData)
    {
        switch (_equipmentBaseData.EquipmentType)
        {
            case Category.Weapon:
                if (GameManager.Instance.WeaponData == GameManager.Instance.Punch) return false;
                else if( GameManager.Instance.WeaponData.ItemID == _equipmentBaseData.ItemID)
                {
                    return true;
                }
                return false;
            case Category.Armor:
                if (GameManager.Instance.ArmorData != null &&GameManager.Instance.ArmorData.ItemID == _equipmentBaseData.ItemID)
                {
                    return true;
                }
                return false;
            case Category.Helmet:
                if (GameManager.Instance.HelmetData != null && GameManager.Instance.HelmetData.ItemID == _equipmentBaseData.ItemID)
                {
                    return true;
                }
                return false;
            case Category.Pants:
                if (GameManager.Instance.PantsData != null && GameManager.Instance.PantsData.ItemID == _equipmentBaseData.ItemID)
                {
                    return true;
                }
                return false;
            case Category.Glove:
                if (GameManager.Instance.GloveData != null && GameManager.Instance.GloveData.ItemID == _equipmentBaseData.ItemID)
                {
                    return true;
                }
                return false;
            case Category.Shoes:
                if (GameManager.Instance.ShoesData != null && GameManager.Instance.ShoesData.ItemID == _equipmentBaseData.ItemID)
                {
                    return true;
                }
                return false;
            case Category.Belt:
                if (GameManager.Instance.BeltData != null && GameManager.Instance.BeltData.ItemID == _equipmentBaseData.ItemID)
                {
                    return true;
                }
                return false;
            case Category.ShoulderArmor:
                if (GameManager.Instance.ShoulderArmorData != null && GameManager.Instance.ShoulderArmorData.ItemID == _equipmentBaseData.ItemID)
                {
                    return true;
                }
                return false;
            case Category.Ring:
                for(int i = 0; i < GameManager.Instance.RingDatas.Length; i++)
                {
                    if(GameManager.Instance.RingDatas[i] != null && GameManager.Instance.RingDatas[i].ItemID == _equipmentBaseData.ItemID)
                    {
                        return true;
                    }
                }
                return false;
            case Category.Neckless:
                if (GameManager.Instance.NecklessData != null && GameManager.Instance.NecklessData.ItemID == _equipmentBaseData.ItemID)
                {
                    return true;
                }
                return false;
            case Category.Clock:
                if (GameManager.Instance.ClockData != null && GameManager.Instance.ClockData.ItemID == _equipmentBaseData.ItemID)
                {
                    return true;
                }
                return false;
            case Category.Other:
                for(int i = 0; i < GameManager.Instance.OtherDatas.Length; i++)
                {
                    if(GameManager.Instance.OtherDatas[i] != null && GameManager.Instance.OtherDatas[i].ItemID == _equipmentBaseData.ItemID)
                    {
                        return true;
                    }
                }
                return false;
            default:
                return false;
        }
    }

    public Sprite GetEquipmentSprite(EquipmentBaseData _equipmentdata)
    {
        string spriteName = _equipmentdata.SpriteName;
        return spriteAtlas.GetSprite(spriteName);
    }

    public Sprite GetBadgeSprite(BadgeData badgeData)
    {
        string spriteName = badgeData.BadgeSprite;
        return spriteAtlas.GetSprite(spriteName);
    }
}
