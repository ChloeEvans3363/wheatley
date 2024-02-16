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
        {0,0,0,0,0,1,2},
        {0,0,0,0,0,0,2},
        {0,0,0,0,0,0,2},
        {0,0,0,0,0,0,2},
        {0,0,0,0,0,1,2},
        {0,0,0,0,0,0,2},
        {0,0,0,0,0,0,2},
        {0,0,0,0,0,0,2},
        {0,0,0,0,0,1,2},
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
                floorElement.GetComponent<GroundObject>().SetupData(new Tuple<int, int>(i, j), map[i,j]);
                /*
                // If there is at least one row above us...
                if (i > 0)
                    // and the tile above us is not an empty obstacle...
                    if (map[i - 1, j] != null)
                        // connect the current tile to the one above.
                        connectTiles(map[i - 1, j], DirectionEnum.Down, floorElement);

                // Similarly, if there is at least one column to the left...
                if (j > 0)
                    // and the tile to the left is not an empty obstacle...
                    if (map[i, j - 1] != null)
                        // connect the current tile to the leftward one.
                        connectTiles(map[i, j - 1], DirectionEnum.Right, floorElement);
                */

                if (i == playerLocation.Item1 && j == playerLocation.Item2)
                {
                    GameObject player = Instantiate(playerPrefab, new Vector3(i, map[i, j] + 1, j), Quaternion.identity, this.transform);
                    objectsOnMap.Add(new Tuple<int, int>(i, j), player);
                }
            }
        }

        Camera.main.transform.position = new Vector3(map.GetUpperBound(0)/2, 10, map.GetLowerBound(1)-3);
    }

    //Check tile for object, let object recursively call check tile 
    //if no object and available space, move objects 
    bool CheckTile(DirectionEnum direction, Tuple<int,int> pos) 
    {
        if (0 <= pos.Item1 && pos.Item1 < map.GetLength(0) && 0 <= pos.Item2 && pos.Item2 < map.GetLength(1))
        {
            if (objectsOnMap.ContainsKey(pos))
            {
                Tuple<int, int> newPos = pos;
                Vector3 newPosition = new Vector3();

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

                if (CheckTile(direction, newPos))
                {
                    switch (direction)
                    {
                        case DirectionEnum.Left:
                            newPosition = new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2);
                            break;
                        case DirectionEnum.Right:
                            newPosition = new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2);
                            break;
                        case DirectionEnum.Up:
                            newPosition = new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2);
                            break;
                        case DirectionEnum.Down:
                            newPosition = new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2);
                            break;
                    }
                    MoveObject(newPosition, pos, newPos);
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }
    
    //If movement checks succeeded, recursively move all objects that would be pushed by the player
    public void MovePlayer(DirectionEnum direction)
    {
        Tuple<int, int> newPos = playerLocation;

        switch (direction)
        {
            case DirectionEnum.Left:
                newPos = new Tuple<int, int>(playerLocation.Item1 - 1, playerLocation.Item2);
                if (CheckTile(direction, newPos))
                    UpdatePlayerLocation(new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2), newPos);
                break;
            case DirectionEnum.Right:
                newPos = new Tuple<int, int>(playerLocation.Item1 + 1, playerLocation.Item2);
                if (CheckTile(direction, newPos))
                    UpdatePlayerLocation(new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2), newPos); 
                    break;
            case DirectionEnum.Up:
                newPos = new Tuple<int, int>(playerLocation.Item1, playerLocation.Item2 + 1);
                if (CheckTile(direction, newPos))
                    UpdatePlayerLocation(new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2), newPos); 
                break;
            case DirectionEnum.Down:
                newPos = new Tuple<int, int>(playerLocation.Item1, playerLocation.Item2 - 1);
                if (CheckTile(direction, newPos))
                    UpdatePlayerLocation(new Vector3(newPos.Item1, map[newPos.Item1, newPos.Item2] + 1, newPos.Item2), newPos); 
                break;
            default:
                break;
        }
    }

    void UpdatePlayerLocation(Vector3 newPosition, Tuple<int,int> newPlayerLoc)
    {
        objectsOnMap[playerLocation].transform.position = newPosition;
        objectsOnMap[newPlayerLoc] = objectsOnMap[playerLocation];
        objectsOnMap.Remove(playerLocation);
        playerLocation = newPlayerLoc;
    }

    void MoveObject(Vector3 newPosition, Tuple<int, int> oldLoc, Tuple<int, int> newLoc)
    {
        objectsOnMap[oldLoc].transform.position = newPosition;
        objectsOnMap[newLoc] = objectsOnMap[oldLoc];
        objectsOnMap.Remove(oldLoc);
    }

    public void SetControlledObject(Tuple<int,int> newLoc)
    {
        playerLocation = newLoc;
    }

    private void connectTiles(GameObject from, DirectionEnum direction, GameObject to)
    {
        // Grab the node scripts attached to the two tile game objects.
        Node fromNode = from.GetComponent<Node>();
        Node toNode = to.GetComponent<Node>();

        // The first direction is simple, add it to the from node.
        fromNode.Connections.Add(direction, to);

        if(direction == DirectionEnum.Up)
        {
            toNode.Connections.Add(DirectionEnum.Down, from);
        }
        else if(direction == DirectionEnum.Down)
        {
            toNode.Connections.Add(DirectionEnum.Up, from);
        }
        else if (direction == DirectionEnum.Left)
        {
            toNode.Connections.Add(DirectionEnum.Right, from);
        }
        else if(direction == DirectionEnum.Right)
        {
            toNode.Connections.Add(DirectionEnum.Left, from);
        }
    }

    //Trigger Baba-Is-You map reading to determine if new rules have been added
    void UpdateTile() { }

    //Generate Map
    void MapLoader() { }
}
