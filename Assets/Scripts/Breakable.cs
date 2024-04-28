using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Breakable : Obstacles
{

    [SerializeField] GameObject wholeObject;
    [SerializeField] GameObject prefracturedObject;
    [SerializeField] string breakerTag;
    GameObject [] breakableCubes;

    Vector3[] breakableStartPosition;
    Quaternion[] breakableStartRotation;

    private bool broken;
    [SerializeField] float timeToDisappear;
    float timer;


    // Start is called before the first frame update
    void Start()
    {
        breakableCubes = prefracturedObject.GetComponentsInChildren<GameObject>();
        breakableStartPosition = new Vector3[breakableCubes.Length];
        breakableStartRotation = new Quaternion[breakableCubes.Length];

        for (int i = 0; i < breakableCubes.Length; i++)
        {
            breakableStartPosition[i] = breakableCubes[i].transform.position;
            breakableStartRotation[i] = breakableCubes[i].transform.rotation;
        }

        wholeObject.SetActive(true);
        prefracturedObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == breakerTag) 
        {
            wholeObject.SetActive(false);
            prefracturedObject.transform.position = wholeObject.transform.position;
            prefracturedObject.transform.rotation = wholeObject.transform.rotation;
            prefracturedObject.SetActive(true);

            GetComponent<Collider>().enabled = false;

            broken = true;
        }

    }

    private void Update()
    {
        if (broken)
        {
            timer += Time.deltaTime;
            //esto se tendria que hacer mas suave, haciendolo desaparecer poco a poco
            //tambien podemos cambiar layer del objeto para que no se bugee con el player
            if (timer >= timeToDisappear)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public override void RestartLevel()
    {
        //PARA RESTART LEVEL, ESTARIA BIEN HACER UNA ARRAY DE LAS POSICIONES / ROTACIÓN DE TODAS LAS PIEZAS Y VOLVERLAS TODAS A SU SITIO

        for (int i = 0; i < breakableCubes.Length; i++)
        {
            breakableCubes[i].transform.position = breakableStartPosition[i];
            breakableCubes[i].transform.rotation = breakableStartRotation[i];
        }
    }

}
