using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackTrap : Traps
{
    [SerializeField] float knockBackImpulse; 
    PlayerMovement player;
    [SerializeField] GameObject damageParticles;
    [SerializeField] GameObject childrenDamageParticles;    
    [Range(0,1)]
    [SerializeField] float stunTime;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == PLAYER_TAG)
        {
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().punchTrapHitSound);

            KnockbackHandler playerKnockbackHandler = collision.gameObject.GetComponent<KnockbackHandler>();
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
        }
    }
}
