using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaxiMoveToPlayer : MonoBehaviour
{
    public NavMeshAgent agent;
    public KeyCode summonTaxiKey;
    public KeyCode enterTaxiKey;
    public GameObject player;
    public GameObject taxiParent;
    public GameObject taxi;
    public Transform enterMapPoint;
    public  Transform exitMapPoint;
    private Vector3 playerPos;
    private bool keyPressed = false;
    public float waitTime;
    private bool arrived = false;
    private float distanceFromPlayer;
    private float timeLeftToReturn;
    private Vector3 enter;
    private Vector3 exit;
    // Update is called once per frame
    void Start()
    {
        enter = enterMapPoint.transform.position;
        exit = exitMapPoint.transform.position;
        timeLeftToReturn = waitTime; 
    }
    void Update()
    {
        distanceFromPlayer = (player.transform.position.magnitude - agent.transform.position.magnitude);
        //Set arrived to destination
        if(keyPressed && (!agent.pathPending))
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                arrived = true;
            }
        }
        //If player exists and presses the summon key, set agent destination to player
        if (player.activeSelf)
        {
            playerPos = player.transform.position;
            if (Input.GetKey(summonTaxiKey) && (null != playerPos))
            {
                taxi.SetActive(true);
                taxiParent.transform.position = enter;
                agent.SetDestination(playerPos);
                keyPressed = true;
            }
        }
        //Return if player moves away from taxi
        if(arrived && movingAway())
        {
            agent.SetDestination(exit);
            arrived = false;
        }
        //CountDown if arrived to player and player not entering
        //wil return to parkingspace after set time
        if (!Input.GetKey(enterTaxiKey) && arrived)
        {
            timeLeftToReturn -= Time.deltaTime;
            if (timeLeftToReturn <= 0)
            {
                agent.SetDestination(exit);
                arrived = false;
                timeLeftToReturn = waitTime;
            }
        }
        if(Input.GetKey(enterTaxiKey) && arrived)
        {
            arrived = false;
        }
    }
    //Checks if distance from player is increasing
    private bool movingAway()
    {
        float tempDistance = distanceFromPlayer;
        if(distanceFromPlayer > tempDistance)
        {
            return true;
        }
        return false;
    }
}
