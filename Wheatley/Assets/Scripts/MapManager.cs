using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //Base Map Concept
    int[,] map =
    {
        {0,0,0},
        {0,1,0},
        {0,0,0},
    };

    Vector2 playerPos = new Vector2(1, 1);


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    //Check tile for object, let object recursively call check tile 
    //if no object and available space, move objects 
    void CheckTile() { }

    //If movement checks succeeded, recursively move all objects that would be pushed by the player
    void MoveObjects() { }

    //Trigger Baba-Is-You map reading to determine if new rules have been added
    void UpdateTile() { }

    //Generate Map
    void MapLoader() { }
}
