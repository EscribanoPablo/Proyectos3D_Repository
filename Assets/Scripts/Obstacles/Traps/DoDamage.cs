using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : Traps
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == PLAYER_TAG)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(transform.position);

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == PLAYER_TAG)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(transform.position);

        }
    }
}
