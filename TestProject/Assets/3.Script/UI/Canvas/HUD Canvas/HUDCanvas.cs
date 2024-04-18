using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDCanvas : MonoBehaviour
{
    public TMP_Text mapText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text gemText;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private Slider HPSlider;
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
        HPSlider.value = (float)GameManager.Instance.PlayerCurHP / GameManager.Instance.PlayerMaxHP;
        HPText.text = $"{GameManager.Instance.PlayerCurHP:N0} / {GameManager.Instance.PlayerMaxHP:N0}";
    }

    private IEnumerator StartGame()
    {
        activeCanvas.SetActive(true);
        transitionObj.SetActive(true);
        loading.SetTransitioning(true);
        StartCoroutine(loading.StartLoadingDark());

        // Loading.isLoading�� false�� �� ������ ���
        while (loading.isLoading)
        {
            yield return null;
        }

        transitionObj.SetActive(false);
        activeCanvas.SetActive(false);
    }

    public void LackOfEnergy()
    {
        if (GameManager.Instance.CurrentEnergy <= 0)
        {
            StartCoroutine(LackOfEnergy_Co());
        }
    }

    private IEnumerator LackOfEnergy_Co()
    {
       GameManager.Instance.ResetRound();

        activeCanvas.SetActive(true);
        transitionObj.SetActive(true);
        loading.SetTransitioning(true);
        StartCoroutine(loading.StartLoadingLight());

        while (loading.isLoading)
        {
            yield return null;
        }

        SceneManager.LoadScene(0);
    }

    public void MainSceneLoad()
    {
        StartCoroutine(MainSceneLoad_Co());
    }

    private IEnumerator MainSceneLoad_Co()
    {
        activeCanvas.SetActive(true);
        transitionObj.SetActive(true);
        loading.SetTransitioning(true);
        StartCoroutine(loading.StartLoadingLight());

        while (loading.isLoading)
        {
            yield return null;
        }

        SceneManager.LoadScene(0);
    }
}
