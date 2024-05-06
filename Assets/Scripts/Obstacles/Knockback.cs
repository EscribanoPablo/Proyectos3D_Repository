using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : Traps
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == PLAYER_TAG)
        {
            collision.gameObject.GetComponent<PlayerHealth>().AddKnockback(transform.position);

        }
    }
}
