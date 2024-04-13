using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static MapManager;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool playerControl = false;

    private Vector3 blockSpawnPos = new Vector3(-6.64f, 1f, 2.56f);
    Stack<DirectionEnum> path = new Stack<DirectionEnum>();

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
            /*
            StartCoroutine(HandleInput
                (TargetTile, MapManager.Instance.currentTile((int)endPosition.x, (int)endPosition.z)));
            */
            path = AStar.Search(TargetTile, MapManager.Instance.currentTile((int)endPosition.x, (int)endPosition.z));

            Debug.Log(AStar.CanFindPath(TargetTile, MapManager.Instance.currentTile((int)endPosition.x, (int)endPosition.z)));
            // Moves the player along the path
            if (path != null)
                StartCoroutine(Move());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

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

    private IEnumerator Move()
    {
        while (path.Count > 0)
        {
            MapManager.Instance.MovePlayer(path.Pop());
            yield return new WaitForSeconds(waitTime);
        }
    }
}
