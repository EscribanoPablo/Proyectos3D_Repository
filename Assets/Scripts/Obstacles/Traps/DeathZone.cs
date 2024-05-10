using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : Traps
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == PLAYER_TAG)
        {
            Debug.Log("player should die");
            other.gameObject.GetComponent<PlayerHealth>().EnterDeathZone();
        }
    }
}
