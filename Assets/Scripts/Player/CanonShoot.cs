using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CanonShoot : MonoBehaviour
{
    [Header("Inputs")]
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;

    [Header("References")]
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private GameObject canonBoomParticles;
    [SerializeField] private GameObject canonParticles;

    [Header("Shoot")]
    private Ability shootAbility;
    public float currentTimeShoot { get; set; }
    public Vector3 CanonForward => cannonForward;
    private Vector3 cannonForward;

    [Header("Aiming")]
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private float maxLineLength = 20f;
    [SerializeField] private float lineGrowTime = 1f;
    [SerializeField] private int linePoints = 5;

    private float lineTimer = 0f;
    public bool GetIfAiming() { return isAiming; }
    private bool isAiming = false;

    [Header("AutoAim")]
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
        shootAbility = playerMovement.GetAbility("Shoot");
        canonParticles.SetActive(false);
        canonBoomParticles.SetActive(false);
    }

    private void Update()
    {
        currentTimeShoot += Time.deltaTime;
        if (Time.timeScale == 1 && shootAbility.abilityData.isUnlocked)
        {
            HandleAiming();
            HandleCannonShoots();
        }

    }

    private void HandleAiming()
    {
        if (playerMovement.GetIfGrounded() && !playerMovement.GetIfCrouching())
        {
            if (playerInput.actions["Aim"].IsPressed() && !isAiming && currentTimeShoot >= shootAbility.abilityData.cooldown)
            {
                playerMovement.ReducePlayerMovement(4f, 10f);
                isAiming = true;

                playerAnimator.SetBool("IsAiming", true);
                //aimLine.enabled = true;
            }

            if (playerInput.actions["Aim"].IsPressed() && isAiming && currentTimeShoot >= shootAbility.abilityData.cooldown)
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

            if (playerInput.actions["Aim"].WasReleasedThisFrame() && currentTimeShoot >= shootAbility.abilityData.cooldown)
            {
                playerMovement.ResetPlayerMovement();
                isAiming = false;
                //aimLine.enabled = false;

                lineTimer = 0f;
                aimLine.positionCount = 0;

                playerAnimator.SetBool("IsAiming", false);
            }
        }
        else
        {
            if (isAiming)
            {
                playerMovement.ResetPlayerMovement();
                isAiming = false;
                //aimLine.enabled = false;

                lineTimer = 0f;
                aimLine.positionCount = 0;
                playerAnimator.SetBool("IsAiming", false);
            }
        }

        if (!isAiming)
            playerAnimator.SetBool("IsAiming", false);
    }

    private void HandleCannonShoots()
    {
        if (playerInput.actions["Shoot"].WasPressedThisFrame() && currentTimeShoot >= shootAbility.abilityData.cooldown)
        {
            StartCoroutine(Shoot());
            ShootBullet(spawnPosition.position, true, false);

            playerAnimator.SetTrigger("Shoot");

            playerMovement.ResetPlayerMovement();
            isAiming = false;
            //aimLine.enabled = false;

            lineTimer = 0f;
            aimLine.positionCount = 0;
        }
    }

    IEnumerator Shoot()
    {
        audioManager.SetPlaySfx(audioManager.ShootSound, 0.5f, transform.position);
        currentTimeShoot = 0;
        cannonForward = transform.forward;
        SpawnCanonParticles();
        playerMovement.GetAbility("Jump").canUse = false;
        playerMovement.GetAbility("DoubleJump").canUse = false;
        playerMovement.GetAbility("Dash").canUse = false;

        yield return new WaitForSeconds(0.25f);

        playerMovement.GetAbility("Jump").canUse = true;
        if (!playerMovement.GetIfDashing())
            playerMovement.GetAbility("Dash").canUse = true;

    }

    public void SpawnCanonParticles()
    {
        canonBoomParticles.SetActive(true);
        canonBoomParticles.GetComponent<ParticleSystem>().Play();

        canonParticles.SetActive(true);
        canonParticles.GetComponent<ParticleSystem>().Play();
    }

    public void ShootBullet(Vector3 position, bool isNormalShoot, bool isDoubleJump)
    {
        if (isNormalShoot) 
        {
            Vector3 shootDirection = BulletAimbot(position);
            GameObject _bullet = Instantiate(bulletPrefab, position, Quaternion.LookRotation(shootDirection));

            Bullet bullet = _bullet.GetComponent<Bullet>();
            if (bullet != null)
                bullet.SetDirection(shootDirection);
        }
        else 
        {
            if (isDoubleJump)
                cannonForward = Vector3.down;
            else
                cannonForward = -transform.forward;

            GameObject _bullet = Instantiate(bulletPrefab, position, bulletPrefab.transform.rotation);

            Bullet bullet = _bullet.GetComponent<Bullet>();
            if (bullet != null)
                bullet.SetDirection(cannonForward);
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
}
