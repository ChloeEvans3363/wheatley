using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject moveableBlock;
    [SerializeField] bool playerControl = false;

    private Vector3 blockSpawnPos = new Vector3(-6.64f, 1f, 2.56f);
    Stack<NodeRecord> path = new Stack<NodeRecord>();

    private GameObject player;
    private GameObject end;

    // Start is called before the first frame update
    void Start()
    {
        player = MapManager.Instance.player;
        end = MapManager.Instance.end;
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
            Vector3 playerPositon = player.transform.position;
            Vector3 endPosition = end.transform.position;
            StartCoroutine(HandleInput
                (MapManager.Instance.currentTile((int)playerPositon.x, (int)playerPositon.z),
                MapManager.Instance.currentTile((int)endPosition.x, (int)endPosition.z)));
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
}
