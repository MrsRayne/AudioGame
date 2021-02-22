using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControllerNew : MonoBehaviour
{
    public Camera mainCamera;

    public float WalkSpeed => 4f;



    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    // Camera rotation sensitivity.
    private const float sensitivity = 2.0f;

    // Maximum allowed vertical rotation angle in degrees.
    private const float clampAngleDegrees = 80.0f;

    // Position 
    [SerializeField] GameObject windowTarget;
    [SerializeField] GameObject tableTarget;
    [SerializeField] GameObject catTarget;
    private Vector3 lastTarget;



    public LayerMask obstacleMask;

    Coroutine movementCoroutine;
    bool movement = false;
    bool leftFootstep;
    public float movementDuration = 0.4f;
    private float movementSpeed = 2f;



    // Start is called before the first frame update
    void Start()
    {
        Vector3 rotation = mainCamera.transform.localRotation.eulerAngles;
        rotationX = rotation.x;
        rotationY = rotation.y;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotationX += sensitivity * mouseY;
        rotationY += sensitivity * mouseX;
        rotationX = Mathf.Clamp(rotationX, -clampAngleDegrees, clampAngleDegrees);
        mainCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);



        SetCursorLock();
    }

    // Sets the cursor lock for first-person control.
    private void SetCursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Change Position

    public void WindowPosition()
    {
        StartCoroutine(LerpPosition(windowTarget.transform.position, 2));
    }
    public void TablePosition()
    {
        StartCoroutine(LerpPosition(tableTarget.transform.position, 2));
    }
    public void CatPosition()
    {
        StartCoroutine(LerpPosition(catTarget.transform.position, 2));
    }


    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            movement = true;
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        movement = false;
        lastTarget = targetPosition;

    }

}
