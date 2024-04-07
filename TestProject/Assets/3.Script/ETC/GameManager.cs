using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public int BattleSpeed = 1;

    [Header("�⺻����")]
    [Space(10)] // ���� �⺻ ����
    public int PlayerATK = 5;
    public int PlayerMaxHP = 50;
    public int PlayerCurHP = 50;
    public int PlayerDef = 3;
    [Space(10)] // �� ü �� �ۼ�Ʈ
    public int PlayerATKPercent = 0;
    public int PlayerHPPercent = 0;
    public int PlayerDefPercent = 0;
    [Space(10)] // �⺻ ������
    public int PlayerLevel = 1;
    public int MoveSpeed = 20;
    public int Gold = 0;
    public int Energy = 25;
    public int CurrentEXP = 0;
    public int RequireEXP = 50;
    public int CurrentAP = 0;
    public int BonusAP = 0;
    public int Power = 0;
    public int PlayCount = 0;

    [Header("����")]
    public int STR = 5; // ���� �ø��� ���ݷ��� 0.01%�� ���
    public int APSTR = 0;
    public int AutoSTR = 0;
    [Space(10)]
    public int DEX = 5; // ������ �ø��� ����, ȸ�� Ȯ���� 0.01%�� ���
    public int APDEX = 0;
    public int AutoDEX = 0;
    [Space(10)]
    public int LUC = 5; // ���� �ø��� ��� ȹ�淮, ������ ����� 0.001%, �޺� ���� Ȯ���� 0.01%�� ���
    public int APLUC = 0;
    public int AutoLUC = 0;
    [Space(10)]
    public int VIT = 5; // ü���� �ø��� ü���� 0.1%, ������ 0.1%�� ���
    public int APVIT = 0;
    public int AutoVIT = 0;

    [Header("Ȯ��")]
    [Space(10)] // ũ��Ƽ��
    public int CriticalPercant = 5;
    public int CriticalResist = 5;
    public float CriticalDamage = 1.2f;
    [Space(10)] // ��Ÿ
    public int ComboPercent = 5;
    public int ComboResist = 5;
    [Space(10)] // ȸ��
    public int AvoidPercent = 5;
    public int AvoidResist = 5;
    [Space(10)] // ����
    public int DrainPercent = 5;
    public int DrainResist = 5;
    public float DrainAmount = 1.05f;

    [Header("�߰� ����")]
    public int EXPPercent = 0;
    public int GoldPercent = 0;
    public int ItemDropRate = 0;
    public int STRPercent = 0;
    public int DEXPercent = 0;
    public int LUCPercent = 0;
    public int VITPercent = 0;

    public WeaponData Punch;
    [Header("�������� ���")]
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

    [Header("������")]
    public string[] QuickSlot = new string[5] { "������ 1", "������ 2", "������ 3", "������ 4", "������ 5" };

    [Header("���� Sprite")]
    public Sprite NoneBackground;

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
        Application.targetFrameRate = 60;
        RenewAbility();
    }

    public void RenewAbility()
    {
        // ���� ���� ����
        RenewStat();

        RenewATK();
        RenewHP();
        RenewDef();
        RenewCritical();
        RenewCombo();
        RenewAvoid();
        RenewDrain();
        RenewReward();
        RenewPlayerPower();
    }

    #region ���� ����
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
        if(WeaponData != null)
        {
            sumSTRPercent += WeaponData.WeaponSTRPercent;
            sumDEXPercent += WeaponData.WeaponDEXPercent;
        }
        if (ArmorData != null)
        {
            sumVITPercent += ArmorData.ArmorVITPercent;
        }
        if (PantsData != null)
        {
            sumVITPercent += PantsData.PantsVITPercent;
        }
        if (HelmetData != null)
        {
            sumSTRPercent += HelmetData.HelmetSTRPercent;
            sumVITPercent += HelmetData.HelmetVITPercent;
        }
        if (GloveData != null)
        {
            sumSTRPercent += GloveData.GloveSTRPercent;
            sumDEXPercent += GloveData.GloveDEXPercent;
        }
        if (ShoesData != null)
        {
            sumVITPercent += ShoesData.ShoesVITPercent;
            sumDEXPercent += ShoesData.ShoesDEXPercent;
        }
        if (BeltData != null)
        {
            sumLUCPercent += BeltData.BeltLUCPercent;
            sumVITPercent += BeltData.BeltVITPercent;
        }
        if (ShoulderArmorData != null)
        {
            sumDEXPercent += ShoulderArmorData.ShoulderDEXPercent;
            sumVITPercent += ShoulderArmorData.ShoulderVITPercent;
        }
        if (NecklessData != null)
        {
            sumDEXPercent += NecklessData.NecklessDEXPercent;
            sumLUCPercent += NecklessData.NecklessLUCPercent;
        }
        for(int i = 0; i < RingDatas.Length; i++)
        {
            if(RingDatas[i] != null)
            {
                sumSTRPercent += RingDatas[i].RingSTRPercent;
                sumLUCPercent += RingDatas[i].RingLUCPercent;
                sumDEXPercent += RingDatas[i].RingDEXPercent;
            }
        }
        for(int i = 0; i < OtherDatas.Length; i++)
        {
            if(OtherDatas[i] != null)
            {
                sumSTRPercent += OtherDatas[i].OtherSTRPercent;
                sumDEXPercent += OtherDatas[i].OtherDEXPercent;
                sumLUCPercent += OtherDatas[i].OtherLUCPercent;
                sumVITPercent += OtherDatas[i].OtherVITPercent;
            }
        }
        STR = (baseSTR + APSTR) * (1 + sumSTRPercent);
        DEX = (baseDEX + APDEX) * (1 + sumDEXPercent);
        LUC = (baseLUC + APLUC) * (1 + sumLUCPercent);
        VIT = (baseVIT + APVIT) * (1 + sumVITPercent);
        STRPercent = sumSTRPercent;
        DEXPercent = sumDEXPercent;
        LUCPercent = sumLUCPercent;
        VITPercent = sumVITPercent;
    }
    #endregion

    #region ���ݷ� ����
    public void RenewATK()
    { // ���ݷ� -> (�⺻ ���ݷ� + ���� ���� ����) * ���� �ۼ�Ʈ �ջ�
        int baseATK = 5;
        int sumATK = 0;
        int sumPercent = 0;

        if (WeaponData != null)
        {
            sumATK += WeaponData.WeaponATK;
            sumPercent += WeaponData.WeaponATKPercent;
        }
        if(GloveData != null)
        {
            sumATK += GloveData.GloveATK;
            sumPercent += GloveData.GloveATKPercent;
        }
        for(int i = 0; i < RingDatas.Length; i++)
        {
            if (RingDatas[i] != null)
            {
                sumATK += RingDatas[i].RingATK;
                sumPercent += RingDatas[i].RingATKPercent;
            }
        }
        for(int i = 0; i < OtherDatas.Length; i++)
        {
            if(OtherDatas[i] != null)
            {
                sumATK += OtherDatas[i].OtherATK;
                sumPercent += OtherDatas[i].OtherATKPercent;
            }
        }
        PlayerATK = Mathf.RoundToInt((baseATK + sumATK) * (1 + (sumPercent / 100) + (STR / 10000)));
        PlayerATKPercent = sumPercent;
    }
    #endregion

    #region ü�� ����
    public void RenewHP()
    {
        int baseHP = 100;
        int sumHP = 0;
        int sumPercent = 0;
        if(ArmorData != null)
        {
            sumHP += ArmorData.ArmorHP;
            sumPercent += ArmorData.ArmorHPPercent;
        }
        if(BeltData != null)
        {
            sumHP += BeltData.BeltHP;
            sumPercent += BeltData.BeltHPPercent;
        }
        if(ClockData != null)
        {
            sumHP += ClockData.CloakHP;
            sumPercent += ClockData.CloakHPPercent;
        }
        if(GloveData != null)
        {
            sumHP += GloveData.GloveHP;
            sumPercent += GloveData.GloveHPPercent;
        }
        if(HelmetData != null)
        {
            sumHP += HelmetData.HelmetHP;
            sumPercent += HelmetData.HelmetHPPercent;
        }
        if(NecklessData != null)
        {
            sumHP += NecklessData.NecklessHP;
            sumPercent += NecklessData.NecklessHPPercent;
        }
        if(PantsData != null)
        {
            sumHP += PantsData.PantsHP;
            sumPercent += PantsData.PantsHPPercent;
        }
        if(ShoesData != null)
        {
            sumHP += ShoesData.ShoesHP;
            sumPercent += ShoesData.ShoesHPPercent;
        }
        for(int i =0; i < OtherDatas.Length; i++)
        {
            if(OtherDatas[i] != null)
            {
                sumHP += OtherDatas[i].OtherHP;
                sumPercent += OtherDatas[i].OtherHPPercent;
            }
        }
        PlayerMaxHP = Mathf.RoundToInt((baseHP + sumHP) * (1 + (sumPercent / 100) + (VIT / 10000)));
        PlayerHPPercent = sumPercent;
    }
    #endregion

    #region ���� ����
    public void RenewDef()
    {
        int baseDef = 3;
        int sumDef = 0;
        int sumPercent = 0;
        if (ArmorData != null)
        {
            sumDef += ArmorData.ArmorDef;
            sumPercent += ArmorData.ArmorDefPercent;
        }
        if (ClockData != null)
        {
            sumDef += ClockData.CloakDef;
            sumPercent += ClockData.CloakDefPercent;
        }
        if (HelmetData != null)
        {
            sumDef += HelmetData.HelmetDef;
            sumPercent += HelmetData.HelmetDefPercent;
        }
        if (ShoulderArmorData != null)
        {
            sumDef += ShoulderArmorData.ShoulderDef;
            sumPercent += ShoulderArmorData.ShoulderDefPercent;
        }
        if (PantsData != null)
        {
            sumDef += PantsData.PantsDef;
            sumPercent += PantsData.PantsDefPercent;
        }
        if (ShoesData != null)
        {
            sumDef += ShoesData.ShoesDef;
            sumPercent += ShoesData.ShoesDefPercent;
        }
        for(int i = 0; i < RingDatas.Length; i++)
        {
            if(RingDatas[i] != null)
            {
                sumDef += RingDatas[i].RingDef;
                sumPercent += RingDatas[i].RingDefPercent;
            }
        }
        for (int i = 0; i < OtherDatas.Length; i++)
        {
            if (OtherDatas[i] != null)
            {
                sumDef += OtherDatas[i].OtherDef;
                sumPercent += OtherDatas[i].OtherDefPercent;
            }
        }
        PlayerDef = Mathf.RoundToInt((baseDef + sumDef) * (1 + (sumPercent / 100) + (VIT / 10000)));
        PlayerDefPercent = sumPercent;
    }
    #endregion

    #region ũ��Ƽ�� ����
    public void RenewCritical()
    {
        int baseCri = 5;
        int baseCriResist = 5;
        float baseCriDamage = 1.2f;
        int sumCri = 0;
        int sumCriResist = 0;
        float sumCriDamage = 0;
        if(ArmorData != null)
        {
            sumCriResist += ArmorData.ArmorCriticalResist;
        }
        if(GloveData != null)
        {
            sumCri += GloveData.GloveCriticalPercent;
            sumCriDamage += GloveData.GloveCriticalDamage;
        }
        if(PantsData != null)
        {
            sumCriResist += PantsData.PantsCriticalResist;
        }
        if(ShoulderArmorData != null)
        {
            sumCriResist += ShoulderArmorData.ShoulderCriticalResist;
        }
        if(WeaponData != null)
        {
            sumCri += WeaponData.WeaponCriticalPercent;
            sumCriDamage += WeaponData.WeaponCriticalDamage;
        }
        for(int i = 0; i < RingDatas.Length; i++)
        {
            if(RingDatas[i] != null)
            {
                sumCri += RingDatas[i].RingCriticalPercent;
                sumCriResist += RingDatas[i].RingCriticalResist;
            }
        }
        for(int i = 0; i < OtherDatas.Length; i++)
        {
            if(OtherDatas[i] != null)
            {
                sumCri += OtherDatas[i].OtherCriticalPercent;
                sumCriResist += OtherDatas[i].OtherCriticalResist;
                sumCriDamage += OtherDatas[i].OtherCriticalDamage;
            }
        }

        CriticalPercant = baseCri + sumCri;
        CriticalResist = baseCriResist + sumCriResist;
        CriticalDamage = baseCriDamage + sumCriDamage;
    }
    #endregion

    #region ��Ÿ ����
    public void RenewCombo()
    {
        int baseComboPercent = 5;
        int sumComboPercent = 0;
        int baseComboResist = 5;
        int sumComboResist = 0;

        if(WeaponData != null)
        {
            sumComboPercent += WeaponData.WeaponComboPercent;
        }
        if(ArmorData != null)
        {
            sumComboResist += ArmorData.ArmorComboResist;
        }
        if(ClockData != null)
        {
            sumComboResist += ClockData.CloakComboResist;
        }
        if(GloveData != null)
        {
            sumComboPercent += GloveData.GloveComboPercent;
        }
        if(NecklessData != null)
        {
            sumComboPercent += NecklessData.NecklessComboPercent;
        }
        if(PantsData != null)
        {
            sumComboResist += PantsData.PantsComboResist;
        }
        for(int i = 0; i < OtherDatas.Length; i++)
        {
            if(OtherDatas[i] != null)
            {
                sumComboPercent += OtherDatas[i].OtherComboPercent;
                sumComboResist += OtherDatas[i].OtherComboResist;
            }
        }
        ComboPercent = (baseComboPercent + sumComboPercent) * (1 + LUC / 10000);
        ComboResist = (baseComboResist + sumComboResist) * (1 + LUC / 10000);
    }
    #endregion

    #region ȸ�� ����
    public void RenewAvoid()
    {
        int baseAvoidPercent = 5;
        int sumAvoidPercent = 0;
        int baseAvoidResist = 5;
        int sumAvoidResist = 0;

        if(BeltData != null)
        {
            sumAvoidPercent += BeltData.BeltAvoidPercent;
        }
        if (ClockData != null)
        {
            sumAvoidPercent += ClockData.CloakAvoidPercent;
        }
        if (HelmetData != null)
        {
            sumAvoidPercent += HelmetData.HelmetAvoidPercent;
        }
        if (NecklessData != null)
        {
            sumAvoidPercent += NecklessData.NecklessAvoidPercent;
        }
        if (ShoesData != null)
        {
            sumAvoidPercent += ShoesData.ShoesAvoidPercent;
            sumAvoidResist += ShoesData.ShoesAvoidResist;
        }
        for(int i = 0; i < OtherDatas.Length; i++)
        {
            if(OtherDatas[i] != null)
            {
                sumAvoidPercent += OtherDatas[i].OtherAvoidPercent;
                sumAvoidResist += OtherDatas[i].OtherAvoidResist;
            }
        }

        AvoidPercent = (baseAvoidPercent + sumAvoidPercent) * (1 + DEX / 10000);
        AvoidResist = (baseAvoidResist + sumAvoidResist) * (1 + DEX / 10000);
    }
    #endregion

    #region ���� ����
    public void RenewDrain()
    {
        int baseDrain = 5;
        int sumDrainPercent = 0;
        int baseDrainResist = 5;
        int sumDrainResist = 0;
        float baseDrainAmount = 1.05f;
        float sumDrainAmount = 0;

        if(ArmorData != null)
        {
            sumDrainResist += ArmorData.ArmorDrainResist;
        }
        if (HelmetData != null)
        {
            sumDrainResist += HelmetData.HelmetDrainResist;
        }
        if (ShoulderArmorData != null)
        {
            sumDrainResist += ShoulderArmorData.ShoulderDrainResist;
        }
        if (WeaponData != null)
        {
            sumDrainPercent += WeaponData.WeaponDrainPercent;
            sumDrainAmount += WeaponData.WeaponDrainAmount;
        }
        for(int i = 0; i < RingDatas.Length; i++)
        {
            if(RingDatas[i] != null)
            {
                sumDrainPercent += RingDatas[i].RingDrainPercent;
                sumDrainResist += RingDatas[i].RingDrainResist;
            }
        }
        for(int i = 0; i < OtherDatas.Length; i++)
        {
            if(OtherDatas[i] != null)
            {
                sumDrainPercent += OtherDatas[i].OtherDrainPercent;
                sumDrainAmount += OtherDatas[i].OtherDrainAmount;
                sumDrainResist += OtherDatas[i].OtherDrainResist;
            }
        }

        DrainPercent = baseDrain + sumDrainPercent;
        DrainResist = baseDrainResist + sumDrainResist;
        DrainAmount = baseDrainAmount + sumDrainAmount;
    }
    #endregion

    #region ����ġ, ��� ����
    public void RenewReward()
    {
        int sumEXP = 0;
        int sumGold = 0;
        if(BeltData != null)
        {
            sumEXP += BeltData.BeltEXPPercent;
        }
        if (NecklessData != null)
        {
            sumEXP += NecklessData.NecklessEXPPercent;
        }
        for (int i = 0; i < RingDatas.Length; i++)
        {
            if (RingDatas[i] != null)
            {
                sumEXP += RingDatas[i].RingEXPPercent;
                sumGold += RingDatas[i].RingGoldPercent;
            }
        }
        for (int i = 0; i < OtherDatas.Length; i++)
        {
            if (OtherDatas[i] != null)
            {
                sumEXP += OtherDatas[i].OtherGoldPercent;
                sumGold += OtherDatas[i].OtherEXPPercent;
            }
        }
        EXPPercent = sumEXP * (1 + LUC / 100000);
        GoldPercent = sumGold * (1 + LUC / 100000);
    }
    #endregion

    #region ������ ����
    public void RenewPlayerPower()
    {
        /*
         ������ ����
        ������ 1�� ��ġ��
        1. ���ݷ� / 20
        2. ü�� / 200
        3. ���� / 20
        4. �޺� Ȯ�� / 3, �޺� ���� / 2
        5. ũ�� Ȯ�� / 2, ũ�� ���� / 2, ũ�� ������ * 10
        6. ȸ�� Ȯ��, ȸ�� ����
        7. ���� Ȯ�� / 3, ���� ���� / 2, ���� * 5
         */
        int power = 0;
        power += Mathf.RoundToInt(PlayerATK / 20);
        power += Mathf.RoundToInt(PlayerMaxHP / 200);
        power += Mathf.RoundToInt(PlayerDef / 20);
        power += Mathf.RoundToInt(ComboPercent / 3);
        power += Mathf.RoundToInt(ComboResist / 2);
        power += Mathf.RoundToInt(CriticalPercant / 2);
        power += Mathf.RoundToInt(CriticalResist / 2);
        power += Mathf.RoundToInt(CriticalDamage*10);
        power += Mathf.RoundToInt(AvoidPercent);
        power += Mathf.RoundToInt(AvoidResist);
        power += Mathf.RoundToInt(DrainPercent / 3);
        power += Mathf.RoundToInt(DrainResist / 2);
        power += Mathf.RoundToInt(DrainAmount*5);

        Power = power;
    }
    #endregion
}
