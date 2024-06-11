using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerCircusMaster : MonoBehaviour
{

    Animation animationCircusMaster;
    [SerializeField] AnimationClip idleAnimation;
    [SerializeField] AnimationClip encenderAnimation;
    [SerializeField] AnimationClip olaAnimation;

    // Start is called before the first frame update
    void Start()
    {
        animationCircusMaster = GetComponent<Animation>();
        animationCircusMaster.Play(olaAnimation.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
