using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    MapManager mapManager;
    Tuple<int, int> position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
            mapManager.MoveObjects(MapManager.DirectionEnum.Up, position);
        else if (Input.GetKeyDown(KeyCode.S))
            mapManager.MoveObjects(MapManager.DirectionEnum.Down, position);
        else if (Input.GetKeyDown(KeyCode.D))
            mapManager.MoveObjects(MapManager.DirectionEnum.Right, position);
        else if (Input.GetKeyDown(KeyCode.A))
            mapManager.MoveObjects(MapManager.DirectionEnum.Left, position);
    }

    public void UpdatePosition(Vector3 newLocation, Tuple<int,int> newMapPosition)
    {
        position = newMapPosition;
        this.transform.position = newLocation;
    }

    public void Setup(MapManager manager, Tuple<int,int> pos)
    {
        mapManager = manager;
        position = pos;
    }
}
