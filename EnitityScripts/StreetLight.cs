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
        if(other.gameObject.CompareTag("Player"))
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
        bool dayTime = (timeOfday.currentTimeOfDay > 0.3 && timeOfday.currentTimeOfDay < 0.7);
        return dayTime;
    }
    private void activate()
    {
        if (!getDayTime())
        {
            streetLight.enabled = true;
        }
    }
}
