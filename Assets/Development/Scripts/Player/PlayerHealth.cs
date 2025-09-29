using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int startExtraLifes;
    private int currentExtraLifes;
    [SerializeField] private int maxHealth;
    private int currentHealth;
    [SerializeField] bool DEV_INVINCIBLE;

    private Rigidbody playerRigidBody;
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
        currentHealth = maxHealth;
        currentExtraLifes = startExtraLifes;
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
            else if(invulnerableCounter >= noInputsTime && currentHealth > 0)
                playerInputs.enabled = true;
        }

        if (playerInputs.actions["Restart"].IsPressed())
        {
            if (restartingCounter >= restartPressedTime)
            {
                restartingCounter = 0.0f;

                currentHealth = 0;
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
                currentHealth--;

                hudController.LifeLost(currentHealth);
                CheckHealth();
                gotHit = true;
                playerInputs.enabled = false;
                GetComponent<KnockbackHandler>().ApplyKnockback(pointOfImpact);

                damageParticles.SetActive(true);
                ParticleSystem particles = damageParticles.GetComponent<ParticleSystem>();
                ParticleSystem childrenParticles = childrenDamageParticles.GetComponent<ParticleSystem>();
                childrenParticles.Emit(10);
                particles.Emit(10);
            }
        }
    }

    public void HealLife()
    {
        //A lo mejor hacer que puedas curarte solo un toque, mirar a futuro si habran items de estos por el mapa

        currentHealth = maxHealth;
        hudController.RestartLifes();
    }

    public void AddExtraLife(int lifesAdded)
    {
        currentExtraLifes += lifesAdded;
        hudController.SetExtraLifesNumber(currentExtraLifes);
    }

    //public void AddKnockback(Vector3 pointOfImpact, float knockbackImpulseAded)
    //{
    //    Vector3 knockbackDirection = transform.position - pointOfImpact;
    //    float impulse = knockbackImpulse + knockbackImpulseAded;
    //    //if (knockbackDirection.y < 0)
    //        knockbackDirection.y = 0.5f;
    //    knockbackDirection.Normalize();
    //    playerRigidBody.AddForce(knockbackDirection.normalized * impulse, ForceMode.Impulse);
    //}

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;

            StartCoroutine(StartDeath());
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            
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
        currentHealth--;
        hudController.LifeLost(currentHealth);
        CheckHealth();
        gotHit = true;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StartDeath();
        }
        else
            GetComponent<PlayerMovement>().ReturnFromDeathZone();

        //StartCoroutine(StartDeath());
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

        currentExtraLifes--;
        hudController.SetExtraLifesNumber(currentExtraLifes);

        if (currentExtraLifes > 0)
            Die();
        else
        {
            //Hacer que te envie al level selector de nuevo
            //GameObject.FindObjectOfType<PlayTransition>().GoBlack(false, SceneToGo.LevelSelector);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Die()
    {   
        hudController.RestartLifes();
        currentHealth = maxHealth;
        GameController.GetGameController().RestartLevelElement();
        playerRigidBody.velocity = Vector3.zero;
    }
}

