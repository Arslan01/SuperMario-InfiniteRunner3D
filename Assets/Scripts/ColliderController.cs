using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "Collectable":
                break;

            case "Danger":

                GameManager._instance.Die();

                break;
        }
    }

}
