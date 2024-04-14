using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState
{
    // This is all the stuff we need to know
    // To do certain actions

    public Dictionary<Goal, float> goals = new Dictionary<Goal, float>();

    public Dictionary<Tuple<int, int>, GameObject> floorElements = 
        new Dictionary<Tuple<int, int>, GameObject>();

    public Dictionary<Tuple<int, int>, GameObject> objectsOnMap =
    new Dictionary<Tuple<int, int>, GameObject>();

    public GameObject playerTile;

    public GameObject endTile;

    public bool GoalAchieved()
    {
        if (playerTile.transform.position == endTile.transform.position)
            return true;
        return false;
    }
}
