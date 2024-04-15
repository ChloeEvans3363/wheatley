using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAT : MonoBehaviour
{

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WorldState initialState = new WorldState(MapManager.Instance.currentMap);
            Action[] plan = Planner.plan(initialState, 4);
            Debug.Log("DFS Plan:");
            foreach (Action action in plan)
                Debug.Log(action);
            Debug.Log("");
        }
    }
}
