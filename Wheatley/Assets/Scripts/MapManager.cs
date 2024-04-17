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
    [SerializeField] GameObject key;
    [SerializeField] GameObject door;
    [SerializeField] public Tuple<int, int> playerLocation = new Tuple<int, int>(0, 6);
    [SerializeField] GameObject moveableBlock;
    public GameObject player;
    public GameObject end;

    public Map currentMap;
    public int currentMapIndex;

    //Tracks which block should be placed
    public int selectedBlock = -1;

    public enum DirectionEnum
    {
        Left, Right, Up, Down, Stay
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMap = MapData.mapList[currentMapIndex];

        loadMap(currentMap);
    }

    private void loadMap(Map map)
    {
        playerLocation = map.playerStart;
        map.objectsOnMap = new Dictionary<Tuple<int, int>, GameObject>();
        map.floorElements = new Dictionary<Tuple<int, int>, GameObject>();

        if (map.mapHeights[0, 0] >= 0)
            CreateTile(0, 0, new Tuple<int, int>(0, 0));
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
                GameObject newTile = CreateTile(i, j, new Tuple<int, int>(i, j));

                if (i == playerLocation.Item1 && j == playerLocation.Item2)
                {
                    player = Instantiate(playerPrefab, new Vector3(i, map.mapHeights[i, j] + 1, j), Quaternion.identity, this.transform);
                    //map.objectsOnMap.Add(new Tuple<int, int>(i, j), player);
                }

                if (i == map.endLocation.Item1 && j == map.endLocation.Item2)
                {
                    end = Instantiate(endPrefab, new Vector3(i, map.mapHeights[i, j] + 0.2f, j), Quaternion.identity, this.transform);
                    //map.objectsOnMap.Add(new Tuple<int, int>(i, j), end);
                }
            }
        }

        Camera.main.transform.position = new Vector3(map.mapHeights.GetUpperBound(0) / 2, 10, map.mapHeights.GetLowerBound(1) - 3);

        for (int i = 0; i < map.numPushBoxes; i++)
        {
            GameObject block = Instantiate(moveableBlock, new Vector3(-0.3f, i * 0.02f, -1.8f), Quaternion.identity, this.transform);
            block.GetComponent<InteractibleObject>().canPush = true;
            block.GetComponent<InteractibleObject>().UpdateTint();
            block.GetComponent<InteractibleObject>().type = InteractibleObject.ObjectType.BasicBlock;
            block.GetComponent<MoveBox>().deselectedLocation = new Vector3(-0.3f, i * 0.02f, -1.8f);
            block.GetComponent<MoveBox>().mapIdentity = i + 1;
        }

        ManageScenes.Instance.SetNumPushBlocks(map.numPushBoxes);

        for (int i = map.numPushBoxes; i < map.numImmoveableBoxes + map.numPushBoxes; i++)
        {
            GameObject block = Instantiate(moveableBlock, new Vector3(2.0f, i * 0.02f, -1.8f), Quaternion.identity, this.transform);
            block.GetComponent<InteractibleObject>().canPush = false;
            block.GetComponent<InteractibleObject>().UpdateTint();
            block.GetComponent<InteractibleObject>().type = InteractibleObject.ObjectType.BasicBlock;
            block.GetComponent<MoveBox>().deselectedLocation = new Vector3(2.0f, i*0.02f, -1.8f);
            block.GetComponent<MoveBox>().mapIdentity = i + 1;
        }

        ManageScenes.Instance.SetNumImmovableBlocks(map.numImmoveableBoxes);

        for (int i = 0; i < map.doors.Count; i++)
        {
            GameObject newDoor = Instantiate(door, new Vector3(map.doors[i].Item1, map.mapHeights[map.doors[i].Item1, map.doors[i].Item2] + 1, map.doors[i].Item2), Quaternion.identity, this.transform);
            newDoor.GetComponent<InteractibleObject>().canPush = false;
            newDoor.GetComponent<InteractibleObject>().type = InteractibleObject.ObjectType.Door;
            currentMap.objectsOnMap.Add(map.doors[i], newDoor);
        }

        for (int i = 0; i < map.keys.Count; i++)
        {
            GameObject newKey = Instantiate(key, new Vector3(map.keys[i].Item1, map.mapHeights[map.keys[i].Item1, map.keys[i].Item2] + 1, map.keys[i].Item2), Quaternion.identity, this.transform);
            newKey.GetComponent<InteractibleObject>().canPush = true;
            newKey.GetComponent<InteractibleObject>().type = InteractibleObject.ObjectType.Key;
            currentMap.objectsOnMap.Add(map.keys[i], newKey);
        }
    }

    public void GenerateConnections()
    {

        for (int i = 0; i < currentMap.mapHeights.GetLength(0); i++)
        {
            for (int j = 0; j < currentMap.mapHeights.GetLength(1); j++)
            {
                if (currentMap.mapHeights[i, j] < 0)
                {
                    continue;
                }

                if (i > 0)
                    // and the tile above us is not an empty obstacle...
                    if (currentMap.floorElements[new Tuple<int, int>(i - 1, j)] != null)
                    {
                        // Checks if there is an object in the same place and if that object is ground level
                        // If so connect the ground to the object
                        if (currentMap.objectsOnMap.ContainsKey(new Tuple<int, int>(i, j)) 
                            && player.transform.position.y - 1 == currentMap.objectsOnMap[new Tuple<int, int>(i, j)].transform.position.y)
                        {
                            ConnectTiles(currentMap.floorElements[new Tuple<int, int>(i - 1, j)], DirectionEnum.Down, currentMap.objectsOnMap[new Tuple<int, int>(i, j)]);
                        }
                        // Otherwise if there isn't an object in the way
                        else if (!currentMap.objectsOnMap.ContainsKey(new Tuple<int, int>(i, j)))
                        {
                            // Check if the piece above us has an object on ground level
                            // If so connect the ground node to that object
                            if(player.transform.position.y - 1 != currentMap.floorElements[new Tuple<int, int>(i - 1, j)].transform.position.y && currentMap.objectsOnMap.ContainsKey(new Tuple<int, int>(i - 1, j)))
                            {
                                ConnectTiles(currentMap.objectsOnMap[new Tuple<int, int>(i - 1, j)], DirectionEnum.Down, currentMap.floorElements[new Tuple<int, int>(i, j)]);
                            }
                            // Otherwise if the current ground piece is floor level
                            // Connect the ground piece to another ground piece
                            else if(player.transform.position.y - 1 == currentMap.floorElements[new Tuple<int, int>(i - 1, j)].transform.position.y)
                            {
                                ConnectTiles(currentMap.floorElements[new Tuple<int, int>(i - 1, j)], DirectionEnum.Down, currentMap.floorElements[new Tuple<int, int>(i, j)]);
                            }
                        }
                    }

                // Similarly, if there is at least one column to the left...
                if (j > 0)
                    // and the tile to the left is not an empty obstacle...
                    if (currentMap.floorElements[new Tuple<int, int>(i, j - 1)] != null)
                    {
                        // Checks if there is an object in the same place and if that object is ground level
                        // If so connect the ground to the object
                        if (currentMap.objectsOnMap.ContainsKey(new Tuple<int, int>(i, j))
                            && player.transform.position.y - 1 == currentMap.objectsOnMap[new Tuple<int, int>(i, j)].transform.position.y)
                        {
                            ConnectTiles(currentMap.floorElements[new Tuple<int, int>(i, j - 1)], DirectionEnum.Right, currentMap.objectsOnMap[new Tuple<int, int>(i, j)]);
                        }
                        // Otherwise if there isn't an object in the way
                        else if (!currentMap.objectsOnMap.ContainsKey(new Tuple<int, int>(i, j)))
                        {
                            // Check if the piece to the left has an object on ground level
                            // If so connect the ground node to that object
                            if (player.transform.position.y - 1 != currentMap.floorElements[new Tuple<int, int>(i, j - 1)].transform.position.y && currentMap.objectsOnMap.ContainsKey(new Tuple<int, int>(i, j - 1)))
                            {
                                ConnectTiles(currentMap.objectsOnMap[new Tuple<int, int>(i, j - 1)], DirectionEnum.Right, currentMap.floorElements[new Tuple<int, int>(i, j)]);
                            }
                            // Otherwise if the current ground piece is floor level
                            // Connect the ground piece to another ground piece
                            else if (player.transform.position.y - 1 == currentMap.floorElements[new Tuple<int, int>(i, j - 1)].transform.position.y)
                            {
                                ConnectTiles(currentMap.floorElements[new Tuple<int, int>(i, j - 1)], DirectionEnum.Right, currentMap.floorElements[new Tuple<int, int>(i, j)]);
                            }
                        }
                    }
            }
        }
    }

    private GameObject CreateTile(int x, int y, Tuple<int, int> pos)
    {
        if (!currentMap.floorElements.ContainsKey(pos))
        {
            GameObject floorElement = Instantiate(floor, new Vector3(x, currentMap.mapHeights[x, y], y), Quaternion.identity, this.transform);
            floorElement.GetComponent<GroundObject>().SetupData(new Tuple<int, int>(x, y), currentMap.mapHeights[x, y]);

            currentMap.floorElements.Add(pos, floorElement);

            return floorElement;
        }

        return currentMap.floorElements[pos];
    }

    //Check tile for object, let object recursively call check tile 
    //if no object and available space, move objects 
    int CheckTilePlayerMovement(DirectionEnum direction, Tuple<int,int> priorPos, Tuple<int,int> pos)
    {

        if (0 <= pos.Item1 && pos.Item1 < currentMap.mapHeights.GetLength(0) && 0 <= pos.Item2 && pos.Item2 < currentMap.mapHeights.GetLength(1) && currentMap.mapHeights[pos.Item1, pos.Item2] >= 0)
        {
            //if heights are the same
            if (currentMap.mapHeights[priorPos.Item1, priorPos.Item2] == currentMap.mapHeights[pos.Item1, pos.Item2])
            {
                //If next tile has block, try to push
                if (currentMap.objectsOnMap.ContainsKey(pos))
                {
                    if (currentMap.objectsOnMap.ContainsKey(priorPos) 
                        && currentMap.objectsOnMap[priorPos].GetComponent<InteractibleObject>().type == InteractibleObject.ObjectType.Key 
                        && currentMap.objectsOnMap[pos].GetComponent<InteractibleObject>().type == InteractibleObject.ObjectType.Door)
                    {
                        currentMap.objectsOnMap[pos].gameObject.SetActive(false);
                        currentMap.objectsOnMap[priorPos].gameObject.SetActive(false);
                        currentMap.objectsOnMap.Remove(pos);
                        currentMap.objectsOnMap.Remove(priorPos);

                        return 1;
                    }

                    if(!currentMap.objectsOnMap[pos].GetComponent<InteractibleObject>().canPush)
                    {
                        return -1;
                    }

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
                        newPosition = new Vector3(newPos.Item1, currentMap.mapHeights[newPos.Item1, newPos.Item2] + 1, newPos.Item2);

                        if(currentMap.objectsOnMap.ContainsKey(pos))
                            MoveObject(newPosition, pos, newPos);
    
                        return 1;
                    }
                }
                
                //else move
                else {
                    return 1;
                }
            }
            //Pushing Block into hole. Raises ground level
            else if (priorPos != playerLocation && !currentMap.objectsOnMap.ContainsKey(pos) && currentMap.objectsOnMap[priorPos].GetComponent<InteractibleObject>().type == InteractibleObject.ObjectType.BasicBlock)
            {
                Tuple<int, int> newPos = priorPos;
                switch (direction)
                {
                    case DirectionEnum.Left:
                        newPos = new Tuple<int, int>(priorPos.Item1 - 1, priorPos.Item2);
                        break;
                    case DirectionEnum.Right:
                        newPos = new Tuple<int, int>(priorPos.Item1 + 1, priorPos.Item2);
                        break;
                    case DirectionEnum.Up:
                        newPos = new Tuple<int, int>(priorPos.Item1, priorPos.Item2 + 1);
                        break;
                    case DirectionEnum.Down:
                        newPos = new Tuple<int, int>(priorPos.Item1, priorPos.Item2 - 1);
                        break;
                }

                Vector3 newPosition = new Vector3(newPos.Item1, currentMap.mapHeights[newPos.Item1, newPos.Item2] + 1, newPos.Item2);
                MoveObject(newPosition, priorPos, newPos);

                currentMap.objectsOnMap[pos].GetComponent<MoveBox>().isFillingHole = true;
                currentMap.objectsOnMap[pos].GetComponent<MoveBox>().newMapPosition = newPos;
                currentMap.objectsOnMap.Remove(pos);
                currentMap.mapHeights[pos.Item1, pos.Item2]++;

                return 1;
            }
            //Exiting Hole
            else if (currentMap.objectsOnMap.ContainsKey(priorPos) && currentMap.mapHeights[priorPos.Item1, priorPos.Item2] + 1 == currentMap.mapHeights[pos.Item1, pos.Item2])
            {
                //If next tile has block, try to push
                if (currentMap.objectsOnMap.ContainsKey(pos))
                {
                    if (!currentMap.objectsOnMap[pos].GetComponent<InteractibleObject>().canPush)
                    {
                        return -1;
                    }
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
                        newPosition = new Vector3(newPos.Item1, currentMap.mapHeights[newPos.Item1, newPos.Item2] + 1, newPos.Item2);
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
                    UpdatePlayerLocation(new Vector3(newPos.Item1, currentMap.mapHeights[newPos.Item1, newPos.Item2] + heightOffset, newPos.Item2), newPos);
                break;
            case DirectionEnum.Right:
                newPos = new Tuple<int, int>(playerLocation.Item1 + 1, playerLocation.Item2);
                heightOffset = CheckTilePlayerMovement(direction, playerLocation, newPos);
                if (heightOffset != -1)
                    UpdatePlayerLocation(new Vector3(newPos.Item1, currentMap.mapHeights[newPos.Item1, newPos.Item2] + heightOffset, newPos.Item2), newPos); 
                    break;
            case DirectionEnum.Up:
                newPos = new Tuple<int, int>(playerLocation.Item1, playerLocation.Item2 + 1);
                heightOffset = CheckTilePlayerMovement(direction, playerLocation, newPos);
                if (heightOffset != -1)
                    UpdatePlayerLocation(new Vector3(newPos.Item1, currentMap.mapHeights[newPos.Item1, newPos.Item2] + heightOffset, newPos.Item2), newPos); 
                break;
            case DirectionEnum.Down:
                newPos = new Tuple<int, int>(playerLocation.Item1, playerLocation.Item2 - 1);
                heightOffset = CheckTilePlayerMovement(direction, playerLocation, newPos);
                if (heightOffset != -1)
                    UpdatePlayerLocation(new Vector3(newPos.Item1, currentMap.mapHeights[newPos.Item1, newPos.Item2] + heightOffset, newPos.Item2), newPos); 
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
        currentMap.objectsOnMap[oldLoc].transform.position = newPosition;
        currentMap.objectsOnMap[newLoc] = currentMap.objectsOnMap[oldLoc];
        currentMap.objectsOnMap.Remove(oldLoc);
    }

    public void SetControlledObject(GameObject newPlayer, Tuple<int,int> newLoc)
    {
        playerLocation = newLoc;
        player = newPlayer;
    }

    private void ConnectTiles(GameObject from, DirectionEnum direction, GameObject to)
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

    public void DisconnectTiles()
    {
        // Loops through all the blocks in a level
        for (int i = 0; i < currentMap.mapHeights.GetLength(0); i++)
        {
            for (int j = 0; j < currentMap.mapHeights.GetLength(1); j++)
            {
                // Check floor block connections
                if (currentMap.floorElements.ContainsKey(new Tuple<int, int>(i, j)) &&
                    currentMap.floorElements[new Tuple<int, int>(i, j)] != null)
                {
                    // Clears connections for the ground blocks
                    currentMap.floorElements[new Tuple<int, int>(i, j)].GetComponent<Node>().Connections.Clear();
                }

                // Checks object block connections
                if (currentMap.objectsOnMap.ContainsKey(new Tuple<int, int>(i, j)) &&
                    currentMap.objectsOnMap[new Tuple<int, int>(i, j)] != null)
                {
                    // Clears connections for the object blocks
                    currentMap.objectsOnMap[new Tuple<int, int>(i, j)].GetComponent<Node>().Connections.Clear();
                }
            }
        }
    }

    public GameObject CurrentTile(int x, int z)
    {
        return currentMap.floorElements[new Tuple<int, int>(x, z)];
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
        Dictionary<Tuple<int, int>, GameObject> objects = currentMap.objectsOnMap;
        foreach (Tuple<int,int> key in objects.Keys)
        {
            //If Current Object has Word
            if (objects[key].GetComponent<InteractibleObject>().heldWord != "")
            {
                Tuple<int, int> rightPos = new Tuple<int, int>(key.Item1 + 1, key.Item2);
                Tuple<int, int> bottomPos = new Tuple<int, int>(key.Item1, key.Item2+1);

                if (key.Item1 < currentMap.mapHeights.GetLength(0) - 2
                    && objects.ContainsKey(rightPos)
                    && objects[rightPos].GetComponent<InteractibleObject>().heldWord != "")
                {
                    //Debug.Log(objects[key].GetComponent<InteractibleObject>().heldWord + objects[rightPos].GetComponent<InteractibleObject>().heldWord);
                }
                else if (key.Item1 < currentMap.mapHeights.GetLength(1) - 2
                            && objects.ContainsKey(bottomPos)
                            && objects[bottomPos].GetComponent<InteractibleObject>().heldWord != "")
                {
                    //Debug.Log(objects[key].GetComponent<InteractibleObject>().heldWord + objects[bottomPos].GetComponent<InteractibleObject>().heldWord);
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