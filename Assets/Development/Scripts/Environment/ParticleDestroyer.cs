using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    ParticleSystem thisParticles;

    void Start()
    {
        thisParticles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (thisParticles.isStopped)
        {
            Destroy(gameObject);
        }
    }
}
