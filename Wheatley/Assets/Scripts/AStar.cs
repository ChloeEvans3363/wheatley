using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapManager;

public class AStar : MonoBehaviour
{
    public static Stack<DirectionEnum> Search(GameObject start, GameObject end)
    {
        NodeRecord startRecord = new NodeRecord();
        startRecord.node = start.GetComponent<Node>();
        startRecord.connection = null;
        startRecord.costSoFar = 0;
        startRecord.Tile = start;
        startRecord.estimatedTotalCost = CrossProduct(start, start, end);

        NodeRecord current = new NodeRecord();

        Node endNode = new Node();
        float endNodeCost = 0;
        NodeRecord endNodeRecord = new NodeRecord();
        Stack<DirectionEnum> path = new Stack<DirectionEnum>();
        float endNodeHeuristic = 0;

        List<NodeRecord> open = new List<NodeRecord>();
        open.Add(startRecord);
        List<NodeRecord> closed = new List<NodeRecord>();

        while(open.Count > 0)
        {
            current = SmallestElement(open, start, end);

            if (current.node == end.GetComponent<Node>())
                break;

            List<NodeRecord> connections = new List<NodeRecord>();
            foreach(DirectionEnum direction in Enum.GetValues(typeof(DirectionEnum)))
                if (current.node.Connections.ContainsKey(direction))
                {
                    NodeRecord newNode = new NodeRecord();
                    newNode.node = current.node.Connections[direction].GetComponent<Node>();
                    newNode.costSoFar = 1;
                    newNode.Tile = current.node.Connections[direction];
                    connections.Add(newNode);
                }

            foreach(NodeRecord connection in connections)
            {
                endNode = connection.node;
                endNodeCost = current.costSoFar;
                endNodeRecord.estimatedTotalCost = CrossProduct(start, connection.Tile, end) + connection.costSoFar;

                if(Contains(closed, endNode))
                {
                    endNodeRecord = Find(closed, endNode);
                    if (endNodeRecord.costSoFar <= endNodeCost)
                        continue;
                    closed.Remove(endNodeRecord);

                    endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
                }

                else if(Contains(open, endNode))
                {
                    endNodeRecord = Find(open, endNode);
                    if (endNodeRecord.costSoFar <= endNodeCost)
                        continue;

                    endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
                }

                else
                {
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.node = endNode;

                    endNodeHeuristic = CrossProduct(start, connection.Tile, end);
                }

                endNodeRecord.Tile = connection.Tile;
                endNodeRecord.connection = current;
                endNodeRecord.costSoFar = endNodeCost;
                endNodeHeuristic += endNodeCost;
                endNodeRecord.estimatedTotalCost = endNodeHeuristic + endNodeCost;

                if (!Contains(open, endNode))
                    open.Add(endNodeRecord);
            }
            open.Remove(current);
            closed.Add(current);

        }
        //Debug.Log("Nodes Expanded: " + closed.Count);

        // Determine whether A* found a path and print it here.
        if (current.node != end.GetComponent<Node>())
        {
            Debug.Log("Search Failed");
        }
        else
        {
            // It's just nice to have the ai stand for a second
            path.Push(DirectionEnum.Stay);
            while (current.node != start.GetComponent<Node>())
            {
                path.Push(GetDirection(current.connection.Tile, current.Tile));
                current = current.connection;
            }

            //Debug.Log("Path Length: " + path.Count);

            return path;
        }

        return null;
    }

    public static bool CanFindPath(GameObject start, GameObject end)
    {
        if (Search(start, end) == null)
        {
            return false;
        }
        return true;
    }

    public static int GetCost(GameObject start, GameObject end)
    {
        return Search(start, end).Count;
    }

    private static NodeRecord SmallestElement(List<NodeRecord> nodes, GameObject start, GameObject end)
    {
        NodeRecord smallest = nodes[0];

        for (int i = 0; i < nodes.Count; i++)
        {
            if (CrossProduct(start, nodes[i].Tile, end) + nodes[i].costSoFar < CrossProduct(start, smallest.Tile, end) + smallest.costSoFar)
            {
                smallest = nodes[i];
            }
        }

        return smallest;
    }

    private static bool Contains(List<NodeRecord> nodeRecords, Node node)
    {
        foreach (NodeRecord nodeRecord in nodeRecords)
        {
            if (nodeRecord.node == node)
                return true;
        }
        return false;
    }

    private static NodeRecord Find(List<NodeRecord> nodeRecords, Node node)
    {
        foreach (NodeRecord nodeRecord in nodeRecords)
        {
            if (nodeRecord.node == node)
                return nodeRecord;
        }
        return null;
    }

    public static float CrossProduct(GameObject start, GameObject tile, GameObject goal)
    {
        float dx1 = tile.transform.position.x - goal.transform.position.x;
        float dy1 = tile.transform.position.y - goal.transform.position.y;
        float dx2 = start.transform.position.x - goal.transform.position.x;
        float dy2 = start.transform.position.y - goal.transform.position.y;
        float cross = Math.Abs(dx1 * dy2 - dx2 * dy1);

        // Manhattan
        float dx = Math.Abs(tile.transform.position.x - goal.transform.position.x);
        float dy = Math.Abs(tile.transform.position.y - goal.transform.position.y);

        return (dx + dy) + cross * 0.001f;
    }



    public static DirectionEnum GetDirection(GameObject from, GameObject to)
    {
        Vector3 toPos = to.transform.position;
        Vector3 fromPos = from.transform.position;

        if (toPos.x > fromPos.x)
            return DirectionEnum.Right;
        if (toPos.x < fromPos.x)
            return DirectionEnum.Left;
        if (toPos.z > fromPos.z)
            return DirectionEnum.Up;
        if (toPos.z < fromPos.z)
            return DirectionEnum.Down;
        return DirectionEnum.Stay;
    }
}

/// <summary>
/// A class for recording search statistics.
/// </summary>
public class NodeRecord
{
    // The tile game object.
    public GameObject Tile { get; set; } = null;

    // Set the other class properties here.

    public Node node { get; set; }
    public NodeRecord connection { get; set; }
    public float costSoFar { get; set; } = 1;
    public float estimatedTotalCost { get; set; }

}
