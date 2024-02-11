using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;

    public static MapManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    [SerializeField] GameObject floor;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Tuple<int, int> playerLocation = new Tuple<int, int>(1, 1);
    public Dictionary<Tuple<int, int>, GameObject> objectsOnMap;

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

    // Start is called before the first frame update
    void Start()
    {
        objectsOnMap = new Dictionary<Tuple<int, int>, GameObject>();

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                GameObject floorElement = Instantiate(floor, new Vector3(i, map[i,j],j), Quaternion.identity, this.transform);
                floorElement.GetComponent<MapObject>().location = new Tuple<int, int>(i, j);

                if (i == playerLocation.Item1 && j == playerLocation.Item2)
                {
                    GameObject player = Instantiate(playerPrefab, new Vector3(i, map[i, j] + 1, j), Quaternion.identity, this.transform);
                    objectsOnMap.Add(new Tuple<int, int>(i, j), player);
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            MoveObjects(MapManager.DirectionEnum.Up, playerLocation);
        else if (Input.GetKeyDown(KeyCode.S))
            MoveObjects(MapManager.DirectionEnum.Down, playerLocation);
        else if (Input.GetKeyDown(KeyCode.D))
            MoveObjects(MapManager.DirectionEnum.Right, playerLocation);
        else if (Input.GetKeyDown(KeyCode.A))
            MoveObjects(MapManager.DirectionEnum.Left, playerLocation);
    }

    //Check tile for object, let object recursively call check tile 
    //if no object and available space, move objects 
    bool CheckTile(DirectionEnum direction, Tuple<int,int> pos) 
    {
        if (0 <= pos.Item1 && pos.Item1 < map.GetLength(0))
            if (0 <= pos.Item2 && pos.Item2 < map.GetLength(1))
                if(!objectsOnMap.ContainsKey(pos))
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
                    MovePlayer(new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2), newPos);
                break;
            case DirectionEnum.Right:
                if (CheckTile(direction, newPos))
                    MovePlayer(new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2), newPos); 
                    break;
            case DirectionEnum.Up:
                if (CheckTile(direction, newPos))
                    MovePlayer(new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2), newPos); 
                break;
            case DirectionEnum.Down:
                if (CheckTile(direction, newPos))
                    MovePlayer(new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2), newPos); 
                break;
            default:
                break;
        }
    }

    void MovePlayer(Vector3 newPosition, Tuple<int,int> newPlayerLoc)
    {
        objectsOnMap[playerLocation].transform.position = newPosition;
        objectsOnMap[newPlayerLoc] = objectsOnMap[playerLocation];
        objectsOnMap.Remove(playerLocation);
        playerLocation = newPlayerLoc;
    }

    public void SetControlledObject(Tuple<int,int> newLoc)
    {
        playerLocation = newLoc;
    }

    //Trigger Baba-Is-You map reading to determine if new rules have been added
    void UpdateTile() { }

    //Generate Map
    void MapLoader() { }
}
