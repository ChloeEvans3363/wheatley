using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Win : Goal
{
    // We set this priority because
    // Getting to the win is always the top priority
    private int priority = 20;

    private float contentment = 1;

    public override int CalculatePriority()
    {
        return priority;
    }

    public override float Contentment()
    {
        return contentment;
    }

    public override bool Satisfied(WorldState state)
    {
        if (state.playerTile.transform.position == state.endTile.transform.position)
            return true;
        return false;

        // if false, set all other goals to have a higher priority
    }
}
