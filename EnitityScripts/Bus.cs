using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Bus : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] points;
    public float TimeToWaitForPassenger;
    public GameObject[] wheels;
    public GameObject busObject;
    public Rigidbody busRigidBody;

    private int destPoint = 0;
    private bool running;
    private Vector3 lastPos;

    // Start is called before the first frame update
    private void Start()
    {
        running = false;
        lastPos = busObject.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance && !running)
        {
            StartCoroutine(WaitForPassenger());
        }
        startStopWheels();
    }

    private void startStopWheels()
    {
        var curPos = busObject.transform.position;
        if (wheels.Length > 0)
        {
            if (curPos == lastPos)
            {
                foreach (var wheel in wheels)
                {
                    var animator = wheel.GetComponent<Animator>();
                    if (animator.enabled == true)
                    {
                        animator.enabled = false;
                    }
                }
            }
            else
            {
                foreach (var wheel in wheels)
                {
                    var animator = wheel.GetComponent<Animator>();
                    if (animator.enabled == false)
                    {
                        animator.enabled = true;
                    }
                }
            }
        }
        lastPos = curPos;
    }

    private void GotoNextPoint()
    {
        //Exit if no waypoints are set
        if (points.Length == 0)
            return;
        if (destPoint < points.Length)
        {
            agent.destination = points[destPoint].position;
            destPoint = destPoint + 1;
        }
        //Roll back to 0 if end is reached
        else if (destPoint >= points.Length)
        {
            destPoint = 0;
        }
    }

    //Some ugly hacks here to get the bus moving like we want.
    //Fillers are waypoints where it should not wait for passengers.
    //These are inserted to make a transition from a bus stop to another
    //without turning around in the middle of the street.
    private IEnumerator WaitForPassenger()
    {
        if (destPoint < points.Length)
        {
            if (destPoint - 1 < 0)
            {
                agent.autoBraking = false;
                GotoNextPoint();
            }
            else
            {
                if (points[destPoint - 1].tag == "filler")
                {
                    agent.autoBraking = false;
                    GotoNextPoint();
                }
                else
                {
                    running = true;
                    agent.autoBraking = true;
                    yield return new WaitForSeconds(TimeToWaitForPassenger);
                    GotoNextPoint();
                    running = false;
                }
            }
        }
        else
        {
            GotoNextPoint();
        }
    }
}