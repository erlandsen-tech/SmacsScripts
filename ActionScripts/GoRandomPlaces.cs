using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoRandomPlaces : MonoBehaviour
{
    //This script just makes vehicles go random places so that it looks like there
    //is some life in the city
    public Transform[] wayPoints;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        SelectRandomPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            SelectRandomPoint();
        }
        
    }

    void SelectRandomPoint()
    {
        if (wayPoints.Length == 0)
            return;
        int random = Random.Range(0, wayPoints.Length - 1);
        agent.destination = wayPoints[random].position;
    }
}
