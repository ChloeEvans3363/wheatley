using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static MapManager;

public class AStarPush : MonoBehaviour
{

    public class State
    {
        public Vector2 playerPos;
        public Vector2 cratePos;

        public State(Vector2 playerPos, Vector2 cratePos)
        {
            this.playerPos = playerPos;
            this.cratePos = cratePos;
        }

        // Override GetHashCode and Equals to use only playerPos and cratePos for hashing and equality comparison
        public override int GetHashCode()
        {
            return playerPos.GetHashCode() ^ cratePos.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is State))
                return false;
            State other = (State)obj;
            return playerPos.Equals(other.playerPos) && cratePos.Equals(other.cratePos);
        }
    }

    public static List<DirectionEnum> SolveSokoban(int[,] map, Vector2 initialPlayerPos, Vector2 initialCratePos, Vector2 goalPos)
    {
        Queue<State> queue = new Queue<State>();
        HashSet<State> visited = new HashSet<State>();
        Dictionary<State, List<DirectionEnum>> moves = new Dictionary<State, List<DirectionEnum>>();

        State initialState = new State(initialPlayerPos, initialCratePos);
        queue.Enqueue(initialState);
        visited.Add(initialState);
        moves.Add(initialState, new List<DirectionEnum>());

        while (queue.Count > 0)
        {
            State currentState = queue.Dequeue();

            Debug.Log("Exploring state: Player at " + currentState.playerPos + ", Crate at " + currentState.cratePos);

            // Check if crate is at the goal position
            if (currentState.cratePos.x == goalPos.x && currentState.cratePos.y == goalPos.y)
            {
                Debug.Log("Goal reached!");
                return moves[currentState];
            }

            // Generate all possible moves
            List<Vector2> possibleMoves = new List<Vector2>()
        {
            new Vector2(currentState.playerPos.x + 1, currentState.playerPos.y),
            new Vector2(currentState.playerPos.x - 1, currentState.playerPos.y),
            new Vector2(currentState.playerPos.x, currentState.playerPos.y + 1),
            new Vector2(currentState.playerPos.x, currentState.playerPos.y - 1)
        };

            // Apply valid moves
            foreach (Vector2 move in possibleMoves)
            {
                if (IsValidMove(map, move))
                {
                    // Clone the current state to create a new state
                    State newState = new State(move, currentState.cratePos);

                    // Move the crate if player pushes it
                    if (move.Equals(currentState.cratePos))
                    {
                        Vector2 newCratePos = new Vector2(move.x + (move.x - currentState.playerPos.x), move.y + (move.y - currentState.playerPos.y));
                        newState.cratePos = newCratePos;
                    }

                    Debug.Log("Move: Player at " + move + ", Crate at " + newState.cratePos);

                    // Mark the new state as visited and enqueue it
                    if (!visited.Contains(newState))
                    {
                        visited.Add(newState);
                        queue.Enqueue(newState);

                        // Clone the list of moves and add the new move
                        List<DirectionEnum> newMoves = new List<DirectionEnum>(moves[currentState]);
                        newMoves.Add(GetDirection(currentState.playerPos, move));
                        moves.Add(newState, newMoves);
                    }
                }
            }
        }

        // No solution found
        return null;
    }

    public static DirectionEnum GetDirection(Vector2 from, Vector2 to)
    {
        if (to.x > from.x)
            return DirectionEnum.Right;
        if (to.x < from.x)
            return DirectionEnum.Left;
        if (to.y > from.y)
            return DirectionEnum.Up;
        if (to.y < from.y)
            return DirectionEnum.Down;
        return DirectionEnum.Stay;
    }

    public static bool IsValidMove(int[,] map, Vector2 pos)
    {
        int numRows = map.GetLength(0);
        int numCols = map.GetLength(1);

        // Check if the position is within the grid boundaries
        if (pos.x < 0 || pos.x >= numRows || pos.y < 0 || pos.y >= numCols)
        {
            return false;
        }

        // Check if the position is not a hole
        if (map[(int)pos.x, (int)pos.y] == -1 || map[(int)pos.x, (int)pos.y] == 0)
        {
            return false;
        }

        return true;
    }

    private static NodeRecord smallestElement(List<NodeRecord> nodes, GameObject start, GameObject end)
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
}
