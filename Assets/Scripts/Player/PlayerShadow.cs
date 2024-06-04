using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private Vector3 _parentOffset = new Vector3(0f, 0.01f, 0f);
    [SerializeField]
    private LayerMask _layerMask;

    void Update()
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, 3f, _layerMask))
        {
            _parent.position = new Vector3(0f, 1000f, 0f);
        }
        else if (Physics.Raycast(ray, out hitInfo, 100f, _layerMask))
        {
            _parent.position = hitInfo.point + _parentOffset;

            _parent.up = hitInfo.normal;
        }
        else
        {
            _parent.position = new Vector3(0f, 1000f, 0f);
        }
    }
}
