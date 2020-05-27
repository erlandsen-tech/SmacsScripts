using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public GameObject pedPrefab1;
    public GameObject pedPrefab2;
    public int pedestriansToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int count = 0;
        int pedNr;
        GameObject ped;



        while (count < pedestriansToSpawn)
        {
            pedNr = Mathf.RoundToInt(Random.Range(0f, 1f));
            if(pedNr == 0)
            {
                ped = pedPrefab1;
            }
            else
            {
                ped = pedPrefab2;
            }

            GameObject obj = Instantiate(ped);
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<WaypointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();
            obj.transform.position = child.position;

            yield return new WaitForEndOfFrame();

            count++;
        }

    }


}
