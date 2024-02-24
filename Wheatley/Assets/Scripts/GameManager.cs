using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject moveableBlock;
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject block = Instantiate(moveableBlock, blockSpawnPos, Quaternion.identity, this.transform);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Vector3 endPosition = end.transform.position;
            MapManager.Instance.GenerateConnections();

            StartCoroutine(HandleInput
                (TargetTile, MapManager.Instance.currentTile((int)endPosition.x, (int)endPosition.z)));

            StartCoroutine(Move());
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

            yield return new WaitForSeconds(waitTime);
        }
    }
}
