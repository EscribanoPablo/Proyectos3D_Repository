using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : Obstacles
{
    [SerializeField]
    UnityEvent m_Event;

    bool doorOpened = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == BULLET_TAG && !doorOpened)
        {
            if (Random.Range(0, 2) == 0)
                FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientClapsSounds);

            m_Event.Invoke();
            doorOpened = true;
        }
    }
}
