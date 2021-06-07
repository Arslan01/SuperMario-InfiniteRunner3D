using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsManager : MonoBehaviour
{
    public GameObject[] props;
    void Start()
    {
        foreach(GameObject g in props)
        {
            g.SetActive(false);
        }

        props[Random.Range(0, props.Length)].SetActive(true);
    }

}
