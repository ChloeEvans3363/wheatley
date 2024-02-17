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

    // The current tile the character is on.
    private GameObject CurrentTile { get; set; } = null;

    // The tile the character is moving to.
    private GameObject TargetTile { get; set; } = null;

    NodeRecord TargetNodeRecord = null;

    // Start is called before the first frame update
    void Start()
    {
        player = MapManager.Instance.player;
        end = MapManager.Instance.end;

        Vector3 playerPositon = player.transform.position;
        CurrentTile = MapManager.Instance.currentTile((int)playerPositon.x, (int)playerPositon.z);
        TargetTile = CurrentTile;
        Debug.Log(CurrentTile.transform.position);
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
            StartCoroutine(HandleInput
                (CurrentTile, MapManager.Instance.currentTile((int)endPosition.x, (int)endPosition.z)));
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

        //AStar Movement
        
        
        if(path.Count > 0)
        {
            Vector3 distance = TargetTile.transform.position - CurrentTile.transform.position;

            if (distance.magnitude < radius)
            {
                TargetNodeRecord = path.Pop();
                TargetTile = TargetNodeRecord.Tile;
            }

            CurrentTile = TargetTile;
        }
        if(TargetNodeRecord != null)
        {
            //MapManager.Instance.UpdatePlayerLocation(new );
        }

        
    }

    private IEnumerator HandleInput(GameObject start, GameObject end)
    {
        yield return StartCoroutine(AStar.search(start, end, path));
    }
}
