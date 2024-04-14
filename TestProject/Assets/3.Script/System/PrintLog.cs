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
        // �α� ����ִ� �κ��� �ִٸ� �ش� ��ġ�� �α� ���

        for (int i = 0; i < battleLogs.Length; i++)
        {
            if (battleLogs[i].text == string.Empty)
            {
                battleLogObj[i].SetActive(true);
                battleLogs[i].text = _logText;
                return;
            }
        }

        // �α� ����ִ� �κ��� ���ٸ� �о��
        // ��� �αװ� �� ���ִ� ���
        // ���� ������ �α׸� �����ϰ�, ���ο� �α׸� �迭�� ���� �ڿ� �߰�.
        for (int i = 0; i < battleLogs.Length - 1; i++)
        {
            battleLogs[i].text = battleLogs[i + 1].text;
        }

        // ���� ������ �α� ��ġ�� ���ο� �α� �߰�
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
            yield return null; // ���� �����ӱ��� ���
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
            yield return null; // ���� �����ӱ��� ���
        }
    }
}
