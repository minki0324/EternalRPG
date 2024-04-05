using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public Category type;

    [Header("¿Â∫Ò SO")]
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

    [Header("¿Â∫Ò Sprite")]
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
            case "π´±‚":
                CreateItemPanel(WeaponDatas, parent);
                type = Category.Weapon;
                break;
            case "∞©ø ":
                CreateItemPanel(ArmorDatas, parent);
                type = Category.Armor;
                break;
            case "«Ô∏‰":
                CreateItemPanel(HelmetDatas, parent);
                type = Category.Helmet;
                break;
            case "∞¢π›":
                CreateItemPanel(PantsDatas, parent);
                type = Category.Pants;
                break;
            case "¿Â∞©":
                CreateItemPanel(GloveDatas, parent);
                type = Category.Glove;
                break;
            case "Ω≈πﬂ":
                CreateItemPanel(ShoesDatas, parent);
                type = Category.Shoes;
                break;
            case "∫ß∆Æ":
                CreateItemPanel(BeltDatas, parent);
                type = Category.Belt;
                break;
            case "∞ﬂ¿Â":
                CreateItemPanel(ShoulderArmorDatas, parent);
                type = Category.ShoulderArmor;
                break;
            case "π›¡ˆ":
                CreateItemPanel(RingDatas, parent);
                type = Category.Ring;
                break;
            case "∏Ò∞…¿Ã":
                CreateItemPanel(NecklessDatas, parent);
                type = Category.Neckless;
                break;
            case "∏¡≈‰":
                CreateItemPanel(ClockDatas, parent);
                type = Category.Clock;
                break;
            case "∫∏¡∂":
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
