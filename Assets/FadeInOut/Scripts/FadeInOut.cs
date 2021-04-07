using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut _instance;
    private Animator anim;
    public bool isFadeComplete;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Fade()
    {
        anim.SetTrigger("Fade");
    }

    void  FadeComplete(bool b)
    {
        isFadeComplete = b;
    }
}
