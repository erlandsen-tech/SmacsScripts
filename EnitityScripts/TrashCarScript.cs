<<<<<<< HEAD
﻿using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class TrashCarScript : MonoBehaviour
{
    public int id;
    public GameObject trashCar;
    //public GameObject trashCarParent;
    public NavMeshAgent agent;
    public Transform mapEnterPoint;
    public Transform mapExitPoint;
    private int destPoint = 0;
    private GameObject[] trashCans;
    private List<TrashCan> trashCanObjects;
    private List<RESTCall> trashCanCalls = new List<RESTCall>();
    private RESTCall trashCarCall;
    private string jsonTrashCan;
    private string jsonTrashCar;
    private const int TIME_BETWEEN_CALLS = 1;
    private float timeLeftToCall = TIME_BETWEEN_CALLS;
    private TrashCan currentCan;
    private Vector3 exit;
    private Vector3 enter;
    private bool done;

    private void Start()
    {
        done = true;
        enter = mapEnterPoint.transform.position;
        exit = mapExitPoint.transform.position;
        trashCarCall = trashCar.GetComponent<RESTCall>();
        timeLeftToCall = 0;
        //Find all trashcans
        if (null == trashCans)
            trashCans = GameObject.FindGameObjectsWithTag("trashcan");
        //Find RESTCall script in trashcan
        foreach (GameObject trashCan in trashCans)
        {
            trashCanCalls.Add(trashCan.GetComponent<RESTCall>());
        }
    }

    private void Update()
    {
        timeLeftToCall -= Time.deltaTime;
        if (timeLeftToCall <= 0)
        {
            StartCoroutine(getFullCans());
            timeLeftToCall = TIME_BETWEEN_CALLS;
        }
    }

    // Long method because async stuff.
    // Gets all trashcans and checks if there is a full one
    // If it is a full one there, create a path based on the volume in the car and distance
    private IEnumerator getFullCans()
    {
        List<string> jsonReturns = new List<string>();
        List<Vector3> path;
        trashCanObjects = new List<TrashCan>();
        float amount;
        foreach (RESTCall call in trashCanCalls)
        {
            yield return StartCoroutine(call.getItem(value => jsonTrashCan = value));
            try
            {
                TrashCan canToAdd = createTrashCanFromJsonResponse(call, jsonTrashCan);
                trashCanObjects.Add(canToAdd);
            }
            catch
            {
                Debug.LogError("Unable to parse jsonResponse from getFullCans in TrashCarScript");
            }
        }
        if (fullCan(trashCanObjects))
        {
            //trashCarParent.transform.position = enter;
            yield return StartCoroutine(trashCarCall.getContent(content => jsonTrashCar = content));
            try
            {
                amount = getSpaceLeft(jsonTrashCar);
                trashCanObjects = sortCans(trashCanObjects);
                path = GetRoute(trashCanObjects, amount);
                if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
                {
                    done = false;
                    GotoNextPoint(path);
                }
            }
            catch
            {
                Debug.LogError($"Unable to fetch content of trashcar with id: {id}");
            }
        }
        else if(!fullCan(trashCanObjects) && !done)
        {
            GoToExit();
            done = true;
        }
    }

    // Check if at least one can from the response is full
    private bool fullCan(List<TrashCan> cans)
    {
        foreach (TrashCan can in cans)
        {
            if (can.current_amount >= 85)
            {
                return true;
            }
        }
        return false;
    }

    // Convert string from WebRequest to C# object
    public TrashCan createTrashCanFromJsonResponse(RESTCall call, string jsonTrashCan)
    {
        var newCan = new TrashCan();
        var canFromResponse = JsonConvert.DeserializeObject<JsonObject>(jsonTrashCan);
        newCan.Id = call.Id;
        newCan.current_amount = canFromResponse.items[0].current_amount;
        newCan.xPos = canFromResponse.items[0].xpos;
        newCan.zPos = canFromResponse.items[0].zpos;
        return newCan;
    }
    public TrashCar createTrashCarFromJsonResponse(RESTCall call, string jsonTrashCar)
    {
        TrashCar newCar = new TrashCar();
        JsonObject carFromResponse = JsonConvert.DeserializeObject<JsonObject>(jsonTrashCar);
        newCar.Id = call.Id;
        newCar.current_amount = carFromResponse.items[0].current_amount;
        newCar.volume = carFromResponse.items[0].volume;
        return newCar;
    }

    //Can easily be converted to return whole car, but for now we just need space left.
    private float getSpaceLeft(string jsonTrashCar)
    {
        JsonObject response = JsonConvert.DeserializeObject<JsonObject>(jsonTrashCar);
        TrashCar car = new TrashCar();
        try
        {
            car.Id = trashCarCall.Id;
            car.current_amount = response.items[0].current_amount;
            car.volume = response.items[0].volume;
            //Convert current_amount which is in percent to volume, and subtract
            float spaceLeft = (car.volume * car.current_amount) / 100;
            spaceLeft = car.volume - spaceLeft;
            return spaceLeft;
        }
        catch
        {
            Debug.LogError($"Unable to getAmount() of trashcar with id: {id}");
            return -1;
        }
    }

    //Find waypoints based on amount of trash in the cans, and amount of room in car
    //Sort on distance when list is created
    private List<Vector3> GetRoute(List<TrashCan> cans, float currentAmount)
    {
        var path = new List<Vector3>();
        foreach (TrashCan can in cans)
        {
            Vector3 v = new Vector3(can.xPos, 0, can.zPos);
            path.Add(v);
        }
        return path;
    }

    // this handles pathpoints and emptying the trash
    private void GotoNextPoint(List<Vector3> path)
    {
        if (path.Count == 0)
        {
            return;
        }
        if (destPoint < path.Count)
        {
            agent.SetDestination(path[destPoint]);
            destPoint += 1;
        }
        else if (destPoint >= path.Count)
        {
            agent.SetDestination(exit);
            destPoint = 0;
        }
    }
    private void GoToExit()
    {
        agent.SetDestination(exit);
        destPoint = 0;
    }
    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);   //Wait
    }
    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("trashcan"))
        {
            GameObject can = other.gameObject;
            agent.speed = 0;
            yield return StartCoroutine(Wait(5));
            StartCoroutine(emptyTheTrash(can));
            yield return StartCoroutine(Wait(5));
            agent.speed = 8;
        }
    }

    private List<TrashCan> sortCans(List<TrashCan> cans)
    {
        cans = cans.OrderBy(o => o.current_amount).ToList();
        return cans;
    }

    //Should be updated with error handling for special cases
    // What if the can has been more filled than calculating the route?
    // What if the car has no space?
    private IEnumerator emptyTheTrash(GameObject can)
    {
        TrashCan canToEmpty;
        TrashCar carToFill;
        RESTCall trashCanCall = can.GetComponent<RESTCall>();
        yield return StartCoroutine(trashCanCall.getItem(value => jsonTrashCan = value));
        try
        {
            canToEmpty = createTrashCanFromJsonResponse(trashCanCall, jsonTrashCan);
            updateCar(canToEmpty);
            updateCan(trashCanCall, -canToEmpty.current_amount);
        }
        catch
        {
            Debug.LogError("Unable to parse jsonResponse from getFullCans in TrashCarScript");
        }
        yield return StartCoroutine(trashCarCall.getContent(content => jsonTrashCar = content));
        try
        {
            carToFill = createTrashCarFromJsonResponse(trashCarCall, jsonTrashCar);
            float spaceLeft = (carToFill.volume * carToFill.current_amount) / 100;
            spaceLeft = carToFill.volume - spaceLeft;
        }
        catch
        {
            Debug.LogError($"Unable to fetch content of trashcar with id: {id}");
        }
    }

    private void updateCan(RESTCall call, float amount)
    {
        call.setContent(amount);
    }

    private void updateCar(TrashCan can)
    {
        float amount = can.current_amount;
        trashCarCall.addContentToTrashCar(amount, can.Id);
    }
=======
﻿using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class TrashCarScript : MonoBehaviour
{
    public int id;
    public GameObject trashCar;
    //public GameObject trashCarParent;
    public NavMeshAgent agent;
    public Transform mapEnterPoint;
    public Transform mapExitPoint;
    private int destPoint = 0;
    private GameObject[] trashCans;
    private List<TrashCan> trashCanObjects;
    private List<RESTCall> trashCanCalls = new List<RESTCall>();
    private RESTCall trashCarCall;
    private string jsonTrashCan;
    private string jsonTrashCar;
    private const int TIME_BETWEEN_CALLS = 1;
    private float timeLeftToCall = TIME_BETWEEN_CALLS;
    private TrashCan currentCan;
    private Vector3 exit;
    private Vector3 enter;
    private bool done;

    private void Start()
    {
        done = true;
        enter = mapEnterPoint.transform.position;
        exit = mapExitPoint.transform.position;
        trashCarCall = trashCar.GetComponent<RESTCall>();
        timeLeftToCall = 0;
        //Find all trashcans
        if (null == trashCans)
            trashCans = GameObject.FindGameObjectsWithTag("trashcan");
        //Find RESTCall script in trashcan
        foreach (GameObject trashCan in trashCans)
        {
            trashCanCalls.Add(trashCan.GetComponent<RESTCall>());
        }
    }

    private void Update()
    {
        timeLeftToCall -= Time.deltaTime;
        if (timeLeftToCall <= 0)
        {
            StartCoroutine(getFullCans());
            timeLeftToCall = TIME_BETWEEN_CALLS;
        }
    }

    // Long method because async stuff.
    // Gets all trashcans and checks if there is a full one
    // If it is a full one there, create a path based on the volume in the car and distance
    private IEnumerator getFullCans()
    {
        List<string> jsonReturns = new List<string>();
        List<Vector3> path;
        trashCanObjects = new List<TrashCan>();
        float amount;
        foreach (RESTCall call in trashCanCalls)
        {
            yield return StartCoroutine(call.getItem(value => jsonTrashCan = value));
            try
            {
                TrashCan canToAdd = createTrashCanFromJsonResponse(call, jsonTrashCan);
                trashCanObjects.Add(canToAdd);
            }
            catch
            {
                Debug.LogError("Unable to parse jsonResponse from getFullCans in TrashCarScript");
            }
        }
        if (fullCan(trashCanObjects))
        {
            //trashCarParent.transform.position = enter;
            yield return StartCoroutine(trashCarCall.getContent(content => jsonTrashCar = content));
            try
            {
                amount = getSpaceLeft(jsonTrashCar);
                trashCanObjects = sortCans(trashCanObjects);
                path = GetRoute(trashCanObjects, amount);
                if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
                {
                    done = false;
                    GotoNextPoint(path);
                }
            }
            catch
            {
                Debug.LogError($"Unable to fetch content of trashcar with id: {id}");
            }
        }
        else if(!fullCan(trashCanObjects) && !done)
        {
            GoToExit();
            done = true;
        }
    }

    // Check if at least one can from the response is full
    private bool fullCan(List<TrashCan> cans)
    {
        foreach (TrashCan can in cans)
        {
            if (can.current_amount >= 85)
            {
                return true;
            }
        }
        return false;
    }

    // Convert string from WebRequest to C# object
    public TrashCan createTrashCanFromJsonResponse(RESTCall call, string jsonTrashCan)
    {
        var newCan = new TrashCan();
        var canFromResponse = JsonConvert.DeserializeObject<JsonObject>(jsonTrashCan);
        newCan.Id = call.Id;
        newCan.current_amount = canFromResponse.items[0].current_amount;
        newCan.xPos = canFromResponse.items[0].xpos;
        newCan.zPos = canFromResponse.items[0].zpos;
        return newCan;
    }
    public TrashCar createTrashCarFromJsonResponse(RESTCall call, string jsonTrashCar)
    {
        TrashCar newCar = new TrashCar();
        JsonObject carFromResponse = JsonConvert.DeserializeObject<JsonObject>(jsonTrashCar);
        newCar.Id = call.Id;
        newCar.current_amount = carFromResponse.items[0].current_amount;
        newCar.volume = carFromResponse.items[0].volume;
        return newCar;
    }

    //Can easily be converted to return whole car, but for now we just need space left.
    private float getSpaceLeft(string jsonTrashCar)
    {
        JsonObject response = JsonConvert.DeserializeObject<JsonObject>(jsonTrashCar);
        TrashCar car = new TrashCar();
        try
        {
            car.Id = trashCarCall.Id;
            car.current_amount = response.items[0].current_amount;
            car.volume = response.items[0].volume;
            //Convert current_amount which is in percent to volume, and subtract
            float spaceLeft = (car.volume * car.current_amount) / 100;
            spaceLeft = car.volume - spaceLeft;
            return spaceLeft;
        }
        catch
        {
            Debug.LogError($"Unable to getAmount() of trashcar with id: {id}");
            return -1;
        }
    }

    //Find waypoints based on amount of trash in the cans, and amount of room in car
    //Sort on distance when list is created
    private List<Vector3> GetRoute(List<TrashCan> cans, float currentAmount)
    {
        var path = new List<Vector3>();
        foreach (TrashCan can in cans)
        {
            Vector3 v = new Vector3(can.xPos, 0, can.zPos);
            path.Add(v);
        }
        return path;
    }

    // this handles pathpoints and emptying the trash
    private void GotoNextPoint(List<Vector3> path)
    {
        if (path.Count == 0)
        {
            return;
        }
        if (destPoint < path.Count)
        {
            agent.SetDestination(path[destPoint]);
            destPoint += 1;
        }
        else if (destPoint >= path.Count)
        {
            agent.SetDestination(exit);
            destPoint = 0;
        }
    }
    private void GoToExit()
    {
        agent.SetDestination(exit);
        destPoint = 0;
    }
    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);   //Wait
    }
    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("trashcan"))
        {
            GameObject can = other.gameObject;
            StartCoroutine(emptyTheTrash(can));
            agent.speed = 0;
            yield return StartCoroutine(Wait(5));
            agent.speed = 8;
        }
    }

    private List<TrashCan> sortCans(List<TrashCan> cans)
    {
        cans = cans.OrderBy(o => o.current_amount).ToList();
        return cans;
    }

    //Should be updated with error handling for special cases
    // What if the can has been more filled than calculating the route?
    // What if the car has no space?
    private IEnumerator emptyTheTrash(GameObject can)
    {
        TrashCan canToEmpty;
        TrashCar carToFill;
        RESTCall trashCanCall = can.GetComponent<RESTCall>();
        yield return StartCoroutine(trashCanCall.getItem(value => jsonTrashCan = value));
        try
        {
            canToEmpty = createTrashCanFromJsonResponse(trashCanCall, jsonTrashCan);
            updateCar(canToEmpty);
            updateCan(trashCanCall, -canToEmpty.current_amount);
        }
        catch
        {
            Debug.LogError("Unable to parse jsonResponse from getFullCans in TrashCarScript");
        }
        yield return StartCoroutine(trashCarCall.getContent(content => jsonTrashCar = content));
        try
        {
            carToFill = createTrashCarFromJsonResponse(trashCarCall, jsonTrashCar);
            float spaceLeft = (carToFill.volume * carToFill.current_amount) / 100;
            spaceLeft = carToFill.volume - spaceLeft;
        }
        catch
        {
            Debug.LogError($"Unable to fetch content of trashcar with id: {id}");
        }
    }

    private void updateCan(RESTCall call, float amount)
    {
        call.setContent(amount);
    }

    private void updateCar(TrashCan can)
    {
        float amount = can.current_amount;
        trashCarCall.addContentToTrashCar(amount, can.Id);
    }
>>>>>>> 17b7a95ff6f97721ad60bea11c7a4257559886a3
}