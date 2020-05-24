<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOutOfThevehicle : MonoBehaviour
{
    public KeyCode getOutOfTheVehicle;
    public GameObject CarCam;
    public GameObject Player;
    public GameObject PlayerCam;
    public GameObject ExitTrigger;
    public GameObject TheCar;
    public GameObject ExitPlace;
    private bool triggerCheck;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(getOutOfTheVehicle))
        {
            CarCam.SetActive(false);
            Player.SetActive(true);
            PlayerCam.SetActive(true);
            Player.transform.position = ExitPlace.transform.position;
            ExitTrigger.SetActive(false);
            if (TheCar.CompareTag("Car"))
            {

            }
        }
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOutOfThevehicle : MonoBehaviour
{
    public KeyCode getOutOfTheVehicle;
    public GameObject CarCam;
    public GameObject Player;
    public GameObject PlayerCam;
    public GameObject ExitTrigger;
    public GameObject TheCar;
    public GameObject ExitPlace;
    private bool triggerCheck;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(getOutOfTheVehicle))
        {
            CarCam.SetActive(false);
            Player.SetActive(true);
            PlayerCam.SetActive(true);
            Player.transform.position = ExitPlace.transform.position;
            ExitTrigger.SetActive(false);
            if (TheCar.CompareTag("Car"))
            {

            }
        }
    }
}
>>>>>>> 17b7a95ff6f97721ad60bea11c7a4257559886a3
