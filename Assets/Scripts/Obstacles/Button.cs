using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : Obstacles
{
    public UnityEvent m_Event;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == PLAYER_TAG)
            m_Event.Invoke();
    }
}
