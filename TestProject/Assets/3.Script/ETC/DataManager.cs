using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using Newtonsoft.Json;
using System;

[System.Serializable]
public class QuickSlotData
{
    public string[] Quickslot;

    public QuickSlotData(string[] quickslot)
    {
        Quickslot = quickslot;
    }
}

[System.Serializable]
public class EquipmentSet
{
    public int QuickSlotIndex;
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

[System.Serializable]
public class PlayerData
{
    public int BattleSpeed;
    public int CurrentLevel;
    public long TotalGold;
    public int Gem;
    public int GemCount;
    public int CurrentHP;
    public int CurrentEnergy;
    public int BonusEnergy;
    public long CurrentEXP;
    public long RequireEXP;
    public int CurrentAP;
    public int APSTR;
    public int APDEX;
    public int APLUC;
    public int APVIT;
    public int PlayCount;
    public int AutoSTR;
    public int AutoDEX;
    public int AutoLUC;
    public int AutoVIT;
    public int QuickSlotIndex;
    public bool GemBonusAP;
    public bool GemGold;
    public bool GemFood;
    public bool GemClover;
    public List<int> DeadMonsterList;
    public float posX;
    public float posY;
    public float posZ;
    public HashSet<string> RuneHashSet;
    public string CurrentMapName;
    public string LayerName;
    public bool FirstStart;
    public int cardBuff;
    public bool isMovePad;

    public PlayerData()
    {

    }
}

public class EliteMonster
{
    public Dictionary<int, bool> EliteMonsterDic;

    public EliteMonster(Dictionary<int, bool> eliteMonsterDic)
    {
        EliteMonsterDic = eliteMonsterDic;
    }
}

[System.Serializable]
public class MasterLevelData
{
    public int MasterLevel;
    public int MasterCurrentEXP;
    public int MasterRequireEXP;
    public int MasterCurrentAP;
    public int MasterRunePoint;
    public int MasterBonusAPPoint;
    public int MasterDropPoint;
    public int MasterMovePoint;
    public int MasterEnergyPoint;
    public int MasterGemPoint;

    public MasterLevelData()
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

    [Header("엘리트 몬스터 잡았는지")]
    public Dictionary<int, bool> EliteMonsterDic;

    [Header("Json")]
    private string ownCountPath;
    private string quickSlotEquipmentPath;
    private string quickSlotEquipmentNamePath;
    private string equipmentPath;
    private string playerDataPath;
    private string eliteMonsterPath;
    private string masterLevelPath;

    [Header("장비 개수에 따른 뱃지 목록")]
    public BadgeData[] badgeDatas;

    [Header("Quick Slot List")]
    public List<EquipmentSet> QuickSlots = new List<EquipmentSet>();

    [Header("몬스터 목록")]
    public MonsterData[] Monsters;

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
        quickSlotEquipmentPath = Application.persistentDataPath + "/EquipmentSet.json";
        equipmentPath = Application.persistentDataPath + "/Equipment.json";
        playerDataPath = Application.persistentDataPath + "/PlayerData.json";
        eliteMonsterPath = Application.persistentDataPath + "/EliteMonsterDic.json";
        quickSlotEquipmentNamePath = Application.persistentDataPath + "/quickSlotEquipment.json";
        masterLevelPath = Application.persistentDataPath + "/MasterLevel.json";
        EliteMonsterDic = new Dictionary<int, bool>();
        QuickSlots = new List<EquipmentSet>();
    }

    private void Start()
    {
        LoadMasterData();
        LoadPlayerData();
        LoadOwnCount();
        LoadEliteMonsterDic();
        LoadQuickSlotName();
        LoadEquipment();
        InitOwnDictionary();
        GameManager.Instance.RenewAbility();
        InvokeRepeating("SaveDataAll", 0, 60);
    }

    private void OnApplicationQuit()
    {
        SaveDataAll();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveDataAll();
        }
    }

    public void SaveDataAll()
    {
        EquipmentSet();
        SaveOwnCount();
        SavePlayerData();
        SaveMasterLevelData();
        SaveEliteMonsterDic();
        SaveQuickSlotName();
    }

    #region Json 세이브&로드
    #region 장비 퀵슬롯 이름

    private void SaveQuickSlotName()
    {
        QuickSlotData quickSlotData = new QuickSlotData(GameManager.Instance.QuickSlot);

        string json = JsonUtility.ToJson(quickSlotData);
        File.WriteAllText(quickSlotEquipmentNamePath, json);
    }

    private void LoadQuickSlotName()
    {
        try
        {
            string json = File.ReadAllText(quickSlotEquipmentNamePath);
            QuickSlotData quickSlotData = JsonUtility.FromJson<QuickSlotData>(json);
            GameManager.Instance.QuickSlot = quickSlotData.Quickslot;
        }
        catch
        {

        }
    }
    #endregion
    #region 장비 퀵슬롯
    public void SaveQuickSlotEquipment(int _slotIndex)
    {
        EquipmentSet equipmentSet = new EquipmentSet();

        equipmentSet.QuickSlotIndex = _slotIndex;
        equipmentSet.EquipWeapon = GameManager.Instance.WeaponData != null ? GameManager.Instance.WeaponData : GameManager.Instance.Punch;
        equipmentSet.EquipArmor = GameManager.Instance.ArmorData != null ? GameManager.Instance.ArmorData : null;
        equipmentSet.EquipHelmet = GameManager.Instance.HelmetData != null ? GameManager.Instance.HelmetData : null;
        equipmentSet.EquipPants = GameManager.Instance.PantsData != null ? GameManager.Instance.PantsData : null;
        equipmentSet.EquipGlove = GameManager.Instance.GloveData != null ? GameManager.Instance.GloveData : null;
        equipmentSet.EquipShoes = GameManager.Instance.ShoesData != null ? GameManager.Instance.ShoesData : null;
        equipmentSet.EquipCloak = GameManager.Instance.clockData != null ? GameManager.Instance.clockData : null;
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
            // QuickSlots 리스트에서 기존에 저장된 장비 세트를 찾음
            bool found = false;
            for (int i = 0; i < QuickSlots.Count; i++)
            {
                if (QuickSlots[i].QuickSlotIndex == _slotIndex)
                {
                    // 기존에 저장된 장비 세트가 있으면 해당 인덱스의 장비 세트를 수정
                    QuickSlots[i] = equipmentSet;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                // 기존에 저장된 장비 세트가 없으면 새로 추가
                QuickSlots.Add(equipmentSet);
            }

            GameManager.Instance.QuickSlotIndex = _slotIndex;
            string json = JsonConvert.SerializeObject(QuickSlots, Formatting.Indented);
            File.WriteAllText(quickSlotEquipmentPath, json);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void LoadQuickSlotEquipment(int _slotIndex)
    {
        if (File.Exists(quickSlotEquipmentPath))
        {
            EquipmentSet equipmentSet = null;
            GameManager.Instance.QuickSlotIndex = _slotIndex;
            try
            {
                string json = File.ReadAllText(quickSlotEquipmentPath);
                QuickSlots = JsonConvert.DeserializeObject<List<EquipmentSet>>(json);

                // 저장된 장비 세트 중에서 _slotIndex에 해당하는 것을 찾음
                for (int i = 0; i < QuickSlots.Count; i++)
                {
                    if (QuickSlots[i].QuickSlotIndex == _slotIndex)
                    { // 저장 되어있는 인덱스 
                        equipmentSet = QuickSlots[i];
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                equipmentSet = null;
            }

            // equipmentSet이 null인 경우 새로운 장비 세트를 생성
            if (equipmentSet == null)
            {
                equipmentSet = new EquipmentSet();
            }

            if(equipmentSet.EquipWeapon == null)
            {
                GameManager.Instance.WeaponData = GameManager.Instance.Punch;
            }
            else
            {
                GameManager.Instance.WeaponData = equipmentSet.EquipWeapon;
            }
            
            GameManager.Instance.ArmorData = equipmentSet.EquipArmor != null ? equipmentSet.EquipArmor : null;
            GameManager.Instance.HelmetData = equipmentSet.EquipHelmet != null ? equipmentSet.EquipHelmet : null;
            GameManager.Instance.PantsData = equipmentSet.EquipPants != null ? equipmentSet.EquipPants : null;
            GameManager.Instance.GloveData = equipmentSet.EquipGlove != null ? equipmentSet.EquipGlove : null;
            GameManager.Instance.ShoesData = equipmentSet.EquipShoes != null ? equipmentSet.EquipShoes : null;
            GameManager.Instance.clockData = equipmentSet.EquipCloak != null ? equipmentSet.EquipCloak : null;
            GameManager.Instance.ShoulderArmorData = equipmentSet.EquipShoulder != null ? equipmentSet.EquipShoulder : null;
            GameManager.Instance.NecklessData = equipmentSet.EquipNeckless != null ? equipmentSet.EquipNeckless : null;
            GameManager.Instance.BeltData = equipmentSet.EquipBelt != null ? equipmentSet.EquipBelt : null;
            GameManager.Instance.RingDatas[0] = equipmentSet.EquipRings[0] != null ? equipmentSet.EquipRings[0] : null;
            GameManager.Instance.RingDatas[1] = equipmentSet.EquipRings[1] != null ? equipmentSet.EquipRings[1] : null;

            for (int i = 0; i < GameManager.Instance.OtherDatas.Length && i < equipmentSet.EquipOther.Length; i++)
            {
                GameManager.Instance.OtherDatas[i] = equipmentSet.EquipOther[i] != null ? equipmentSet.EquipOther[i] : null;
            }

            EquipmentCanvas canvas = FindObjectOfType<EquipmentCanvas>();
            if (canvas != null)
            {
                canvas.InitImage();
            }
        }
    }
    #endregion
    #region 엘리트 몬스터 잡았는지 여부
    private void SaveEliteMonsterDic()
    {
        EliteMonster eliteMonsterDic = new EliteMonster(EliteMonsterDic);

        try
        {
            string json = JsonConvert.SerializeObject(eliteMonsterDic, Formatting.Indented);
            File.WriteAllText(eliteMonsterPath, json);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void LoadEliteMonsterDic()
    {
        EliteMonster eliteMonsterDic;
        try
        {
            string json = File.ReadAllText(eliteMonsterPath);
            eliteMonsterDic = JsonConvert.DeserializeObject<EliteMonster>(json);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            eliteMonsterDic = new EliteMonster(EliteMonsterDic);
        }
        if (eliteMonsterDic != null)
        {
            EliteMonsterDic = eliteMonsterDic.EliteMonsterDic;
        }
    }
    #endregion
    #region 장비 보유개수
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
        try
        {
            string json = JsonConvert.SerializeObject(ownCount, Formatting.Indented);
            File.WriteAllText(ownCountPath, json);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
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
            catch (Exception e)
            {
                Debug.Log(e.Message);
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
    #endregion
    #region 플레이어 데이터
    public void SavePlayerData()
    {
        PlayerData playerData = new PlayerData();

        playerData.BattleSpeed = GameManager.Instance.BattleSpeed;
        playerData.CurrentLevel = GameManager.Instance.PlayerLevel;
        playerData.TotalGold = GameManager.Instance.Gold;
        playerData.Gem = GameManager.Instance.Gem;
        playerData.CurrentHP = GameManager.Instance.PlayerCurHP;
        playerData.CurrentEnergy = GameManager.Instance.CurrentEnergy;
        playerData.BonusEnergy = GameManager.Instance.BonusEnergy;
        playerData.CurrentEXP = GameManager.Instance.CurrentEXP;
        playerData.RequireEXP = GameManager.Instance.RequireEXP;
        playerData.CurrentAP = GameManager.Instance.CurrentAP;
        playerData.APSTR = GameManager.Instance.APSTR;
        playerData.APDEX = GameManager.Instance.APDEX;
        playerData.APLUC = GameManager.Instance.APLUC;
        playerData.APVIT = GameManager.Instance.APVIT;
        playerData.PlayCount = GameManager.Instance.PlayCount;
        playerData.AutoSTR = GameManager.Instance.AutoSTR;
        playerData.AutoDEX = GameManager.Instance.AutoDEX;
        playerData.AutoLUC = GameManager.Instance.AutoLUC;
        playerData.AutoVIT = GameManager.Instance.AutoVIT;
        playerData.QuickSlotIndex = GameManager.Instance.QuickSlotIndex;
        playerData.GemBonusAP = GameManager.Instance.isAPBook;
        playerData.GemClover = GameManager.Instance.isClover;
        playerData.GemFood = GameManager.Instance.isFood;
        playerData.GemGold = GameManager.Instance.isGoldPack;
        playerData.DeadMonsterList = GameManager.Instance.DeadMonsterList;
        playerData.posX = GameManager.Instance.LastPos.x;
        playerData.posY = GameManager.Instance.LastPos.y;
        playerData.posZ = GameManager.Instance.LastPos.z;
        playerData.RuneHashSet = GameManager.Instance.RuneHashSet;
        playerData.CurrentMapName = GameManager.Instance.CurrentMapName;
        playerData.FirstStart = GameManager.Instance.FirstConnect;
        playerData.LayerName = GameManager.Instance.LayerName;
        playerData.cardBuff = (int)GameManager.Instance.CardBuff;
        playerData.GemCount = GameManager.Instance.GemCount;
        playerData.isMovePad = GameManager.Instance.isMovePad;

        try
        {
            string json = JsonConvert.SerializeObject(playerData, Formatting.Indented);
            File.WriteAllText(playerDataPath, json);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void LoadPlayerData()
    {
        if (File.Exists(playerDataPath))
        {
            PlayerData playerData;
            try
            {
                string json = File.ReadAllText(playerDataPath);
                playerData = JsonConvert.DeserializeObject<PlayerData>(json);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                playerData = null;
            }

            GameManager.Instance.BattleSpeed = playerData.BattleSpeed;
            GameManager.Instance.PlayerLevel = playerData.CurrentLevel;
            GameManager.Instance.Gold = playerData.TotalGold;
            GameManager.Instance.Gem = playerData.Gem;
            GameManager.Instance.PlayerCurHP = playerData.CurrentHP;
            GameManager.Instance.CurrentEnergy = playerData.CurrentEnergy;
            GameManager.Instance.BonusEnergy = playerData.BonusEnergy;
            GameManager.Instance.CurrentEXP = playerData.CurrentEXP;
            GameManager.Instance.RequireEXP = playerData.RequireEXP;
            GameManager.Instance.CurrentAP = playerData.CurrentAP;
            GameManager.Instance.APSTR = playerData.APSTR;
            GameManager.Instance.APDEX = playerData.APDEX;
            GameManager.Instance.APLUC = playerData.APLUC;
            GameManager.Instance.APVIT = playerData.APVIT;
            GameManager.Instance.PlayCount = playerData.PlayCount;
            GameManager.Instance.AutoSTR = playerData.AutoSTR;
            GameManager.Instance.AutoDEX = playerData.AutoDEX;
            GameManager.Instance.AutoLUC = playerData.AutoLUC;
            GameManager.Instance.AutoVIT = playerData.AutoVIT;
            GameManager.Instance.QuickSlotIndex = playerData.QuickSlotIndex;
            GameManager.Instance.isGoldPack = playerData.GemGold;
            GameManager.Instance.isFood = playerData.GemFood;
            GameManager.Instance.isClover = playerData.GemClover;
            GameManager.Instance.isAPBook = playerData.GemBonusAP;
            GameManager.Instance.DeadMonsterList = playerData.DeadMonsterList;
            GameManager.Instance.StartPos.x = playerData.posX;
            GameManager.Instance.StartPos.y = playerData.posY;
            GameManager.Instance.StartPos.z = playerData.posZ;
            GameManager.Instance.RuneHashSet = playerData.RuneHashSet;
            GameManager.Instance.CurrentMapName = playerData.CurrentMapName;
            GameManager.Instance.FirstConnect = playerData.FirstStart;
            GameManager.Instance.LayerName = playerData.LayerName;
            GameManager.Instance.CardBuff = (CardBuffEnum)playerData.cardBuff;
            GameManager.Instance.GemCount = playerData.GemCount;
            GameManager.Instance.isMovePad = playerData.isMovePad;
        }
    }
    #endregion
    #region 마스터 레벨 데이터
    public void SaveMasterLevelData()
    {
        MasterLevelData masterData = new MasterLevelData();

        masterData.MasterLevel = GameManager.Instance.MasterLevel;
        masterData.MasterCurrentEXP = GameManager.Instance.MasterCurrentEXP;
        masterData.MasterRequireEXP = GameManager.Instance.MasterRequireEXP;
        masterData.MasterCurrentAP = GameManager.Instance.MasterCurrentAP;
        masterData.MasterRunePoint = GameManager.Instance.MasterRunePoint;
        masterData.MasterBonusAPPoint = GameManager.Instance.MasterBonusAPPoint;
        masterData.MasterDropPoint = GameManager.Instance.MasterDropPoint;
        masterData.MasterEnergyPoint = GameManager.Instance.MasterEnergyPoint;
        masterData.MasterGemPoint = GameManager.Instance.MasterGemPoint;
        masterData.MasterMovePoint = GameManager.Instance.MasterMovePoint;

        try
        {
            string json = JsonConvert.SerializeObject(masterData, Formatting.Indented);
            File.WriteAllText(masterLevelPath, json);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void LoadMasterData()
    {
        if (File.Exists(masterLevelPath))
        {
            MasterLevelData masterData;
            try
            {
                string json = File.ReadAllText(masterLevelPath);
                masterData = JsonConvert.DeserializeObject<MasterLevelData>(json);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                masterData = null;
            }
            GameManager.Instance.MasterLevel = masterData.MasterLevel;
            GameManager.Instance.MasterCurrentEXP = masterData.MasterCurrentEXP;
            GameManager.Instance.MasterRequireEXP = masterData.MasterRequireEXP;
            GameManager.Instance.MasterCurrentAP = masterData.MasterCurrentAP;
            GameManager.Instance.MasterRunePoint = masterData.MasterRunePoint;
            GameManager.Instance.MasterBonusAPPoint = masterData.MasterBonusAPPoint;
            GameManager.Instance.MasterDropPoint = masterData.MasterDropPoint;
            GameManager.Instance.MasterEnergyPoint = masterData.MasterEnergyPoint;
            GameManager.Instance.MasterGemPoint = masterData.MasterGemPoint;
            GameManager.Instance.MasterMovePoint = masterData.MasterMovePoint;

        }
    }
    #endregion
    #region 장착중인 장비
    public void SaveEquipment()
    {
        try
        {
            EquipmentSet equipmentSet = new EquipmentSet();

            equipmentSet.EquipWeapon = GameManager.Instance.WeaponData != null ? GameManager.Instance.WeaponData : GameManager.Instance.Punch;
            equipmentSet.EquipArmor = GameManager.Instance.ArmorData != null ? GameManager.Instance.ArmorData : null;
            equipmentSet.EquipHelmet = GameManager.Instance.HelmetData != null ? GameManager.Instance.HelmetData : null;
            equipmentSet.EquipPants = GameManager.Instance.PantsData != null ? GameManager.Instance.PantsData : null;
            equipmentSet.EquipGlove = GameManager.Instance.GloveData != null ? GameManager.Instance.GloveData : null;
            equipmentSet.EquipShoes = GameManager.Instance.ShoesData != null ? GameManager.Instance.ShoesData : null;
            equipmentSet.EquipCloak = GameManager.Instance.clockData != null ? GameManager.Instance.clockData : null;
            equipmentSet.EquipShoulder = GameManager.Instance.ShoulderArmorData != null ? GameManager.Instance.ShoulderArmorData : null;
            equipmentSet.EquipNeckless = GameManager.Instance.NecklessData != null ? GameManager.Instance.NecklessData : null;
            equipmentSet.EquipBelt = GameManager.Instance.BeltData != null ? GameManager.Instance.BeltData : null;
            equipmentSet.EquipRings[0] = GameManager.Instance.RingDatas[0] != null ? GameManager.Instance.RingDatas[0] : null;
            equipmentSet.EquipRings[1] = GameManager.Instance.RingDatas[1] != null ? GameManager.Instance.RingDatas[1] : null;
            for (int i = 0; i < GameManager.Instance.OtherDatas.Length; i++)
            {
                equipmentSet.EquipOther[i] = GameManager.Instance.OtherDatas[i] != null ? GameManager.Instance.OtherDatas[i] : null;
            }

            string json = JsonConvert.SerializeObject(equipmentSet, Formatting.Indented);
            File.WriteAllText(equipmentPath, json);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public void LoadEquipment()
    {
        if (File.Exists(equipmentPath))
        {
            string json = File.ReadAllText(equipmentPath);
            EquipmentSet equipmentSet = JsonConvert.DeserializeObject<EquipmentSet>(json);

            GameManager.Instance.WeaponData = equipmentSet.EquipWeapon == GameManager.Instance.Punch ? GameManager.Instance.Punch : equipmentSet.EquipWeapon;

            GameManager.Instance.ArmorData = equipmentSet.EquipArmor != null ? equipmentSet.EquipArmor : null;
            GameManager.Instance.HelmetData = equipmentSet.EquipHelmet != null ? equipmentSet.EquipHelmet : null;
            GameManager.Instance.PantsData = equipmentSet.EquipPants != null ? equipmentSet.EquipPants : null;
            GameManager.Instance.GloveData = equipmentSet.EquipGlove != null ? equipmentSet.EquipGlove : null;
            GameManager.Instance.ShoesData = equipmentSet.EquipShoes != null ? equipmentSet.EquipShoes : null;
            GameManager.Instance.clockData = equipmentSet.EquipCloak != null ? equipmentSet.EquipCloak : null;
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
        else
        {
            GameManager.Instance.WeaponData = GameManager.Instance.Punch;

            GameManager.Instance.ArmorData = null;
            GameManager.Instance.HelmetData = null;
            GameManager.Instance.PantsData = null;
            GameManager.Instance.GloveData = null;
            GameManager.Instance.ShoesData = null;
            GameManager.Instance.clockData = null;
            GameManager.Instance.ShoulderArmorData = null;
            GameManager.Instance.NecklessData = null;
            GameManager.Instance.BeltData = null;
            GameManager.Instance.RingDatas[0] = null;
            GameManager.Instance.RingDatas[1] = null;

            for (int i = 0; i < GameManager.Instance.OtherDatas.Length; i++)
            {
                GameManager.Instance.OtherDatas[i] = null;
            }
        }
    }
    #endregion
    #endregion

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

    public void InitOwnDictionary()
    {
        EquipmentSet();
        if (WeaponOwnCount == null) WeaponOwnCount = new Dictionary<int, int>();
        if (ArmorOwnCount == null) ArmorOwnCount = new Dictionary<int, int>();
        if (HelmetOwnCount == null) HelmetOwnCount = new Dictionary<int, int>();
        if (PantsOwnCount == null) PantsOwnCount = new Dictionary<int, int>();
        if (GloveOwnCount == null) GloveOwnCount = new Dictionary<int, int>();
        if (ShoesOwnCount == null) ShoesOwnCount = new Dictionary<int, int>();
        if (ShoulderArmorOwnCount == null) ShoulderArmorOwnCount = new Dictionary<int, int>();
        if (BeltOwnCount == null) BeltOwnCount = new Dictionary<int, int>();
        if (CloakOwnCount == null) CloakOwnCount = new Dictionary<int, int>();
        if (NecklessOwnCount == null) NecklessOwnCount = new Dictionary<int, int>();
        if (RingOwnCount == null) RingOwnCount = new Dictionary<int, int>();
        if (OtherOwnCount == null) OtherOwnCount = new Dictionary<int, int>();
    }

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

    public EquipmentSet GetQuickSlotData(int _slotIndex)
    {
        EquipmentSet equipmentSet = null;
        List<EquipmentSet> equipmentList = new List<EquipmentSet>();
        try
        {
            string json = File.ReadAllText(quickSlotEquipmentPath);
            equipmentList = JsonConvert.DeserializeObject<List<EquipmentSet>>(json);

            // 저장된 장비 세트 중에서 _slotIndex에 해당하는 것을 찾음
            for (int i = 0; i < equipmentList.Count; i++)
            {
                if (equipmentList[i].QuickSlotIndex == _slotIndex)
                { // 저장 되어있는 인덱스 
                    equipmentSet = equipmentList[i];
                    break;
                }
            }
            // equipmentSet이 null인 경우 새로운 장비 세트를 생성합니다.
            if (equipmentSet == null)
            {
                equipmentSet = new EquipmentSet();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        return equipmentSet;
    }

    public void OwnCountReset()
    {
        ResetDictionaryValuesToZero(WeaponOwnCount);
        ResetDictionaryValuesToZero(ArmorOwnCount);
        ResetDictionaryValuesToZero(HelmetOwnCount);
        ResetDictionaryValuesToZero(PantsOwnCount);
        ResetDictionaryValuesToZero(GloveOwnCount);
        ResetDictionaryValuesToZero(ShoesOwnCount);
        ResetDictionaryValuesToZero(ShoulderArmorOwnCount);
        ResetDictionaryValuesToZero(BeltOwnCount);
        ResetDictionaryValuesToZero(NecklessOwnCount);
        ResetDictionaryValuesToZero(CloakOwnCount);
        ResetDictionaryValuesToZero(RingOwnCount);
        ResetDictionaryValuesToZero(OtherOwnCount);
        SaveOwnCount();
    }

    private void ResetDictionaryValuesToZero(Dictionary<int, int> dictionary)
    {
        foreach (var key in dictionary.Keys.ToList())
        {
            dictionary[key] = 0;
        }
    }

    public void QuickSlotReset()
    {
        // 기존의 JSON 파일 삭제
        if (File.Exists(quickSlotEquipmentNamePath))
        {
            File.Delete(quickSlotEquipmentNamePath);
            File.Delete(quickSlotEquipmentPath);
            Debug.Log("기존의 퀵 슬롯 JSON 파일이 삭제되었습니다.");
        }
        else
        {
            Debug.Log("기존의 퀵 슬롯 JSON 파일이 존재하지 않습니다.");
        }

        GameManager.Instance.QuickSlotIndex = 0;
        GameManager.Instance.QuickSlot = new string[5] { "퀵슬롯 1", "퀵슬롯 2", "퀵슬롯 3", "퀵슬롯 4", "퀵슬롯 5" };
    }

    public void EquipmentReset()
    {
        if(File.Exists(equipmentPath))
        {
            File.Delete(equipmentPath);
        }
    }

    public int GetOwnCount()
    {
        int sumOwnCount = 0;

        #region
        if (WeaponOwnCount != null)
        {
            foreach (var weaponCount in WeaponOwnCount.Values)
            {
                sumOwnCount += weaponCount;
            }
        }
        if (ArmorOwnCount != null)
        {
            foreach (var armorCount in ArmorOwnCount.Values)
            {
                sumOwnCount += armorCount;
            }
        }
        if (HelmetOwnCount != null)
        {
            foreach (var helmetCount in HelmetOwnCount.Values)
            {
                sumOwnCount += helmetCount;
            }
        }
        if (PantsOwnCount != null)
        {
            foreach (var pantsCount in PantsOwnCount.Values)
            {
                sumOwnCount += pantsCount;
            }
        }
        if (GloveOwnCount != null)
        {
            foreach (var gloveCount in GloveOwnCount.Values)
            {
                sumOwnCount += gloveCount;
            }
        }
        if (ShoesOwnCount != null)
        {
            foreach (var shoesCount in ShoesOwnCount.Values)
            {
                sumOwnCount += shoesCount;
            }
        }
        if (CloakOwnCount != null)
        {
            foreach (var cloakCount in CloakOwnCount.Values)
            {
                sumOwnCount += cloakCount;
            }
        }
        if (BeltOwnCount != null)
        {
            foreach (var beltCount in BeltOwnCount.Values)
            {
                sumOwnCount += beltCount;
            }
        }
        if (ShoulderArmorOwnCount != null)
        {
            foreach (var shoulderCount in ShoulderArmorOwnCount.Values)
            {
                sumOwnCount += shoulderCount;
            }
        }
        if (NecklessOwnCount != null)
        {
            foreach (var necklessCount in NecklessOwnCount.Values)
            {
                sumOwnCount += necklessCount;
            }
        }
        if (RingOwnCount != null)
        {
            foreach (var ringCount in RingOwnCount.Values)
            {
                sumOwnCount += ringCount;
            }
        }
        if (OtherOwnCount != null)
        {
            foreach (var otherCount in OtherOwnCount.Values)
            {
                sumOwnCount += otherCount;
            }
        }
        #endregion

        return sumOwnCount;
    }

    public int TotalOwnCount()
    {
        int totalOwnCount = 0;

        totalOwnCount += WeaponOwnCount.Count * 10;
        totalOwnCount += ArmorOwnCount.Count * 10;
        totalOwnCount += HelmetOwnCount.Count * 10;
        totalOwnCount += PantsOwnCount.Count * 10;
        totalOwnCount += GloveOwnCount.Count * 10;
        totalOwnCount += ShoesOwnCount.Count * 10;
        totalOwnCount += CloakOwnCount.Count * 10;
        totalOwnCount += ShoulderArmorOwnCount.Count * 10;
        totalOwnCount += BeltOwnCount.Count * 10;
        totalOwnCount += NecklessOwnCount.Count * 10;
        totalOwnCount += RingOwnCount.Count * 10;
        totalOwnCount += OtherOwnCount.Count * 10;

        return totalOwnCount;
    }
}
