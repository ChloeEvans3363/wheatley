using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapManager;

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

    public virtual WorldState OnActivated(WorldState state)
    {
        return null;
    }

    public virtual Stack<DirectionEnum> GetDirections(GameObject start, GameObject end)
    {
        return null;
    }
}
