using System.Collections.Generic;
using UnityEngine;

public class CircusMasterMovement : MonoBehaviour
{
    Animation animationCircusMaster;
    [SerializeField] AnimationClip idleAnimation;
    [SerializeField] AnimationClip encenderAnimation;
    [SerializeField] AnimationClip olaAnimation;

    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float snapRotationFactor = 2;
    [SerializeField] List<GameObject> lights;
    private int currentLight;
    private float rotationDestination;

    private bool introducingScenario;
    private bool moving;
    private const float ROTATIONFORWARD_OFFSET = 180;


    // Start is called before the first frame update
    void Start()
    {
        animationCircusMaster = GetComponent<Animation>();
        animationCircusMaster.Play(idleAnimation.name);
        introducingScenario = true;
        currentLight = 0;

        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (introducingScenario)
        {
            TurningLightsOn();
        }
        else
        {
            // poner la ola cada 10 segundos o cuando llegue el player a un checkpoint
        }
    }

    private void TurningLightsOn()
    {
        if (rotationDestination == 0)
        {
            SetRotationDestination();
            moving = true;
        }

        if (moving)
        {
            float yRotationValue = Mathf.Lerp(transform.parent.transform.rotation.eulerAngles.y, rotationDestination, Time.deltaTime * rotateSpeed);
            //Debug.Log("Myangles: " + transform.rotation.eulerAngles.y + ", my new Y value: " + yRotationValue + ", my destination: " + rotationDestination);
            transform.parent.transform.rotation = Quaternion.Euler(0, yRotationValue, 0);
            //Debug.Log(transform.rotation.eulerAngles + "    , " + Quaternion.Euler(0, yRotationValue, 0));
            if (Snapping.Snap(rotationDestination, snapRotationFactor) == Snapping.Snap(transform.parent.transform.rotation.eulerAngles.y, snapRotationFactor))
            {
                moving = false;
                animationCircusMaster.CrossFade(encenderAnimation.name);
            }
        }

    }

    private void SetRotationDestination()
    {
        Vector3 direction = lights[currentLight].transform.position - transform.position;
        direction.y = transform.position.y;
        rotationDestination = Quaternion.LookRotation(direction).eulerAngles.y;
    }

    private void EncenderLuz()
    {
        lights[currentLight].SetActive(true);
    }

    private void SeguirAnimacion()
    {
        currentLight++;
        if (currentLight > lights.Count - 1)
        {
            introducingScenario = false;
            animationCircusMaster.CrossFadeQueued(olaAnimation.name);
        }
        else
        {
            moving = true;
            SetRotationDestination();
        }
        animationCircusMaster.CrossFadeQueued(idleAnimation.name);
    }
}

