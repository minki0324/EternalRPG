using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionFade : MonoBehaviour
{
    public static TransitionFade instance;

    private float fadeDuration = 0.6f; // 페이드 인/아웃에 걸릴 시간 (초)

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
    { // true => 까매지기, false => 밝아지기
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
            yield return null; // 다음 프레임까지 대기
        }

        yield return new WaitForSeconds(0.2f);
        isLoading = false;
        FadeCoroutine = null;
    }
}
