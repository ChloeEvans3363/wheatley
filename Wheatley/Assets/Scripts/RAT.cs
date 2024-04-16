using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static MapManager;

public class RAT : MonoBehaviour
{
    Stack<DirectionEnum> path = new Stack<DirectionEnum>();
    private GameObject player;
    private GameObject end;
    Vector3 endPosition;
    private float waitTime = 1f;

    private int count = 0;

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

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WorldState initialState = new WorldState(MapManager.Instance.currentMap);
            Action[] plan = Planner.plan(initialState, 4);
            endPosition = end.transform.position;

            if (plan != null)
            {
                /*
                Debug.Log("DFS Plan:");
                foreach (Action action in plan)
                    Debug.Log(action);
                */

                StartCoroutine(Move(plan));
            }
            else
            {
                Debug.Log("No plan found");
                ManageScenes.Instance.DisplaySearchFailed();
            }
        }
    }

    private IEnumerator Move(Action[] plan)
    {
        while (path.Count > 0)
        {
            Instance.MovePlayer(path.Pop());
            yield return new WaitForSeconds(waitTime);
        }

        if (count < plan.Length)
        {
            path = plan[count].GetDirections(TargetTile, Instance.CurrentTile((int)endPosition.x, (int)endPosition.z));
            count++;
            yield return StartCoroutine(Move(plan));
        }
    }
}
