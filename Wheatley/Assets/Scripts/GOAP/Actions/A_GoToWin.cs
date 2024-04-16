using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A_GoToWin : Action
{
    public override bool PreconditionsMet(WorldState state)
    {
        GameObject end = state.CurrentTile((int)state.endTilePos.x, (int)state.endTilePos.z);
        GameObject start = state.CurrentTile((int)state.playerTilePos.x, (int)state.playerTilePos.z);

        state.DisconnectTiles();
        state.GenerateConnections();
        return AStar.CanFindPath(start, end);
    }

    public override float GetCost(WorldState state)
    {
        GameObject end = state.CurrentTile((int)state.endTilePos.x, (int)state.endTilePos.z);
        GameObject start = state.CurrentTile((int)state.playerTilePos.x, (int)state.playerTilePos.z);

        state.DisconnectTiles();
        state.GenerateConnections();
        return AStar.GetCost(start, end);
    }

    public override WorldState OnActivated(WorldState state)
    {
        WorldState successorState = state.Clone();

        // Sets the player to be at the end
        successorState.playerTilePos = successorState.endTilePos;

        return successorState;
    }

    public override Stack<MapManager.DirectionEnum> GetDirections(GameObject start, GameObject end)
    {
        MapManager.Instance.DisconnectTiles();
        MapManager.Instance.GenerateConnections();
        return AStar.Search(start, end);
    }
}
