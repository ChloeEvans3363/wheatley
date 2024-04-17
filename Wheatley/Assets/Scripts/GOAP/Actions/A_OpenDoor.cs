using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapManager;

public class A_OpenDoor : Action
{
    public GameObject aKey;
    public GameObject aDoor;
    public override bool PreconditionsMet(WorldState state)
    {
        Map map = Instance.currentMap;

        if(state.doors.Count > 0 && state.doorKeys.Count > 0)
        {
            Vector2 playerPositon = new Vector2(state.playerTilePos.x, state.playerTilePos.z);

            foreach(var kKey in state.doorKeys.Keys)
            {
                foreach(var dKey in state.doors.Keys)
                {
                    if(!(state.doorKeys.ContainsKey(dKey) &&
                        state.doors.ContainsKey(kKey)))
                    {
                        Vector2 doorPos = new Vector2(state.doors[dKey].transform.position.x, 
                            state.doors[dKey].transform.position.z);

                        Vector2 keyPos = new Vector2(state.doorKeys[kKey].transform.position.x,
                            state.doorKeys[kKey].transform.position.z);

                        if (PushBoxSearch.CanPushBox(map.mapHeights, playerPositon, keyPos, doorPos))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public override float GetCost(WorldState state)
    {
        Map map = Instance.currentMap;

        if (state.doors.Count > 0 && state.doorKeys.Count > 0)
        {
            Vector2 playerPositon = new Vector2(state.playerTilePos.x, state.playerTilePos.z);

            foreach (var kKey in state.doorKeys.Keys)
            {
                foreach (var dKey in state.doors.Keys)
                {
                    if (!(state.doorKeys.ContainsKey(dKey) &&
                        state.doors.ContainsKey(kKey)))
                    {
                        Vector2 doorPos = new Vector2(state.doors[dKey].transform.position.x,
                            state.doors[dKey].transform.position.z);

                        Vector2 keyPos = new Vector2(state.doorKeys[kKey].transform.position.x,
                            state.doorKeys[kKey].transform.position.z);

                        if (PushBoxSearch.CanPushBox(map.mapHeights, playerPositon, keyPos, doorPos))
                        {
                            return PushBoxSearch.GetCost(map.mapHeights, playerPositon,
                                keyPos, doorPos);
                        }
                    }
                }
            }
        }
        return 0;
    }

    public override WorldState OnActivated(WorldState state)
    {
        WorldState successorState = state.Clone();
        Map map = Instance.currentMap;

        if (successorState.doors.Count > 0 && successorState.doorKeys.Count > 0)
        {
            Vector2 playerPositon = new Vector2(state.playerTilePos.x, state.playerTilePos.z);

            foreach (var kKey in successorState.doorKeys.Keys)
            {
                foreach (var dKey in successorState.doors.Keys)
                {
                    if (!(successorState.doorKeys.ContainsKey(dKey)
                        && successorState.doors.ContainsKey(kKey)))
                    {
                        Vector2 doorPos = new Vector2(successorState.doors[dKey].transform.position.x,
                            successorState.doors[dKey].transform.position.z);

                        Vector2 keyPos = new Vector2(successorState.objectsPositions[kKey].x,
                            successorState.objectsPositions[kKey].z);

                        if (PushBoxSearch.CanPushBox(map.mapHeights, playerPositon, keyPos, doorPos))
                        {
                            Stack<DirectionEnum> moves = PushBoxSearch.PushBoxPathSearch(map.mapHeights, playerPositon, keyPos, doorPos);
                            Vector2 newPos = PushBoxSearch.PosAfterMoves(moves, playerPositon);
                            state.playerTilePos = newPos;

                            aDoor = successorState.objectsOnMap[dKey];
                            aKey = successorState.objectsOnMap[kKey];

                            successorState.objectsOnMap.Remove(kKey);
                            successorState.objectsOnMap.Remove(dKey);
                            successorState.objectsPositions.Remove(kKey);
                            successorState.objectsPositions.Remove(dKey);
                            successorState.doorKeys.Remove(kKey);
                            successorState.doors.Remove(dKey);

                            return successorState;
                        }
                    }
                }
            }
        }

        return successorState;
    }

    public override Stack<DirectionEnum> GetDirections(GameObject start, GameObject end)
    {
        Vector2 playerPos = new Vector2(start.transform.position.x, start.transform.position.z);
        Vector2 keyPos = new Vector2(aKey.transform.position.x, aKey.transform.position.z);
        Vector2 doorPos = new Vector2(aDoor.transform.position.x, aDoor.transform.position.z);
        Map map = Instance.currentMap;

        return PushBoxSearch.PushBoxPathSearch(map.mapHeights, playerPos,
            keyPos, doorPos);
    }
}
