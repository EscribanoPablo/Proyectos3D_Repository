using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashingTrap : Traps
{
    [SerializeField] private List<Collider> CollidersList = new List<Collider>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.tag == PLAYER_TAG)
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().GetIfGrounded())
            {
                collision.transform.SetParent(null);
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();
                StartCoroutine(GotSmashed());
            }
            else
            {
                collision.gameObject.transform.SetParent(this.gameObject.transform);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.SetParent(null);
    }

    IEnumerator GotSmashed()
    {
        foreach(Collider coll in CollidersList)
        {
            coll.isTrigger = true;
        }

        yield return new WaitForSeconds(1f);

        foreach (Collider coll in CollidersList)
        {
            coll.isTrigger = false;
        }
    }
}
