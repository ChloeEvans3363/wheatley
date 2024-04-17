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
        mapList.Clear();
        int[,] mapHeights1 =
        {
            {-1,-1,0},
            {0,0,0},
            {0,0,0},
            {0,0,0},
            {0,0,0},
            {0,0,0},
            {0,0,0},
            {0,0,0},
            {-1,-1,0},
        };
        DirectionEnum[] intendedPath1 = {DirectionEnum.Right,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Right};
        Map map1 = new Map(mapHeights1, intendedPath1, new Tuple<int, int>(0, 2), new Tuple<int, int>(8, 2), 0, 6);
        mapList.Add(map1);

        int[,] mapHeights2 =
        {
            {-1, 1, 1,-1, 1},
            { 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1},
            {-1,-1,-1,-1, 1},
            {-1,-1,-1,-1, 1},
            {-1,-1,-1,-1, 1},
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
        Map map2 = new Map(mapHeights2, intendedPath2, new Tuple<int, int>(0, 4), new Tuple<int, int>(5, 4), 0, 0);
        map2.AddKey(new Tuple<int, int>(1, 3));
        map2.AddDoor(new Tuple<int, int>(3, 4));
        mapList.Add(map2);

        int[,] mapHeights3 =
        {
            {-1, -1, 1},
            {-1, -1, 1},
            {-1, -1, 1},
            {-1, 1, 1},
            {1, 1, 1},
            {1, 1, 1},
            {-1, -1, 1},
            {-1, -1, 0},
            {-1, -1, 1}
        };
        DirectionEnum[] intendedPath3 = {DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Down,
                                        DirectionEnum.Right,
                                        DirectionEnum.Down,
                                        DirectionEnum.Right,
                                        DirectionEnum.Up,
                                        DirectionEnum.Left,
                                        DirectionEnum.Up,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right};
        Map map3 = new Map(mapHeights3, intendedPath3, new Tuple<int, int>(0, 2), new Tuple<int, int>(8, 2), 1, 0);
        map3.AddKey(new Tuple<int, int>(4, 2));
        map3.AddDoor(new Tuple<int, int>(5, 2));
        mapList.Add(map3);

        int[,] mapHeights4 =
        {
            {-1, 0, -1, 0, -1, -1},
            {1, 1, 1, 1, 1, 1},
            {1, 1, 1, 1, -1, 1},
            {-1, -1, 0, -1, -1, 1}
        };
        DirectionEnum[] intendedPath4 = {DirectionEnum.Right,
                                        DirectionEnum.Up,
                                        DirectionEnum.Left,
                                        DirectionEnum.Up,
                                        DirectionEnum.Right,
                                        DirectionEnum.Up,
                                        DirectionEnum.Left,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Down};
        Map map4 = new Map(mapHeights4, intendedPath4, new Tuple<int, int>(1, 0), new Tuple<int, int>(3, 5), 4, 1);
        mapList.Add(map4);

        int[,] mapHeights5 =
        {
            {-1, -1,  1,  1, 0,  1,  1},
            {-1, -1, -1, -1, 0, -1,  1},
            { 1,  1,  1,  1, 1,  0,  1},
            {-1,  1, -1, -1, 1, -1, -1},
            {-1,  1, -1, -1, 1, -1, -1},
            {-1,  1, -1, -1, 1, -1, -1},
            {-1,  1, -1, -1, 1, -1, -1},
            {-1,  1,  1,  1, 1, -1, -1},
        };
        DirectionEnum[] intendedPath5 = {DirectionEnum.Up,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Left,
                                        DirectionEnum.Left,
                                        DirectionEnum.Left,
                                        DirectionEnum.Left,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Left,
                                        DirectionEnum.Left,
                                        DirectionEnum.Left,
                                        DirectionEnum.Left,
                                        DirectionEnum.Left,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Left,
                                        DirectionEnum.Right,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Left,
                                        DirectionEnum.Left,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down};
        Map map5 = new Map(mapHeights5, intendedPath5, new Tuple<int, int>(2, 0), new Tuple<int, int>(0, 2), 3, 0);
        map5.AddKey(new Tuple<int, int>(5, 4));
        map5.AddDoor(new Tuple<int, int>(0, 3));
        mapList.Add(map5);

        int[,] mapHeight6 =
        {
            {-1, -1, 1},
            {1, 1, 1 },
            {1, -1, 1 },
            {1, 1, 1 },
            {-1, -1, 0},
            {-1, -1, 1}
        };
        DirectionEnum[] intendedPath6 = {DirectionEnum.Right,
                                        DirectionEnum.Down,
                                        DirectionEnum.Down,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Up,
                                        DirectionEnum.Down,
                                        DirectionEnum.Left,
                                        DirectionEnum.Left,
                                        DirectionEnum.Up,
                                        DirectionEnum.Up,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right,
                                        DirectionEnum.Right};
        Map map6 = new Map(mapHeight6, intendedPath6, new Tuple<int, int>(0, 2), new Tuple<int, int>(5, 2), 1, 1);
        //map6.AddKey(new Tuple<int, int>(3, 1));
        //map6.AddDoor(new Tuple<int, int>(4, 2));
        mapList.Add(map6);
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