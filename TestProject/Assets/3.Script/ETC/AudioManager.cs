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
        { // ���� ���� ���
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
            BGMSource.volume -= startVolume * Time.deltaTime / 1f; // 1f�� ���̵� �ƿ� �ð�(��)
            yield return null;
        }

        BGMSource.Stop();
        BGMSource.volume = startVolume;
    }

    private IEnumerator FadeInNewBGM(string name)
    {
        // ���� ��� ���� BGM ���̵� �ƿ�
        if (BGMSource.isPlaying)
        {
            yield return FadeOutCurrentBGM();
        }

        // ���ο� BGM ã��
        Sound sound = Array.Find(BGM, x => x.Name == name);
        if (sound == null)
        {
            Debug.Log("���尡 �����ϴ�.");
            yield break;
        }

        // ���ο� BGM ��� �� ���̵� ��
        BGMSource.clip = sound.Clip;
        BGMSource.Play();

        BGMSource.volume = 0f;
        float targetVolume = BGMVolume; // ���ϴ� ���� ����

        while (BGMSource.volume < targetVolume)
        {
            BGMSource.volume += targetVolume * Time.deltaTime / 1f; // 1f�� ���̵� �� �ð�(��)
            yield return null;
        }

        BGMSource.volume = targetVolume;
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(SFX, x => x.Name == name);
        if (sound == null)
        {
            Debug.Log("���尡 �����ϴ�.");
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
