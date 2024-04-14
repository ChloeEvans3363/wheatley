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

    public override float GetCost()
    {
        Vector2 playerPositon = MapManager.Instance.player.transform.position;
        Vector2 endPos = end.transform.position;
        Vector2 cratePos = box.transform.position;

        Map map = MapManager.Instance.mapList[MapManager.Instance.currentMap];

        return PushBoxSearch.GetCost(map.mapHeights, playerPositon, cratePos, endPos);
    }

    // TODO: Make this return some kind of info to the world state
    public override void OnActivated()
    {
        Vector2 playerPositon = MapManager.Instance.player.transform.position;
        Vector2 endPos = end.transform.position;
        Vector2 cratePos = box.transform.position;

        Map map = MapManager.Instance.mapList[MapManager.Instance.currentMap];

        PushBoxSearch.PushBoxPathSearch(map.mapHeights, playerPositon, cratePos, endPos);
    }
}
