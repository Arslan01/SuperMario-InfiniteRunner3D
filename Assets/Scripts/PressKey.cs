using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressKey : MonoBehaviour
{
    public Animator anim;
    private bool isStart;
    // Start is called before the first frame update
    void Start()
    {
        if (FadeInOut._instance != null)
        {
            FadeInOut._instance.Fade();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown && isStart == false)
        {
            StartCoroutine("StartGame");
        }        
    }

    IEnumerator StartGame()
    {
        isStart = true;
        anim.SetTrigger("StartGame");

        yield return new WaitForSeconds(4.5f);
        FadeInOut._instance.Fade();
        yield return new WaitForEndOfFrame();

        yield return new WaitUntil(() => FadeInOut._instance.isFadeComplete);
        FadeInOut._instance.GoScene("Gameplay");
    }
}
