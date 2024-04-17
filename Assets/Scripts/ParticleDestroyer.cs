using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    ParticleSystem thisParticles;
    // Start is called before the first frame update
    void Start()
    {
        thisParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (thisParticles.isStopped)
        {
            Destroy(gameObject);
        }
    }
}
