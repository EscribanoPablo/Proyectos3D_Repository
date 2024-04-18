using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : Traps
{
    [SerializeField]
    Transform pivotTransform;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == PLAYER_TAG)
        {
            collision.transform.SetParent(pivotTransform);
            collision.transform.rotation = new Quaternion(0, collision.transform.rotation.y, 0, collision.transform.rotation.w);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == PLAYER_TAG)
        {
            collision.transform.rotation = new Quaternion(0, collision.transform.rotation.y, 0, collision.transform.rotation.w);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == PLAYER_TAG)
        {
            collision.transform.SetParent(null);
        }
    }
}
