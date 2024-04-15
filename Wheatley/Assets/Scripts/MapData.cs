using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
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
        int[,] path1 =
        {
            {0, 1, 1, 1, 1, 1, 1, 1, 2, 3, 3, 3, 3, 3, 3, 3, 4, 5, 5, 5, 5, 5, 5, 5, 6, 7, 7, 7, 7, 7, 7, 7, 8},
            {6, 6, 5, 4, 3, 2, 1 ,0, 0, 0, 1, 2, 3, 4, 5, 6, 6, 6, 5, 4, 3, 2, 1, 0, 0, 0, 1, 2, 3, 4, 5, 6, 6},
        };
        Map map1 = new Map(mapHeights1, new Tuple<int, int>(0, 6), new Tuple<int, int>(8, 6), 1, 0);
        for (int i = 0; i < path1.GetLength(1); i++)
        {
            map1.intendedPath.Add(new Tuple<int, int>(path1[0, i], path1[1, i]));
        }
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
        int[,] path2 =
        {
            {0, 1, 1, 1, 0, 0, 1, 1, 2, 2, 2, 2, 1, 1, 2, 3, 4, 5},
            {5, 5, 4, 3, 3, 2, 2, 1, 1, 2, 3, 4, 4, 5, 5, 5, 5, 5},
        };
        Map map2 = new Map(mapHeights2, new Tuple<int, int>(0, 5), new Tuple<int, int>(5, 5), 1, 1);
        for (int i = 0; i < path2.GetLength(1); i++)
        {
            map2.intendedPath.Add(new Tuple<int, int>(path2[0, i], path2[1, i]));
        }
        mapList.Add(map2);
    }
}

public class Map
{
    public Dictionary<Tuple<int, int>, GameObject> objectsOnMap { get; set; } = new Dictionary<Tuple<int, int>, GameObject>();
    public Dictionary<Tuple<int, int>, GameObject> floorElements { get; set; } = new Dictionary<Tuple<int, int>, GameObject>();
    public int[,] mapHeights { get; set; }
    public List<Tuple<int, int>> intendedPath { get; set; } = new List<Tuple<int, int>>();
    public Tuple<int, int> playerStart { get; set; }
    public Tuple<int, int> endLocation { get; set; }
    public int numPushBoxes;
    public int numImmoveableBoxes;

    public Map(int[,] mapHeights, Tuple<int, int> playerStart, Tuple<int, int> endLocation, int numPushBoxes, int numImmoveableBoxes)
    {
        this.mapHeights = mapHeights;
        this.playerStart = playerStart;
        this.endLocation = endLocation;
        this.numPushBoxes = numPushBoxes;
        this.numImmoveableBoxes = numImmoveableBoxes;
    }
}