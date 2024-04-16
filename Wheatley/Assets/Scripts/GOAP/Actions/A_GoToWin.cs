using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A_GoToWin : Action
{
    // Change this later
    List<System.Type> SupportedGoals = new List<System.Type>() { typeof(G_Win)};
    public override List<System.Type> GetSupportedGoals()
    {
        return SupportedGoals;
    }
    public override bool PreconditionsMet(WorldState state)
    {
        GameObject end = state.endTile;
        GameObject start = state.playerTile;
        Debug.Log("start: " + start.transform.position);
        Debug.Log("end " + end.transform.position);

        state.DisconnectTiles();
        state.GenerateConnections();
        Debug.Log("path found: " + AStar.CanFindPath(start, end));
        return AStar.CanFindPath(start, end);
    }

    public override float GetCost(WorldState state)
    {
        GameObject end = state.endTile;
        GameObject start = state.playerTile;

        state.DisconnectTiles();
        state.GenerateConnections();
        return AStar.GetCost(start, end);
    }

    public override WorldState OnActivated(WorldState state)
    {
        WorldState successorState = state.Clone();

        // Sets the player to be at the end
        successorState.playerTile.transform.position = successorState.playerTile.transform.position;

        return successorState;
    }
}
