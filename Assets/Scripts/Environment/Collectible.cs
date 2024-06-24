using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //añadir a la UI y al game controller
            gameObject.SetActive(false);
            FindObjectOfType<HudController>().CollectibleTaken();
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().GrabCollectibleSound, transform.position);

            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().SetTrigger("Celebrate");

            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientClapsSounds);
            FindObjectOfType<AudioManager>().PlayCircusMasterAudio(FindObjectOfType<AudioManager>().instanceLevelCollectibleSound);
        }
    }
}
