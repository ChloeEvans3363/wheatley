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

    public override float GetCost()
    {
        GameObject end = MapManager.Instance.end;
        Vector3 playerPositon = MapManager.Instance.player.transform.position;
        GameObject start = MapManager.Instance.currentTile((int)playerPositon.x, (int)playerPositon.z);
        return AStar.GetCost(start, end);
    }

    // TODO: Make this return some kind of info to the world state
    public override void OnActivated()
    {
        GameObject end = MapManager.Instance.end;
        Vector3 playerPositon = MapManager.Instance.player.transform.position;
        GameObject start = MapManager.Instance.currentTile((int)playerPositon.x, (int)playerPositon.z);
        AStar.Search(start, end);
    }
}
