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

                Collectable item = other.gameObject.GetComponent<Collectable>();
                GameManager._instance.GetItem(item.type);
                Destroy(other.gameObject);

                break;

            case "Danger":

                GameManager._instance.Die();

                break;
        }
    }

}
