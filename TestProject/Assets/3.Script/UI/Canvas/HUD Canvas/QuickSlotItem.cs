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
                quickSlotItemName.text = "체력 포션";
                costImage.sprite = costIconSprite[0];
                quickSlotItemDes.text = des[0];
                int costGold = Mathf.RoundToInt(GameManager.Instance.Gold / 10);
                costText.text = $"{costGold:N0} Gold";
                break;
            case QuickItem.Book:
                quickSlotItemName.text = "능력의 책";
                costImage.sprite = costIconSprite[1];
                quickSlotItemDes.text = des[1];
                costText.text = "7 Gem";
                break;
            case QuickItem.GoldPack:
                quickSlotItemName.text = "골드 보따리";
                costImage.sprite = costIconSprite[1];
                quickSlotItemDes.text = des[2];
                costText.text = "2 Gem";
                break;
            case QuickItem.Food:
                quickSlotItemName.text = "맛있는 고기";
                costImage.sprite = costIconSprite[1];
                quickSlotItemDes.text = des[3];
                costText.text = "4 Gem";
                break;
            case QuickItem.Clover:
                quickSlotItemName.text = "행운의 클로버";
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
                PrintLog.Instance.StaticLog("체력 회복 완료!");

                activeCanvas.SetActive(false);
                quickSlotItemPanel.SetActive(false);
                break;
            case QuickItem.Book:
                if(GameManager.Instance.Gem >= 7 && !GameManager.Instance.isAPBook)
                {
                    GameManager.Instance.isAPBook = true;
                    GameManager.Instance.Gem -= 7;
                    PrintLog.Instance.StaticLog("능력의 책 구매 완료!");
                    activeCanvas.SetActive(false);
                    quickSlotItemPanel.SetActive(false);
                }
                else if(GameManager.Instance.Gem < 7)
                {
                    PrintLog.Instance.StaticLog("보유 중인 젬이 부족합니다.");
                }
                else if(GameManager.Instance.isAPBook)
                {
                    PrintLog.Instance.StaticLog("이미 능력의 책을 구입 했습니다.");
                }
                break;
            case QuickItem.GoldPack:
                if(GameManager.Instance.Gem >= 2 && !GameManager.Instance.isGoldPack)
                {
                    GameManager.Instance.isGoldPack = true;
                    GameManager.Instance.Gem -= 2;
                    PrintLog.Instance.StaticLog("골드 보따리 구매 완료!");
                    activeCanvas.SetActive(false);
                    quickSlotItemPanel.SetActive(false);
                }
                else if (GameManager.Instance.Gem < 2)
                {
                    PrintLog.Instance.StaticLog("보유 중인 젬이 부족합니다.");
                }
                else if (GameManager.Instance.isGoldPack)
                {
                    PrintLog.Instance.StaticLog("이미 골드 보따리를 구입 했습니다.");
                }
                break;
            case QuickItem.Food:
                if(GameManager.Instance.Gem >= 4 && !GameManager.Instance.isFood)
                {
                    GameManager.Instance.isFood = true;
                    GameManager.Instance.Gem -= 4;
                    PrintLog.Instance.StaticLog("맛있는 고기 구매 완료!");
                    GameManager.Instance.CurrentEnergy += 3;
                    activeCanvas.SetActive(false);
                    quickSlotItemPanel.SetActive(false);
                }
                else if (GameManager.Instance.Gem < 4)
                {
                    PrintLog.Instance.StaticLog("보유 중인 젬이 부족합니다.");
                }
                else if (GameManager.Instance.isFood)
                {
                    PrintLog.Instance.StaticLog("이번 라운드는 이미 고기를 먹었습니다.");
                }
                break;
            case QuickItem.Clover:
                if(GameManager.Instance.Gem >= 5 && !GameManager.Instance.isClover)
                {
                    GameManager.Instance.isClover = true;
                    GameManager.Instance.Gem -= 5;
                    PrintLog.Instance.StaticLog("행운의 클로버 구매 완료!");
                    activeCanvas.SetActive(false);
                    quickSlotItemPanel.SetActive(false);
                }
                else if (GameManager.Instance.Gem < 5)
                {
                    PrintLog.Instance.StaticLog("보유 중인 젬이 부족합니다.");
                }
                else if (GameManager.Instance.isClover)
                {
                    PrintLog.Instance.StaticLog("이미 행운의 클로버를 구입 했습니다.");
                }
                break;
        }
    }
}