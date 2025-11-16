using System.Collections;
using UnityEngine;

public class WallBetweenDetector : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    private GameObject target;
    [SerializeField] private LayerMask SeeThroughLayers;

    private bool behindWall;
    private Coroutine currentRoutine;

    void Start()
    {
        target = this.gameObject;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, (target.transform.position - camera.transform.position).normalized, out hit, Mathf.Infinity, SeeThroughLayers))
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
    }
}
