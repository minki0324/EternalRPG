using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionFade : MonoBehaviour
{
    public static TransitionFade instance;

    private float fadeDuration = 0.6f; // ���̵� ��/�ƿ��� �ɸ� �ð� (��)

    public bool isLoading = false;
    public Coroutine FadeCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public IEnumerator fade(Image image, bool isFadeIn)
    { // true => �������, false => �������
        isLoading = true;
        float startAlpha = isFadeIn ? 0f : 1f;
        float endAlpha = isFadeIn ? 1f : 0f;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            Color newColor = image.color;
            newColor.a = Mathf.Lerp(startAlpha, endAlpha, t);
            image.color = newColor;
            yield return null; // ���� �����ӱ��� ���
        }

        yield return new WaitForSeconds(0.2f);
        isLoading = false;
        FadeCoroutine = null;
    }
}
