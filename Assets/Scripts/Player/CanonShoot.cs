using System;
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
    [SerializeField] GameObject canonParticles;

    [SerializeField] float fireRate = 0.5f;
    [SerializeField] float nextTimeFire = 0.2f;

    public Vector3 CanonForward => canonForward;
    private Vector3 canonForward;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
        canonParticles.SetActive(false);
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            if (/*Input.GetMouseButton(shootButton)*/playerInput.actions["Shoot"].WasPressedThisFrame() && Time.time >= nextTimeFire)
            {
                audioManager.SetPlaySfx(audioManager.ShootSound[Random.Range(0, audioManager.ShootSound.Count)], 0.5f);
                nextTimeFire = Time.time + fireRate;
                canonForward = transform.forward;

                canonParticles.SetActive(true);
                canonParticles.GetComponent<ParticleSystem>().Play();
                Shoot();
            }
            else if (/*Input.GetKeyDown(KeyCode.LeftShift)*/playerInput.actions["Dash"].WasPressedThisFrame()) /////////////////////// ARREGLAR
            {
                audioManager.SetPlaySfx(audioManager.DashSound, 0.5f);
                canonForward = -transform.forward;
                Debug.Log(canonForward);
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

    public void Shoot()
    {
        GameObject _bullet = Instantiate(bulletPrefab, spawnPosition.position, bulletPrefab.transform.rotation);
        Destroy(_bullet, 10);
    }
}
