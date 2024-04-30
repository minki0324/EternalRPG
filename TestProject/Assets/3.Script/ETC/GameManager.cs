using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CardBuffEnum
{
    None,
    RuneBuff,
    BonusAPBuff,
    EXPandGoldBuff,
    DropBuff
}

[System.Serializable]
public class MasterLevelEXP
{
    public int level;
    public int requireEXP;

    public MasterLevelEXP(int level, int requireEXP)
    {
        this.level = level;
        this.requireEXP = requireEXP;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public CardBuffEnum CardBuff;
    public int BattleSpeed = 1;
    public int QuickSlotIndex = 0;
    public bool FirstConnect = false;

    [Header("마스터 레벨")]
    public List<MasterLevelEXP> masterLevelTable;
    public int MasterLevel = 1;
    public int MasterCurrentEXP = 0;
    public int MasterRequireEXP = 1000;
    public int MasterCurrentAP = 5;
    public int MasterRunePoint = 0;
    public int MasterBonusAPPoint = 0;
    public int MasterDropPoint = 0;
    public int MasterMovePoint = 0;
    public int MasterEnergyPoint = 0;
    public int MasterGemPoint = 0;

    [Header("기본정보")]
    [Space(10)] // 위는 기본 스텟
    public int PlayerATK = 5;
    public int PlayerMaxHP = 50;
    public int PlayerCurHP = 50;
    public int PlayerDef = 3;
    [Space(10)] // 공 체 방 퍼센트
    public int PlayerATKPercent = 0;
    public int PlayerHPPercent = 0;
    public int PlayerDefPercent = 0;
    [Space(10)] // 기본 정보들
    public int PlayerLevel = 1;
    public int MoveSpeed = 30;
    public long Gold = 0;
    public int Gem = 0;
    public int GemCount = 0;
    public int CurrentEnergy = 25;
    public int BonusEnergy = 0;
    public long CurrentEXP = 0;
    public long RequireEXP = 50;
    public int CurrentAP = 0;
    public int BonusAP = 0;
    public int Power = 0;
    public int PlayCount = 0;
    public Vector3 LastPos = Vector3.zero;
    public Vector3 StartPos = Vector3.zero;
    public string CurrentMapName = string.Empty;
    public string LayerName = "Default";

    [Header("스텟")]
    public int STR = 5; // 힘을 올리면 공격력이 0.01%씩 상승
    public int APSTR = 0;
    public int AutoSTR = 0;
    [Space(10)]
    public int DEX = 5; // 덱스를 올리면 명중, 회피 확률이 0.01%씩 상승
    public int APDEX = 0;
    public int AutoDEX = 0;
    [Space(10)]
    public int LUC = 5; // 럭을 올리면 골드 획득량, 아이템 드랍율 0.001%, 콤보 어택 확률이 0.01%씩 상승
    public int APLUC = 0;
    public int AutoLUC = 0;
    [Space(10)]
    public int VIT = 5; // 체력을 올리면 체력이 0.1%, 방어력이 0.1%씩 상승
    public int APVIT = 0;
    public int AutoVIT = 0;

    [Header("확률")]
    [Space(10)] // 크리티컬
    public int CriticalPercant = 5;
    public int CriticalResist = 5;
    public float CriticalDamage = 1.2f;
    [Space(10)] // 연타
    public int ComboPercent = 5;
    public int ComboResist = 5;
    [Space(10)] // 회피
    public int AvoidPercent = 5;
    public int AvoidResist = 5;
    [Space(10)] // 흡혈
    public int DrainPercent = 5;
    public int DrainResist = 5;
    public float DrainAmount = 1.05f;

    [Header("추가 정보")]
    public int EXPPercent = 0;
    public int GoldPercent = 0;
    public int ItemDropRate = 0;
    public int STRPercent = 0;
    public int DEXPercent = 0;
    public int LUCPercent = 0;
    public int VITPercent = 0;
    public float RuneDropRate = 1;

    [Header("죽인 몬스터")]
    public List<int> DeadMonsterList = new List<int>();

    public WeaponData Punch;
    [Header("착용중인 장비")]
    public WeaponData WeaponData;
    public ArmorData ArmorData;
    public PantsData PantsData;
    public HelmetData HelmetData;
    public GloveData GloveData;
    public ShoesData ShoesData;
    public CloakData ClockData;
    public BeltData BeltData;
    public ShoulderData ShoulderArmorData;
    public RingData[] RingDatas = new RingData[2];
    public NecklessData NecklessData;
    public OtherData[] OtherDatas = new OtherData[4];

    [Header("뱃지")]
    public BadgeData BadgeData;

    [Header("룬석")]
    public HashSet<string> RuneHashSet;

    [Header("퀵슬롯 아이템")]
    public bool isAPBook = false;
    public bool isGoldPack = false;
    public bool isFood = false;
    public bool isClover = false;

    [Header("퀵슬롯 장비")]
    public string[] QuickSlot = new string[5] { "퀵슬롯 1", "퀵슬롯 2", "퀵슬롯 3", "퀵슬롯 4", "퀵슬롯 5" };

    [Header("투명 Sprite")]
    public Sprite NoneBackground;

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
        Application.targetFrameRate = 60;
        RuneHashSet = new HashSet<string>();
    }

    private void Start()
    {
        if(CurrentEnergy <= 0)
        {
            ResetRound();
        }
        MasterLevelEXPData();
    }

    public void RenewAbility()
    {
        BadgeGrade();
        // 스텟 먼저 갱신
        RenewStat();

        RenewATK();
        RenewHP();
        RenewDef();
        RenewCritical();
        RenewCombo();
        RenewAvoid();
        RenewDrain();
        RenewOther();
        RenewReward();
        RenewPlayerPower();
        if (PlayerCurHP > PlayerMaxHP) PlayerCurHP = PlayerMaxHP;
    }

    #region 스텟 갱신
    public void RenewStat()
    {
        int baseSTR = 5;
        int sumSTRPercent = 0;
        int baseDEX = 5;
        int sumDEXPercent = 0;
        int baseLUC = 5;
        int sumLUCPercent = 0;
        int baseVIT = 5;
        int sumVITPercent = 0;
        if (WeaponData != null)
        {
            sumSTRPercent += Mathf.RoundToInt((float)(WeaponData.WeaponSTRPercent * (1 + GetOwnPercent(WeaponData))));
            sumDEXPercent += Mathf.RoundToInt((float)(WeaponData.WeaponDEXPercent * (1 + GetOwnPercent(WeaponData))));
        }
        if (ArmorData != null)
        {
            sumVITPercent += Mathf.RoundToInt((float)(ArmorData.ArmorVITPercent * (1 + GetOwnPercent(ArmorData))));
        }
        if (PantsData != null)
        {
            sumVITPercent += Mathf.RoundToInt((float)(PantsData.PantsVITPercent * (1 + GetOwnPercent(PantsData))));
        }
        if (HelmetData != null)
        {
            sumSTRPercent += Mathf.RoundToInt((float)(HelmetData.HelmetSTRPercent * (1 + GetOwnPercent(HelmetData))));
            sumVITPercent += Mathf.RoundToInt((float)(HelmetData.HelmetVITPercent * (1 + GetOwnPercent(HelmetData))));
        }
        if (GloveData != null)
        {
            sumSTRPercent += Mathf.RoundToInt((float)(GloveData.GloveSTRPercent * (1 + GetOwnPercent(GloveData))));
            sumDEXPercent += Mathf.RoundToInt((float)(GloveData.GloveDEXPercent * (1 + GetOwnPercent(GloveData))));
        }
        if (ShoesData != null)
        {
            sumVITPercent += Mathf.RoundToInt((float)(ShoesData.ShoesVITPercent * (1 + GetOwnPercent(ShoesData))));
            sumDEXPercent += Mathf.RoundToInt((float)(ShoesData.ShoesDEXPercent * (1 + GetOwnPercent(ShoesData))));
        }
        if (BeltData != null)
        {
            sumLUCPercent += Mathf.RoundToInt((float)(BeltData.BeltLUCPercent * (1 + GetOwnPercent(BeltData))));
            sumVITPercent += Mathf.RoundToInt((float)(BeltData.BeltVITPercent * (1 + GetOwnPercent(BeltData))));
        }
        if (ShoulderArmorData != null)
        {
            sumDEXPercent += Mathf.RoundToInt((float)(ShoulderArmorData.ShoulderDEXPercent * (1 + GetOwnPercent(ShoulderArmorData))));
            sumVITPercent += Mathf.RoundToInt((float)(ShoulderArmorData.ShoulderVITPercent * (1 + GetOwnPercent(ShoulderArmorData))));
        }
        if (NecklessData != null)
        {
            sumDEXPercent += Mathf.RoundToInt((float)(NecklessData.NecklessDEXPercent * (1 + GetOwnPercent(NecklessData))));
            sumLUCPercent += Mathf.RoundToInt((float)(NecklessData.NecklessLUCPercent * (1 + GetOwnPercent(NecklessData))));
        }
        for (int i = 0; i < RingDatas.Length; i++)
        {
            if (RingDatas[i] != null)
            {
                sumSTRPercent += Mathf.RoundToInt((float)(RingDatas[i].RingSTRPercent * (1 + GetOwnPercent(RingDatas[i]))));
                sumLUCPercent += Mathf.RoundToInt((float)(RingDatas[i].RingLUCPercent * (1 + GetOwnPercent(RingDatas[i]))));
                sumDEXPercent += Mathf.RoundToInt((float)(RingDatas[i].RingDEXPercent * (1 + GetOwnPercent(RingDatas[i]))));
            }
        }
        for (int i = 0; i < OtherDatas.Length; i++)
        {
            if (OtherDatas[i] != null)
            {
                sumSTRPercent += Mathf.RoundToInt((float)(OtherDatas[i].OtherSTRPercent * (1 + GetOwnPercent(OtherDatas[i]))));
                sumDEXPercent += Mathf.RoundToInt((float)(OtherDatas[i].OtherDEXPercent * (1 + GetOwnPercent(OtherDatas[i]))));
                sumLUCPercent += Mathf.RoundToInt((float)(OtherDatas[i].OtherLUCPercent * (1 + GetOwnPercent(OtherDatas[i]))));
                sumVITPercent += Mathf.RoundToInt((float)(OtherDatas[i].OtherVITPercent * (1 + GetOwnPercent(OtherDatas[i]))));
            }
        }
        STR = Mathf.RoundToInt((baseSTR + APSTR) * (1 + (float)((sumSTRPercent + BadgeData.BadgeSTRPercent) / 100f)));
        DEX = Mathf.RoundToInt((baseDEX + APDEX) * (1 + (float)((sumDEXPercent + BadgeData.BadgeDEXPercent) / 100f)));
        LUC = Mathf.RoundToInt((baseLUC + APLUC) * (1 + (float)((sumLUCPercent + BadgeData.BadgeLUCPercent) / 100f)));
        VIT = Mathf.RoundToInt((baseVIT + APVIT) * (1 + (float)((sumVITPercent + BadgeData.BadgeVITPercent) / 100f)));
        STRPercent = sumSTRPercent + BadgeData.BadgeSTRPercent;
        DEXPercent = sumDEXPercent + BadgeData.BadgeDEXPercent;
        LUCPercent = sumLUCPercent + BadgeData.BadgeLUCPercent;
        VITPercent = sumVITPercent + BadgeData.BadgeVITPercent;
    }
    #endregion

    #region 공격력 갱신
    public void RenewATK()
    { // 공격력 -> (기본 공격력 + 장비로 인한 깡뎀) * 각종 퍼센트 합산
        int baseATK = 5;
        int sumATK = 0;
        int sumPercent = 0;


        if (WeaponData != null)
        {
            sumATK += Mathf.RoundToInt((float)(WeaponData.WeaponATK * (1 + GetOwnPercent(WeaponData))));
            sumPercent += Mathf.RoundToInt((float)(WeaponData.WeaponATKPercent * (1 + GetOwnPercent(WeaponData))));
        }
        if (GloveData != null)
        {
            sumATK += Mathf.RoundToInt((float)(GloveData.GloveATK * (1 + GetOwnPercent(GloveData))));
            sumPercent += Mathf.RoundToInt((float)(GloveData.GloveATKPercent * (1 + GetOwnPercent(GloveData))));
        }
        for (int i = 0; i < RingDatas.Length; i++)
        {
            if (RingDatas[i] != null)
            {
                sumATK += Mathf.RoundToInt((float)(RingDatas[i].RingATK * (1 + GetOwnPercent(RingDatas[i]))));
                sumPercent += Mathf.RoundToInt((float)(RingDatas[i].RingATKPercent * (1 + GetOwnPercent(RingDatas[i]))));
            }
        }
        for (int i = 0; i < OtherDatas.Length; i++)
        {
            if (OtherDatas[i] != null)
            {
                sumATK += Mathf.RoundToInt((float)(OtherDatas[i].OtherATK * (1 + GetOwnPercent(OtherDatas[i]))));
                sumPercent += Mathf.RoundToInt((float)(OtherDatas[i].OtherATKPercent * (1 + GetOwnPercent(OtherDatas[i]))));
            }
        }

        PlayerATK = Mathf.RoundToInt((float)((baseATK + sumATK + (STR / 7f)) * (1 + (sumPercent / 100.0)) + BadgeData.BadgeATKPercent));
        PlayerATKPercent = sumPercent;
    }
    #endregion

    #region 체력 갱신
    public void RenewHP()
    {
        int baseHP = 100;
        int sumHP = 0;
        int sumPercent = 0;
        if (ArmorData != null)
        {
            sumHP += Mathf.RoundToInt((float)(ArmorData.ArmorHP * (1 + GetOwnPercent(ArmorData))));
            sumPercent += Mathf.RoundToInt((float)(ArmorData.ArmorHPPercent * (1 + GetOwnPercent(ArmorData))));
        }
        if (BeltData != null)
        {
            sumHP += Mathf.RoundToInt((float)(BeltData.BeltHP * (1 + GetOwnPercent(BeltData))));
            sumPercent += Mathf.RoundToInt((float)(BeltData.BeltHPPercent * (1 + GetOwnPercent(BeltData))));
        }
        if (ClockData != null)
        {
            sumHP += Mathf.RoundToInt((float)(ClockData.CloakHP * (1 + GetOwnPercent(ClockData))));
            sumPercent += Mathf.RoundToInt((float)(ClockData.CloakHPPercent * (1 + GetOwnPercent(ClockData))));
        }
        if (GloveData != null)
        {
            sumHP += Mathf.RoundToInt((float)(GloveData.GloveHP * (1 + GetOwnPercent(GloveData))));
            sumPercent += Mathf.RoundToInt((float)(GloveData.GloveHPPercent * (1 + GetOwnPercent(GloveData))));
        }
        if (HelmetData != null)
        {
            sumHP += Mathf.RoundToInt((float)(HelmetData.HelmetHP * (1 + GetOwnPercent(HelmetData))));
            sumPercent += Mathf.RoundToInt((float)(HelmetData.HelmetHPPercent * (1 + GetOwnPercent(HelmetData))));
        }
        if (NecklessData != null)
        {
            sumHP += Mathf.RoundToInt((float)(NecklessData.NecklessHP * (1 + GetOwnPercent(NecklessData))));
            sumPercent += Mathf.RoundToInt((float)(NecklessData.NecklessHPPercent * (1 + GetOwnPercent(NecklessData))));
        }
        if (PantsData != null)
        {
            sumHP += Mathf.RoundToInt((float)(PantsData.PantsHP * (1 + GetOwnPercent(PantsData))));
            sumPercent += Mathf.RoundToInt((float)(PantsData.PantsHPPercent * (1 + GetOwnPercent(PantsData))));
        }
        if (ShoesData != null)
        {
            sumHP += Mathf.RoundToInt((float)(ShoesData.ShoesHP * (1 + GetOwnPercent(ShoesData))));
            sumPercent += Mathf.RoundToInt((float)(ShoesData.ShoesHPPercent * (1 + GetOwnPercent(ShoesData))));
        }
        for (int i = 0; i < OtherDatas.Length; i++)
        {
            if (OtherDatas[i] != null)
            {
                sumHP += Mathf.RoundToInt((float)(OtherDatas[i].OtherHP * (1 + GetOwnPercent(OtherDatas[i]))));
                sumPercent += Mathf.RoundToInt((float)(OtherDatas[i].OtherHPPercent * (1 + GetOwnPercent(OtherDatas[i]))));
            }
        }
        PlayerMaxHP = Mathf.RoundToInt((float)((baseHP + sumHP + VIT/1.5f) * (1 + (sumPercent / 100.0))));
        PlayerHPPercent = sumPercent;
    }
    #endregion

    #region 방어력 갱신
    public void RenewDef()
    {
        int baseDef = 3;
        int sumDef = 0;
        int sumPercent = 0;
        if (ArmorData != null)
        {
            sumDef += Mathf.RoundToInt((float)(ArmorData.ArmorDef * (1 + GetOwnPercent(ArmorData))));
            sumPercent += Mathf.RoundToInt((float)(ArmorData.ArmorDef * (1 + GetOwnPercent(ArmorData))));
        }
        if (ClockData != null)
        {
            sumDef += Mathf.RoundToInt((float)(ClockData.CloakDef * (1 + GetOwnPercent(ClockData))));
            sumPercent += Mathf.RoundToInt((float)(ClockData.CloakDefPercent * (1 + GetOwnPercent(ClockData))));
        }
        if (HelmetData != null)
        {
            sumDef += Mathf.RoundToInt((float)(HelmetData.HelmetDef * (1 + GetOwnPercent(HelmetData))));
            sumPercent += Mathf.RoundToInt((float)(HelmetData.HelmetDefPercent * (1 + GetOwnPercent(HelmetData))));
        }
        if (ShoulderArmorData != null)
        {
            sumDef += Mathf.RoundToInt((float)(ShoulderArmorData.ShoulderDef * (1 + GetOwnPercent(ShoulderArmorData))));
            sumPercent += Mathf.RoundToInt((float)(ShoulderArmorData.ShoulderDefPercent * (1 + GetOwnPercent(ShoulderArmorData))));
        }
        if (PantsData != null)
        {
            sumDef += Mathf.RoundToInt((float)(PantsData.PantsDef * (1 + GetOwnPercent(PantsData))));
            sumPercent += Mathf.RoundToInt((float)(PantsData.PantsDefPercent * (1 + GetOwnPercent(PantsData))));
        }
        if (ShoesData != null)
        {
            sumDef += Mathf.RoundToInt((float)(ShoesData.ShoesDef * (1 + GetOwnPercent(ShoesData))));
            sumPercent += Mathf.RoundToInt((float)(ShoesData.ShoesDefPercent * (1 + GetOwnPercent(ShoesData))));
        }
        for (int i = 0; i < RingDatas.Length; i++)
        {
            if (RingDatas[i] != null)
            {
                sumDef += Mathf.RoundToInt((float)(RingDatas[i].RingDef * (1 + GetOwnPercent(RingDatas[i]))));
                sumPercent += Mathf.RoundToInt((float)(RingDatas[i].RingDefPercent * (1 + GetOwnPercent(RingDatas[i]))));
            }
        }
        for (int i = 0; i < OtherDatas.Length; i++)
        {
            if (OtherDatas[i] != null)
            {
                sumDef += Mathf.RoundToInt((float)(OtherDatas[i].OtherDef * (1 + GetOwnPercent(OtherDatas[i]))));
                sumPercent += Mathf.RoundToInt((float)(OtherDatas[i].OtherDefPercent * (1 + GetOwnPercent(OtherDatas[i]))));
            }
        }
        PlayerDef = Mathf.RoundToInt((float)((baseDef + sumDef + VIT/9f) * (1 + (sumPercent / 100.0))));
        PlayerDefPercent = sumPercent;
    }
    #endregion

    #region 크리티컬 갱신
    public void RenewCritical()
    {
        int baseCri = 5;
        int baseCriResist = 5;
        float baseCriDamage = 1.2f;
        int sumCri = 0;
        int sumCriResist = 0;
        float sumCriDamage = 0;
        if (ArmorData != null)
        {
            sumCriResist += Mathf.RoundToInt((float)(ArmorData.ArmorCriticalResist * (1 + GetOwnPercent(ArmorData))));
        }
        if (GloveData != null)
        {
            sumCri += Mathf.RoundToInt((float)(GloveData.GloveCriticalPercent * (1 + GetOwnPercent(GloveData))));
            sumCriDamage += (float)(GloveData.GloveCriticalDamage * (1 + GetOwnPercent(GloveData)));
        }
        if (PantsData != null)
        {
            sumCriResist += Mathf.RoundToInt((float)(PantsData.PantsCriticalResist * (1 + GetOwnPercent(PantsData))));
        }
        if (ShoulderArmorData != null)
        {
            sumCriResist += Mathf.RoundToInt((float)(ShoulderArmorData.ShoulderCriticalResist * (1 + GetOwnPercent(ShoulderArmorData))));
        }
        if (WeaponData != null)
        {
            sumCri += Mathf.RoundToInt((float)(WeaponData.WeaponCriticalPercent * (1 + GetOwnPercent(WeaponData))));
            sumCriDamage += (float)(WeaponData.WeaponCriticalDamage * (1 + GetOwnPercent(WeaponData)));
        }
        for (int i = 0; i < RingDatas.Length; i++)
        {
            if (RingDatas[i] != null)
            {
                sumCri += Mathf.RoundToInt((float)(RingDatas[i].RingCriticalPercent * (1 + GetOwnPercent(RingDatas[i]))));
                sumCriResist += Mathf.RoundToInt((float)(RingDatas[i].RingCriticalResist * (1 + GetOwnPercent(RingDatas[i]))));
            }
        }
        for (int i = 0; i < OtherDatas.Length; i++)
        {
            if (OtherDatas[i] != null)
            {
                sumCri += Mathf.RoundToInt((float)(OtherDatas[i].OtherCriticalPercent * (1 + GetOwnPercent(OtherDatas[i]))));
                sumCriResist += Mathf.RoundToInt((float)(OtherDatas[i].OtherCriticalResist * (1 + GetOwnPercent(OtherDatas[i]))));
                sumCriDamage += (float)(OtherDatas[i].OtherCriticalDamage * (1 + GetOwnPercent(OtherDatas[i])));
            }
        }

        CriticalPercant = baseCri + sumCri;
        CriticalResist = baseCriResist + sumCriResist;
        CriticalDamage = baseCriDamage + sumCriDamage;
    }
    #endregion

    #region 연타 갱신
    public void RenewCombo()
    {
        int baseComboPercent = 5;
        int sumComboPercent = 0;
        int baseComboResist = 5;
        int sumComboResist = 0;

        if (WeaponData != null)
        {
            sumComboPercent += Mathf.RoundToInt((float)(WeaponData.WeaponComboPercent * (1 + GetOwnPercent(WeaponData))));
        }
        if (ArmorData != null)
        {
            sumComboResist += Mathf.RoundToInt((float)(ArmorData.ArmorComboResist * (1 + GetOwnPercent(ArmorData))));
        }
        if (ClockData != null)
        {
            sumComboResist += Mathf.RoundToInt((float)(ClockData.CloakComboResist * (1 + GetOwnPercent(ClockData))));
        }
        if (GloveData != null)
        {
            sumComboPercent += Mathf.RoundToInt((float)(GloveData.GloveComboPercent * (1 + GetOwnPercent(GloveData))));
        }
        if (NecklessData != null)
        {
            sumComboPercent += Mathf.RoundToInt((float)(NecklessData.NecklessComboPercent * (1 + GetOwnPercent(NecklessData))));
        }
        if (PantsData != null)
        {
            sumComboResist += Mathf.RoundToInt((float)(PantsData.PantsComboResist * (1 + GetOwnPercent(PantsData))));
        }
        for (int i = 0; i < OtherDatas.Length; i++)
        {
            if (OtherDatas[i] != null)
            {
                sumComboPercent += Mathf.RoundToInt((float)(OtherDatas[i].OtherComboPercent * (1 + GetOwnPercent(OtherDatas[i]))));
                sumComboResist += Mathf.RoundToInt((float)(OtherDatas[i].OtherComboResist * (1 + GetOwnPercent(OtherDatas[i]))));
            }
        }
        ComboPercent = Mathf.RoundToInt((float)(baseComboPercent + sumComboPercent + LUC / 3000.0));
        ComboResist = Mathf.RoundToInt((float)(baseComboResist + sumComboResist + LUC / 3000.0));
    }
    #endregion

    #region 회피 갱신
    public void RenewAvoid()
    {
        int baseAvoidPercent = 5;
        int sumAvoidPercent = 0;
        int baseAvoidResist = 5;
        int sumAvoidResist = 0;
        int runeAvoidResist = RuneHashSet.Contains("명중의 룬") ? 25 : 0;

        if (BeltData != null)
        {
            sumAvoidPercent += Mathf.RoundToInt((float)(BeltData.BeltAvoidPercent * (1 + GetOwnPercent(BeltData))));
        }
        if (ClockData != null)
        {
            sumAvoidPercent += Mathf.RoundToInt((float)(ClockData.CloakAvoidPercent * (1 + GetOwnPercent(ClockData))));
        }
        if (HelmetData != null)
        {
            sumAvoidPercent += Mathf.RoundToInt((float)(HelmetData.HelmetAvoidPercent * (1 + GetOwnPercent(HelmetData))));
        }
        if (NecklessData != null)
        {
            sumAvoidResist += Mathf.RoundToInt((float)(NecklessData.NecklessAvoidResist * (1 + GetOwnPercent(NecklessData))));
        }
        if (ShoesData != null)
        {
            sumAvoidPercent += Mathf.RoundToInt((float)(ShoesData.ShoesAvoidPercent * (1 + GetOwnPercent(ShoesData))));
            sumAvoidResist += Mathf.RoundToInt((float)(ShoesData.ShoesAvoidResist * (1 + GetOwnPercent(ShoesData))));
        }
        for (int i = 0; i < OtherDatas.Length; i++)
        {
            if (OtherDatas[i] != null)
            {
                sumAvoidPercent += Mathf.RoundToInt((float)(OtherDatas[i].OtherAvoidPercent * (1 + GetOwnPercent(OtherDatas[i]))));
                sumAvoidResist += Mathf.RoundToInt((float)(OtherDatas[i].OtherAvoidResist * (1 + GetOwnPercent(OtherDatas[i]))));
            }
        }

        AvoidPercent = Mathf.RoundToInt((float)(baseAvoidPercent + sumAvoidPercent + DEX / 5000.0));
        AvoidResist = Mathf.RoundToInt((float)(baseAvoidResist + sumAvoidResist + runeAvoidResist + DEX / 3000.0 + BadgeData.BadgeAvoidResist));
    }
    #endregion

    #region 흡혈 갱신
    public void RenewDrain()
    {
        int baseDrain = 5;
        int sumDrainPercent = 0;
        int baseDrainResist = 5;
        int sumDrainResist = 0;
        float baseDrainAmount = 1.05f;
        float sumDrainAmount = 0;

        if (ArmorData != null)
        {
            sumDrainResist += Mathf.RoundToInt((float)(ArmorData.ArmorDrainResist * (1 + GetOwnPercent(ArmorData))));
        }
        if (HelmetData != null)
        {
            sumDrainResist += Mathf.RoundToInt((float)(HelmetData.HelmetDrainResist * (1 + GetOwnPercent(HelmetData))));
        }
        if (ShoulderArmorData != null)
        {
            sumDrainResist += Mathf.RoundToInt((float)(ShoulderArmorData.ShoulderDrainResist * (1 + GetOwnPercent(ShoulderArmorData))));
        }
        if (WeaponData != null)
        {
            sumDrainPercent += Mathf.RoundToInt((float)(WeaponData.WeaponDrainPercent * (1 + GetOwnPercent(WeaponData))));
            sumDrainAmount += (float)(WeaponData.WeaponDrainAmount * (1 + GetOwnPercent(WeaponData)));
        }
        for (int i = 0; i < RingDatas.Length; i++)
        {
            if (RingDatas[i] != null)
            {
                sumDrainPercent += Mathf.RoundToInt((float)(RingDatas[i].RingDrainPercent * (1 + GetOwnPercent(RingDatas[i]))));
                sumDrainResist += Mathf.RoundToInt((float)(RingDatas[i].RingDrainResist * (1 + GetOwnPercent(RingDatas[i]))));
            }
        }
        for (int i = 0; i < OtherDatas.Length; i++)
        {
            if (OtherDatas[i] != null)
            {
                sumDrainPercent += Mathf.RoundToInt((float)(OtherDatas[i].OtherDrainPercent * (1 + GetOwnPercent(OtherDatas[i]))));
                sumDrainAmount += (float)(OtherDatas[i].OtherDrainAmount * (1 + GetOwnPercent(OtherDatas[i])));
                sumDrainResist += Mathf.RoundToInt((float)(OtherDatas[i].OtherDrainResist * (1 + GetOwnPercent(OtherDatas[i]))));
            }
        }

        DrainPercent = baseDrain + sumDrainPercent;
        DrainResist = baseDrainResist + sumDrainResist;
        DrainAmount = baseDrainAmount + sumDrainAmount;
    }
    #endregion

    #region 경험치, 골드 갱신
    public void RenewReward()
    {
        int sumEXP = 0;
        int sumGold = 0;
        int sumDrop = 0;
        int runeEXP = RuneHashSet.Contains("경험의 룬") ? 50 : 0;
        int runeGold = RuneHashSet.Contains("부유의 룬") ? 40 : 0;
        int cardBuff = CardBuff == CardBuffEnum.EXPandGoldBuff ? 20 : 0;

        if (BeltData != null)
        {
            sumEXP += Mathf.RoundToInt((float)(BeltData.BeltEXPPercent * (1 + GetOwnPercent(BeltData))));
        }
        if (NecklessData != null)
        {
            sumEXP += Mathf.RoundToInt((float)(NecklessData.NecklessEXPPercent * (1 + GetOwnPercent(NecklessData))));
            sumGold += Mathf.RoundToInt((float)(NecklessData.NecklessGoldPercent * (1 + GetOwnPercent(NecklessData))));
            sumDrop += Mathf.RoundToInt((float)(NecklessData.NecklessItemDropRate * (1 + GetOwnPercent(NecklessData))));
        }
        for (int i = 0; i < RingDatas.Length; i++)
        {
            if (RingDatas[i] != null)
            {
                sumEXP += Mathf.RoundToInt((float)(RingDatas[i].RingEXPPercent * (1 + GetOwnPercent(RingDatas[i]))));
                sumGold += Mathf.RoundToInt((float)(RingDatas[i].RingGoldPercent * (1 + GetOwnPercent(RingDatas[i]))));
                sumDrop += Mathf.RoundToInt((float)RingDatas[i].RingItemDropRate * (1 + GetOwnPercent(RingDatas[i])));
            }
        }
        for (int i = 0; i < OtherDatas.Length; i++)
        {
            if (OtherDatas[i] != null)
            {
                sumEXP += Mathf.RoundToInt((float)(OtherDatas[i].OtherEXPPercent * (1 + GetOwnPercent(OtherDatas[i]))));
                sumGold += Mathf.RoundToInt((float)(OtherDatas[i].OtherGoldPercent * (1 + GetOwnPercent(OtherDatas[i]))));
                sumDrop += Mathf.RoundToInt((float)(OtherDatas[i].OtherItemDropRate * (1 + GetOwnPercent(OtherDatas[i]))));
            }
        }
        EXPPercent = Mathf.RoundToInt((float)(100 + LUC / 3000f + cardBuff + sumEXP + runeEXP + BadgeData.BadgeEXPPercent));
        GoldPercent = Mathf.RoundToInt((float)(100 + LUC / 3000f + cardBuff + sumGold + runeGold+ BadgeData.BadgeGoldPercent));
        ItemDropRate = sumDrop;
    }
    #endregion

    #region 기타 갱신
    private void RenewOther()
    {
        // 보너스 AP
        int sumBonusAP = 0;

        // 이동속도
        float sumMoveSpeed = 0;
        float runeMoveSpeed = RuneHashSet.Contains("속도의 룬") ? 10f : 0f;

        // 룬 드롭
        float runeDropCardbuff = CardBuff == CardBuffEnum.RuneBuff ? 0.5f : 0;
        int runeDropMasterBuff = MasterRunePoint != 0 ? 1 : 0;
        float basicRuneBuff = RuneHashSet.Contains("평범한 룬") ? 0.5f : 0f;

        if (ShoesData != null)
        {
            sumMoveSpeed += ShoesData.ShoesMoveSpeed;
        }
        for (int i = 0; i < OtherDatas.Length; i++)
        {
            if (OtherDatas[i] != null)
            {
                sumBonusAP += OtherDatas[i].OtherBonusAP;
            }
        }
        int quickSlotBook = isAPBook ? 1 : 0;
        int cardBuff = CardBuff == CardBuffEnum.BonusAPBuff ? 1 : 0;

        BonusAP = 5 + sumBonusAP + quickSlotBook + cardBuff + MasterBonusAPPoint + BadgeData.BadgeBonusAP;
        MoveSpeed = 65 + Mathf.RoundToInt(sumMoveSpeed + runeMoveSpeed + MasterMovePoint + BadgeData.BadgeMoveSpeed);
        RuneDropRate = 1 + cardBuff + runeDropMasterBuff + basicRuneBuff + BadgeData.BadgeRuneDrop;
    }
    #endregion

    #region 전투력 갱신
    public void RenewPlayerPower()
    {
        /*
         전투력 공식
        전투력 1당 수치들
        1. 공격력 / 50
        2. 체력 / 200
        3. 방어력 / 10
        4. 콤보 확률 / 3, 콤보 저항 / 2
        5. 크리 확률 / 2, 크리 저항 / 2, 크리 데미지 * 10
        6. 회피 확률, 회피 저항
        7. 흡혈 확률 / 3, 흡혈 저항 / 2, 흡혈 * 5
         */
        int power = 0;
        power += Mathf.RoundToInt((float)PlayerATK / 30f);
        power += Mathf.RoundToInt((float)PlayerMaxHP / 10f);
        power += Mathf.RoundToInt((float)PlayerDef / 10f);
        power += Mathf.RoundToInt((float)ComboPercent / 3f);
        power += Mathf.RoundToInt((float)ComboResist / 2f);
        power += Mathf.RoundToInt((float)CriticalPercant / 2f);
        power += Mathf.RoundToInt((float)CriticalResist / 2f);
        power += Mathf.RoundToInt((float)CriticalDamage * 10f);
        power += Mathf.RoundToInt((float)AvoidPercent);
        power += Mathf.RoundToInt((float)AvoidResist);
        power += Mathf.RoundToInt((float)DrainPercent / 3f);
        power += Mathf.RoundToInt((float)DrainResist / 2f);
        power += Mathf.RoundToInt((float)DrainAmount * 5f);

        Power = power;
    }
    #endregion

    private float GetOwnPercent(EquipmentBaseData equipmentBaseData)
    {
        Dictionary<int, int> owndictionary = new Dictionary<int, int>();
        int ownCount = 0;

        // 보유 아이템 개수 가져오기
        owndictionary = DataManager.Instance.GetOwnDictionary(equipmentBaseData);
        ownCount = owndictionary.ContainsKey(equipmentBaseData.ItemID) ? owndictionary[equipmentBaseData.ItemID] : 0;

        // 장비로 인한 공격력 계산
        float additionalPercent = Mathf.Min(1f, ownCount / 9f); // 보유 아이템 개수에 따른 퍼센트 계산

        return additionalPercent;
    }

    public void ResetRound()
    {
        isAPBook = false;
        isClover = false;
        isFood = false;
        isGoldPack = false;
        int runeBonusEnergy = RuneHashSet.Contains("무명의 룬") ? 2 : 0;

        CardBuff = CardBuffEnum.None;

        // 잡은 몬스터 초기화
        DeadMonsterList.Clear();

        // 위치 초기화
        StartPos = Vector3.zero;
        LastPos = Vector3.zero;

        // 기본 정보 초기화
        PlayerLevel = 1;
        RequireEXP = 50;
        CurrentEXP = 0;
        APDEX = 0;
        APLUC = 0;
        APSTR = 0;
        APVIT = 0;
        Gold = 0;
        CurrentAP = 0;
        PlayCount++;
        CurrentEnergy = 25 + BonusEnergy + runeBonusEnergy + BadgeData.BadgeBonusEnergy;
        CurrentMapName = string.Empty;

        RenewAbility();
        PlayerCurHP = PlayerMaxHP;
    }

    private void MasterLevelEXPData()
    {
        int requireEXP = 1000;
        for(int i = 0; i < 29; i++)
        {
            masterLevelTable.Add(new MasterLevelEXP(i + 2, (i + 1) * (requireEXP + 4000) * (i + 2)));
            requireEXP += 500;
        }
    }
    

    public int GetRequireEXPForLevel(int level)
    {
        foreach (var data in masterLevelTable)
        {
            if (data.level == level)
            {
                return data.requireEXP;
            }
        }
        // 해당 레벨에 대한 정보를 찾을 수 없으면 기본값 반환
        return 2100000000; // 만렙시
    }

    public void BadgeGrade()
    {
        int totalOwnCount = DataManager.Instance.GetOwnCount();

        if (totalOwnCount >= 1200) BadgeData = DataManager.Instance.badgeDatas[8];
        else if (totalOwnCount >= 1050 && totalOwnCount < 1200) BadgeData = DataManager.Instance.badgeDatas[7];
        else if (totalOwnCount >= 900 && totalOwnCount < 1050) BadgeData = DataManager.Instance.badgeDatas[6];
        else if (totalOwnCount >= 750 && totalOwnCount < 900) BadgeData = DataManager.Instance.badgeDatas[5];
        else if (totalOwnCount >= 600 && totalOwnCount < 750) BadgeData = DataManager.Instance.badgeDatas[4];
        else if (totalOwnCount >= 450 && totalOwnCount < 600) BadgeData = DataManager.Instance.badgeDatas[3];
        else if (totalOwnCount >= 300 && totalOwnCount < 450) BadgeData = DataManager.Instance.badgeDatas[2];
        else if (totalOwnCount >= 150 && totalOwnCount < 300) BadgeData = DataManager.Instance.badgeDatas[1];
        else if (totalOwnCount >= 0 && totalOwnCount < 150) BadgeData = DataManager.Instance.badgeDatas[0];
    }
}
