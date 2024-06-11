using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.15f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
