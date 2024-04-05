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

    [Header("��� Sprite")]
    public SpriteAtlas spriteAtlas;

    [SerializeField] private GameObject itemPanel;

    private void Awake()
    {
        if(Instance == null)
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

    public void ItemListSet(string _type, Transform parent)
    {
        for(int i = 0; i < parent.childCount; i++)
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
                CreateItemPanel(OtherDatas, parent);
                type = Category.Other;
                break;
        }
    }

    public void CreateItemPanel(EquipmentBaseData[] _equipmentDatas, Transform parent)
    {
        for(int i = 0; i < _equipmentDatas.Length; i++)
        {
            GameObject item = Instantiate(itemPanel);
            ItemPanel itempanel = item.GetComponent<ItemPanel>();
            itempanel.EquipmentData = _equipmentDatas[i];
            item.transform.SetParent(parent);
        }
    }

    public Sprite GetEquipmentSprite(EquipmentBaseData _equipmentdata)
    {
        string spriteName = _equipmentdata.SpriteName;
        return spriteAtlas.GetSprite(spriteName);
    }
}
