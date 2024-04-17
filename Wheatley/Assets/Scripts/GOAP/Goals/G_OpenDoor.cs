using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_OpenDoor : Goal
{
    private float contentment = 0;

    public override float Contentment()
    {
        return contentment;
    }
}
