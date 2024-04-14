using System.Collections;
using System.Collections.Generic;
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
        return AStar.CanFindPath(start, end);
    }

    public override float GetCost(WorldState state)
    {
        GameObject end = state.endTile;
        GameObject start = state.playerTile;
        return AStar.GetCost(start, end);
    }

    public override void OnActivated(WorldState state)
    {
        /*
        GameObject end = MapManager.Instance.end;
        Vector3 playerPositon = MapManager.Instance.player.transform.position;
        GameObject start = MapManager.Instance.currentTile((int)playerPositon.x, (int)playerPositon.z);
        AStar.Search(start, end);
        */

        // Sets the player to be at the end
        state.playerTile.transform.position = state.playerTile.transform.position;
    }
}
