using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            Debug.Log("Check Point");
            other.GetComponent<PlayerController>().SetRespawnPos(gameObject.transform);
            this.enabled = false;
        }
    }
}
