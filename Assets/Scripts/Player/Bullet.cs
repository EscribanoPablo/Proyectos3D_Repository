using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20;
    [SerializeField] ParticleSystem explosionParticles;
    CanonShoot canonShoot;
    Rigidbody rb;
    Vector3 direction;
    // Start is called before the first frame update
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canonShoot = FindObjectOfType<CanonShoot>();
        direction = canonShoot.CanonForward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canonShoot != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Breakable")
        {
        }
        else
        {
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().cannonballHit, transform.position);
            ParticleSystem explosionParticle = GameObject.Instantiate(explosionParticles, transform.position, transform.rotation);
            explosionParticle.Play();
            Destroy(explosionParticle, 3);
            Destroy(this.gameObject);
        }
    }


}
