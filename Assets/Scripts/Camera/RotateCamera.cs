using UnityEngine;

public class RotateCamera :  MonoBehaviour, IRestartLevelElement
{
    private Transform cameraTransform;
    private Quaternion rotationOnRespawn;
    private bool isRotating = false;
    private bool isRotatingTwoAngles = false;
    private bool isX = false;
    [SerializeField] private float rotationSpeed = 10f;
    private float targetAngleX;
    private float targetAngleY;

    [Header("Multiple Rotation Related")]
    private float totalRotationTime = 1f;
    private float elapsedRotationTime = 0f;
    private float startAngleX, startAngleY;
    private float angleDeltaX, angleDeltaY;

    public bool GetIsRotating()
    {
        return isRotating;
    }

    public void SaveActualRotation()
    {
        if (isRotating) 
            rotationOnRespawn = Quaternion.Euler(targetAngleX, targetAngleY, cameraTransform.rotation. eulerAngles.z);
        else
            rotationOnRespawn = cameraTransform.rotation;
    }

    public virtual void Awake()
    {
        GameController.GetGameController().AddRestartLevelElement(this);
    }

    private void Start()
    {
        cameraTransform = GetComponent<Transform>();
        SaveActualRotation();
    }

    private void Update()
    {
        if (isRotating && !isRotatingTwoAngles)
        {
            if (isX)
            {
                float newXRotation = Mathf.MoveTowardsAngle(cameraTransform.rotation.eulerAngles.x, targetAngleX, rotationSpeed * Time.deltaTime);
                cameraTransform.rotation = Quaternion.Euler(newXRotation, cameraTransform.rotation.eulerAngles.y, cameraTransform.rotation.eulerAngles.z);

                //Mathf.Approximately(newY, targetAngle)
                if (newXRotation == targetAngleX)
                    isRotating = false;
            }
            else
            {
                float newYRotation = Mathf.MoveTowardsAngle(cameraTransform.rotation.eulerAngles.y, targetAngleY, rotationSpeed * Time.deltaTime);
                cameraTransform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles.x, newYRotation, cameraTransform.rotation.eulerAngles.z);

                //Mathf.Approximately(newY, targetAngle)
                if (newYRotation == targetAngleY)
                    isRotating = false;
            }
        }

        if (isRotating && isRotatingTwoAngles)
        {
            /*Vector2 currentXRotation = new Vector2(cameraTransform.rotation.eulerAngles.x, cameraTransform.rotation.eulerAngles.y);
            float newXRotation = Mathf.MoveTowardsAngle(currentXRotation.x, targetAngleX, rotationSpeed * Time.deltaTime);
            float newYRotation = Mathf.MoveTowardsAngle(currentXRotation.y, targetAngleY, rotationSpeed * Time.deltaTime);
            cameraTransform.rotation = Quaternion.Euler(newXRotation, newYRotation, cameraTransform.rotation.eulerAngles.z);

            //Mathf.Approximately(newY, targetAngle)
            if (newXRotation == targetAngleX && newYRotation == targetAngleY)
                isRotatingTwoAngles = false; */

            elapsedRotationTime += Time.deltaTime;
            float timeToRotate = Mathf.Clamp01(elapsedRotationTime / totalRotationTime);

            float newX = Mathf.LerpAngle(startAngleX, targetAngleX, timeToRotate);
            float newY = Mathf.LerpAngle(startAngleY, targetAngleY, timeToRotate);
            cameraTransform.rotation = Quaternion.Euler(newX, newY, cameraTransform.rotation.eulerAngles.z);

            if (timeToRotate >= 1f)
            {
                isRotating = false;
                isRotatingTwoAngles = false;
                elapsedRotationTime = 0f;
            }
        }
    }

    public void StartRotation(bool rotatesOnX, float angleOffset)
    {
        if (rotatesOnX)
        {
            targetAngleX = Mathf.Repeat(cameraTransform.eulerAngles.x + angleOffset, 360f);
            targetAngleY = cameraTransform.eulerAngles.y;
            isX = true;
        }
        else
        {
            targetAngleY = Mathf.Repeat(cameraTransform.eulerAngles.y + angleOffset, 360f);
            targetAngleX = cameraTransform.eulerAngles.x;
            isX = false;
        }


        isRotating = true;
    }

    public void StartRotation(float angleOffsetX, float angleOffsetY)
    {
        /*targetAngleX = Mathf.Repeat(cameraTransform.eulerAngles.x + angleOffsetX, 360f);
        targetAngleY = Mathf.Repeat(cameraTransform.eulerAngles.y + angleOffsetY, 360f);

        isRotatingTwoAngles = true; */

        startAngleX = cameraTransform.eulerAngles.x;
        startAngleY = cameraTransform.eulerAngles.y;

        targetAngleX = Mathf.Repeat(startAngleX + angleOffsetX, 360f);
        targetAngleY = Mathf.Repeat(startAngleY + angleOffsetY, 360f);

        angleDeltaX = Mathf.Abs(Mathf.DeltaAngle(startAngleX, targetAngleX));
        angleDeltaY = Mathf.Abs(Mathf.DeltaAngle(startAngleY, targetAngleY));
        float maxAngle = Mathf.Max(angleDeltaX, angleDeltaY);
        totalRotationTime = maxAngle / rotationSpeed;

        //elapsedRotationTime = 0f;
        isRotating = true;
        isRotatingTwoAngles = true;
    }

    public void Restart()
    {
        cameraTransform.rotation = rotationOnRespawn;
    }
}
