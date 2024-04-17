using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapManager;

public class MapData : MonoBehaviour
{
    [SerializeField]
    public static List<Map> mapList = new List<Map>();

    void Awake()
    {
        int[,] mapHeights1 =
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
        DirectionEnum[] intendedPath1 = {DirectionEnum.Right,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Right};
        Map map1 = new Map(mapHeights1, intendedPath1, new Tuple<int, int>(0, 6), new Tuple<int, int>(8, 6), 1, 0);
        mapList.Add(map1);

        int[,] mapHeights2 =
        {
            {-1,-1, 1, 1,-1, 1,-1},
            {-1, 1, 1, 1, 1, 1,-1},
            {-1, 1, 1, 1, 1, 1,-1},
            {-1,-1,-1,-1,-1, 0,-1},
            {-1,-1,-1,-1,-1, 1,-1},
            {-1,-1,-1,-1,-1, 1,-1},
        };
        DirectionEnum[] intendedPath2 = {DirectionEnum.Right,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Left,
                                        DirectionEnum.Down,
                                        DirectionEnum.Right,
                                        DirectionEnum.Down,
                                        DirectionEnum.Right,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Left,
                                        DirectionEnum.Up,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right};
        Map map2 = new Map(mapHeights2, intendedPath2, new Tuple<int, int>(0, 5), new Tuple<int, int>(5, 5), 1, 1);
        map2.AddKey(new Tuple<int, int>(2, 5));
        map2.AddDoor(new Tuple<int, int>(2, 2));
        mapList.Add(map2);
    }
}

public class Map
{
    public Dictionary<Tuple<int, int>, GameObject> objectsOnMap { get; set; } = new Dictionary<Tuple<int, int>, GameObject>();
    public Dictionary<Tuple<int, int>, GameObject> floorElements { get; set; } = new Dictionary<Tuple<int, int>, GameObject>();
    public int[,] mapHeights { get; set; }
    public DirectionEnum[] intendedPath;
    public Tuple<int, int> playerStart { get; set; }
    public Tuple<int, int> endLocation { get; set; }

    public List<Tuple<int, int>> doors { get; } = new List<Tuple<int, int>>();
    public List<Tuple<int, int>> keys { get; } = new List<Tuple<int, int>>();

    public int numPushBoxes;
    public int numImmoveableBoxes;

    public Map(int[,] mapHeights, DirectionEnum[] intendedPath, Tuple<int, int> playerStart, Tuple<int, int> endLocation, int numPushBoxes, int numImmoveableBoxes)
    {
        this.mapHeights = mapHeights;
        this.intendedPath = intendedPath;
        this.playerStart = playerStart;
        this.endLocation = endLocation;
        this.numPushBoxes = numPushBoxes;
        this.numImmoveableBoxes = numImmoveableBoxes;
    }

    public void AddKey(Tuple<int, int> location)
    {
        keys.Add(location);
    }

    public void AddDoor(Tuple<int, int> location)
    {
        doors.Add(location);
    }
}