using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource effectSource;
    public AudioSource effectSource2;
    public AudioSource effectSource3;
    public AudioSource bgmSource;
    public List<AudioClip> effectClips;
    public AudioClip morningBgm;
    public AudioClip afternoonBgm;
    public AudioClip eveningBgm;
    public AudioClip nightBgm;
    public AudioClip mainBgm;
    public AudioClip battleBgm;
    public AudioClip bossBGM;

    public Dictionary<string, AudioClip> effectDictionary;

    private float checkInterval = 60f; 
    private float nextCheckTime = 0f;
    private float fadeDuration = 1.0f;

    private void Start()
    {
        effectDictionary = new Dictionary<string, AudioClip>();
        foreach(AudioClip clip in effectClips)
        {
            effectDictionary[clip.name] = clip;
        }

        UpdateBGMByTime();
    }

    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            UpdateBGMByTime();
            nextCheckTime = Time.time + checkInterval;
        }
    }

    public void PlayEffect(string clipName)
    {
        if (effectDictionary.ContainsKey(clipName))
        {
            effectSource3.PlayOneShot(effectDictionary[clipName]);
        }
        else
        {
            Debug.LogWarning("효과음을 찾을 수 없습니다: " + clipName);
        }
    }


    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("배경음을 찾을 수 없습니다");
        }
    }

    public void PlayMainBGM()
    {
        ChangeBGM(mainBgm);
    }

    public void ChangeBGM(AudioClip newBgmClip)
    {
        StartCoroutine(FadeOutAndChangeBGM(newBgmClip));
    }

    public void StopEffect()
    {
        effectSource.Stop();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void UpdateBGMByTime()
    {
        System.DateTime now = System.DateTime.Now;
        if (now.Hour >= 6 && now.Hour < 12)
        {
            ChangeBGM(morningBgm);
        }
        else if (now.Hour >= 12 && now.Hour < 18)
        {
            ChangeBGM(afternoonBgm);
        }
        else if (now.Hour >= 18 && now.Hour < 21)
        {
            ChangeBGM(eveningBgm);
        }
        else
        {
            ChangeBGM(nightBgm);
        }
    }

    private IEnumerator FadeOutAndChangeBGM(AudioClip newBgmClip)
    {
        yield return StartCoroutine(FadeOut(bgmSource, fadeDuration));
        bgmSource.clip = newBgmClip;
        bgmSource.Play();
        yield return StartCoroutine(FadeIn(bgmSource, fadeDuration));
    }

    private IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float startVolume = 0.2f;

        audioSource.volume = 0;
        audioSource.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, startVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = startVolume;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
