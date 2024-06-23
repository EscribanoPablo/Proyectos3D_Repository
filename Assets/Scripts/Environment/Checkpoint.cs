using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool checkpointGrabbed = false;
    [SerializeField]
    private GameObject spawnPosition;
    [SerializeField]
    private Animation checkpointAnimation;
    [SerializeField]
    private AnimationClip checkpointClip;
    [SerializeField] GameObject checkPointParticles;
    Animator animator;

    private void Start()
    {
        checkPointParticles.SetActive(false);
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player" && !checkpointGrabbed)
        {
            other.GetComponent<PlayerController>().SetRespawnPos(spawnPosition.transform);
            checkpointGrabbed = true;
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().SetTrigger("Celebrate");

            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "TutorialLevel")
                FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientClapsSounds);

            StartCoroutine(SpawnCheckPointParticles());

            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ActivateCheckPointSound, transform.position);
            checkpointAnimation.Play(checkpointClip.name);

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "TutorialLevel")
                FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().LevelCheckpointSound);
        }
    }

    IEnumerator SpawnCheckPointParticles()
    {

        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        float clipDuration = clipInfo.Length > 0 ? clipInfo[0].clip.length : 0f;

        yield return new WaitForSeconds(clipDuration);
        checkPointParticles.SetActive(true);
        ParticleSystem particleCheckPoint = checkPointParticles.GetComponent<ParticleSystem>();
        particleCheckPoint.Emit(40);

    }


}
