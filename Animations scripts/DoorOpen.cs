using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    
    void OnTriggerEnter(Collider other) 
    {
        anim.SetBool("open", true);
    }

    private void OnTriggerExit(Collider other) 
    {
        anim.SetBool("open", false);
    }

    void Update()
    {
        
    }
}
