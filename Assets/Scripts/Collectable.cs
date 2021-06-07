using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public ItemType type;
    public bool isFollow;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PowerUp" && isFollow == false)
        {
            isFollow = true;
        }
    }

    private void Update()
    {
        if(type == ItemType.STAR && GameManager._instance.isPowerUp == true)
        {
            Destroy(this.gameObject);
        }

        if(isFollow == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameManager._instance.collectableTarget.position, (GameManager._instance.movementSpeed + 2) * Time.deltaTime);
        }
    }
}
