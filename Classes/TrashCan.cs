using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrashCan
{
    public int Id { get; set; }
    public float current_amount { get; set; }
    public float xPos { get; set; }
    public float zPos { get; set; }
}
