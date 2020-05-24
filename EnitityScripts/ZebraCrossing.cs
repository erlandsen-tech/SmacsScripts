using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZebraCrossing : MonoBehaviour
{
    private bool personCrossing;
    private List<GameObject> vehicles;
    private List<NavMeshAgent> agents;

    // Start is called before the first frame update
    void Start()
    {
        personCrossing = false;
        agents = new List<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                personCrossing = true;
                break;
            case "Vehicle":
                if (personCrossing)
                    StopAgent(other.gameObject);
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                if (personCrossing)
                {
                    personCrossing = false;
                    StartAgents(agents);
                }
                break;
        }
    }

    private void StopAgent(GameObject vehicle)
    {
        NavMeshAgent vehicleAgent = vehicle.GetComponent<NavMeshAgent>();
        vehicleAgent.isStopped = true;
        agents.Add(vehicleAgent);
    }
    private void StartAgents(List<NavMeshAgent> vehicles)
    {
        //start all agents in list
        foreach(NavMeshAgent vehicle in vehicles)
        {
            vehicle.isStopped = false;
        }
        //reset list
        agents = new List<NavMeshAgent>();
    }
}
