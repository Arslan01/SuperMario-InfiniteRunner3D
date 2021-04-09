using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    public Transform parent;
    public bool isActive;

    private void Update()
    {
        if(isActive == true)
        {
            if(Vector3.Distance(transform.position, GameManager._instance.player.position) > GameManager._instance.blockSize)
            {
                Destroy(parent.gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && isActive == false)
        {
            isActive = true;
            GameManager._instance.NewBlock();
        }
    }

}
