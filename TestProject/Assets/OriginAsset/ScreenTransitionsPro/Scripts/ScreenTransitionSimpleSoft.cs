// (c) Copyright Andrey Torchinskiy, 2019. All rights reserved.

using UnityEngine;
using System;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Screen Transitions Pro/Simple Soft")]
public class ScreenTransitionSimpleSoft : MonoBehaviour
{
    #region Variables

    public bool isLoading = false;

    /// <summary>
    /// Material that will be applied to rendered image during transition.
    /// </summary>
    [Tooltip("Material that will be applied to rendered image during transition.")]
    public Material transitionMaterial;

    /// <summary>
    /// Background color that will be used during transition.
    /// </summary>
    [SerializeField] // 추가
    private Color backgroundColor = Color.white; // 변경

    /// <summary>
    /// Texture that will be used as background during transition.
    /// Render Texture also allowed.
    /// </summary>
    [Tooltip("Texture that will be used as background during transition. Render Texture also allowed.")]
    public Texture backgroundTexture;

    public enum BackgroundType
    {
        COLOR,
        TEXTURE
    }
    /// <summary>
    /// Defines what type background will be used during transition.
    /// </summary>
    [Tooltip("Defines what type background will be used during transition.")]
    [SerializeField] // 추가
    private BackgroundType backgroundType = BackgroundType.COLOR;

    /// <summary>
    /// Represents current progress of the transition.
    /// from -1 to 0 - no transition (depends on the falloff)
    /// 1 - full transition to background color.
    /// </summary>
    [Range(-1f, 1f), Tooltip("Represents current progress of the transition.")]
    public float cutoff = 0f;

    /// <summary>
    /// Smooth blend between rendered texture and background color.
    /// 0 - no blend (sharp border)
    /// 1 - max blend (size depends on the gradient in the blue channel of the transition texture). 
    /// </summary>
    [Range(0f, 1f), Tooltip("Smooth blend between rendered texture and background color.")]
    public float falloff = 0f;

    /// <summary>
    /// Flag that tells Unity to process transition. 
    /// Set this flag at the beginning of the transition and unset at the end 
    /// to avoid unnecessary calculations and save some performance.
    /// </summary>
    [Tooltip("Flag that tells Unity to process transition. Set this flag at the beginning of the transition and unset it at the end to avoid unnecessary calculations and to save some performance.")]
    public bool transitioning;

    /// <summary>
    /// Set this flag if you want transition texture to fit the screen.
    /// If unset, the transition texture will maintain 1:1 aspect ratio and
    /// will fit screen horizontally if screen width is greater than screen height
    /// or it will fit screen vertically if screen height is greater than screen width.
    /// </summary>
    [Tooltip("Set this flag if you want transition texture to fit the screen. If unset, the transition texture will maintain 1:1 aspect ratio and will fit the screen horizontally if screen width is greater than screen height or it will fit the screen vertically if screen height is greater than screen width.")]
    public bool fitToScreen;

    /// <summary>
    /// Set this flag if you want to flip transition texture horizontally.
    /// </summary>
    [Tooltip("Set this flag if you want to flip transition texture horizontally.")]
    public bool flipHorizontally;

    /// <summary>
    /// Set this flag if you want to flip transition texture vertically.
    /// </summary>
    [Tooltip("Set this flag if you want to flip transition texture vertically.")]
    public bool flipVertically;

    /// <summary>
    /// Set this flag if you want to invert transition texture.
    /// It will swap rendered image with background color.
    /// </summary>
    [Tooltip("Set this flag if you want to invert transition texture. It will swap rendered image with background color.")]
    public bool invert;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        if (transitionMaterial)
        {
            switch (backgroundType)
            {
                case BackgroundType.COLOR:
                    transitionMaterial.DisableKeyword("USE_TEXTURE");
                    break;
                case BackgroundType.TEXTURE:
                    transitionMaterial.EnableKeyword("USE_TEXTURE");
                    break;
            }
        }
    }

    private void LateUpdate()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            transitioning = false;
            return;
        }

        if (transitioning && transitionMaterial)
        {
            transitionMaterial.SetInt("_Fit", fitToScreen ? 1 : 0);
            transitionMaterial.SetInt("_FlipH", flipHorizontally ? 1 : 0);
            transitionMaterial.SetInt("_FlipV", flipVertically ? 1 : 0);
            transitionMaterial.SetInt("_Invert", invert ? 1 : 0);
            transitionMaterial.SetFloat("_Cutoff", cutoff);
            transitionMaterial.SetFloat("_Falloff", falloff);
            switch (backgroundType)
            {
                case BackgroundType.COLOR:
                    transitionMaterial.SetColor("_Color", backgroundColor);
                    break;
                case BackgroundType.TEXTURE:
                    transitionMaterial.SetTexture("_Texture", backgroundTexture);
                    break;
            }
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (transitioning && transitionMaterial)
        {
            Graphics.Blit(source, destination, transitionMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    #endregion

    #region Interface Implementation

    public void SetTransitioning(bool t)
    {
        transitioning = t;
    }

    public void SetMaterial(Material m)
    {
        transitionMaterial = m;
    }

    public void SetCutoff(float c)
    {
        cutoff = Mathf.Clamp(c, -1f, 1f);
    }

    public void SetFalloff(float f)
    {
        falloff = Mathf.Clamp01(f);
    }

    public void SetBackgroundColor(Color bc)
    {
        backgroundColor = bc;
    }

    public void SetBackgroundTexture(Texture tex)
    {
        backgroundTexture = tex;
    }

    public void SetFitToScreen(bool fts)
    {
        fitToScreen = fts;
    }

    public void SetHorizontalFlip(bool hf)
    {
        flipHorizontally = hf;
    }

    public void SetVerticalFlip(bool vf)
    {
        flipVertically = vf;
    }

    public void SetInvert(bool i)
    {
        invert = i;
    }

    public void AddFocus(Transform f)
    {
        Debug.LogWarning("Current screen transition doesn't support adding focus. Value will be ignored.");
    }

    public void RemoveFocus(Transform f)
    {
        Debug.LogWarning("Current screen transition doesn't support removing focus. Value will be ignored.");
    }

    public void SetNoiseScale(float s)
    {
        Debug.LogWarning("Current screen transition doesn't support noise scale. Value will be ignored.");
    }

    public void SetNoiseVelocity(Vector2 v)
    {
        Debug.LogWarning("Current screen transition doesn't support noise velocity. Value will be ignored.");
    }

    #endregion

    public IEnumerator StartLoadingDark()
    {
        cutoff = -0.5f;
        isLoading = true;
        // Cutoff 값을 -0.5에서 1까지 증가시키는데 1초 동안의 시간을 사용합니다.
        float duration = 1.5f;
        float startValue = -0.5f;
        float endValue = 1f;
        float elapsedTime = 0f;

        // 증가되는 값에 따라 반복합니다.
        while (elapsedTime < duration)
        {
            // 시간에 따라 Cutoff 값을 조절합니다.
            cutoff = Mathf.Lerp(startValue, endValue, elapsedTime / duration);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 최종 값을 설정합니다.
        cutoff = endValue;
        isLoading = false;
    }

    public IEnumerator StartLoadingLight()
    {
        cutoff = 1f;
        isLoading = true;
        // Cutoff 값을 -0.5에서 1까지 증가시키는데 1초 동안의 시간을 사용합니다.
        float duration = 1.5f;
        float startValue = 1f;
        float endValue = -0.5f;
        float elapsedTime = 0f;

        // 증가되는 값에 따라 반복합니다.
        while (elapsedTime < duration)
        {
            // 시간에 따라 Cutoff 값을 조절합니다.
            cutoff = Mathf.Lerp(startValue, endValue, elapsedTime / duration);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 최종 값을 설정합니다.
        cutoff = endValue;
        isLoading = false;
    }
}
