using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JsonObject
{
    public int Id { get; set; }
    public List<Item> items { get; set; }
    public bool hasMore { get; set; }
    public int limit { get; set; }
    public int offset { get; set; }
    public int count { get; set; }
    public List<Link> links { get; set; }
}

[Serializable]
public class Item
{
    public float current_amount { get; set; }
    public float count { get; set; }
    public int occupied { get; set; }
    public float volume { get; set; }
    public float xpos { get; set; }
    public float zpos { get; set; }
}

[Serializable]
public class Link
{
    public string rel { get; set; }
    public string href { get; set; }
}