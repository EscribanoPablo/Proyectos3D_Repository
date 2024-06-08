using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CanonShoot : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] int shootButton;

    PlayerInput playerInput;
    PlayerMovement playerMovement;

    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] GameObject canonBoomParticles;
    [SerializeField] GameObject canonParticles;

    [SerializeField] float nextTimeFire = 1f;
    public float currentTimeShoot { get; set; }

    public Vector3 CanonForward => canonForward;
    private Vector3 canonForward;

    private AudioManager audioManager;
    [SerializeField]
    private Animator playerAnimator;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
        canonParticles.SetActive(false);
        canonBoomParticles.SetActive(false);

    }

    private void Update()
    {
        currentTimeShoot += Time.deltaTime;
        if (Time.timeScale == 1)
        {
            if (/*Input.GetMouseButton(shootButton)*/playerInput.actions["Shoot"].WasPressedThisFrame() && currentTimeShoot >= nextTimeFire)
            {
                StartCoroutine(Shoot());
                ShootBullet(spawnPosition.position);
                
                playerAnimator.SetTrigger("Shoot");
            }
            else if (/*Input.GetKeyDown(KeyCode.LeftShift)*/playerInput.actions["Dash"].WasPressedThisFrame()) /////////////////////// ARREGLAR
            {
                canonForward = -transform.forward;
            }
            else if (playerMovement.DoubleJump)
            {
                if (/*Input.GetKeyDown(KeyCode.Space)*/playerInput.actions["Jump"].WasPressedThisFrame())
                {
                    canonForward = Vector3.down;
                }
            }
        }

    }

    IEnumerator Shoot()
    {
        audioManager.SetPlaySfx(audioManager.ShootSound, 0.5f, transform.position);
        currentTimeShoot = 0;
        canonForward = transform.forward;
        SpawnCanonParticles();
        playerMovement.canJump = false;
        playerMovement.canDash = false;
        yield return new WaitForSeconds(0.25f);
        playerMovement.canJump = true;
        if (!playerMovement.IsDashing)
        {
            playerMovement.canDash = true;
        }

    }

    public void SpawnCanonParticles()
    {
        canonBoomParticles.SetActive(true);
        canonBoomParticles.GetComponent<ParticleSystem>().Play();

        canonParticles.SetActive(true);
        canonParticles.GetComponent<ParticleSystem>().Play();
    }

    public void ShootBullet(Vector3 position)
    {
        GameObject _bullet = Instantiate(bulletPrefab, position, bulletPrefab.transform.rotation);
    }
}
