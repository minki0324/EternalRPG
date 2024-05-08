using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionPanel : MonoBehaviour
{
    [SerializeField] private bool isTitle;
    [SerializeField] private TMP_Text speedText;

    [Header("�̵� ��Ʈ�ѷ�")]
    [SerializeField] private Image padToggle;
    [SerializeField] private Image directToggle;
    [SerializeField] private Sprite[] toggleSprite;

    private void OnEnable()
    {
        speedText.text = $"���� ���� ��� : x{GameManager.Instance.BattleSpeed}";
        if(isTitle)
        {
            padToggle.sprite = GameManager.Instance.isMovePad ? toggleSprite[1] : toggleSprite[0];
            directToggle.sprite = GameManager.Instance.isMovePad ? toggleSprite[0] : toggleSprite[1];
        }
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BattleSpeed(int _rate)
    {
        GameManager.Instance.BattleSpeed = _rate;
        speedText.text = $"���� ���� ��� : x{GameManager.Instance.BattleSpeed}";
    }

    public void MoveMethod(string method)
    {
        switch (method)
        {
            case "Direct":
                GameManager.Instance.isMovePad = false;
                break;
            case "Pad":
                GameManager.Instance.isMovePad=true;
                break;
        }
        padToggle.sprite = GameManager.Instance.isMovePad ? toggleSprite[1] : toggleSprite[0];
        directToggle.sprite = GameManager.Instance.isMovePad ? toggleSprite[0] : toggleSprite[1];
    }
}
