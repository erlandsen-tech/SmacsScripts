using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MoveToWaypoint : MonoBehaviour
{
    // Make entity go to waypoint(s)
    public bool patrol;
    public Transform[] wayPoints;
    public NavMeshAgent agent;
    public GameObject theCar;
    public float parkingSpeed = 1.0F;


    private int wayPointNumber = 0;
    private Vector3 parkingPos;
    private Quaternion parkingRot;
    private float journeyLength;
    private float rotationLength;
    private bool parked;
    private float startTime;
    void Start()
    {
        agent.destination = wayPoints[wayPointNumber].position;
        agent.autoBraking = false;
        parkingPos = wayPoints.Last().position;
        parkingRot = wayPoints.Last().rotation;
        startTime = Time.time;
        journeyLength = Vector3.Distance(theCar.transform.position, parkingPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (!parked)
        {
            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            {
                if (!patrol && wayPointNumber >= wayPoints.Length)
                {
                    StartCoroutine(Parking());
                }
                Move();
            }
        }

    }
    void Move()
    {
        if (wayPoints.Length == 0)
            return;
        if (wayPointNumber < wayPoints.Length)
        {
            agent.destination = wayPoints[wayPointNumber].position;
            wayPointNumber++;
        }
        if (wayPointNumber >= wayPoints.Length && patrol)
        {
            wayPointNumber = 0;
        }
    }
    IEnumerator Parking()
    {
        float timeSinceStarted = 0f;
        float distCovered = (Time.time - startTime) * parkingSpeed;
        float fractionOfJourney = distCovered / journeyLength;
        while (true)
        {
            timeSinceStarted += (Time.deltaTime / 2);
            theCar.transform.position = Vector3.Lerp(theCar.transform.position, parkingPos, fractionOfJourney);
            theCar.transform.rotation = Quaternion.Lerp(theCar.transform.rotation, parkingRot, fractionOfJourney);

            // If the object has arrived, stop the coroutine
            if (theCar.transform.position == parkingPos && theCar.transform.rotation == parkingRot)
            {
                parked = true;
                yield break;
            }
            // Otherwise, continue next frame
            yield return null;
        }
    }
}
