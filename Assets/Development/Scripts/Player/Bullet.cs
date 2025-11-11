using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bullet : MonoBehaviour, IRestartLevelElement
{
    [SerializeField] float bulletSpeed = 20;
    [SerializeField] ParticleSystem explosionParticles;
    CanonShoot canonShoot;
    Rigidbody rb;
    Vector3 direction;
    CinemachineImpulseSource shakeSource;

    [Header("Decal")]
    [SerializeField] GameObject decalExplosion;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float decalOffsetForward = 0.5f;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canonShoot = FindObjectOfType<CanonShoot>();
        //direction = canonShoot.CanonForward;

        GameController.GetGameController().AddRestartLevelElement(this);

        StartCoroutine(DestroyBullet());

        shakeSource = GetComponent<CinemachineImpulseSource>();
        shakeSource.GenerateImpulseWithForce(0.1f);
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
            //GetComponent<SphereCollider>().isTrigger = true;
        }
        else
        {
            if(whatIsGround == (whatIsGround | (1 << collision.gameObject.layer)))
            {
                GameObject decal = GameObject.Instantiate(decalExplosion);
                decal.transform.rotation = Quaternion.LookRotation(collision.GetContact(0).normal);
                decal.transform.position = collision.GetContact(0).point + decal.transform.forward.normalized * decalOffsetForward;
            }
            shakeSource.GenerateImpulseWithForce(0.3f);

            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().cannonballHit, transform.position);
            ParticleSystem explosionParticle = GameObject.Instantiate(explosionParticles, transform.position, transform.rotation);
            explosionParticle.Play();
            Destroy(explosionParticle, 3);
            GameController.GetGameController().RemoveRestartLevelElement(this);
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DoorButton")
        {
            GetComponent<SphereCollider>().isTrigger = false;
        }
    }

    public void Restart()
    {
        GameController.GetGameController().RemoveRestartLevelElement(this);
        Destroy(this.gameObject);
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(10.0f);
        GameController.GetGameController().RemoveRestartLevelElement(this);
        Destroy(this.gameObject);
    }
}
