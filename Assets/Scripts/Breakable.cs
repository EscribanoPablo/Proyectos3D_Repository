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

    Vector3[] breakableStartPosition;
    Quaternion[] breakableStartRotation;

    [SerializeField] float timeToDisappear;
    float timer;

    [SerializeField] float fuerzaPene;

    Vector3 startPositionParent;
    Quaternion startRotationParent;

    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

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
                //hacer que pille la dirección de la bala y añadirle velocidad, no crear un vector nuevo
                //breakableCubes[i].GetComponent<Rigidbody>().velocity = new Vector3(1, 1, 0)* fuerzaPene;
            }
            GetComponent<Collider>().enabled = false;

            if (rigidBody != null) rigidBody.isKinematic = true;

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

    private void Update()
    {
        //prefracturedObject.transform.position = transform.position;
    }

    public override void RestartLevel()
    {
        StartCoroutine(Restart());
    }

    IEnumerator DesactivateGameObject()
    {
        yield return new WaitForSeconds(timeToDisappear);
        //esto se tendria que hacer mas suave, haciendolo desaparecer poco a poco
        //tambien podemos cambiar layer del objeto para que no se bugee con el player
        prefracturedObject.SetActive(false);

    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(0.2f);
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

}
