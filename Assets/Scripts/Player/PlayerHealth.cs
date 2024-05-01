using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int startLifes;
    private int currentLifes;
    [SerializeField] bool DEV_INVINCIBLE;

    private Rigidbody playerRigidBody;
    [SerializeField] float knockbackImpulse;

    private HudController hudController;

    private void Start()
    {
        currentLifes = startLifes;
        playerRigidBody = GetComponent<Rigidbody>();
        hudController = FindObjectOfType<HudController>();
    }

    public void TakeDamage(Vector3 pointOfImpact)
    {
        if (!DEV_INVINCIBLE)
        {
            AddKnockback(pointOfImpact);
            currentLifes --;
            hudController.LifeLost(currentLifes);
            CheckHealth();
            Debug.Log("Player current health = " + currentLifes);

            //tiempo de invencibilidad mientras recibe daño??
        }
    }

    public void TakeDamage()
    {
        if (!DEV_INVINCIBLE)
        {
            currentLifes--;
            hudController.LifeLost(currentLifes);
            CheckHealth();
            Debug.Log("Player current health = " + currentLifes);

            //tiempo de invencibilidad mientras recibe daño??
        }
    }

    public void AddKnockback(Vector3 pointOfImpact)
    {
        Vector3 knockbackDirection = transform.position - pointOfImpact;
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

