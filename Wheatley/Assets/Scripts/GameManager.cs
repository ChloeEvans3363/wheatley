using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject moveableBlock;

    private Vector3 blockSpawnPos = new Vector3(-6.64f, 1f, 2.56f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject block = Instantiate(moveableBlock, blockSpawnPos, Quaternion.identity, this.transform);
        }

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
