using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PrintLog : MonoBehaviour
{
    public static PrintLog Instance = null;

    [SerializeField] private GameObject[] battleLogObj;
    [SerializeField] private TMP_Text[] battleLogs;

    [SerializeField] private GameObject staticLogPanel;
    [SerializeField] private Image staticFrameImage;
    [SerializeField] private Image staticFrameBackground;
    [SerializeField] private TMP_Text staticLog;

    private Coroutine frameCoroutine;
    private Coroutine backgroundCoroutine;
    private Coroutine logCoroutine;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    public void BattleLog(string _logText)
    {
        // 로그 비어있는 부분이 있다면 해당 위치에 로그 출력

        for (int i = 0; i < battleLogs.Length; i++)
        {
            if (battleLogs[i].text == string.Empty)
            {
                battleLogObj[i].SetActive(true);
                battleLogs[i].text = _logText;
                return;
            }
        }

        // 로그 비어있는 부분이 없다면 밀어내기
        // 모든 로그가 꽉 차있는 경우
        // 가장 오래된 로그를 삭제하고, 새로운 로그를 배열의 가장 뒤에 추가.
        for (int i = 0; i < battleLogs.Length - 1; i++)
        {
            battleLogs[i].text = battleLogs[i + 1].text;
        }

        // 가장 오래된 로그 위치에 새로운 로그 추가
        battleLogs[battleLogs.Length - 1].text = _logText;
    }

    public void BattleLogClear()
    {
        for(int i = 0; i < battleLogs.Length; i++)
        {
            battleLogs[i].text = string.Empty;
            battleLogObj[i].SetActive(false);
        }
    }

    public void StaticLog(string _log)
    {
        staticLog.text = _log;

        if(frameCoroutine != null || backgroundCoroutine != null || logCoroutine != null)
        {
            StopAllCoroutines();

        }
        frameCoroutine = StartCoroutine(fade_out(staticFrameImage));
        backgroundCoroutine = StartCoroutine(fade_out(staticFrameBackground));
        logCoroutine = StartCoroutine(fade_out());
    }

    public IEnumerator fade_out(Image image)
    {
        float startAlpha = 1f;
        float endAlpha = 0f;
        float fadeDuration = 3f;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime / fadeDuration;
            Color newColor = image.color;
            newColor.a = Mathf.Lerp(startAlpha, endAlpha, t);
            image.color = newColor;
            yield return null; // 다음 프레임까지 대기
        }
    }

    public IEnumerator fade_out()
    {
        float startAlpha = 1f;
        float endAlpha = 0f;
        float fadeDuration = 3f;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime / fadeDuration;
            Color newColor = staticLog.color;
            newColor.a = Mathf.Lerp(startAlpha, endAlpha, t);
            staticLog.color = newColor;
            yield return null; // 다음 프레임까지 대기
        }
    }
}
