using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    public AudioSource Music;
    public AudioSource FX;

    [Header("Music")]
    public AudioClip Intro;
    public AudioClip IntroStage;
    public AudioClip LoopStage;
    public AudioClip GameOver;
    public AudioClip PlayAgain;
    public AudioClip PowerUp;

    [Header("FX")]
    public AudioClip fxCoin;
    public AudioClip fxJump;
    public AudioClip fxItsMe;
    public AudioClip fxGameOver;
    public AudioClip fxIntro;
    public AudioClip fxStart;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayMusic(AudioClip clip, bool loop)
    {
        Music.loop = loop;
        Music.clip = clip;
        Music.Play();
    }

    public void PlayFX(AudioClip clip)
    {
        FX.PlayOneShot(clip);
    }
 
    public IEnumerator PlayMusicGameplay()
    {
        PlayMusic(IntroStage, false);
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => !Music.isPlaying);
        PlayMusic(LoopStage, true);
    }
}
