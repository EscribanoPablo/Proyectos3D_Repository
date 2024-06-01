using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool checkpointGrabbed = false;
    [SerializeField]
    private Animation checkpointAnimation;
    [SerializeField]
    private AnimationClip checkpointClip;
    [SerializeField] GameObject checkPointParticles;

    private void Start()
    {
        checkPointParticles.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player" && !checkpointGrabbed)
        {
            other.GetComponent<PlayerController>().SetRespawnPos(gameObject.transform);
            checkpointGrabbed = true;

            StartCoroutine(SpawnCheckPointParticles());

            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ActivateCheckPointSound, transform.position);
            checkpointAnimation.Play(checkpointClip.name);
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().circusMasterHeySound);
        }
    }

    IEnumerator SpawnCheckPointParticles()
    {
        yield return new WaitForSeconds(0.75f);
        checkPointParticles.SetActive(true);
        ParticleSystem particleCheckPoint = checkPointParticles.GetComponent<ParticleSystem>();
        particleCheckPoint.Emit(40);

    }
}
