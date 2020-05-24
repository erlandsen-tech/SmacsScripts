<<<<<<< HEAD
﻿using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingSpace : MonoBehaviour
{
    public int ParkingSpaceId;
    public bool resetOnStart;
    private RESTCall call;
    private string json;
    private int occupied;
    void Start()
    {
        call = this.GetComponent<RESTCall>();
        if (resetOnStart)
        {
            call.setContent(0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vehicle") || (other.gameObject.CompareTag("Car")))
        {
            yield return StartCoroutine(call.getContent(content => json = content));
            try
            {
                JsonObject parkingLot = JsonConvert.DeserializeObject<JsonObject>(json);
                occupied = parkingLot.items[0].occupied;
                if (occupied == 0)
                {
                    call.setContent(1);
                }
            }
            catch
            {

                Debug.LogError("Unable to update parkingspace");
            }
        }
    }
    private IEnumerator OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Vehicle") || (other.gameObject.CompareTag("Car")))
        {
            yield return StartCoroutine(call.getContent(content => json = content));
            try
            {
                JsonObject parkingLot = JsonConvert.DeserializeObject<JsonObject>(json);
                occupied = parkingLot.items[0].occupied;
                if (occupied == 1)
                {
                    call.setContent(0);
                }
            }
            catch
            {
                Debug.LogError("Unable to update parkingspace");
            }
        }
    }
}
=======
﻿using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingSpace : MonoBehaviour
{
    public int ParkingSpaceId;
    private RESTCall call;
    private string json;
    private int occupied;
    void Start()
    {
        call = this.GetComponent<RESTCall>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vehicle") || (other.gameObject.CompareTag("Car")))
        {
            yield return StartCoroutine(call.getContent(content => json = content));
            try
            {
                JsonObject parkingLot = JsonConvert.DeserializeObject<JsonObject>(json);
                occupied = parkingLot.items[0].occupied;
                if (occupied == 0)
                {
                    call.setContent(1);
                }
            }
            catch
            {

                Debug.LogError("Unable to update parkingspace");
            }
        }
    }
    private IEnumerator OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Vehicle") || (other.gameObject.CompareTag("Car")))
        {
            yield return StartCoroutine(call.getContent(content => json = content));
            try
            {
                JsonObject parkingLot = JsonConvert.DeserializeObject<JsonObject>(json);
                occupied = parkingLot.items[0].occupied;
                if (occupied == 1)
                {
                    call.setContent(0);
                }
            }
            catch
            {
                Debug.LogError("Unable to update parkingspace");
            }
        }
    }
}
>>>>>>> 17b7a95ff6f97721ad60bea11c7a4257559886a3
