using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Win : Goal
{
    // We set this priority because
    // Getting to the win is always the top priority
    private int priority = 20;

    public override int OnCalculatePriority()
    {
        return priority;
    }

    public override bool CanRun()
    {
        GameObject end = MapManager.Instance.end;
        Vector3 playerPositon = MapManager.Instance.player.transform.position;
        GameObject start = MapManager.Instance.currentTile((int)playerPositon.x, (int)playerPositon.z);
        return AStar.CanFindPath(start, end);
    }
}
