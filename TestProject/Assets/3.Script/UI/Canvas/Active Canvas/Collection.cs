using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectionType
{
    Rune,
    Monster
}

public class Collection : MonoBehaviour
{
    [SerializeField] private CollectionType CType;

    [Header("��")]
    [SerializeField] private GameObject runePrefabs;
    [SerializeField] private Transform runeParent;

    [Header("����")]
    [SerializeField] private GameObject monsterPrefabs;
    [SerializeField] private Transform monsterParent;

    private void OnEnable()
    {
        switch (CType)
        {
            case CollectionType.Rune:
                for(int i = 0; i < EquipmentManager.Instance.RuneDatas.Length; i++)
                {
                    GameObject runeObj = Instantiate(runePrefabs);
                    runeObj.transform.SetParent(runeParent);
                    runeObj.transform.localScale = runeParent.localScale;
                    RunePanel rune = runeObj.GetComponent<RunePanel>();
                    if(GameManager.Instance.RuneHashSet.Contains(EquipmentManager.Instance.RuneDatas[i].EquipmentName))
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
                for(int i = 0; i < DataManager.Instance.Monsters.Length; i++)
                {
                    GameObject monster = Instantiate(monsterPrefabs);
                    monster.transform.SetParent(monsterParent);
                    monster.transform.localScale = monsterParent.localScale;

                    // �⺻ ���� ���
                    MonsterColletionPanel data = monster.GetComponent<MonsterColletionPanel>();
                    data.MonsterName.text = $"No.{DataManager.Instance.Monsters[i].MonsterID} {DataManager.Instance.Monsters[i].MonsterName}";
                    data.MonsterSprite.sprite = DataManager.Instance.Monsters[i].MonsterSprite;

                    // ��� ������ ���
                    if(DataManager.Instance.Monsters[i].RewardItem != null)
                    { // ��� �� �������� �ִٸ�
                        for(int j = 0; j < DataManager.Instance.Monsters[i].RewardItem.Length; j++)
                        {
                            GameObject item = Instantiate(data.ItemCollection);
                            item.transform.SetParent(data.DropItemParent);
                            item.transform.localScale = data.DropItemParent.localScale;

                            DropItem Icon = item.GetComponent<DropItem>();
                            Icon.DropItemImage.sprite = EquipmentManager.Instance.GetEquipmentSprite(DataManager.Instance.Monsters[i].RewardItem[j]);
                        }
                    }

                    if(DataManager.Instance.Monsters[i].isElite)
                    { // �����̶�� �� ���
                        GameObject rune = Instantiate(data.RuneCollection);
                        rune.transform.SetParent(data.DropItemParent);
                        rune.transform.localScale = data.DropItemParent.localScale;

                        RunePanel runeData = rune.GetComponent<RunePanel>();
                        if(GameManager.Instance.RuneHashSet.Contains(DataManager.Instance.Monsters[i].RewardRune.EquipmentName))
                        { // ������ �ִ°Ŷ�� 
                            runeData.ItemIcon.SetActive(true);
                            runeData.IconSprite.sprite = EquipmentManager.Instance.GetEquipmentSprite(DataManager.Instance.Monsters[i].RewardRune);
                        }
                        else
                        { // ���ٸ�
                            runeData.QuestionMark.SetActive(true);
                        }
                    }
                }
                break;
        }
    }

    private void OnDisable()
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
    }
}