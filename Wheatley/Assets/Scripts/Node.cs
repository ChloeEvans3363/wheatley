using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapManager;

public class Node : MonoBehaviour
{
    // A dictionary that stores up to four connections associated with Up, Down, Left, and Right.
    public Dictionary<DirectionEnum, GameObject> Connections { get; set; } = new Dictionary<DirectionEnum, GameObject>();
}
