using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSpikes : Traps
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            //player.GotHit
        }
    }
}
