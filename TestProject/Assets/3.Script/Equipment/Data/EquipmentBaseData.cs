﻿using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum Category
{
    Weapon,
    Armor,
    Helmet,
    Pants,
    Glove,
    Shoes,
    Belt,
    ShoulderArmor,
    Ring,
    Neckless,
    Clock,
    Other
}

public class EquipmentBaseData : ScriptableObject
{
    [Header("기본 정보")]
    [JsonConverter(typeof(StringEnumConverter))]
    public Category EquipmentType;
    public string SpriteName; // 스프라이트 이름 추가
    public int ItemID;
    public string EquipmentName;
    public int RequireCost;
    public bool isCanBuy;
}