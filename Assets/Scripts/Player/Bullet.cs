using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IRestartLevelElement
{
    [SerializeField] float bulletSpeed = 20;
    [SerializeField] ParticleSystem explosionParticles;
    CanonShoot canonShoot;
    Rigidbody rb;
    Vector3 direction;

    [Header("Decal")]
    [SerializeField] GameObject decalExplosion;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float decalOffsetForward = 0.5f;

    // Start is called before the first frame update

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canonShoot = FindObjectOfType<CanonShoot>();
        direction = canonShoot.CanonForward;

        GameController.GetGameController().AddRestartLevelElement(this);
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
            if(whatIsGround == (whatIsGround | (1 << collision.gameObject.layer)))
            {
                GameObject decal = GameObject.Instantiate(decalExplosion);
                decal.transform.rotation = Quaternion.LookRotation(collision.GetContact(0).normal);
                decal.transform.position = collision.GetContact(0).point + decal.transform.forward.normalized * decalOffsetForward;
            }
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().cannonballHit, transform.position);
            ParticleSystem explosionParticle = GameObject.Instantiate(explosionParticles, transform.position, transform.rotation);
            explosionParticle.Play();
            Destroy(explosionParticle, 3);
            Destroy(this.gameObject);
        }
    }

    public void Restart()
    {
        Destroy(this.gameObject);
    }
}
