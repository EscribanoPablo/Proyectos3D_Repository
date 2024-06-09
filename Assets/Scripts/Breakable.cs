using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Breakable : Obstacles, IRestartLevelElement
{
    [SerializeField] GameObject wholeObject;
    [SerializeField] GameObject prefracturedObject;
    [SerializeField] string breakerTag;
    GameObject[] breakableCubes;

    [SerializeField] bool isPlatform;
    [SerializeField] float platformReappearTime;

    Vector3[] breakableStartPosition;
    Quaternion[] breakableStartRotation;

    [SerializeField] float timeToDisappear;
    MeshRenderer[] meshRenderers;
    Color originalColor;
    bool isFading;

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

        SetUpMeshRenderers();
        originalColor = meshRenderers[0].material.color;

        wholeObject.SetActive(true);
        prefracturedObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == breakerTag)
        {

            wholeObject.SetActive(false);
            prefracturedObject.SetActive(true);

            for (int i = 0; i < prefracturedObject.transform.childCount - 1; i++)
            {
                breakableCubes[i].GetComponent<Rigidbody>().velocity += canonShoot.CanonForward * boxExplosionForce;
            }
            GetComponent<Collider>().enabled = false;

            if (rigidBody != null) rigidBody.isKinematic = true;

            StartCoroutine(DesactivateGameObject());
        }
    }

    private void FixedUpdate()
    {
        if (isFading)
        {
            FadeBreakables();

        }
    }

    private void SaveBreakablesPosition()
    {
        breakableCubes = new GameObject[prefracturedObject.transform.childCount];

        for (int i = 0; i < prefracturedObject.transform.childCount; i++)
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
        yield return new WaitForSeconds(timeToDisappear/4);

        isFading = true;

        yield return new WaitForSeconds(timeToDisappear*3/4);

        isFading = false;
        ResetBreakablesColor();

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

    private void SetUpMeshRenderers()
    {
        meshRenderers = new MeshRenderer[breakableCubes.Length];

        for (int i = 0; i < breakableCubes.Length; i++)
        {
            meshRenderers[i] = breakableCubes[i].GetComponent<MeshRenderer>();
        }
    }

    private void FadeBreakables()
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, Color.clear, 0.01f);
        }
    }

    private void ResetBreakablesColor()
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.color = originalColor;
        }
    }
}
