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
    private float invulnerableCounter = 0;
    [SerializeField]
    private float invulnerableTime = 1.0f;
    [SerializeField]
    private float noInputsTime = 0.3f;

    private float restartingCounter = 0;
    [SerializeField]
    private float restartPressedTime = 2.0f;

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
            invulnerableCounter += 1.0f * Time.deltaTime;
            if (invulnerableCounter >= invulnerableTime)
            {
                gotHit = false;
                invulnerableCounter = 0;
            }
            else if(invulnerableCounter >= noInputsTime && currentLifes > 0)
                playerInputs.enabled = true;
        }

        if (playerInputs.actions["Restart"].IsPressed())
        {
            if (restartingCounter >= restartPressedTime)
            {
                restartingCounter = 0.0f;

                currentLifes = 0;
                playerInputs.enabled = false;
                CheckHealth();
                gotHit = true;
            }
            else
                restartingCounter += 1.0f * Time.deltaTime;
        }
        else
            restartingCounter = 0.0f;
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
    }

    IEnumerator StartDeath()
    {
        playerAnimator.SetTrigger("Death");
        
        audioManager.SetPlaySfx(audioManager.DieSound, transform.position);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "TutorialLevel_Cat")
        {
            audioManager.StopMusic(FindObjectOfType<AudioManager>().instanceGameSong);
            audioManager.PlayCircusMasterAudio(FindObjectOfType<AudioManager>().instanceLevelDeathSound);
            audioManager.SetPlaySfx(audioManager.ambientLaughtsSounds);
        }
        
        yield return new WaitForSeconds(0.8f);

        Die();
    }

    private void Die()
    {   
        hudController.RestartLifes();
        currentLifes = startLifes;
        GameController.GetGameController().RestartLevelElement();
        playerRigidBody.velocity = Vector3.zero;
    }
}

