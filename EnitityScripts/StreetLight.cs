using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLight : MonoBehaviour
{
    public Light streetLight;
    public Light sun;
    // Start is called before the first frame update
    void Start()
    {
        streetLight.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
    }
    //Turn on the light if object enters its zone. Not if its a vehicle
    private void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
        if(other.gameObject.CompareTag("Player"))
=======
        if(!other.gameObject.CompareTag("Player"))
>>>>>>> 67b7ae05906c0268259ea3d952d1b45a69fde071
        {
            activate();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        streetLight.enabled = false;
    }

    private bool getDayTime()
    {
        var timeOfday = sun.GetComponent<SetTimeOfDay>();
<<<<<<< HEAD
        bool dayTime = (timeOfday.currentTimeOfDay > 0.3 && timeOfday.currentTimeOfDay < 0.7);
=======
        bool dayTime = (timeOfday.currentTimeOfDay < 0.3 && timeOfday.currentTimeOfDay > 0.6);
>>>>>>> 67b7ae05906c0268259ea3d952d1b45a69fde071
        return dayTime;
    }
    private void activate()
    {
<<<<<<< HEAD
        if (!getDayTime())
=======
        if (getDayTime())
>>>>>>> 67b7ae05906c0268259ea3d952d1b45a69fde071
        {
            streetLight.enabled = true;
        }
    }
}
