using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    public Stopwatch timer = new Stopwatch();
    // The tile the character is moving to.
    private GameObject TargetTile { get; set; } = null;

    bool isRandomAI = false;

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
                UnityEngine.Debug.Log("DFS Plan:");
                foreach (Action action in plan)
                {
                    UnityEngine.Debug.Log(test + ": " + action);
                    test++;
                }
                

                StartCoroutine(Move(plan));
                timer.Start();
            }
            else
            {
                UnityEngine.Debug.Log("No plan found");
                ManageScenes.Instance.DisplaySearchFailed();
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            isRandomAI = true;
            timer.Start();
            StartCoroutine(MoveRandom());
        }

        if (Instance.playerLocation.Item1 == Instance.currentMap.endLocation.Item1 && Instance.playerLocation.Item2 == Instance.currentMap.endLocation.Item2 && !simulatingPath && !checkedPath)
        {
            isRandomAI = false;
            timer.Stop();
            bool matched = true;
            UnityEngine.Debug.Log("Path Length: " + utilizedPath.Count);
            UnityEngine.Debug.Log("Time Spent: " + timer.Elapsed.Seconds + "." + timer.ElapsedMilliseconds%1000);
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

    private IEnumerator MoveRandom()
    {
        System.Random random = new System.Random();
        int choice = random.Next(4);
        DirectionEnum dir;
        switch (choice)
        {
            case 0:
                dir = DirectionEnum.Left;
                break;
            case 1:
                dir = DirectionEnum.Right;
                break;
            case 2:
                dir = DirectionEnum.Up;
                break;
            case 3:
                dir = DirectionEnum.Down;
                break;
            default:
                dir = DirectionEnum.Stay;
                break;
        }
        UnityEngine.Debug.Log(dir);
        utilizedPath.Add(dir);
        Instance.MovePlayer(dir);
        //this is the slowdown. feel free to remove the wait to speed testing up :)
        yield return new WaitForSeconds(0.01f);

        if(isRandomAI)
            yield return StartCoroutine(MoveRandom());
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
