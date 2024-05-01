using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int startHearts;
    private int currentHearts;
    [SerializeField] bool DEV_INVINCIBLE;

    private Rigidbody rb;
    [SerializeField] float knockbackImpulse;

    private void Start()
    {
        currentHearts = startHearts;
        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(Vector3 pointOfImpact)
    {
        if (!DEV_INVINCIBLE)
        {
            AddKnockback(pointOfImpact);
            currentHearts --;
            CheckHealth();
            Debug.Log("Player current health = " + currentHearts);

            //tiempo de invencibilidad mientras recibe daño??
        }
    }

    public void TakeDamage()
    {
        if (!DEV_INVINCIBLE)
        {
            currentHearts--;
            CheckHealth();
            Debug.Log("Player current health = " + currentHearts);

            //tiempo de invencibilidad mientras recibe daño??
        }
    }

    public void AddKnockback(Vector3 pointOfImpact)
    {
        Vector3 knockbackDirection = transform.position - pointOfImpact;
        knockbackDirection.Normalize();
        rb.AddForce(knockbackDirection * knockbackImpulse, ForceMode.Impulse);
    }

    private void CheckHealth()
    {
        if (currentHearts <= 0)
        {
            Die();
            currentHearts = 0;
        }
        else if (currentHearts > startHearts)
        {
            currentHearts = startHearts;
        }
    }

    public void EnterDeathZone()
    {
        TakeDamage();
        //podemos o quitarle vida y respawnear o que tenga que volver a empezar de nuevo, preguntar jowy
        GameController.GetGameController().RestartLevelElment();
        //RESPAWN
        rb.velocity = Vector3.zero;
    }

    private void Die()
    {
        Debug.Log("Player DEAD");
        //respawn y recargar escena
    }
}

