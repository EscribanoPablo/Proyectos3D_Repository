using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Breakable : Obstacles
{

    [SerializeField] GameObject wholeObject;
    [SerializeField] GameObject prefracturedObject;

    private bool broken;
    [SerializeField] float timeToDisappear;
    float timer;

    public override void RestartLevel()
    {
    //PARA RESTART LEVEL, ESTARIA BIEN HACER UNA ARRAY DE LAS POSICIONES / ROTACIÓN DE TODAS LAS PIEZAS Y VOLVERLAS TODAS A SU SITIO

    }

    // Start is called before the first frame update
    void Start()
    {
        wholeObject.SetActive(true);
        prefracturedObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") //AQUI SE TIENE QUE CAMBIAR EL TAG POR EL DE LA BALA
        {
            wholeObject.SetActive(false);
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

}
