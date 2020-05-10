using Newtonsoft.Json;
using System.Collections;
using UnityEngine;

public class TrashCanScript : MonoBehaviour
{
    public GameObject entity;
    public Light[] lights;
    public int BlinkTime;
    private float timer;
    private const int TIME_BETWEEN_CALLS = 1;
    private float timeLeftToCall = TIME_BETWEEN_CALLS;
    public int id;
    private string json;
    private bool blinking;
    private RESTCall call;

    // At start get content from database
    private void Start()
    {
        call = entity.GetComponent<RESTCall>();
        blinking = false;
        timer = BlinkTime;
        foreach (Light l in lights)
        {
            l.enabled = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        timeLeftToCall -= Time.deltaTime;
        if (timeLeftToCall <= 0)
        {
            StartCoroutine(isFull());
            timeLeftToCall = TIME_BETWEEN_CALLS;
        }
        if (blinking)
            BlinkingLight();
        else
        {
            foreach (Light l in lights)
            {
                l.enabled = false;
            }
        }
    }

    //Light blinks if trashcan is full.
    private void BlinkingLight()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            foreach (Light l in lights)
            {
                l.enabled = !l.enabled;
            }
            timer = BlinkTime;
        }
    }

    //Checks if this bin is full. If it IS full, the light starts to blink
    private IEnumerator isFull()
    {
        float amount;
        yield return StartCoroutine(call.getContent(content => json = content));
        try
        {
            JsonObject can = JsonConvert.DeserializeObject<JsonObject>(json);
            amount = can.items[0].current_amount;
            if (isFull(amount))
                blinking = true;
            else
                blinking = false;
        }
        catch
        {
            Debug.LogError("Unable to parse json from TrashcanScript");
        }
    }

    private bool isFull(float amount)
    {
        return (amount >= 85);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Before tag on can");
    //    if (other.gameObject.CompareTag("trashcar"))
    //    {
    //        Debug.Log("Collison detected with trashcar");
    //    }
    //}
}