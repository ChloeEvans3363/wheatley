using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

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
    [SerializeField] GameObject endPrefab;
    [SerializeField] Tuple<int, int> playerLocation = new Tuple<int, int>(0, 6);
    public GameObject player;
    public GameObject end;

    public List<Map> mapList = new List<Map>();
    public int currentMap;

    public enum DirectionEnum
    {
        Left, Right, Up, Down, Stay
    }

    int[,] map1 =
    {
            {-1,-1,-1,-1,-1,-1,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {-1,-1,-1,-1,-1,-1,0},
    };
    int[,] map2 =
    {
            {-1,-1, 1, 1,-1, 1,-1},
            {-1, 1, 1, 1, 1, 1,-1},
            {-1, 1, 1, 1, 1, 1,-1},
            {-1,-1,-1,-1,-1, 0,-1},
            {-1,-1,-1,-1,-1, 1,-1},
            {-1,-1,-1,-1,-1, 1,-1},
    };

    int[,] path1 =
    {
        {0, 1, 1, 1, 1, 1, 1, 1, 2, 3, 3, 3, 3, 3, 3, 3, 4, 5, 5, 5, 5, 5, 5, 5, 6, 7, 7, 7, 7, 7, 7, 7, 8},
        {6, 6, 5, 4, 3, 2, 1 ,0, 0, 0, 1, 2, 3, 4, 5, 6, 6, 6, 5, 4, 3, 2, 1, 0, 0, 0, 1, 2, 3, 4, 5, 6, 6},
    };
    int[,] path2 =
    {
        {0, 1, 1, 1, 0, 0, 1, 1, 2, 2, 2, 2, 1, 1, 2, 3, 4, 5},
        {5, 5, 4, 3, 3, 2, 2, 1, 1, 2, 3, 4, 4, 5, 5, 5, 5, 5},
    };

    // Start is called before the first frame update
    void Start()
    {
        currentMap = 1;
        mapList.Add(new Map());
        mapList.Add(new Map());

        mapList[0].mapHeights = map1;
        mapList[1].mapHeights = map2;

        for (int i = 0; i < path1.GetLength(1); i++)
        {
            mapList[0].intendedPath.Add(new Tuple<int, int>(path1[0, i], path1[1, i]));
        }
        for (int i = 0; i < path2.GetLength(1); i++)
        {
            mapList[1].intendedPath.Add(new Tuple<int, int>(path2[0, i], path2[1, i]));
        }

        mapList[0].endLocation = new Tuple<int, int>(8, 6);
        mapList[1].endLocation = new Tuple<int, int>(5, 5);

        mapList[0].playerStart = new Tuple<int, int>(0, 6);
        mapList[1].playerStart = new Tuple<int, int>(0, 5);

        loadMap(mapList[currentMap]);
    }

    private void loadMap(Map map)
    {
        playerLocation = map.playerStart;
        map.objectsOnMap = new Dictionary<Tuple<int, int>, GameObject>();
        map.floorElements = new Dictionary<Tuple<int, int>, GameObject>();

        if (map.mapHeights[0, 0] >= 0)
            createTile(0, 0, new Tuple<int, int>(0, 0));
        else
            map.floorElements.Add(new Tuple<int, int>(0, 0), null);

        for (int i = 0; i < map.mapHeights.GetLength(0); i++)
        {
            for (int j = 0; j < map.mapHeights.GetLength(1); j++)
            {
                if (map.mapHeights[i, j] < 0 && !map.floorElements.ContainsKey(new Tuple<int, int>(i, j)))
                {
                    map.floorElements.Add(new Tuple<int, int>(i, j), null);
                    continue;
                }
                GameObject newTile = createTile(i, j, new Tuple<int, int>(i, j));

                if (i == playerLocation.Item1 && j == playerLocation.Item2)
                {
                    player = Instantiate(playerPrefab, new Vector3(i, map.mapHeights[i, j] + 1, j), Quaternion.identity, this.transform);
                    //map.objectsOnMap.Add(new Tuple<int, int>(i, j), player);
                }

                if (i == mapList[currentMap].endLocation.Item1 && j == mapList[currentMap].endLocation.Item2)
                {
                    end = Instantiate(endPrefab, new Vector3(i, map.mapHeights[i, j] + 0.2f, j), Quaternion.identity, this.transform);
                    //map.objectsOnMap.Add(new Tuple<int, int>(i, j), end);
                }
            }
        }

        Camera.main.transform.position = new Vector3(map.mapHeights.GetUpperBound(0) / 2, 10, map.mapHeights.GetLowerBound(1) - 3);
    }

    public void GenerateConnections()
    {

        for (int i = 0; i < mapList[currentMap].mapHeights.GetLength(0); i++)
        {
            for (int j = 0; j < mapList[currentMap].mapHeights.GetLength(1); j++)
            {
                if (mapList[currentMap].mapHeights[i, j] < 0)
                {
                    continue;
                }

                if (i > 0)
                    // and the tile above us is not an empty obstacle...
                    if (mapList[currentMap].floorElements[new Tuple<int, int>(i - 1, j)] != null)
                    {
                        // connect the current tile to the one above.
                        if (player.transform.position.y - 1 == mapList[currentMap].floorElements[new Tuple<int, int>(i - 1, j)].transform.position.y 
                            && !mapList[currentMap].objectsOnMap.ContainsKey(new Tuple<int, int>(i, j)))
                        {
                            connectTiles(mapList[currentMap].floorElements[new Tuple<int, int>(i - 1, j)], DirectionEnum.Down, mapList[currentMap].floorElements[new Tuple<int, int>(i, j)]);
                        }
                        else if(mapList[currentMap].objectsOnMap.ContainsKey(new Tuple<int, int>(i - 1, j)) && player.transform.position.y - 1 == mapList[currentMap].objectsOnMap[new Tuple<int, int>(i - 1, j)].transform.position.y)
                        {
                            connectTiles(mapList[currentMap].objectsOnMap[new Tuple<int, int>(i - 1, j)], DirectionEnum.Down, mapList[currentMap].floorElements[new Tuple<int, int>(i, j)]);
                            //connectTiles(mapList[currentMap].floorElements[new Tuple<int, int>(i, j)], DirectionEnum.Up, mapList[currentMap].objectsOnMap[new Tuple<int, int>(i - 1, j)]);
                        }
                    }

                // Similarly, if there is at least one column to the left...
                if (j > 0)
                    // and the tile to the left is not an empty obstacle...
                    if (mapList[currentMap].floorElements[new Tuple<int, int>(i, j - 1)] != null)
                    {
                        if (player.transform.position.y - 1 == mapList[currentMap].floorElements[new Tuple<int, int>(i, j - 1)].transform.position.y
                            && !mapList[currentMap].objectsOnMap.ContainsKey(new Tuple<int, int>(i, j)))
                        {
                            // connect the current tile to the leftward one.
                            connectTiles(mapList[currentMap].floorElements[new Tuple<int, int>(i, j - 1)], DirectionEnum.Right, mapList[currentMap].floorElements[new Tuple<int, int>(i, j)]);
                        }
                        else if (mapList[currentMap].objectsOnMap.ContainsKey(new Tuple<int, int>(i, j - 1)) && player.transform.position.y - 1 == mapList[currentMap].objectsOnMap[new Tuple<int, int>(i, j - 1)].transform.position.y)
                        {
                            connectTiles(mapList[currentMap].objectsOnMap[new Tuple<int, int>(i, j - 1)], DirectionEnum.Right, mapList[currentMap].floorElements[new Tuple<int, int>(i, j)]);
                            //connectTiles(mapList[currentMap].floorElements[new Tuple<int, int>(i, j)], DirectionEnum.Left, mapList[currentMap].objectsOnMap[new Tuple<int, int>(i, j - 1)]);
                        }
                    }
            }
        }
    }

    private GameObject createTile(int x, int y, Tuple<int, int> pos)
    {
        if (!mapList[currentMap].floorElements.ContainsKey(pos))
        {
            GameObject floorElement = Instantiate(floor, new Vector3(x, mapList[currentMap].mapHeights[x, y], y), Quaternion.identity, this.transform);
            floorElement.GetComponent<GroundObject>().SetupData(new Tuple<int, int>(x, y), mapList[currentMap].mapHeights[x, y]);

            mapList[currentMap].floorElements.Add(pos, floorElement);

            return floorElement;
        }

        return mapList[currentMap].floorElements[pos];
    }

    //Check tile for object, let object recursively call check tile 
    //if no object and available space, move objects 
    int CheckTilePlayerMovement(DirectionEnum direction, Tuple<int,int> priorPos, Tuple<int,int> pos)
    {

        if (0 <= pos.Item1 && pos.Item1 < mapList[currentMap].mapHeights.GetLength(0) && 0 <= pos.Item2 && pos.Item2 < mapList[currentMap].mapHeights.GetLength(1) && mapList[currentMap].mapHeights[pos.Item1, pos.Item2] >= 0)
        {
            //if heights are the same
            if (mapList[currentMap].mapHeights[priorPos.Item1, priorPos.Item2] == mapList[currentMap].mapHeights[pos.Item1, pos.Item2])
            {
                //If next tile has block, try to push
                if (mapList[currentMap].objectsOnMap.ContainsKey(pos))
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

                    if (CheckTilePlayerMovement(direction, pos, newPos) != -1)
                    {
                        newPosition = new Vector3(newPos.Item1, mapList[currentMap].mapHeights[newPos.Item1, newPos.Item2] + 1, newPos.Item2);
                        MoveObject(newPosition, pos, newPos);
                        return 1;
                    }
                }
                
                //else move
                else {
                    return 1;
                }
            }
            //Entering Hole with block in it
            //Checks if height difference is -1, and if player is the one being checked, and if hole is filled
            else if ((priorPos == playerLocation && mapList[currentMap].objectsOnMap.ContainsKey(pos) || (priorPos != playerLocation && !mapList[currentMap].objectsOnMap.ContainsKey(pos)))
                && mapList[currentMap].mapHeights[priorPos.Item1, priorPos.Item2] - 1 == mapList[currentMap].mapHeights[pos.Item1, pos.Item2])
            {
                return 2;
            }
            //Exiting Hole
            else if (mapList[currentMap].objectsOnMap.ContainsKey(priorPos) && mapList[currentMap].mapHeights[priorPos.Item1, priorPos.Item2] + 1 == mapList[currentMap].mapHeights[pos.Item1, pos.Item2])
            {
                //If next tile has block, try to push
                if (mapList[currentMap].objectsOnMap.ContainsKey(pos))
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

                    if (CheckTilePlayerMovement(direction, pos, newPos) != -1)
                    {
                        newPosition = new Vector3(newPos.Item1, mapList[currentMap].mapHeights[newPos.Item1, newPos.Item2] + 1, newPos.Item2);
                        MoveObject(newPosition, pos, newPos);
                        return 1;
                    }
                }

                //else move
                else
                {
                    return 1;
                }
            }
        }
        return -1;
    }
    
    //If movement checks succeeded, recursively move all objects that would be pushed by the player
    public void MovePlayer(DirectionEnum direction)
    {
        Tuple<int, int> newPos = playerLocation;
        int heightOffset;

        switch (direction)
        {
            case DirectionEnum.Left:
                newPos = new Tuple<int, int>(playerLocation.Item1 - 1, playerLocation.Item2);
                heightOffset = CheckTilePlayerMovement(direction, playerLocation, newPos);
                if (heightOffset != -1)
                    UpdatePlayerLocation(new Vector3(newPos.Item1, mapList[currentMap].mapHeights[newPos.Item1, newPos.Item2] + heightOffset, newPos.Item2), newPos);
                break;
            case DirectionEnum.Right:
                newPos = new Tuple<int, int>(playerLocation.Item1 + 1, playerLocation.Item2);
                heightOffset = CheckTilePlayerMovement(direction, playerLocation, newPos);
                if (heightOffset != -1)
                    UpdatePlayerLocation(new Vector3(newPos.Item1, mapList[currentMap].mapHeights[newPos.Item1, newPos.Item2] + heightOffset, newPos.Item2), newPos); 
                    break;
            case DirectionEnum.Up:
                newPos = new Tuple<int, int>(playerLocation.Item1, playerLocation.Item2 + 1);
                heightOffset = CheckTilePlayerMovement(direction, playerLocation, newPos);
                if (heightOffset != -1)
                    UpdatePlayerLocation(new Vector3(newPos.Item1, mapList[currentMap].mapHeights[newPos.Item1, newPos.Item2] + heightOffset, newPos.Item2), newPos); 
                break;
            case DirectionEnum.Down:
                newPos = new Tuple<int, int>(playerLocation.Item1, playerLocation.Item2 - 1);
                heightOffset = CheckTilePlayerMovement(direction, playerLocation, newPos);
                if (heightOffset != -1)
                    UpdatePlayerLocation(new Vector3(newPos.Item1, mapList[currentMap].mapHeights[newPos.Item1, newPos.Item2] + heightOffset, newPos.Item2), newPos); 
                break;
            default:
                break;
        }
    }

    public bool CanPlayerMove(DirectionEnum direction)
    {
        Tuple<int, int> newPos = playerLocation;
        int heightOffset;

        switch (direction)
        {
            case DirectionEnum.Left:
                newPos = new Tuple<int, int>(playerLocation.Item1 - 1, playerLocation.Item2);
                heightOffset = CheckTilePlayerMovement(direction, playerLocation, newPos);
                if (heightOffset != -1)
                    return true;
                break;
            case DirectionEnum.Right:
                newPos = new Tuple<int, int>(playerLocation.Item1 + 1, playerLocation.Item2);
                heightOffset = CheckTilePlayerMovement(direction, playerLocation, newPos);
                if (heightOffset != -1)
                    return true;
                break;
            case DirectionEnum.Up:
                newPos = new Tuple<int, int>(playerLocation.Item1, playerLocation.Item2 + 1);
                heightOffset = CheckTilePlayerMovement(direction, playerLocation, newPos);
                if (heightOffset != -1)
                    return true;
                break;
            case DirectionEnum.Down:
                newPos = new Tuple<int, int>(playerLocation.Item1, playerLocation.Item2 - 1);
                heightOffset = CheckTilePlayerMovement(direction, playerLocation, newPos);
                if (heightOffset != -1)
                    return true;
                break;
            default:
                return true;
                break;
        }

        return false;
    }

    public void UpdatePlayerLocation(Vector3 newPosition, Tuple<int,int> newPlayerLoc)
    {
        //mapList[currentMap].objectsOnMap[playerLocation].transform.position = newPosition;
        //mapList[currentMap].objectsOnMap[newPlayerLoc] = mapList[currentMap].objectsOnMap[playerLocation];
        //mapList[currentMap].objectsOnMap.Remove(playerLocation);
        player.transform.position = newPosition;
        playerLocation = newPlayerLoc;

        ReadBabaObjectWordConnections();
    }

    void MoveObject(Vector3 newPosition, Tuple<int, int> oldLoc, Tuple<int, int> newLoc)
    {
        mapList[currentMap].objectsOnMap[oldLoc].transform.position = newPosition;
        mapList[currentMap].objectsOnMap[newLoc] = mapList[currentMap].objectsOnMap[oldLoc];
        mapList[currentMap].objectsOnMap.Remove(oldLoc);
    }

    public void SetControlledObject(GameObject newPlayer, Tuple<int,int> newLoc)
    {
        playerLocation = newLoc;
        player = newPlayer;
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

    public GameObject currentTile(int x, int z)
    {
        return mapList[currentMap].floorElements[new Tuple<int, int>(x, z)];
    }

    public DirectionEnum getDirection(Vector3 currentPos, Vector3 newPos)
    {
        if(currentPos.x > newPos.x)
            return DirectionEnum.Left;

        if (currentPos.x < newPos.x)
            return DirectionEnum.Right;

        if (currentPos.z > newPos.z)
            return DirectionEnum.Down;

        if (currentPos.z < newPos.z)
            return DirectionEnum.Up;

        return DirectionEnum.Stay;
    }

    void ReadBabaObjectWordConnections()
    {
        Dictionary<Tuple<int, int>, GameObject> objects = mapList[currentMap].objectsOnMap;
        foreach (Tuple<int,int> key in objects.Keys)
        {
            //If Current Object has Word
            if (objects[key].GetComponent<InteractibleObject>().heldWord != "")
            {
                Tuple<int, int> rightPos = new Tuple<int, int>(key.Item1 + 1, key.Item2);
                Tuple<int, int> bottomPos = new Tuple<int, int>(key.Item1, key.Item2+1);

                if (key.Item1 < mapList[currentMap].mapHeights.GetLength(0) - 2
                    && objects.ContainsKey(rightPos)
                    && objects[rightPos].GetComponent<InteractibleObject>().heldWord != "")
                {
                    Debug.Log(objects[key].GetComponent<InteractibleObject>().heldWord + objects[rightPos].GetComponent<InteractibleObject>().heldWord);
                }
                else if (key.Item1 < mapList[currentMap].mapHeights.GetLength(1) - 2
                            && objects.ContainsKey(bottomPos)
                            && objects[bottomPos].GetComponent<InteractibleObject>().heldWord != "")
                {
                    Debug.Log(objects[key].GetComponent<InteractibleObject>().heldWord + objects[bottomPos].GetComponent<InteractibleObject>().heldWord);
                }
            }
        }
    }

    //Trigger Baba-Is-You map reading to determine if new rules have been added
    void UpdateTile() { }

    //Generate Map
    void MapLoader()
    {

    }
}

public class Map
{
    public Dictionary<Tuple<int, int>, GameObject> objectsOnMap { get; set; } = new Dictionary<Tuple<int, int>, GameObject>();
    public Dictionary<Tuple<int, int>, GameObject> floorElements { get; set; } = new Dictionary<Tuple<int, int>, GameObject>();
    public int[,] mapHeights { get; set; }
    public List<Tuple<int, int>> intendedPath { get; set; } = new List<Tuple<int, int>>();
    public Tuple<int, int> endLocation { get; set; }
    public Tuple<int, int> playerStart { get; set; }

    public Map()
    {

    }
}