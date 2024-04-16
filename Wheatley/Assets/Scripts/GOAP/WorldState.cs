using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapManager;

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
        floorElements = map.floorElements;
        objectsOnMap = map.objectsOnMap;

        playerTile = floorElements[map.playerStart];
        endTile = floorElements[map.endLocation];

        Goals = new List<Goal>();
        Actions = new List<Action>();

        GetMoveableBlocks();
        GetPits();

        Goals.Add(new G_MoveBlock());
        Goals.Add(new G_Win());

        Actions.Add(new A_MoveBlock());
        Actions.Add(new A_GoToWin());
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

    public void DisconnectTiles()
    {
        // Loops through all the blocks in a level
        for (int i = 0; i < MapManager.Instance.currentMap.mapHeights.GetLength(0); i++)
        {
            for (int j = 0; j < MapManager.Instance.currentMap.mapHeights.GetLength(1); j++)
            {
                // Check floor block connections
                if (floorElements.ContainsKey(new Tuple<int, int>(i, j)) &&
                    floorElements[new Tuple<int, int>(i, j)] != null)
                {
                    // Clears connections for the ground blocks
                    floorElements[new Tuple<int, int>(i, j)].GetComponent<Node>().Connections.Clear();
                }

                // Checks object block connections
                if (objectsOnMap.ContainsKey(new Tuple<int, int>(i, j)) &&
                    objectsOnMap[new Tuple<int, int>(i, j)] != null)
                {
                    // Clears connections for the object blocks
                    objectsOnMap[new Tuple<int, int>(i, j)].GetComponent<Node>().Connections.Clear();
                }
            }
        }
    }

    // Needed for AStar
    public void GenerateConnections()
    {

        for (int i = 0; i < MapManager.Instance.currentMap.mapHeights.GetLength(0); i++)
        {
            for (int j = 0; j < MapManager.Instance.currentMap.mapHeights.GetLength(1); j++)
            {
                if (MapManager.Instance.currentMap.mapHeights[i, j] < 0)
                {
                    continue;
                }

                if (i > 0)
                    // and the tile above us is not an empty obstacle...
                    if (floorElements[new Tuple<int, int>(i - 1, j)] != null)
                    {
                        // Checks if there is an object in the same place and if that object is ground level
                        // If so connect the ground to the object
                        if (objectsOnMap.ContainsKey(new Tuple<int, int>(i, j))
                            && 1 == objectsOnMap[new Tuple<int, int>(i, j)].transform.position.y)
                        {
                            connectTiles(floorElements[new Tuple<int, int>(i - 1, j)], DirectionEnum.Down, objectsOnMap[new Tuple<int, int>(i, j)]);
                        }
                        // Otherwise if there isn't an object in the way
                        else if (!objectsOnMap.ContainsKey(new Tuple<int, int>(i, j)))
                        {
                            // Check if the piece above us has an object on ground level
                            // If so connect the ground node to that object
                            if (1 != floorElements[new Tuple<int, int>(i - 1, j)].transform.position.y && objectsOnMap.ContainsKey(new Tuple<int, int>(i - 1, j)))
                            {
                                connectTiles(objectsOnMap[new Tuple<int, int>(i - 1, j)], DirectionEnum.Down, floorElements[new Tuple<int, int>(i, j)]);
                            }
                            // Otherwise if the current ground piece is floor level
                            // Connect the ground piece to another ground piece
                            else if (1 == floorElements[new Tuple<int, int>(i - 1, j)].transform.position.y)
                            {
                                connectTiles(floorElements[new Tuple<int, int>(i - 1, j)], DirectionEnum.Down, floorElements[new Tuple<int, int>(i, j)]);
                            }
                        }
                    }

                // Similarly, if there is at least one column to the left...
                if (j > 0)
                    // and the tile to the left is not an empty obstacle...
                    if (floorElements[new Tuple<int, int>(i, j - 1)] != null)
                    {
                        // Checks if there is an object in the same place and if that object is ground level
                        // If so connect the ground to the object
                        if (objectsOnMap.ContainsKey(new Tuple<int, int>(i, j))
                            && 1 == objectsOnMap[new Tuple<int, int>(i, j)].transform.position.y)
                        {
                            connectTiles(floorElements[new Tuple<int, int>(i, j - 1)], DirectionEnum.Right, objectsOnMap[new Tuple<int, int>(i, j)]);
                        }
                        // Otherwise if there isn't an object in the way
                        else if (!objectsOnMap.ContainsKey(new Tuple<int, int>(i, j)))
                        {
                            // Check if the piece to the left has an object on ground level
                            // If so connect the ground node to that object
                            if (1 != floorElements[new Tuple<int, int>(i, j - 1)].transform.position.y && objectsOnMap.ContainsKey(new Tuple<int, int>(i, j - 1)))
                            {
                                connectTiles(objectsOnMap[new Tuple<int, int>(i, j - 1)], DirectionEnum.Right, floorElements[new Tuple<int, int>(i, j)]);
                            }
                            // Otherwise if the current ground piece is floor level
                            // Connect the ground piece to another ground piece
                            else if (1 - 1 == floorElements[new Tuple<int, int>(i, j - 1)].transform.position.y)
                            {
                                connectTiles(floorElements[new Tuple<int, int>(i, j - 1)], DirectionEnum.Right, floorElements[new Tuple<int, int>(i, j)]);
                            }
                        }
                    }
            }
        }
    }

    private void connectTiles(GameObject from, DirectionEnum direction, GameObject to)
    {
        // Grab the node scripts attached to the two tile game objects.
        Node fromNode = from.GetComponent<Node>();
        Node toNode = to.GetComponent<Node>();

        // The first direction is simple, add it to the from node.
        fromNode.Connections.Add(direction, to);

        if (direction == DirectionEnum.Up)
        {
            toNode.Connections.Add(DirectionEnum.Down, from);
        }
        else if (direction == DirectionEnum.Down)
        {
            toNode.Connections.Add(DirectionEnum.Up, from);
        }
        else if (direction == DirectionEnum.Left)
        {
            toNode.Connections.Add(DirectionEnum.Right, from);
        }
        else if (direction == DirectionEnum.Right)
        {
            toNode.Connections.Add(DirectionEnum.Left, from);
        }
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
            if(objectsOnMap[key] != null && objectsOnMap[key].tag == "MoveableBlock")
                moveableBlocks.Add(key, objectsOnMap[key]);
        }
    }

    public void GetPits()
    {

        foreach (var key in floorElements.Keys)
        {
            // This is the y level for pit blocks
            if(floorElements[key] != null && floorElements[key].transform.position.y == 0)
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
