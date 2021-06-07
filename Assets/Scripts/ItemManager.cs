using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] coins;
    public GameObject star;

    void Start()
    {
        if (star != null)
        {
            star.SetActive(false);
        }


            foreach (GameObject e in coins)
        {
            e.SetActive(false);
        }

    coins[Random.Range(0, coins.Length)].SetActive(true);

        if(star != null)
        {
            if(Random.Range(0,100) <GameManager._instance.percPowerUp)
            {
                star.SetActive(true);
            }
        }
    }

}
