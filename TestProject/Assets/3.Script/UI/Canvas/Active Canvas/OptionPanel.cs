using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text speedText;

    private void OnEnable()
    {
        speedText.text = $"현재 적용 배속 : x{GameManager.Instance.BattleSpeed}";
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
        speedText.text = $"현재 적용 배속 : x{GameManager.Instance.BattleSpeed}";
    }
}
