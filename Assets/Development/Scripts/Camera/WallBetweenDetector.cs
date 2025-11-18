using System.Collections;
using System.Linq;
using UnityEngine;

public class WallBetweenDetector : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    private GameObject target;
    [SerializeField] private LayerMask SeeThroughLayers;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float seeThroughMaxRadius = 3f;

    private bool behindWall;
    private Coroutine currentRoutine;

    [SerializeField] private Material wallMaterial;
    private MeshRenderer[] wallsRenderers;

    /*void Start()
    {
        target = this.gameObject;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCamera.transform.position, (target.transform.position - mainCamera.transform.position).normalized, out hit, Mathf.Infinity, SeeThroughLayers))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                if (behindWall)
                {
                    if (currentRoutine != null)
                        StopCoroutine(currentRoutine);

                    currentRoutine = StartCoroutine(ScaleOverTime(0.5f, 0.5f));
                    behindWall = false;
                }
            }
            else
            {
                if (!behindWall)
                {
                    if (currentRoutine != null)
                        StopCoroutine(currentRoutine);

                    currentRoutine = StartCoroutine(ScaleOverTime(1f, 5.5f));
                    behindWall = true;
                }
            }
        }
    }

    private IEnumerator ScaleOverTime(float duration, float scale)
    {
        Vector3 startScale = target.transform.localScale;
        Vector3 endScale = Vector3.one * scale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            target.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.transform.localScale = endScale;
    }*/

    private MaterialPropertyBlock[] mpbs;

    void Start()
    {
        target = this.gameObject;

        wallsRenderers = GameObject.FindGameObjectsWithTag("SeeThroughWalls").Select(w => w.GetComponent<MeshRenderer>()).Where(r => r != null).ToArray();

        mpbs = new MaterialPropertyBlock[wallsRenderers.Length];

        for (int i = 0; i < wallsRenderers.Length; i++)
        {
            mpbs[i] = new MaterialPropertyBlock();
            wallsRenderers[i].GetPropertyBlock(mpbs[i]);
        }
    }

    void Update()
    {
        Vector3 playerPos = target.transform.position;
        Vector3 camPos = mainCamera.transform.position;

        for (int i = 0; i < wallsRenderers.Length; i++)
        {
            mpbs[i].SetVector("_PlayerPos", playerPos);
            mpbs[i].SetVector("_CameraPos", camPos);
            wallsRenderers[i].SetPropertyBlock(mpbs[i]);
        }

        DetectWall();
    }

    private void DetectWall()
    {
        RaycastHit hit;
        Vector3 dir = (target.transform.position - mainCamera.transform.position).normalized;

        if (Physics.Raycast(mainCamera.transform.position, dir, out hit, Mathf.Infinity, SeeThroughLayers))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (behindWall)
                {
                    StartRadiusChange(0f);
                    behindWall = false;
                }
            }
            else
            {
                if (!behindWall)
                {
                    StartRadiusChange(seeThroughMaxRadius);
                    behindWall = true;
                }
            }
        }
    }

    private void StartRadiusChange(float targetRadius)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ScaleOverTime(animationDuration, targetRadius));
    }

    private IEnumerator ScaleOverTime(float duration, float newRadius)
    {
        float elapsed = 0f;
        float startRadius = mpbs[0].GetFloat("_Radius");

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float radius = Mathf.Lerp(startRadius, newRadius, t);

            for (int i = 0; i < wallsRenderers.Length; i++)
            {
                mpbs[i].SetFloat("_Radius", radius);
                wallsRenderers[i].SetPropertyBlock(mpbs[i]);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < wallsRenderers.Length; i++)
        {
            mpbs[i].SetFloat("_Radius", newRadius);
            wallsRenderers[i].SetPropertyBlock(mpbs[i]);
        }
    }
}
