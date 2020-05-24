<<<<<<< HEAD
﻿using System.Collections;
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

    
    IEnumerator OnTriggerEnter(Collider other) 
    {
        anim.SetBool("open", true);
        yield return StartCoroutine(wait());
        anim.SetBool("open", false);
    }

    private void OnTriggerExit(Collider other) 
    {
        anim.SetBool("open", false);
    }

    void Update()
    {
        
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);
    }
}
=======
﻿using System.Collections;
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
>>>>>>> 17b7a95ff6f97721ad60bea11c7a4257559886a3
