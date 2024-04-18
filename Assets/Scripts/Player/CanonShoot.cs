using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonShoot : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] int shootButton;

    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] float fireRate = 0.5f;
    [SerializeField] float nextTimeFire = 0.2f;

    public Vector3 CanonForward => canonForward;
    private Vector3 canonForward;

    private void Update()
    {
        if (Input.GetMouseButton(shootButton) && Time.time >= nextTimeFire)
        {
            nextTimeFire = Time.time + fireRate;
            GameObject _bullet = Instantiate(bulletPrefab, spawnPosition.position, bulletPrefab.transform.rotation);

        }
        canonForward = transform.forward;
    }
}
