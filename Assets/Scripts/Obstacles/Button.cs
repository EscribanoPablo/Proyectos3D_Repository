using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : Obstacles
{
    [SerializeField]
    UnityEvent m_Event;
    
    [SerializeField]
    Animation buttonAnimations;
    [SerializeField]
    AnimationClip buttonPressedAnimation;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == PLAYER_TAG) //aqui tiene que detecta la bala
        {
            m_Event.Invoke();
            buttonAnimations.Play(buttonPressedAnimation.name);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
