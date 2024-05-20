using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] BGM, SFX;
    public AudioSource BGMSource, SFXSource;
    public float BGMVolume, SFXVolume;
    private Coroutine BGMCoroutine;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if(PlayerPrefs.HasKey("BGMVolume"))
        { // 원래 있을 경우
            BGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0.4f);
        }
        else
        {
            BGMVolume = 0.4f;
        }
        if(PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.6f);
            SFXSource.volume = SFXVolume;
        }
        else
        {
            SFXVolume = 0.6f;
        }
    }

    public void SetBGMVolume(float volume)
    {
        BGMSource.volume = volume;
        BGMVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        SFXSource.volume = volume;
        SFXVolume = volume;
    }

    public void PlayBGM(string name)
    {
        if(BGMCoroutine != null)
        {
            StopCoroutine(BGMCoroutine);
        }
        BGMCoroutine = StartCoroutine(FadeInNewBGM(name));
    }

    private IEnumerator FadeOutCurrentBGM()
    {
        float startVolume = BGMSource.volume;

        while (BGMSource.volume > 0)
        {
            BGMSource.volume -= startVolume * Time.deltaTime / 1f; // 1f는 페이드 아웃 시간(초)
            yield return null;
        }

        BGMSource.Stop();
        BGMSource.volume = startVolume;
    }

    private IEnumerator FadeInNewBGM(string name)
    {
        // 현재 재생 중인 BGM 페이드 아웃
        if (BGMSource.isPlaying)
        {
            yield return FadeOutCurrentBGM();
        }

        // 새로운 BGM 찾기
        Sound sound = Array.Find(BGM, x => x.Name == name);
        if (sound == null)
        {
            Debug.Log("사운드가 없습니다.");
            yield break;
        }

        // 새로운 BGM 재생 및 페이드 인
        BGMSource.clip = sound.Clip;
        BGMSource.Play();

        BGMSource.volume = 0f;
        float targetVolume = BGMVolume; // 원하는 최종 볼륨

        while (BGMSource.volume < targetVolume)
        {
            BGMSource.volume += targetVolume * Time.deltaTime / 1f; // 1f는 페이드 인 시간(초)
            yield return null;
        }

        BGMSource.volume = targetVolume;
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(SFX, x => x.Name == name);
        if (sound == null)
        {
            Debug.Log("사운드가 없습니다.");
        }
        else
        {
            SFXSource.PlayOneShot(sound.Clip);
        }
    }

    public void TEST()
    {
        PlayerPrefs.DeleteAll();
    }
}
