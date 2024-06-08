using UnityEngine;

public class AmbientSounds : MonoBehaviour, IRestartLevelElement
{
    private bool eventTriggered = false;

    private void Start()
    {
        GameController.GetGameController().AddRestartLevelElement(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!eventTriggered)
        {
            if (Random.Range(0, 3) == 0)
                FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientLaughtsSounds);

            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientClapsSounds);

            eventTriggered = true;
        }
    }

    public void Restart()
    {
        eventTriggered = false;
    }
}
