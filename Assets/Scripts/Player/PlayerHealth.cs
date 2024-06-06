using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int startLifes;
    private int currentLifes;
    [SerializeField] bool DEV_INVINCIBLE;

    private Rigidbody playerRigidBody;
    [SerializeField] float knockbackImpulse;
    private PlayerInput playerInputs;

    private HudController hudController;

    private bool gotHit = false;
    private float timeCounter = 0;
    [SerializeField]
    private float invulnerableTime = 1.0f;
    [SerializeField]
    private float noInputsTime = 0.3f;

    private AudioManager audioManager;
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField] GameObject damageParticles;
    [SerializeField] GameObject childrenDamageParticles;


    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        currentLifes = startLifes;
        playerRigidBody = GetComponent<Rigidbody>();
        hudController = FindObjectOfType<HudController>();
        playerInputs = GetComponent<PlayerInput>();

        damageParticles.SetActive(false);
    }

    private void Update()
    {
        if (gotHit)
        {
            timeCounter += 1.0f * Time.deltaTime;
            if (timeCounter >= invulnerableTime)
            {
                gotHit = false;
                timeCounter = 0;
            }
            else if(timeCounter >= noInputsTime)
                playerInputs.enabled = true;
        }
    }

    public void TakeDamage(Vector3 pointOfImpact)
    {
        if (!DEV_INVINCIBLE)
        {
            if (!gotHit)
            {
                currentLifes--;

                hudController.LifeLost(currentLifes);
                CheckHealth();
                gotHit = true;
                playerInputs.enabled = false;
                AddKnockback(pointOfImpact, knockbackImpulse);

                damageParticles.SetActive(true);
                ParticleSystem particles = damageParticles.GetComponent<ParticleSystem>();
                ParticleSystem childrenParticles = childrenDamageParticles.GetComponent<ParticleSystem>();
                childrenParticles.Emit(10);
                particles.Emit(10);

                //Debug.Log("Player current health = " + currentLifes);
            }
        }
    }

    public void TakeDamage()
    {
        if (!DEV_INVINCIBLE)
        {
            if (!gotHit)
            {
                playerInputs.enabled = false;
                currentLifes--;
                hudController.LifeLost(currentLifes);
                CheckHealth();
                gotHit = true;

                //Debug.Log("Player current health = " + currentLifes);
            }
        }
    }

    public void AddKnockback(Vector3 pointOfImpact, float knockbackImpulseAded)
    {
        Vector3 knockbackDirection = transform.position - pointOfImpact;
        float impulse = knockbackImpulse + knockbackImpulseAded;
        //if (knockbackDirection.y < 0)
            knockbackDirection.y = 0.5f;
        knockbackDirection.Normalize();
        playerRigidBody.AddForce(knockbackDirection.normalized * impulse, ForceMode.Impulse);
    }

    private void CheckHealth()
    {
        if (currentLifes <= 0)
        {
            currentLifes = 0;

            StartCoroutine(StartDeath());
        }
        else if (currentLifes > startLifes)
        {
            currentLifes = startLifes;
            
            audioManager.SetPlaySfx(audioManager.RecieveDamageSound, transform.position);
            playerAnimator.SetTrigger("Hit");
        }
        else
        {
            audioManager.SetPlaySfx(audioManager.RecieveDamageSound, transform.position);
            playerAnimator.SetTrigger("Hit");
        }
    }

    public void EnterDeathZone()
    {
        StartCoroutine(StartDeath());
        //podemos o quitarle vida y respawnear o que tenga que volver a empezar de nuevo, preguntar jowy
        //GameController.GetGameController().RestartLevelElment();
        //RESPAWN
        //playerRigidBody.velocity = Vector3.zero;
        //hudController.RestartLifes();
        //currentHearts = startHearts;
    }

    IEnumerator StartDeath()
    {
        playerAnimator.SetTrigger("Death");
        audioManager.SetPlaySfx(audioManager.DieSound, transform.position);

        audioManager.SetPlaySfx(audioManager.cirucsMasterLaughSound, transform.position);
        audioManager.SetPlaySfx(audioManager.ambientLaughtsSounds, transform.position);
        
        yield return new WaitForSeconds(1f);
        FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceGameSong);

        Die();
    }

    private void Die()
    {
        playerAnimator.SetTrigger("Respawn");
        audioManager.SetPlaySfx(audioManager.RespawnSound, transform.position);
        
        hudController.RestartLifes();
        currentLifes = startLifes;
        GameController.GetGameController().RestartLevelElment();
        playerRigidBody.velocity = Vector3.zero;
    }
}

