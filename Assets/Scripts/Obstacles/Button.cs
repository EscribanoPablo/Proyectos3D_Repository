using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : Obstacles
{
    [SerializeField]
    UnityEvent m_Event;
    
    //[SerializeField]
    //Animation buttonAnimations;
    //[SerializeField]
    //AnimationClip buttonPressedAnimation;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == BULLET_TAG)
        {
            m_Event.Invoke();
            //buttonAnimations.Play(buttonPressedAnimation.name);
            this.enabled = false;
        }
    }
}
