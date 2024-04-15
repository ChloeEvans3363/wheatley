using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState
{
    // This is all the stuff we need to know
    // To do certain actions

    List<Goal> Goals;

    List<Action> Actions;

    public Stack<Action> SatisfiedActions { get; set; } = null;

    public Dictionary<Tuple<int, int>, GameObject> floorElements = 
        new Dictionary<Tuple<int, int>, GameObject>();

    public Dictionary<Tuple<int, int>, GameObject> objectsOnMap =
        new Dictionary<Tuple<int, int>, GameObject>();

    public Dictionary<Tuple<int, int>, GameObject> pits =
        new Dictionary<Tuple<int, int>, GameObject>();

    public Dictionary<Tuple<int, int>, GameObject> moveableBlocks =
        new Dictionary<Tuple<int, int>, GameObject>();

    public GameObject playerTile;

    public GameObject endTile;

    public WorldState(Map map)
    {
        Vector3 playerPositon = MapManager.Instance.player.transform.position;
        playerTile = MapManager.Instance.currentTile((int)playerPositon.x, (int)playerPositon.z);
        endTile = MapManager.Instance.end;

        floorElements = map.floorElements;
        objectsOnMap = map.objectsOnMap;

        GetMoveableBlocks();
        GetPits();

        Goals.Add(new G_Win());
        Goals.Add(new G_MoveBlock());

        Actions.Add(new A_GoToWin());
        Actions.Add(new A_MoveBlock());
    }

    public WorldState(WorldState state)
    {
        playerTile = state.playerTile;
        floorElements = state.floorElements;
        objectsOnMap = state.objectsOnMap;
        endTile = state.endTile;

        moveableBlocks = state.moveableBlocks;
        pits = state.pits;

        Goals = new List<Goal>(state.Goals);
        Actions = new List<Action>(state.Actions);
    }

    public float GetContentment()
    {
        float contentment = 0;

        foreach (Goal goal in Goals)
            contentment += goal.Contentment();

        return contentment;
    }

    public Action NextAction()
    {
        if (SatisfiedActions == null)
            GenerateActions();

        if (SatisfiedActions.Count > 0)
            return SatisfiedActions.Pop();

        return null;
    }

    private void GenerateActions()
    {
        SatisfiedActions = new Stack<Action>();
        foreach(Action action in Actions)
            if(action.PreconditionsMet(this))
                SatisfiedActions.Push(action);
    }

    public WorldState Clone()
    {
        WorldState stateClone = new WorldState(this);
        return stateClone;
    }

    public bool GoalAchieved()
    {
        if (playerTile.transform.position == endTile.transform.position)
            return true;
        return false;
    }

    public void GetMoveableBlocks()
    {

        foreach (var key in objectsOnMap.Keys)
        {
            if(objectsOnMap[key].tag == "MoveableBlock")
                moveableBlocks.Add(key, objectsOnMap[key]);
        }
    }

    public void GetPits()
    {

        foreach (var key in floorElements.Keys)
        {
            // This is the y level for pit blocks
            if(floorElements[key].transform.position.y == 0)
                pits.Add(key, floorElements[key]);
        }
    }

    public Dictionary<Tuple<int, int>, GameObject> GetDoors()
    {
        return null;
    }

    public Dictionary<Tuple<int, int>, GameObject> GetKeys()
    {
        return null;
    }
}
