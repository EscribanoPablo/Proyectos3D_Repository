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

    private void Start()
    {
        currentLifes = startLifes;
        playerRigidBody = GetComponent<Rigidbody>();
        hudController = FindObjectOfType<HudController>();
        playerInputs = GetComponent<PlayerInput>();
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
                playerInputs.enabled = false;
                AddKnockback(pointOfImpact);
                currentLifes--;
                hudController.LifeLost(currentLifes);
                CheckHealth();
                gotHit = true;

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

    public void AddKnockback(Vector3 pointOfImpact)
    {
        Vector3 knockbackDirection = transform.position - pointOfImpact;
        //if (knockbackDirection.y < 0)
            knockbackDirection.y = 0.5f;
        knockbackDirection.Normalize();
        playerRigidBody.AddForce(knockbackDirection * knockbackImpulse, ForceMode.Impulse);
    }

    private void CheckHealth()
    {
        if (currentLifes <= 0)
        {
            currentLifes = 0;
            Die();
        }
        else if (currentLifes > startLifes)
        {
            currentLifes = startLifes;
        }
    }

    public void EnterDeathZone()
    {
        Die();
        //podemos o quitarle vida y respawnear o que tenga que volver a empezar de nuevo, preguntar jowy
        //GameController.GetGameController().RestartLevelElment();
        //RESPAWN
        //playerRigidBody.velocity = Vector3.zero;
        //hudController.RestartLifes();
        //currentHearts = startHearts;
    }

    private void Die()
    {
        playerRigidBody.velocity = Vector3.zero;
        hudController.RestartLifes();
        currentLifes = startLifes;
        GameController.GetGameController().RestartLevelElment();
    }
}

