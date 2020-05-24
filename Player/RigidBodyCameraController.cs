using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyCameraController : MonoBehaviour
{
    public Rigidbody characterRb;
    public float lookSpeed = 10f;

    private float _camRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float y = Input.GetAxis("Mouse Y");
        float x = Input.GetAxis("Mouse X");
        
        _camRotation -= y * lookSpeed * Time.deltaTime * 10f;
        _camRotation = Mathf.Clamp(_camRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(_camRotation , 0f ,0f);
        
        characterRb.rotation =  Quaternion.Euler(characterRb.rotation.eulerAngles + x * lookSpeed * Time.deltaTime * 10f * Vector3.up);
        
        if(Input.GetMouseButton(0))
            Cursor.lockState = CursorLockMode.Locked;
        
        if(Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.Confined;
    }
}