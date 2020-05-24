<<<<<<< HEAD
﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetRequestTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
            callback(request);
        }
    }
    public void GetPosts()
    {
        StartCoroutine(GetRequest("https://vpstxcvvc2zdblp-db202002241040.adb.eu-frankfurt-1.oraclecloudapps.com/ords/smartcitydb/trashcan/get/1", (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                Debug.Log(req.downloadHandler.text);
                RootObject root = JsonConvert.DeserializeObject<RootObject>(req.downloadHandler.text);
                Debug.Log(root.items[0].current_amount);
            }
        }));
    }
    [Serializable]
    public class Item
    {
        public int current_amount { get; set; }
    }

    [Serializable]
    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    [Serializable]
    public class RootObject
    {
        public List<Item> items { get; set; }
        public bool hasMore { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public List<Link> links { get; set; }
    }
}
=======
﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetRequestTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
            callback(request);
        }
    }
    public void GetPosts()
    {
        StartCoroutine(GetRequest("https://vpstxcvvc2zdblp-db202002241040.adb.eu-frankfurt-1.oraclecloudapps.com/ords/smartcitydb/trashcan/get/1", (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                Debug.Log(req.downloadHandler.text);
                RootObject root = JsonConvert.DeserializeObject<RootObject>(req.downloadHandler.text);
                Debug.Log(root.items[0].current_amount);
            }
        }));
    }
    [Serializable]
    public class Item
    {
        public int current_amount { get; set; }
    }

    [Serializable]
    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    [Serializable]
    public class RootObject
    {
        public List<Item> items { get; set; }
        public bool hasMore { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public List<Link> links { get; set; }
    }
}
>>>>>>> 17b7a95ff6f97721ad60bea11c7a4257559886a3
