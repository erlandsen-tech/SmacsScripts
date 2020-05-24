<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
public class ParkingPanel : MonoBehaviour
{
    public TextMeshPro text;
    public GameObject ParkingSignObject;
    private const int TIMEBETWEENCALLS = 5;
    private float timeToCall;
    private RESTCall call;
    private string json;
    private float spaces;
    // Start is called before the first frame update
    void Start()
    {
       timeToCall = TIMEBETWEENCALLS; 
       text.text = "";
       call = ParkingSignObject.GetComponent<RESTCall>();

    }

    // Update is called once per frame
    void Update()
    {
        if(timeToCall <= 0)
        {
            timeToCall = TIMEBETWEENCALLS;
            StartCoroutine(setFreeSpaces());
        }
        timeToCall -= Time.deltaTime;
    }

    private  IEnumerator setFreeSpaces(){
        yield return StartCoroutine(call.getContent(content => json = content));
        try{
            JsonObject spacesObject = JsonConvert.DeserializeObject<JsonObject>(json);
            spaces = spacesObject.items[0].count;
            text.text = spaces.ToString();
        }
        catch {
            Debug.LogError("Unable to parse json from ParkingPanel script");
        }
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
public class ParkingPanel : MonoBehaviour
{
    public TextMeshPro text;
    public GameObject ParkingSignObject;
    private const int TIMEBETWEENCALLS = 1;
    private float timeToCall;
    private RESTCall call;
    private string json;
    private float spaces;
    // Start is called before the first frame update
    void Start()
    {
       timeToCall = TIMEBETWEENCALLS; 
       text.text = "";
       call = ParkingSignObject.GetComponent<RESTCall>();

    }

    // Update is called once per frame
    void Update()
    {
        if(timeToCall <= 0)
        {
            timeToCall = TIMEBETWEENCALLS;
            StartCoroutine(setFreeSpaces());
        }
        timeToCall -= Time.deltaTime;
    }

    private  IEnumerator setFreeSpaces(){
        yield return StartCoroutine(call.getContent(content => json = content));
        try{
            JsonObject spacesObject = JsonConvert.DeserializeObject<JsonObject>(json);
            spaces = spacesObject.items[0].count;
            text.text = spaces.ToString();
        }
        catch {
            Debug.LogError("Unable to parse json from ParkingPanel script");
        }
    }
}
>>>>>>> 17b7a95ff6f97721ad60bea11c7a4257559886a3
