using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaPresiónPinchos : Traps
{
    [SerializeField] private float damage;
    [SerializeField] private float timeBetweenChangeState;
    private float timer;
    //private bool spikesHidden;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenChangeState)
        {
            timer = 0;
            ChangeState();
        }
    }

    private void DoDamage()
    {
        //player gets damaged
    }

    private void ChangeState()
    {
        //spikes animation /appear o disappear
        // (de)activate hit collider
    }
}
