using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class RESTCall : MonoBehaviour
{
    public int Id;
    public string WhatIsThisThing;
    public GameObject entity;
    public NavMeshAgent agent;
    private const int TIME_BETWEEN_CALLS = 1;
    private float timeLeftToCall = TIME_BETWEEN_CALLS;
    private string fullRequest;
    private JsonObject trashCan;

    public enum methods
    {
        GET,
        PUT
    }

    public enum WhatToModifyOrGet
    {
        content,
        position
    }

    public enum Stationary
    {
        yes,
        no
    }

    // TODO make this a general put get method
    public methods GetMethods;

    public WhatToModifyOrGet GetWhatToModify;
    public Stationary isStationary;
    private string action;
    private string baseUri = "https://vpstxcvvc2zdblp-db202002241040.adb.eu-frankfurt-1.oraclecloudapps.com/ords/smartcitydb/";

    // Start is called before the first frame update
    private void Start()
    {
        switch (GetMethods.ToString())
        {
            case "GET":
                action = "get";
                break;

            case "PUT":
                action = "update";
                break;
        }
        //Set position of stationary object when starting program
        if ((isStationary.ToString() == "yes") && (GetWhatToModify.ToString() == "position"))
        {
            setPosition(entity.transform.position.x, entity.transform.position.z);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        timeLeftToCall -= Time.deltaTime;
        if ((null != agent) && (timeLeftToCall <= 0) && action == "update")
        {
            if (isMoving(agent))
            {
                timeLeftToCall = TIME_BETWEEN_CALLS;
                setPosition(entity.transform.position.x, entity.transform.position.z);
            }
        }
    }

    //Put position to database

    public IEnumerator getContent(Action<string> action)
    {
        fullRequest = baseUri + $"{WhatIsThisThing.ToLower()}/get/content/{Id}";
        yield return (SendRequest(fullRequest, action));
    }

    public IEnumerator getItem(Action<string> action)
    {
        fullRequest = baseUri + $"{WhatIsThisThing.ToLower()}/get/{Id}";
        yield return (SendRequest(fullRequest, action));
    }

    public IEnumerator getPosition(Action<string> action)
    {
        fullRequest = baseUri + $"{WhatIsThisThing.ToLower()}/get/position/{Id}";
        yield return StartCoroutine(SendRequest(fullRequest, action));
    }
    //TODO fix this 
    public UnityWebRequest setContent(float value)
    {
        fullRequest = baseUri + $"{WhatIsThisThing.ToLower()}/update/content/{Id}/{value}";
        UnityWebRequest www = UnityWebRequest.Put(fullRequest, "dummyData");
        www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            return www;
        }
        else
        {
            Debug.Log($"{action} complete!");
            return www;
        }
    }
    //TODO fix this
    public UnityWebRequest addContentToTrashCar(float amount, int trashCanId)
    {
        fullRequest = baseUri + $"{WhatIsThisThing.ToLower()}/{action}/content/{Id}/{trashCanId}/{amount}";
        UnityWebRequest www = UnityWebRequest.Put(fullRequest, "dummyData");
        www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            return www;
        }
        else
        {
            Debug.Log($"{action} complete!");
            return www;
        }
    }

    private IEnumerator SendRequest(string fullUri, Action<string> action)
    {
        yield return StartCoroutine(GetRequest(fullUri, (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isNetworkError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                action(req.downloadHandler.text);
            }
        }));
    }

    private IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            callback(request);
        }
    }

    private bool isMoving(NavMeshAgent posAgent)
    {
        return posAgent.velocity.sqrMagnitude > 0;
    }

    private void setPosition(float x, float z)
    {
        fullRequest = baseUri + $"{WhatIsThisThing.ToLower()}/{action}/{GetWhatToModify.ToString()}/{Id}/{x}/{z}";
        UnityWebRequest www = UnityWebRequest.Put(fullRequest, "dummyData");
        www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log($"{action} complete!");
        }
    }
}