using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonGrabbedDetector : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (other.GetComponent<PlayerMovement>().GetIfGrounded())
                other.GetComponent<PlayerMovement>().CannonGrabbed(gameObject);
        }
    }
}
