using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : Traps
{

    PlayerMovement player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == PLAYER_TAG)
        {
            player.GetComponent<PlayerMovement>().playerControllerEnabled = false;
            collision.gameObject.GetComponent<PlayerHealth>().AddKnockback(transform.position);
            StartCoroutine(PunchPlayer());
        }
    }

    private IEnumerator PunchPlayer()
    {
        //Rigidbody playerRb = player.GetComponent<PlayerMovement>().rigidBody;
        //playerRb.AddForce(playerRb.velocity * 2, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerMovement>().playerControllerEnabled = true; 

    }
}
