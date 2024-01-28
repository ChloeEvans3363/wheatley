using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject floor;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Tuple<int,int> playerStartLocation = new Tuple<int,int>(1, 1);

    public enum DirectionEnum
    {
        Left, Right, Up, Down
    }

    //Base Map Concept
    int[,] map =
    {
        {0,0,0,1,2},
        {0,0,0,0,2},
        {0,0,0,1,2},
    };

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Instantiate(floor, new Vector3(i * 1.05f, map[i,j],j*1.05f), Quaternion.identity, this.transform);

                if (i == playerStartLocation.Item1 && j == playerStartLocation.Item2)
                {
                    playerController = Instantiate(playerPrefab, new Vector3(i * 1.05f, map[i,j]+1, j * 1.05f), Quaternion.identity, this.transform).GetComponent<PlayerController>();
                    playerController.Setup(this, playerStartLocation);
                }
            }
        }
    }

    //Check tile for object, let object recursively call check tile 
    //if no object and available space, move objects 
    bool CheckTile(DirectionEnum direction, Tuple<int,int> pos) 
    {
        if (0 <= pos.Item1 && pos.Item1 < map.GetLength(0))
            if (0 <= pos.Item2 && pos.Item2 < map.GetLength(1))
                return true;
        return false;
    }

    //If movement checks succeeded, recursively move all objects that would be pushed by the player
    public void MoveObjects(DirectionEnum direction, Tuple<int,int> pos) 
    {
        Tuple<int, int> newPos = pos;

        switch (direction)
        {
            case DirectionEnum.Left:
                newPos = new Tuple<int, int>(pos.Item1 - 1, pos.Item2);
                break;
            case DirectionEnum.Right:
                newPos = new Tuple<int, int>(pos.Item1 + 1, pos.Item2);
                break;
            case DirectionEnum.Up:
                newPos = new Tuple<int, int>(pos.Item1, pos.Item2 + 1);
                break;
            case DirectionEnum.Down:
                newPos = new Tuple<int, int>(pos.Item1, pos.Item2 - 1);
                break;
        }

        switch (direction)
        {
            case DirectionEnum.Left:
                if (CheckTile(direction, newPos))
                    playerController.UpdatePosition(new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2), newPos);
                break;
            case DirectionEnum.Right:
                if (CheckTile(direction, newPos))
                    playerController.UpdatePosition(new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2), newPos); 
                    break;
            case DirectionEnum.Up:
                if (CheckTile(direction, newPos))
                    playerController.UpdatePosition(new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2), newPos); 
                break;
            case DirectionEnum.Down:
                if (CheckTile(direction, newPos))
                    playerController.UpdatePosition(new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2), newPos); 
                break;
            default:
                break;
        }

    }

    //Trigger Baba-Is-You map reading to determine if new rules have been added
    void UpdateTile() { }

    //Generate Map
    void MapLoader() { }
}
