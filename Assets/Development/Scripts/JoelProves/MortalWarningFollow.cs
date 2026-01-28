using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarWarningFollow : MonoBehaviour
{
    public Transform sonJackson;      // El Jackson hijo
    public float groundY = 0f; // Altura del suelo

    void Update()
    {
        if (sonJackson == null) return;

        transform.position = new Vector3(
            sonJackson.position.x,
            groundY,
            sonJackson.position.z
        );
    }
}
