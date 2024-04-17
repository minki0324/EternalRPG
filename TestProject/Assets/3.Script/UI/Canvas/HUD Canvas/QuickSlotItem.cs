using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum QuickItem
{
    Potion,
    Book,
    GoldPack,
    Food,
    Clover
}


public class QuickSlotItem : MonoBehaviour
{
    public QuickItem quickItem;

    [SerializeField] private Sprite[] costIconSprite;

    [Header("UI")]
    [SerializeField] private GameObject activeCanvas;
    [SerializeField] private GameObject quickSlotItemPanel;
    [SerializeField] private Image costImage;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text quickSlotItemName;
    [SerializeField] private TMP_Text quickSlotItemDes;
    [TextArea] [SerializeField] private string[] des;

    public void QuickSlotItemButton(int item)
    {
        quickItem = (QuickItem)item;

        activeCanvas.SetActive(true);
        quickSlotItemPanel.SetActive(true);

        switch (quickItem)
        {
            case QuickItem.Potion:
                quickSlotItemName.text = "ü�� ����";
                costImage.sprite = costIconSprite[0];
                quickSlotItemDes.text = des[0];
                int costGold = Mathf.RoundToInt(GameManager.Instance.Gold / 10);
                costText.text = $"{costGold:N0} Gold";
                break;
            case QuickItem.Book:
                quickSlotItemName.text = "�ɷ��� å";
                costImage.sprite = costIconSprite[1];
                quickSlotItemDes.text = des[1];
                costText.text = "7 Gem";
                break;
            case QuickItem.GoldPack:
                quickSlotItemName.text = "��� ������";
                costImage.sprite = costIconSprite[1];
                quickSlotItemDes.text = des[2];
                costText.text = "2 Gem";
                break;
            case QuickItem.Food:
                quickSlotItemName.text = "���ִ� ���";
                costImage.sprite = costIconSprite[1];
                quickSlotItemDes.text = des[3];
                costText.text = "4 Gem";
                break;
            case QuickItem.Clover:
                quickSlotItemName.text = "����� Ŭ�ι�";
                costImage.sprite = costIconSprite[1];
                quickSlotItemDes.text = des[4];
                costText.text = "5 Gem";
                break;
        }
    }

    public void AcceptButton()
    {
        switch (quickItem)
        {
            case QuickItem.Potion:
                int costGold = Mathf.RoundToInt(GameManager.Instance.Gold / 10);
                GameManager.Instance.Gold -= costGold;
                GameManager.Instance.PlayerCurHP = GameManager.Instance.PlayerMaxHP;

                costGold = Mathf.RoundToInt(GameManager.Instance.Gold / 10);
                costText.text = $"{costGold:N0} Gold";
                PrintLog.Instance.StaticLog("ü�� ȸ�� �Ϸ�!");

                activeCanvas.SetActive(false);
                quickSlotItemPanel.SetActive(false);
                break;
            case QuickItem.Book:
                if(GameManager.Instance.Gem >= 7 && !GameManager.Instance.isAPBook)
                {
                    GameManager.Instance.isAPBook = true;
                    GameManager.Instance.Gem -= 7;
                    PrintLog.Instance.StaticLog("�ɷ��� å ���� �Ϸ�!");
                    activeCanvas.SetActive(false);
                    quickSlotItemPanel.SetActive(false);
                }
                else if(GameManager.Instance.Gem < 7)
                {
                    PrintLog.Instance.StaticLog("���� ���� ���� �����մϴ�.");
                }
                else if(GameManager.Instance.isAPBook)
                {
                    PrintLog.Instance.StaticLog("�̹� �ɷ��� å�� ���� �߽��ϴ�.");
                }
                break;
            case QuickItem.GoldPack:
                if(GameManager.Instance.Gem >= 2 && !GameManager.Instance.isGoldPack)
                {
                    GameManager.Instance.isGoldPack = true;
                    GameManager.Instance.Gem -= 2;
                    PrintLog.Instance.StaticLog("��� ������ ���� �Ϸ�!");
                    activeCanvas.SetActive(false);
                    quickSlotItemPanel.SetActive(false);
                }
                else if (GameManager.Instance.Gem < 2)
                {
                    PrintLog.Instance.StaticLog("���� ���� ���� �����մϴ�.");
                }
                else if (GameManager.Instance.isGoldPack)
                {
                    PrintLog.Instance.StaticLog("�̹� ��� �������� ���� �߽��ϴ�.");
                }
                break;
            case QuickItem.Food:
                if(GameManager.Instance.Gem >= 4 && !GameManager.Instance.isFood)
                {
                    GameManager.Instance.isFood = true;
                    GameManager.Instance.Gem -= 4;
                    PrintLog.Instance.StaticLog("���ִ� ��� ���� �Ϸ�!");
                    GameManager.Instance.CurrentEnergy += 3;
                    activeCanvas.SetActive(false);
                    quickSlotItemPanel.SetActive(false);
                }
                else if (GameManager.Instance.Gem < 4)
                {
                    PrintLog.Instance.StaticLog("���� ���� ���� �����մϴ�.");
                }
                else if (GameManager.Instance.isFood)
                {
                    PrintLog.Instance.StaticLog("�̹� ����� �̹� ��⸦ �Ծ����ϴ�.");
                }
                break;
            case QuickItem.Clover:
                if(GameManager.Instance.Gem >= 5 && !GameManager.Instance.isClover)
                {
                    GameManager.Instance.isClover = true;
                    GameManager.Instance.Gem -= 5;
                    PrintLog.Instance.StaticLog("����� Ŭ�ι� ���� �Ϸ�!");
                    activeCanvas.SetActive(false);
                    quickSlotItemPanel.SetActive(false);
                }
                else if (GameManager.Instance.Gem < 5)
                {
                    PrintLog.Instance.StaticLog("���� ���� ���� �����մϴ�.");
                }
                else if (GameManager.Instance.isClover)
                {
                    PrintLog.Instance.StaticLog("�̹� ����� Ŭ�ι��� ���� �߽��ϴ�.");
                }
                break;
        }
    }
}