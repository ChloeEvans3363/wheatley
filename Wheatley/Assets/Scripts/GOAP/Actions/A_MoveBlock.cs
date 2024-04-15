using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class A_MoveBlock : Action
{
    // Find a way to get this from world state
    public GameObject end;
    public GameObject box;

    // Change this later
    List<System.Type> SupportedGoals = new List<System.Type>() { typeof(G_MoveBlock) };
    public override List<System.Type> GetSupportedGoals()
    {
        return SupportedGoals;
    }

    public override bool PreconditionsMet(WorldState state)
    {
        if(state.objectsOnMap != null)
        {
            Vector2 playerPositon = state.playerTile.transform.position;
            Vector2 endPos = state.endTile.transform.position;
            Vector2 cratePos = box.transform.position;

            Map map = MapManager.Instance.currentMap;

            return PushBoxSearch.CanPushBox(map.mapHeights, playerPositon, cratePos, endPos);
        }
        return false;
    }

    public override float GetCost(WorldState state)
    {
        Vector2 playerPositon = MapManager.Instance.player.transform.position;
        Vector2 endPos = end.transform.position;
        Vector2 cratePos = box.transform.position;

        Map map = MapManager.Instance.currentMap;

        return PushBoxSearch.GetCost(map.mapHeights, playerPositon, cratePos, endPos);
    }

    // TODO: Make change the crate location in the world state
    public override void OnActivated(WorldState state)
    {
        Vector2 playerPositon = MapManager.Instance.player.transform.position;
        Vector2 endPos = end.transform.position;
        Vector2 cratePos = box.transform.position;

        Map map = MapManager.Instance.currentMap;

        PushBoxSearch.PushBoxPathSearch(map.mapHeights, playerPositon, cratePos, endPos);
    }
}
