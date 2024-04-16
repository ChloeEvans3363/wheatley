using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class A_MoveBlock : Action
{

    // Change this later
    List<System.Type> SupportedGoals = new List<System.Type>() { typeof(G_MoveBlock) };
    public override List<System.Type> GetSupportedGoals()
    {
        return SupportedGoals;
    }

    public override bool PreconditionsMet(WorldState state)
    {
        Map map = MapManager.Instance.currentMap;

        if (state.pits.Count > 0 && state.moveableBlocks.Count > 0)
        {
            Vector2 playerPositon = new Vector2(state.playerTile.transform.position.x, state.playerTile.transform.position.z);

            foreach(GameObject block in state.moveableBlocks.Values)
            {
                foreach(GameObject pit in state.pits.Values)
                {
                    Vector2 endPos = new Vector2(pit.transform.position.x, pit.transform.position.z);
                    Vector2 cratePos = new Vector2(block.transform.position.x, block.transform.position.z);

                    if(PushBoxSearch.CanPushBox(map.mapHeights, playerPositon, cratePos, endPos))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public override float GetCost(WorldState state)
    {
        Map map = MapManager.Instance.currentMap;

        if (state.pits.Count > 0 && state.moveableBlocks.Count > 0)
        {
            Vector2 playerPositon = state.playerTile.transform.position;

            foreach (GameObject block in state.moveableBlocks.Values)
            {
                foreach (GameObject pit in state.pits.Values)
                {
                    Vector2 endPos = pit.transform.position;
                    Vector2 cratePos = block.transform.position;

                    if (PushBoxSearch.CanPushBox(map.mapHeights, playerPositon, cratePos, endPos))
                    {
                        return PushBoxSearch.GetCost(map.mapHeights, playerPositon, cratePos, endPos);
                    }
                }
            }
        }

        return 0;
    }

    // TODO: Make change the crate location in the world state
    public override WorldState OnActivated(WorldState state)
    {
        WorldState successorState = state.Clone();
        Map map = MapManager.Instance.currentMap;

        if (successorState.pits.Count > 0 && successorState.moveableBlocks.Count > 0)
        {
            Vector2 playerPositon = successorState.playerTile.transform.position;

            foreach (var bKey in successorState.moveableBlocks.Keys)
            {
                foreach (var pKey in successorState.pits.Keys)
                {
                    Vector2 endPos = successorState.pits[pKey].transform.position;
                    Vector2 cratePos = successorState.moveableBlocks[bKey].transform.position;

                    if (PushBoxSearch.CanPushBox(map.mapHeights, playerPositon, cratePos, endPos))
                    {
                        successorState.moveableBlocks[bKey].transform.position =
                            successorState.pits[pKey].transform.position;
                    }
                }
            }
        }

        return successorState;
    }
}
