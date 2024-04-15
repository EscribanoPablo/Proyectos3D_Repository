using UnityEngine;

public class FallingPlatform : Traps
{
    [SerializeField] private float timeToVanish;
    private float timer;

    private bool playerTouched = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerTouched)
        {
            timer += Time.deltaTime;
            if (timer >= timeToVanish)
            {
                timer = 0;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == PLAYER_TAG)
        {
            playerTouched = true;
        }
    }
}
