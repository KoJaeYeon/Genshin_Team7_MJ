using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WolfSound : MonoBehaviour
{
    enum Audiosound
    {
        Howl,
        Jump,
        Stamp,
        Drift,
        IceRain,
        BackGround
    }

    private Dictionary<Audiosound, AudioClip> SoundDic;
    private AudioSource SoundEffect;
    private AudioSource BackgroundSound;

    public AudioClip[] BossSound;
    public GameObject FirstObject;
    public GameObject SecondObject;

    private void OnEnable()
    {
        playBackGroundSound();
    }

    private void Awake()
    {
        SoundDic = new Dictionary<Audiosound, AudioClip>();
        SoundEffect = FirstObject.GetComponent<AudioSource>();
        BackgroundSound = SecondObject.GetComponent<AudioSource>();

        AddSound(Audiosound.Howl, BossSound[0]);
        AddSound(Audiosound.Jump, BossSound[1]);
        AddSound(Audiosound.Stamp, BossSound[2]);
        AddSound(Audiosound.Drift, BossSound[3]);
        AddSound(Audiosound.IceRain, BossSound[4]);
        AddSound(Audiosound.BackGround, BossSound[5]);
    }

    private void AddSound(Audiosound audiosound, AudioClip sound)
    {
        SoundDic.Add(audiosound, sound);
    }

    private AudioClip GetSound(Audiosound audiosound)
    {
        return SoundDic[audiosound];
    }

    public void PlayHowl()
    {
        AudioClip Howl = GetSound(Audiosound.Howl);
        SoundEffect.clip = Howl;
        SoundEffect.Play();
    }

    public void PlayJump()
    {
        AudioClip Jump = GetSound(Audiosound.Jump);
        SoundEffect.clip = Jump;
        SoundEffect.Play();
    }

    public void PlayStamp()
    {
        AudioClip Stamp = GetSound(Audiosound.Stamp);
        SoundEffect.clip = Stamp;
        SoundEffect.Play();
    }

    public void PlayDrift()
    {
        AudioClip Drift = GetSound(Audiosound.Drift);
        SoundEffect.clip = Drift;
        SoundEffect.Play();
    }

    public void PlayIceRain()
    {
        AudioClip IceRain = GetSound(Audiosound.IceRain);
        BackgroundSound.clip = IceRain;
        BackgroundSound.loop = true;
        BackgroundSound.Play();

        StartCoroutine(IceloopStop(BackgroundSound));
    }

    public void playBackGroundSound()
    {
        AudioClip Back = GetSound(Audiosound.BackGround);
        BackgroundSound.clip = Back;
        BackgroundSound.loop = true;
        BackgroundSound.Play();
    }

    public void StopBackGroundSound() //사망 애니메이션에서 제어
    {
        BackgroundSound.loop = false;
        BackgroundSound.Stop();
    }
    private IEnumerator IceloopStop(AudioSource audioSource)
    {
        yield return new WaitForSeconds(3.5f);
        audioSource.loop = false;
    }
}
