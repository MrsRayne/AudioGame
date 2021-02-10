using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;
    private CharacterController characterController = null;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    // Camera rotation sensitivity.
    private const float sensitivity = 2.0f;

    // Maximum allowed vertical rotation angle in degrees.
    private const float clampAngleDegrees = 80.0f;

    // Position 
    [SerializeField] GameObject windowTarget;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
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

    }

    public void WindowPosition()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, windowTarget.transform.position, Time.deltaTime);   //is ugly, should be rewritten
    }
}
