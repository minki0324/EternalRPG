using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text gemText;
    [SerializeField] private ScreenTransitionSimpleSoft loading;
    [SerializeField] private GameObject activeCanvas;
    [SerializeField] private GameObject transitionObj;

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private void LateUpdate()
    {
        levelText.text = $"{GameManager.Instance.PlayerLevel:N0}Lv";
        goldText.text = $"{GameManager.Instance.Gold:N0}";
        energyText.text = $"{GameManager.Instance.CurrentEnergy:N0}";
        gemText.text = $"{GameManager.Instance.Gem:N0}";

    }

    private IEnumerator StartGame()
    {
        activeCanvas.SetActive(true);
        transitionObj.SetActive(true);
        loading.SetTransitioning(true);
        StartCoroutine(loading.StartLoadingDark());

        // Loading.isLoading이 false가 될 때까지 대기
        while (loading.isLoading)
        {
            yield return null;
        }

        transitionObj.SetActive(false);
        activeCanvas.SetActive(false);
    }
}
