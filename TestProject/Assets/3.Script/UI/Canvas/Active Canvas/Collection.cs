using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CollectionType
{
    Rune,
    Monster
}

public enum Location
{
    RelicForest,
    FairyForest,
    RedForest,
    Mine,
    InTree,
    DeepMine
}

public class Collection : MonoBehaviour
{
    [SerializeField] private CollectionType CType;
    [SerializeField] private Location monsterLocation;

    [Header("룬")]
    [SerializeField] private GameObject runePrefabs;
    [SerializeField] private Transform runeParent;

    [Header("몬스터")]
    [SerializeField] private GameObject monsterPrefabs;
    [SerializeField] private Transform monsterParent;
    [SerializeField] private TMP_Dropdown monsterCollectionDropdown;
    [SerializeField] private TMP_Dropdown locationDropdown;
    [SerializeField] private ScrollRect collectionScroll;

    private void OnEnable()
    {
        switch (CType)
        {
            case CollectionType.Rune:
                for (int i = 0; i < EquipmentManager.Instance.RuneDatas.Length; i++)
                {
                    GameObject runeObj = Instantiate(runePrefabs);
                    runeObj.transform.SetParent(runeParent);
                    runeObj.transform.localScale = runeParent.localScale;
                    RunePanel rune = runeObj.GetComponent<RunePanel>();
                    if (GameManager.Instance.RuneHashSet.Contains(EquipmentManager.Instance.RuneDatas[i].EquipmentName))
                    { // 있으면
                        rune.ItemIcon.SetActive(true);
                        rune.NameText.text = EquipmentManager.Instance.RuneDatas[i].EquipmentName;
                        rune.DesText.text = EquipmentManager.Instance.RuneDatas[i].EquipmentDes;
                        rune.IconSprite.sprite = EquipmentManager.Instance.GetEquipmentSprite(EquipmentManager.Instance.RuneDatas[i]);
                    }
                    else
                    {
                        rune.QuestionMark.SetActive(true);
                        rune.NameText.text = "???";
                        rune.DesText.text = string.Empty;
                    }
                }
                break;
            case CollectionType.Monster:
                MonsterCollectionAll();
                break;
        }
    }

    private void OnDisable()
    {
        CollectionClear();
    }

    private void CollectionClear()
    {
        if (runeParent != null && runeParent.childCount != 0)
        {
            for (int i = 0; i < runeParent.childCount; i++)
            {
                Destroy(runeParent.GetChild(i).gameObject);
            }
        }

        if (monsterParent != null && monsterParent.childCount != 0)
        {
            for (int i = 0; i < monsterParent.childCount; i++)
            {
                Destroy(monsterParent.GetChild(i).gameObject);
            }
        }

        collectionScroll.normalizedPosition = new Vector2(0, 1);
    }

    public void MonsterLocationType()
    {
        int selectDropDown = locationDropdown.value;
        switch (locationDropdown.options[selectDropDown].text)
        {
            case "깊은 숲":
                monsterLocation = (Location)0;
                break;
            case "정령의 숲":
                monsterLocation = (Location)1;
                break;
            case "붉은 기운의 숲":
                monsterLocation = (Location)2;
                break;
            case "광산 초입부":
                monsterLocation = (Location)3;
                break;
            case "죽은 나무 동굴":
                monsterLocation= (Location)4;
                break;
            case "광산 나가는길":
                monsterLocation= (Location)5;
                break;
        }
        MonsterColletionLocation();
    }

    public void MonsterCollectionDropDown()
    {
        int selectDropDown = monsterCollectionDropdown.value;
        locationDropdown.gameObject.SetActive(false);

        switch (monsterCollectionDropdown.options[selectDropDown].text)
        {
            case "전체":
                MonsterCollectionAll();
                break;
            case "엘리트 몬스터":
                MonsterColletionElite();
                break;
            case "지역별":
                locationDropdown.gameObject.SetActive(true);
                MonsterColletionLocation();
                break;
            case "레벨별":
                MonsterCollectionLevel();
                break;
        }
    }

    private void MonsterCollectionLevel()
    {
        CollectionClear();

        // 모든 몬스터를 레벨 순으로 정렬
        List<MonsterData> sortedMonsters = new List<MonsterData>(DataManager.Instance.Monsters);
        sortedMonsters.Sort((a, b) => a.MonsterLevel.CompareTo(b.MonsterLevel));

        // 정렬된 몬스터를 반복하면서 컬렉션을 표시
        for (int i = 0; i < sortedMonsters.Count; i++)
        {
            // 정렬된 몬스터에 대해 컬렉션 패널을 인스턴스화하고 표시
            GetMonsterData(sortedMonsters[i]);
        }
    }
    private void MonsterColletionElite()
    {
        CollectionClear();
        for(int i = 0; i < DataManager.Instance.Monsters.Length; i++)
        {
            if(DataManager.Instance.Monsters[i].isElite)
            { // 엘리트 인것만 가져오기
                GetMonsterData(DataManager.Instance.Monsters[i]);
            }
        }
    }

    private void MonsterColletionLocation()
    {
        CollectionClear();
        for (int i = 0; i < DataManager.Instance.Monsters.Length; i++)
        {
            if (LocationText() == DataManager.Instance.Monsters[i].MonsterLocation)
            { // 서식 지역 같은 것만 가져오기
                GetMonsterData(DataManager.Instance.Monsters[i]);
            }
        }
    }

    private void MonsterCollectionAll()
    {
        CollectionClear();
        for (int i = 0; i < DataManager.Instance.Monsters.Length; i++)
        {
            GetMonsterData(DataManager.Instance.Monsters[i]);
        }
    }

    private GameObject GetMonsterData(MonsterData mon)
    {
        GameObject monster = Instantiate(monsterPrefabs);
        monster.transform.SetParent(monsterParent);
        monster.transform.localScale = monsterParent.localScale;

        // 기본 정보 출력
        MonsterColletionPanel data = monster.GetComponent<MonsterColletionPanel>();
        data.MonsterName.text = $"No.{mon.MonsterID} {mon.MonsterName}";
        data.MonsterSprite.sprite = mon.MonsterSprite;
        data.MonsterLevel.text = $"Lv. {mon.MonsterLevel:N0}";
        data.MonsterLocation.text = $"위치 : {mon.MonsterLocation}";
        data.Level = mon.MonsterLevel;

        // 드랍 아이템 출력
        if (mon.RewardItem != null)
        { // 드랍 할 아이템이 있다면
            for (int i = 0; i < mon.RewardItem.Length; i++)
            {
                GameObject item = Instantiate(data.ItemCollection);
                item.transform.SetParent(data.DropItemParent);
                item.transform.localScale = data.DropItemParent.localScale;

                DropItem Icon = item.GetComponent<DropItem>();
                Icon.DropItemImage.sprite = EquipmentManager.Instance.GetEquipmentSprite(mon.RewardItem[i]);

                Icon.OwnCountText.text = $"{DataManager.Instance.GetOwnDictionary(mon.RewardItem[i])[mon.RewardItem[i].ItemID]} / 10";
            }
        }

        if (mon.isElite)
        { // 엘몹이라면 룬 출력
            GameObject rune = Instantiate(data.RuneCollection);
            rune.transform.SetParent(data.DropItemParent);
            rune.transform.localScale = data.DropItemParent.localScale;

            RunePanel runeData = rune.GetComponent<RunePanel>();
            if (GameManager.Instance.RuneHashSet.Contains(mon.RewardRune.EquipmentName))
            { // 가지고 있는거라면 
                runeData.ItemIcon.SetActive(true);
                runeData.IconSprite.sprite = EquipmentManager.Instance.GetEquipmentSprite(mon.RewardRune);
            }
            else
            { // 없다면
                runeData.QuestionMark.SetActive(true);
            }

            // 엘몹 마크 출력
            data.EliteMark.gameObject.SetActive(true);

            // 잡았으면 빨간색 출력해줘야함
            if (DataManager.Instance.EliteMonsterDic.ContainsKey(mon.MonsterID) &&
                DataManager.Instance.EliteMonsterDic[mon.MonsterID] == true)
            {
                data.EliteMark.color = Color.red;
            }
        }
        return monster;
    }
    
    private string LocationText()
    {
        string location = string.Empty;

        switch (monsterLocation)
        {
            case Location.RelicForest:
                location = "깊은 숲";
                break;
            case Location.FairyForest:
                location = "정령의 숲";
                break;
            case Location.RedForest:
                location = "붉은 기운의 숲";
                break;
            case Location.Mine:
                location = "광산 초입부";
                break;
            case Location.InTree:
                location = "죽은 나무 동굴";
                break;
            case Location.DeepMine:
                location = "광산 나가는길";
                break;
        }
        
        return location;
    }
}