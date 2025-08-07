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

    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private float maxLineLength = 20f;
    [SerializeField] private float lineGrowTime = 1f;
    [SerializeField] private int linePoints = 5;

    private float lineTimer = 0f;
    private bool isPressing = false;

    public Vector3 CanonForward => canonForward;
    private Vector3 canonForward;

    [SerializeField] private float autoAimRange = 20f;
    [SerializeField] private float autoAimAngle = 10f;
    [SerializeField] private LayerMask isEnemyLayer;
    [SerializeField] private LayerMask isObstacleLayer;

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
            if (playerMovement.GetIfGrounded())
            {
                if (playerInput.actions["Aim"].IsPressed() && !isPressing && currentTimeShoot >= nextTimeFire)
                {
                    playerMovement.ReducePlayerMovement();
                    isPressing = true;

                    playerAnimator.SetBool("IsAiming", true);
                    //aimLine.enabled = true;
                }

                if (playerInput.actions["Aim"].IsPressed() && isPressing && currentTimeShoot >= nextTimeFire)
                {
                    lineTimer += Time.deltaTime;
                    float t = Mathf.Clamp01(lineTimer / lineGrowTime);
                    int activePoints = Mathf.FloorToInt(t * linePoints);

                    Vector3 flatForward = transform.forward;
                    flatForward.y = 0f;
                    flatForward.Normalize();

                    aimLine.positionCount = activePoints;

                    for (int i = 0; i < activePoints; i++)
                    {
                        float segmentLength = (i / (float)(linePoints - 1)) * maxLineLength;
                        Vector3 point = spawnPosition.position + flatForward * segmentLength;
                        aimLine.SetPosition(i, point);
                    }
                }

                if (playerInput.actions["Aim"].WasReleasedThisFrame() && currentTimeShoot >= nextTimeFire)
                {
                    playerMovement.ResetPlayerMovement();
                    isPressing = false;
                    //aimLine.enabled = false;

                    lineTimer = 0f;
                    aimLine.positionCount = 0;

                    playerAnimator.SetBool("IsAiming", false);
                }
            }
            else 
            {
                if (isPressing) 
                {
                    playerMovement.ResetPlayerMovement();
                    isPressing = false;
                    //aimLine.enabled = false;

                    lineTimer = 0f;
                    aimLine.positionCount = 0;
                    playerAnimator.SetBool("IsAiming", false);
                }
            }

            if(!isPressing)
                playerAnimator.SetBool("IsAiming", false);

            if (playerInput.actions["Shoot"].WasPressedThisFrame() && currentTimeShoot >= nextTimeFire)
            {
                StartCoroutine(Shoot());
                ShootBullet(spawnPosition.position, true);
                
                playerAnimator.SetTrigger("Shoot");

                playerMovement.ResetPlayerMovement();
                isPressing = false;
                //aimLine.enabled = false;

                lineTimer = 0f;
                aimLine.positionCount = 0;
            }
            else if (playerInput.actions["Dash"].WasPressedThisFrame()) 
            {
                canonForward = -transform.forward;
            }
            else if (playerMovement.DoubleJump)
            {
                if (playerInput.actions["Jump"].WasPressedThisFrame())
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

    public void ShootBullet(Vector3 position, bool isNormalShoot)
    {
        if (isNormalShoot) 
        {
            Vector3 shootDirection = BulletAimbot(position);
            GameObject _bullet = Instantiate(bulletPrefab, position, Quaternion.LookRotation(shootDirection));

            Bullet bullet = _bullet.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetDirection(shootDirection);
            }
        }
        else 
        {
            GameObject _bullet = Instantiate(bulletPrefab, position, bulletPrefab.transform.rotation);

            Bullet bullet = _bullet.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetDirection(CanonForward);
            }
        }
    }

    private Vector3 BulletAimbot(Vector3 position) 
    {
        int rayCount = 15;
        float halfAngle = autoAimAngle;
        float step = (halfAngle * 2f) / (rayCount - 1);

        Transform bestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 bestDirection = transform.forward;

        int combinedLayerMask = isEnemyLayer | isObstacleLayer;

        for (int i = 0; i < rayCount; i++)
        {
            float angleOffset = -halfAngle + step * i;
            Quaternion rotation = Quaternion.AngleAxis(angleOffset, Vector3.up);
            Vector3 rayDirection = rotation * transform.forward;

            rayDirection.y = 0;
            rayDirection.Normalize();

            if (Physics.Raycast(position, rayDirection, out RaycastHit hit, autoAimRange, combinedLayerMask))
            {
                // Si choca con enemigo
                if (((1 << hit.collider.gameObject.layer) & isEnemyLayer) != 0)
                {
                    float distance = hit.distance;
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        bestTarget = hit.transform;
                        bestDirection = rayDirection;
                    }

                    Debug.DrawRay(position, rayDirection * autoAimRange, Color.green, 1f);
                }
                else
                {
                    Debug.DrawRay(position, rayDirection * autoAimRange, Color.yellow, 1f);
                }
            }
            else
            {
                Debug.DrawRay(position, rayDirection * autoAimRange, Color.red, 0.5f);
            }
        }

        return bestDirection;
    }

    public bool GetIsPressing()
    {
        return isPressing;
    }

}
