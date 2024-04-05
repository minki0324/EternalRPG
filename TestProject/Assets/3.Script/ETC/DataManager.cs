using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System;

[System.Serializable]
public class EquipmentSet
{
    public WeaponData EquipWeapon;
    public ArmorData EquipArmor;
    public PantsData EquipPants;
    public HelmetData EquipHelmet;
    public GloveData EquipGlove;
    public ShoesData EquipShoes;
    public CloakData EquipCloak;
    public BeltData EquipBelt;
    public NecklessData EquipNeckless;
    public ShoulderData EquipShoulder;
    public RingData[] EquipRings = new RingData[2] { null, null };
    public OtherData[] EquipOther = new OtherData[4] { null, null, null, null };

    public EquipmentSet()
    {

    }
}

[System.Serializable]
public class EquipmentOwnCount
{
    public Dictionary<int, int> WeaponOwnCount;
    public Dictionary<int, int> ArmorOwnCount;
    public Dictionary<int, int> HelmetOwnCount;
    public Dictionary<int, int> PantsOwnCount;
    public Dictionary<int, int> GloveOwnCount;
    public Dictionary<int, int> ShoesOwnCount;
    public Dictionary<int, int> ShoulderArmorOwnCount;
    public Dictionary<int, int> BeltOwnCount;
    public Dictionary<int, int> CloakOwnCount;
    public Dictionary<int, int> NecklessOwnCount;
    public Dictionary<int, int> RingOwnCount;
    public Dictionary<int, int> OtherOwnCount;

    public EquipmentOwnCount()
    {
    }
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [Header("장비 보유 개수")]
    public Dictionary<int, int> WeaponOwnCount;
    public Dictionary<int, int> ArmorOwnCount;
    public Dictionary<int, int> HelmetOwnCount;
    public Dictionary<int, int> PantsOwnCount;
    public Dictionary<int, int> GloveOwnCount;
    public Dictionary<int, int> ShoesOwnCount;
    public Dictionary<int, int> ShoulderArmorOwnCount;
    public Dictionary<int, int> BeltOwnCount;
    public Dictionary<int, int> CloakOwnCount;
    public Dictionary<int, int> NecklessOwnCount;
    public Dictionary<int, int> RingOwnCount;
    public Dictionary<int, int> OtherOwnCount;

    [Header("Json")]
    private string ownCountPath;
    private string equipmentPath;

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
        ownCountPath = Application.persistentDataPath + "/EquipmentOwnCount.json";
        Debug.Log("보유 개수 경로 : " + ownCountPath);
        equipmentPath = Application.persistentDataPath + "/EquipmentSet.json";
        Debug.Log("장비 경로 : " + equipmentPath);
    }

    private void Start()
    {
        LoadEquipSet();
        LoadOwnCount();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("부름 ?");
        EquipmentSet();
        SaveOwnCount();
        SaveEquipSet();
    }

    #region Json 세이브&로드
    private void SaveOwnCount()
    { // 보유 데이터 JSON으로 저장
        EquipmentOwnCount ownCount = new EquipmentOwnCount();

        // 2. 각 장비 유형별 보유 개수 할당
        ownCount.WeaponOwnCount = WeaponOwnCount;
        ownCount.ArmorOwnCount = ArmorOwnCount;
        ownCount.HelmetOwnCount = HelmetOwnCount;
        ownCount.PantsOwnCount = PantsOwnCount;
        ownCount.GloveOwnCount = GloveOwnCount;
        ownCount.ShoesOwnCount = ShoesOwnCount;
        ownCount.ShoulderArmorOwnCount = ShoulderArmorOwnCount;
        ownCount.BeltOwnCount = BeltOwnCount;
        ownCount.CloakOwnCount = CloakOwnCount;
        ownCount.NecklessOwnCount = NecklessOwnCount;
        ownCount.RingOwnCount = RingOwnCount;
        ownCount.OtherOwnCount = OtherOwnCount;

        string json = JsonConvert.SerializeObject(ownCount, Formatting.Indented);
        try
        {
            File.WriteAllText(ownCountPath, json);
        }
        catch(Exception e)
        {
            Debug.Log("Save Own Count : " + e.Message);
        }
    }

    private void LoadOwnCount()
    { // JSON에서 보유 데이터 불러오기
        if (File.Exists(ownCountPath))
        {
            EquipmentOwnCount ownCount;
            try
            {
                string json = File.ReadAllText(ownCountPath);
                ownCount = JsonConvert.DeserializeObject<EquipmentOwnCount>(json);
            }
            catch(Exception e)
            {
                Debug.Log("Load Own Count : " + e.Message);
                ownCount = null;
            }
            WeaponOwnCount = ownCount.WeaponOwnCount;
            ArmorOwnCount = ownCount.ArmorOwnCount;
            HelmetOwnCount = ownCount.HelmetOwnCount;
            PantsOwnCount = ownCount.PantsOwnCount;
            GloveOwnCount = ownCount.GloveOwnCount;
            ShoesOwnCount = ownCount.ShoesOwnCount;
            ShoulderArmorOwnCount = ownCount.ShoulderArmorOwnCount;
            BeltOwnCount = ownCount.BeltOwnCount;
            CloakOwnCount = ownCount.CloakOwnCount;
            NecklessOwnCount = ownCount.NecklessOwnCount;
            RingOwnCount = ownCount.RingOwnCount;
            OtherOwnCount = ownCount.OtherOwnCount;
        }
        else
        {
            WeaponOwnCount = new Dictionary<int, int>();
            ArmorOwnCount = new Dictionary<int, int>();
            HelmetOwnCount = new Dictionary<int, int>();
            PantsOwnCount = new Dictionary<int, int>();
            GloveOwnCount = new Dictionary<int, int>();
            ShoesOwnCount = new Dictionary<int, int>();
            ShoulderArmorOwnCount = new Dictionary<int, int>();
            BeltOwnCount = new Dictionary<int, int>();
            CloakOwnCount = new Dictionary<int, int>();
            NecklessOwnCount = new Dictionary<int, int>();
            RingOwnCount = new Dictionary<int, int>();
            OtherOwnCount = new Dictionary<int, int>();
        }
    }

    public void SaveEquipSet()
    {
        EquipmentSet equipmentSet = new EquipmentSet();

        equipmentSet.EquipWeapon = GameManager.Instance.WeaponData != null ? GameManager.Instance.WeaponData : null;
        equipmentSet.EquipArmor = GameManager.Instance.ArmorData != null ? GameManager.Instance.ArmorData : null;
        equipmentSet.EquipHelmet = GameManager.Instance.HelmetData != null ? GameManager.Instance.HelmetData : null;
        equipmentSet.EquipPants = GameManager.Instance.PantsData != null ? GameManager.Instance.PantsData : null;
        equipmentSet.EquipGlove = GameManager.Instance.GloveData != null ? GameManager.Instance.GloveData : null;
        equipmentSet.EquipShoes = GameManager.Instance.ShoesData != null ? GameManager.Instance.ShoesData : null;
        equipmentSet.EquipCloak = GameManager.Instance.ClockData != null ? GameManager.Instance.ClockData : null;
        equipmentSet.EquipShoulder = GameManager.Instance.ShoulderArmorData != null ? GameManager.Instance.ShoulderArmorData : null;
        equipmentSet.EquipNeckless = GameManager.Instance.NecklessData != null ? GameManager.Instance.NecklessData : null;
        equipmentSet.EquipBelt = GameManager.Instance.BeltData != null ? GameManager.Instance.BeltData : null;
        equipmentSet.EquipRings[0] = GameManager.Instance.RingDatas[0] != null ? GameManager.Instance.RingDatas[0] : null;
        equipmentSet.EquipRings[1] = GameManager.Instance.RingDatas[1] != null ? GameManager.Instance.RingDatas[1] : null;
        for (int i = 0; i < GameManager.Instance.OtherDatas.Length; i++)
        {
            equipmentSet.EquipOther[i] = GameManager.Instance.OtherDatas[i] != null ? GameManager.Instance.OtherDatas[i] : null;
        }

        try
        {
            string json = JsonConvert.SerializeObject(equipmentSet, Formatting.Indented);
            File.WriteAllText(equipmentPath, json);
        }
        catch(Exception e)
        {
            Debug.Log("SaveEquip : " + e.Message);
        }
    }

    public void LoadEquipSet()
    {
        if (File.Exists(equipmentPath))
        {
            EquipmentSet equipmentSet;
            try
            {
                string json = File.ReadAllText(equipmentPath);
                equipmentSet = JsonConvert.DeserializeObject<EquipmentSet>(json);
            }
            catch(Exception e)
            {
                Debug.Log("LoadEquip : " + e.Message);
                equipmentSet = null;
            }
            GameManager.Instance.WeaponData = equipmentSet.EquipWeapon != null ? equipmentSet.EquipWeapon : null;
            GameManager.Instance.ArmorData = equipmentSet.EquipArmor != null ? equipmentSet.EquipArmor : null;
            GameManager.Instance.HelmetData = equipmentSet.EquipHelmet != null ? equipmentSet.EquipHelmet : null;
            GameManager.Instance.PantsData = equipmentSet.EquipPants != null ? equipmentSet.EquipPants : null;
            GameManager.Instance.GloveData = equipmentSet.EquipGlove != null ? equipmentSet.EquipGlove : null;
            GameManager.Instance.ShoesData = equipmentSet.EquipShoes != null ? equipmentSet.EquipShoes : null;
            GameManager.Instance.ClockData = equipmentSet.EquipCloak != null ? equipmentSet.EquipCloak : null;
            GameManager.Instance.ShoulderArmorData = equipmentSet.EquipShoulder != null ? equipmentSet.EquipShoulder : null;
            GameManager.Instance.NecklessData = equipmentSet.EquipNeckless != null ? equipmentSet.EquipNeckless : null;
            GameManager.Instance.BeltData = equipmentSet.EquipBelt != null ? equipmentSet.EquipBelt : null;
            GameManager.Instance.RingDatas[0] = equipmentSet.EquipRings[0] != null ? equipmentSet.EquipRings[0] : null;
            GameManager.Instance.RingDatas[1] = equipmentSet.EquipRings[1] != null ? equipmentSet.EquipRings[1] : null;

            for (int i = 0; i < GameManager.Instance.OtherDatas.Length && i < equipmentSet.EquipOther.Length; i++)
            {
                GameManager.Instance.OtherDatas[i] = equipmentSet.EquipOther[i] != null ? equipmentSet.EquipOther[i] : null;
            }
        }
        GameManager.Instance.RenewAbility();
    }
    #endregion

    #region 장비 데이터 셋
    public void EquipmentSet()
    {
        // 각 장비 유형별로 보유 개수 초기화
        InitializeEquipmentCount(ref WeaponOwnCount, EquipmentManager.Instance.WeaponDatas);
        InitializeEquipmentCount(ref ArmorOwnCount, EquipmentManager.Instance.ArmorDatas);
        InitializeEquipmentCount(ref HelmetOwnCount, EquipmentManager.Instance.HelmetDatas);
        InitializeEquipmentCount(ref PantsOwnCount, EquipmentManager.Instance.PantsDatas);
        InitializeEquipmentCount(ref GloveOwnCount, EquipmentManager.Instance.GloveDatas);
        InitializeEquipmentCount(ref ShoesOwnCount, EquipmentManager.Instance.ShoesDatas);
        InitializeEquipmentCount(ref ShoulderArmorOwnCount, EquipmentManager.Instance.ShoulderArmorDatas);
        InitializeEquipmentCount(ref BeltOwnCount, EquipmentManager.Instance.BeltDatas);
        InitializeEquipmentCount(ref CloakOwnCount, EquipmentManager.Instance.ClockDatas);
        InitializeEquipmentCount(ref NecklessOwnCount, EquipmentManager.Instance.NecklessDatas);
        InitializeEquipmentCount(ref RingOwnCount, EquipmentManager.Instance.RingDatas);
        InitializeEquipmentCount(ref OtherOwnCount, EquipmentManager.Instance.OtherDatas);
    }

    private void InitializeEquipmentCount(ref Dictionary<int, int> equipmentCountDict, EquipmentBaseData[] equipmentDatas)
    {
        // 장비 데이터 배열 순회
        for (int i = 0; i < equipmentDatas.Length; i++)
        {
            // 해당 아이템 ID가 딕셔너리에 없는 경우, 0으로 초기화하여 추가
            if (equipmentCountDict.ContainsKey(equipmentDatas[i].ItemID))
            {
                continue;
            }
            equipmentCountDict.Add(equipmentDatas[i].ItemID, 0);
        }
    }
    #endregion



    public Dictionary<int, int> GetOwnDictionary(EquipmentBaseData _equipmentBaseData)
    {
        switch (_equipmentBaseData.EquipmentType)
        {
            case Category.Weapon: return WeaponOwnCount;
            case Category.Armor: return ArmorOwnCount;
            case Category.Helmet: return HelmetOwnCount;
            case Category.Pants: return PantsOwnCount;
            case Category.Glove: return GloveOwnCount;
            case Category.Shoes: return ShoesOwnCount;
            case Category.Belt: return BeltOwnCount;
            case Category.ShoulderArmor: return ShoulderArmorOwnCount;
            case Category.Ring: return RingOwnCount;
            case Category.Neckless: return NecklessOwnCount;
            case Category.Clock: return CloakOwnCount;
            case Category.Other: return OtherOwnCount;
            default: return null;
        }
    }
}
