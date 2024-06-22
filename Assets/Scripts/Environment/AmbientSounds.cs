using UnityEngine;

public class AmbientSounds : MonoBehaviour, IRestartLevelElement
{
    private bool eventTriggered = false;

    [SerializeField]
    private bool isFireRing = false;

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

            if(isFireRing)
                FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().LevelFireRingSound);

            eventTriggered = true;
        }
    }

    public void Restart()
    {
        eventTriggered = false;
    }
}
