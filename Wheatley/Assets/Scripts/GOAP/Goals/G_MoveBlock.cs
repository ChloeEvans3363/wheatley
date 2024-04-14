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
        return Mathf.FloorToInt(currentPriority);
    }
    public override void OnGoalActivated()
    {
        currentPriority = maxPriority;
    }

    public override bool CanRun()
    {
        Vector2 playerPositon = MapManager.Instance.player.transform.position;
        Vector2 endPos = end.transform.position;
        Vector2 cratePos = box.transform.position;

        Map map = MapManager.Instance.mapList[MapManager.Instance.currentMap];

        return PushBoxSearch.CanPushBox(map.mapHeights, playerPositon, cratePos, endPos);
    }
}
