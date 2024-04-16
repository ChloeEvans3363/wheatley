using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{

    public virtual float GetCost(WorldState state)
    {
        return 0f;
    }

    public virtual bool PreconditionsMet(WorldState state)
    {
        return false;
    }

    // Consider linking a goal to the action here
    public virtual WorldState OnActivated(WorldState state)
    {
        return null;
    }
}
