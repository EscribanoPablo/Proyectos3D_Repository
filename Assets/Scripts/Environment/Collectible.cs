using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //a�adir a la UI y al game controller
            gameObject.SetActive(false);
            FindObjectOfType<HudController>().CollectibleTaken();
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().GrabCollectibleSound[Random.Range(0, FindObjectOfType<AudioManager>().GrabCollectibleSound.Count)], transform.position);
        }
    }
}