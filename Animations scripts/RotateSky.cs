using UnityEngine;
using System.Collections;

public class RotateSky : MonoBehaviour
{
    public float rotateSpeed = 1.2f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
