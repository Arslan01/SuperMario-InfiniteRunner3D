using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{

    void Start()
    {
        StartCoroutine("IntroGame");
    }

    IEnumerator IntroGame()
    {
        AudioManager._instance.PlayMusic(AudioManager._instance.Intro, true);

        yield return new WaitForSeconds(1.5f);
        FadeInOut._instance.Fade();
        yield return new WaitForSeconds(3f);
        FadeInOut._instance.Fade();
        yield return new WaitForEndOfFrame();
        AudioManager._instance.PlayFX(AudioManager._instance.fxItsMe);

        yield return new WaitUntil(() => FadeInOut._instance.isFadeComplete);
        FadeInOut._instance.GoScene("pressStart 1");
    }

}
