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

    [Header("�����")]
    [SerializeField] private Slider BGMSlider, SFXSlider;

    private void OnEnable()
    {
        speedText.text = $"���� ���� ��� : x{GameManager.Instance.BattleSpeed}";
        if(isTitle)
        {
            padToggle.sprite = GameManager.Instance.isMovePad ? toggleSprite[1] : toggleSprite[0];
            directToggle.sprite = GameManager.Instance.isMovePad ? toggleSprite[0] : toggleSprite[1];
        }
        // �����̴� �ʱⰪ ����
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.4f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.6f);

        BGMSlider.minValue = 0f;
        BGMSlider.maxValue = 0.4f;
        BGMSlider.value = bgmVolume;

        SFXSlider.minValue = 0f;
        SFXSlider.maxValue = 0.6f;
        SFXSlider.value = sfxVolume;

        // �̺�Ʈ ������ �߰�
        BGMSlider.onValueChanged.AddListener(delegate { OnBGMSliderValueChanged(); });
        SFXSlider.onValueChanged.AddListener(delegate { OnSFXSliderValueChanged(); });
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

    public void OnBGMSliderValueChanged()
    {
        float value = BGMSlider.value;
        PlayerPrefs.SetFloat("BGMVolume", value);
        PlayerPrefs.Save(); // �� ����
        AudioManager.instance.SetBGMVolume(value);
    }

    public void OnSFXSliderValueChanged()
    {
        float value = SFXSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save(); // �� ����
        AudioManager.instance.SetSFXVolume(value);
    }
}
