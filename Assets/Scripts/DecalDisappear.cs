using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalDisappear : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float timeToDisappear = 3f;
    [SerializeField] float opacitySpeed = 1f;
    DecalProjector decalProjector;
    float timer = 0;

    void Start()
    {
        decalProjector = GetComponent<DecalProjector>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToDisappear)
        {
            decalProjector.fadeFactor = decalProjector.fadeFactor - opacitySpeed * Time.deltaTime;
        }
        if(decalProjector.fadeFactor <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
