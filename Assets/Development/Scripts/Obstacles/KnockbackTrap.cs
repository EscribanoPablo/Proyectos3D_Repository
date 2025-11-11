using FMODUnity;
using UnityEngine;

public class KnockbackTrap : Traps
{
    [SerializeField] float knockBackImpulse;
    PlayerMovement player;
    [SerializeField] GameObject damageParticles;
    [SerializeField] GameObject childrenDamageParticles;
    [Range(0, 1)]
    [SerializeField] float stunTime;

    [SerializeField] private EventReference audioClip;
    [SerializeField] private bool dealsDamage = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == PLAYER_TAG)
            ManageKnockback();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == PLAYER_TAG)
            ManageKnockback();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PLAYER_TAG)
            ManageKnockback();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == PLAYER_TAG)
            ManageKnockback();
    }

    private void ManageKnockback()
    {
        if (!player.GetComponent<PlayerHealth>().gotHit)
        {
            if (dealsDamage)
                player.GetComponent<PlayerHealth>().TakeDamage(transform.position);

            player.GetComponent<PlayerHealth>().gotHit = true;

            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().punchTrapHitSound);

            KnockbackHandler playerKnockbackHandler = player.gameObject.GetComponent<KnockbackHandler>();
            playerKnockbackHandler.ApplyKnockback(transform.position, knockBackImpulse);
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.playerControllerEnabled = false;
            StartCoroutine(playerMovement.GradientPlayerMassAfterKnockbackImpact(1));
            if (damageParticles != null && childrenDamageParticles != null)
            {
                ParticleSystem particles = damageParticles.GetComponent<ParticleSystem>();
                ParticleSystem childrenParticles = childrenDamageParticles.GetComponent<ParticleSystem>();
                childrenParticles.Emit(5);
                particles.Emit(5);
            }

            if(!audioClip.IsNull)
                FindObjectOfType<AudioManager>().SetPlaySfx(audioClip, 0.5f, transform.position);
        }
    }
}
