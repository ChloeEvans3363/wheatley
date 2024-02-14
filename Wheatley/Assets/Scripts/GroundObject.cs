using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObject : MonoBehaviour
{
    public Tuple<int, int> location;
    public int height;

    public void SetupData(Tuple<int, int> loc, int h)
    {
        location = loc;
        height = h;
    }
}
