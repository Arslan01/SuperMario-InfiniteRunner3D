using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text score;
    public Text distance;


    private bool isPlayAgain;
    public GameObject playAgainText;
    private bool isStart;
    // Start is called before the first frame update
    void Start()
    {
        score.text = "Score: " + PlayerPrefs.GetInt("score").ToString("N0");
        distance.text = "Distance: " + PlayerPrefs.GetInt("distance").ToString("N0") + "m";

        StartCoroutine("gameover");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown && isPlayAgain == true && isStart == false)
        {
            StartCoroutine("StartGame");
        }
    }

    IEnumerator gameover()
    {
        AudioManager._instance.PlayMusic(AudioManager._instance.GameOver, false);
        //yield return new WaitForSeconds(0.5f);
        //FadeInOut._instance.Fade();
        yield return new WaitUntil(() => !AudioManager._instance.Music.isPlaying);
        //FadeInOut._instance.Fade();
        //yield return new WaitForEndOfFrame();
        //yield return new WaitUntil(() => FadeInOut._instance.isFadeComplete);
        AudioManager._instance.PlayMusic(AudioManager._instance.PlayAgain, false);
        yield return new WaitForSeconds(0.5f);
        FadeInOut._instance.Fade();
        yield return new WaitForSeconds(2f);
        isPlayAgain = true;
    }

    IEnumerator StartGame()
    {
        isStart = true;
        FadeInOut._instance.Fade();
        AudioManager._instance.StartCoroutine("PlayMusicGameplay");
        yield return new WaitForEndOfFrame();

        yield return new WaitUntil(() => FadeInOut._instance.isFadeComplete);
        AudioManager._instance.PlayFX(AudioManager._instance.fxIntro);
        FadeInOut._instance.GoScene("Gameplay");
    }
}
