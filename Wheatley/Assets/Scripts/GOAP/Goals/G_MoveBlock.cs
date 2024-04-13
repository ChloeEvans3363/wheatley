using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_MoveBlock : Goal
{
    public override int OnCalculatePriority()
    {
        return -1;
    }

    public override bool CanRun()
    {
        return false;
    }
}
