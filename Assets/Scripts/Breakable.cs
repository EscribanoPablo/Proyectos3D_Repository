using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Breakable : Obstacles, IRestartLevelElement
{
    [SerializeField] GameObject wholeObject;
    [SerializeField] GameObject prefracturedObject;
    [SerializeField] string breakerTag;
    GameObject[] breakableCubes;
    [SerializeField] GameObject damageParticles;
    [SerializeField] GameObject childrenDamageParticles;

    [SerializeField] bool isPlatform;
    [SerializeField] float platformReappearTime;

    Vector3[] breakableStartPosition;
    Quaternion[] breakableStartRotation;

    [SerializeField] float timeToDisappear;
    float timer;

    [SerializeField] float boxExplosionForce;

    Vector3 startPositionParent;
    Quaternion startRotationParent;

    Rigidbody rigidBody;

    CanonShoot canonShoot;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        canonShoot = FindObjectOfType<CanonShoot>();

        startPositionParent = transform.position;
        startRotationParent = transform.rotation;

        SaveBreakablesPosition();

        wholeObject.SetActive(true);
        prefracturedObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == breakerTag)
        {

            wholeObject.SetActive(false);
            //prefracturedObject.transform.rotation = wholeObject.transform.rotation;
            prefracturedObject.SetActive(true);

            //prefracturedObject.transform.position = wholeObject.transform.position;
            for (int i = 0; i < prefracturedObject.transform.childCount - 1; i++)
            {
                //hacer que pille la dirección de la bala y añadirle velocidad
                breakableCubes[i].GetComponent<Rigidbody>().velocity += canonShoot.CanonForward * boxExplosionForce;
            }
            GetComponent<Collider>().enabled = false;

            if (rigidBody != null) rigidBody.isKinematic = true;

            if (damageParticles!= null && childrenDamageParticles!= null)
            {
                ParticleSystem particles = damageParticles.GetComponent<ParticleSystem>();
                ParticleSystem childrenParticles = childrenDamageParticles.GetComponent<ParticleSystem>();
                childrenParticles.Emit(5);
                particles.Emit(5);
            }


            StartCoroutine(DesactivateGameObject());
        }
    }

    private void SaveBreakablesPosition()
    {
        breakableCubes = new GameObject[prefracturedObject.transform.childCount - 1];

        for (int i = 0; i < prefracturedObject.transform.childCount - 1; i++)
        {
            breakableCubes[i] = prefracturedObject.transform.GetChild(i).gameObject;
        }

        breakableStartPosition = new Vector3[breakableCubes.Length];
        breakableStartRotation = new Quaternion[breakableCubes.Length];

        for (int i = 0; i < breakableCubes.Length; i++)
        {
            breakableStartPosition[i] = breakableCubes[i].transform.localPosition;
            breakableStartRotation[i] = breakableCubes[i].transform.localRotation;
        }
    }


    public override void RestartLevel()
    {
        for (int i = 0; i < breakableCubes.Length; i++)
        {
            breakableCubes[i].transform.localPosition = breakableStartPosition[i];
            breakableCubes[i].transform.localRotation = breakableStartRotation[i];
            breakableCubes[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (rigidBody != null)
        {
            rigidBody.isKinematic = false;
            rigidBody.velocity = Vector3.zero;
        }

        gameObject.SetActive(true);
        wholeObject.SetActive(true);
        prefracturedObject.SetActive(false);
        GetComponent<Collider>().enabled = true;

        transform.position = startPositionParent;
        transform.rotation = startRotationParent;
    }

    IEnumerator DesactivateGameObject()
    {
        yield return new WaitForSeconds(timeToDisappear);
        //esto se tendria que hacer mas suave, haciendolo desaparecer poco a poco
        prefracturedObject.SetActive(false);
        if (isPlatform)
        {
            StartCoroutine(ReappearPlatform());
        }
    }

    IEnumerator ReappearPlatform()
    {
        yield return new WaitForSeconds(platformReappearTime);

        RestartLevel();
    }
}
