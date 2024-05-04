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

    [Header("��")]
    [SerializeField] private GameObject runePrefabs;
    [SerializeField] private Transform runeParent;

    [Header("����")]
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
                    { // ������
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
            case "���� ��":
                monsterLocation = (Location)0;
                break;
            case "������ ��":
                monsterLocation = (Location)1;
                break;
            case "���� ����� ��":
                monsterLocation = (Location)2;
                break;
            case "���� ���Ժ�":
                monsterLocation = (Location)3;
                break;
            case "���� ���� ����":
                monsterLocation= (Location)4;
                break;
            case "���� �����±�":
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
            case "��ü":
                MonsterCollectionAll();
                break;
            case "����Ʈ ����":
                MonsterColletionElite();
                break;
            case "������":
                locationDropdown.gameObject.SetActive(true);
                MonsterColletionLocation();
                break;
            case "������":
                MonsterCollectionLevel();
                break;
        }
    }

    private void MonsterCollectionLevel()
    {
        CollectionClear();

        // ��� ���͸� ���� ������ ����
        List<MonsterData> sortedMonsters = new List<MonsterData>(DataManager.Instance.Monsters);
        sortedMonsters.Sort((a, b) => a.MonsterLevel.CompareTo(b.MonsterLevel));

        // ���ĵ� ���͸� �ݺ��ϸ鼭 �÷����� ǥ��
        for (int i = 0; i < sortedMonsters.Count; i++)
        {
            // ���ĵ� ���Ϳ� ���� �÷��� �г��� �ν��Ͻ�ȭ�ϰ� ǥ��
            GetMonsterData(sortedMonsters[i]);
        }
    }
    private void MonsterColletionElite()
    {
        CollectionClear();
        for(int i = 0; i < DataManager.Instance.Monsters.Length; i++)
        {
            if(DataManager.Instance.Monsters[i].isElite)
            { // ����Ʈ �ΰ͸� ��������
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
            { // ���� ���� ���� �͸� ��������
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

        // �⺻ ���� ���
        MonsterColletionPanel data = monster.GetComponent<MonsterColletionPanel>();
        data.MonsterName.text = $"No.{mon.MonsterID} {mon.MonsterName}";
        data.MonsterSprite.sprite = mon.MonsterSprite;
        data.MonsterLevel.text = $"Lv. {mon.MonsterLevel:N0}";
        data.MonsterLocation.text = $"��ġ : {mon.MonsterLocation}";
        data.Level = mon.MonsterLevel;

        // ��� ������ ���
        if (mon.RewardItem != null)
        { // ��� �� �������� �ִٸ�
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
        { // �����̶�� �� ���
            GameObject rune = Instantiate(data.RuneCollection);
            rune.transform.SetParent(data.DropItemParent);
            rune.transform.localScale = data.DropItemParent.localScale;

            RunePanel runeData = rune.GetComponent<RunePanel>();
            if (GameManager.Instance.RuneHashSet.Contains(mon.RewardRune.EquipmentName))
            { // ������ �ִ°Ŷ�� 
                runeData.ItemIcon.SetActive(true);
                runeData.IconSprite.sprite = EquipmentManager.Instance.GetEquipmentSprite(mon.RewardRune);
            }
            else
            { // ���ٸ�
                runeData.QuestionMark.SetActive(true);
            }

            // ���� ��ũ ���
            data.EliteMark.gameObject.SetActive(true);

            // ������� ������ ����������
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
                location = "���� ��";
                break;
            case Location.FairyForest:
                location = "������ ��";
                break;
            case Location.RedForest:
                location = "���� ����� ��";
                break;
            case Location.Mine:
                location = "���� ���Ժ�";
                break;
            case Location.InTree:
                location = "���� ���� ����";
                break;
            case Location.DeepMine:
                location = "���� �����±�";
                break;
        }
        
        return location;
    }
}