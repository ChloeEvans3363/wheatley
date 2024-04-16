using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Win : Goal
{

    private float contentment = 1;


    public override float Contentment()
    {
        return contentment;
    }

}
