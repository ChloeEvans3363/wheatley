using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool playerControl = false;

    private Vector3 blockSpawnPos = new Vector3(-6.64f, 1f, 2.56f);
    Stack<NodeRecord> path = new Stack<NodeRecord>();

    private GameObject player;
    private GameObject end;
    private float radius = 0.1f;
    private float waitTime = 1f;

    // The tile the character is moving to.
    private GameObject TargetTile { get; set; } = null;

    // Start is called before the first frame update
    void Start()
    {
        player = MapManager.Instance.player;
        end = MapManager.Instance.end;

        Vector3 playerPositon = player.transform.position;
        TargetTile = MapManager.Instance.currentTile((int)playerPositon.x, (int)playerPositon.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ManageScenes.Instance.ReloadScene();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Vector3 endPosition = end.transform.position;

            // Clears previous connections on map
            MapManager.Instance.DisconnectTiles();

            // Creates the connections on the map
            MapManager.Instance.GenerateConnections();

            // Generates the path using A*
            StartCoroutine(HandleInput
                (TargetTile, MapManager.Instance.currentTile((int)endPosition.x, (int)endPosition.z)));

            // Moves the player along the path
            StartCoroutine(Move());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            /*Map currentMap = MapManager.Instance.mapList[MapManager.Instance.currentMap];
            for (int i = 0; i < currentMap.mapHeights.GetLength(0); i++)
            {
                for (int j = 0; j < currentMap.mapHeights.GetLength(1); j++)
                {

                }
            }*/

            //Vector2 initialPlayerLocation = new Vector2(MapManager.Instance.mapList[MapManager.Instance.currentMap].playerStart.Item1, MapManager.Instance.mapList[MapManager.Instance.currentMap].playerStart.Item2);
            //Vector2 initialGoalLocation = new Vector2(MapManager.Instance.mapList[MapManager.Instance.currentMap].playerStart.Item1, MapManager.Instance.mapList[MapManager.Instance.currentMap].playerStart.Item2);
            List<MapManager.DirectionEnum> solution = AStarPush.SolveSokoban(MapManager.Instance.mapList[MapManager.Instance.currentMap].mapHeights, new Vector2(0, 5), new Vector2(1, 3), new Vector2(3, 5));
            for (int i = 0; i < solution.Count; i++)
            {
                Debug.Log("Step " + i + ": " + solution[i]);
            }
        }

        // Move character
        if (playerControl)
        {
            if (Input.GetKeyDown(KeyCode.W))
                MapManager.Instance.MovePlayer(MapManager.DirectionEnum.Up);
            else if (Input.GetKeyDown(KeyCode.S))
                MapManager.Instance.MovePlayer(MapManager.DirectionEnum.Down);
            else if (Input.GetKeyDown(KeyCode.D))
                MapManager.Instance.MovePlayer(MapManager.DirectionEnum.Right);
            else if (Input.GetKeyDown(KeyCode.A))
                MapManager.Instance.MovePlayer(MapManager.DirectionEnum.Left);

            Vector3 playerPositon = player.transform.position;
            TargetTile = MapManager.Instance.currentTile((int)playerPositon.x, (int)playerPositon.z);
        }

    }

    private IEnumerator HandleInput(GameObject start, GameObject end)
    {
        yield return StartCoroutine(AStar.search(start, end, path));
    }

    private IEnumerator Move()
    {

        while (path.Count > 0)
        {
            TargetTile = path.Pop().Tile;

            MapManager.DirectionEnum direction =
               MapManager.Instance.getDirection(player.transform.position, TargetTile.transform.position);

            MapManager.Instance.MovePlayer(direction);
            //Debug.Log(MapManager.Instance.CanPlayerMove(direction));

            yield return new WaitForSeconds(waitTime);
        }
    }
}
