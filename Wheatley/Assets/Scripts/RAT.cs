using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static MapManager;

public class RAT : MonoBehaviour
{
    Stack<DirectionEnum> path = new Stack<DirectionEnum>();
    List<DirectionEnum> utilizedPath = new List<DirectionEnum>();
    private GameObject player;
    private GameObject end;
    Vector3 endPosition;
    private float waitTime = 0.4f;
    private float simTime = 0.25f;
    private int count = 0;
    private bool simulatingPath = false;
    private bool checkedPath = false;

    // The tile the character is moving to.
    private GameObject TargetTile { get; set; } = null;

    void Start()
    {
        player = this.gameObject;
        end = Instance.end;

        Vector3 playerPositon = player.transform.position;
        TargetTile = Instance.CurrentTile((int)playerPositon.x, (int)playerPositon.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instance.loadMap(Instance.currentMap.CleanMap());
            for (int i = MapManager.Instance.currentMap.intendedPath.Length - 1; i >= 0; i--)
            {
                path.Push(MapManager.Instance.currentMap.intendedPath[i]);
            }
            simulatingPath = true;
            StartCoroutine(SimulatePath());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WorldState initialState = new WorldState(MapManager.Instance.currentMap);
            Action[] plan = Planner.plan(initialState, 5);
            endPosition = end.transform.position;

            if (plan != null)
            {
                int test = 0;
                Debug.Log("DFS Plan:");
                foreach (Action action in plan)
                {
                    Debug.Log(test + ": " + action);
                    test++;
                }
                

                StartCoroutine(Move(plan));
            }
            else
            {
                Debug.Log("No plan found");
                ManageScenes.Instance.DisplaySearchFailed();
            }
        }

        if (Instance.playerLocation.Item1 == Instance.currentMap.endLocation.Item1 && Instance.playerLocation.Item2 == Instance.currentMap.endLocation.Item2 && !simulatingPath && !checkedPath)
        {
            bool matched = true;
            if (Instance.currentMap.intendedPath.Length != utilizedPath.Count)
            {
                matched = false;
            }
            else
            {
                for (int i = 0; i < Math.Min(Instance.currentMap.intendedPath.Length, utilizedPath.Count); i++)
                {
                    if (Instance.currentMap.intendedPath[i] != utilizedPath[i])
                    {
                        matched = false;
                        break;
                    }
                }
            }

            if (matched)
            {
                ManageScenes.Instance.DisplaySuccess();
            }
            else
            {
                ManageScenes.Instance.DisplayWrongPath();
            }
            checkedPath = true;
        }
    }

    private IEnumerator Move(Action[] plan)
    {
        while (path.Count > 0)
        {
            utilizedPath.Add(path.Pop());
            Instance.MovePlayer(utilizedPath[utilizedPath.Count - 1]);
            yield return new WaitForSeconds(waitTime);
        }
        if (count < plan.Length)
        {
            Vector3 playerPositon = player.transform.position;
            TargetTile = Instance.CurrentTile((int)playerPositon.x, (int)playerPositon.z);
            path = plan[count].GetDirections(TargetTile, Instance.CurrentTile((int)endPosition.x, (int)endPosition.z));
            count++;
            yield return StartCoroutine(Move(plan));
        }
    }

    private IEnumerator SimulatePath()
    {
        while (path.Count > 0)
        {
            MapManager.Instance.MovePlayer(path.Pop());
            yield return new WaitForSeconds(simTime);
        }
        if (path.Count == 0)
        {
            Instance.loadMap(MapData.mapList[MapManager.currentMapIndex]);
            ManageScenes.Instance.ReloadScene();
            simulatingPath = false;
        }
    }
}
