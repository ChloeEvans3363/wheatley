using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_MoveBlock : Goal
{
    private int minPriority = 0;
    private int maxPriority = 10;

    private float priorityBuildRate = 1f;
    private float currentPriority = 0f;

    // Find a way to get this from world state
    public GameObject end;
    public GameObject box;

    public override int CalculatePriority()
    {
        // TODO: Calculate priority based on if there are any
        // Movable boxes on the field
        //return Mathf.FloorToInt(currentPriority);
        return 1;
    }
    public override void OnGoalActivated()
    {
        currentPriority = maxPriority;
    }

    // TODO: Change this so that it instead says that the
    // goal is satisfied when the block location is equal
    // to the given location
    public override bool Satisfied(WorldState state)
    {
        Vector2 playerPositon = state.playerTile.transform.position;
        Vector2 endPos = state.endTile.transform.position;
        Vector2 cratePos = box.transform.position;

        Map map = MapManager.Instance.mapList[MapManager.Instance.currentMap];

        return PushBoxSearch.CanPushBox(map.mapHeights, playerPositon, cratePos, endPos);
    }
}
