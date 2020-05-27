using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets;

public class GetInsideTheVehicle : MonoBehaviour
{
    public NavMeshAgent taxiAgent;
    public KeyCode getInTheVehicle;
    public GameObject CarCam;
    public GameObject Player;
    public GameObject PlayerCam;
    public GameObject ExitTrigger;
    public GameObject TheCar;
    public Transform MapExit;
    public Transform MapEnter;
    public Transform[] points;
    private Vector3 startPos;
    private Vector3 exitPos;
    private Vector3 enterPos;
    private Quaternion parkingDirection;
    private bool triggerCheck;
    private bool isInCar;
    private int destPoint = 0;

    // Start is called before the first frame update
    private void Start()
    {
        isInCar = false;
        if (TheCar.CompareTag("Taxi"))
        {
            exitPos = MapExit.transform.position;
            enterPos = MapEnter.transform.position;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (triggerCheck)
        {
            if (Input.GetKeyDown(getInTheVehicle))
            {
                startPos = Player.transform.position;
                CarCam.SetActive(true);
                Player.SetActive(false);
                PlayerCam.SetActive(false);
                ExitTrigger.SetActive(true);
                isInCar = true;
                taxiAgent.autoBraking = false;
            }
        }
        if (isInCar)
        {
            if (!taxiAgent.pathPending && taxiAgent.remainingDistance < taxiAgent.stoppingDistance)
            {
                GotoNextPoint();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerCheck = true;
    }

    private void OnTriggerExit(Collider other)
    {
        triggerCheck = false;
        isInCar = false;
        destPoint = 0;
        taxiAgent.SetDestination(exitPos);
    }

    private void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;
        // Set the agent to go to the currently selected destination.
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        if (destPoint < points.Length)
        {
            taxiAgent.SetDestination(points[destPoint].position);
            destPoint = destPoint + 1;
        }
        else if (destPoint >= points.Length)
        {
            taxiAgent.destination = startPos;
            destPoint = 0;
            isInCar = false;
        }
    }
}