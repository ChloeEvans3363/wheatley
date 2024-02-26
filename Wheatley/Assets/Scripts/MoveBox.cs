using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    Vector3 newPosition;
    private Tuple<int, int> newMapPosition;

    protected Camera Camera { get; private set; }
    bool placed;

    [SerializeField] protected LayerMask _raycastLayerMask;
    [SerializeField] protected float _raycastDistance = 100f;
    [SerializeField] LayerMask _dropTargetLayerMask;
    [SerializeField] LayerMask _ignoreLayerMasks;

    protected virtual void Awake()
    {
        Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(placed) return;
        if (DetectDropTarget())
        {
            gameObject.GetComponent<Renderer>().enabled = true;
            transform.position = new Vector3(newPosition.x, newPosition.y + 1, newPosition.z);
        }
        else
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

    }

    private void OnMouseDown()
    {
        if (DetectBlock(LayerMask.GetMask("Moveable Block")) && placed)
        {
            MapManager.Instance.mapList[MapManager.Instance.currentMap].objectsOnMap.Remove(newMapPosition);
            Destroy(gameObject);
        }
        else if (DetectDropTarget())
        {
            MapManager.Instance.mapList[MapManager.Instance.currentMap].objectsOnMap.Add(newMapPosition, this.gameObject);
            placed = true;
        }
    }

    bool DetectDropTarget()
    {
        var ray = Camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hitInfo, _raycastDistance, _dropTargetLayerMask))
            return false;
        newMapPosition = hitInfo.collider.gameObject.GetComponent<GroundObject>().location;
        if(MapManager.Instance.mapList[MapManager.Instance.currentMap].objectsOnMap.ContainsKey(newMapPosition))
            return false;

        newPosition = hitInfo.collider.gameObject.transform.position;
        return true;
    }

    bool DetectBlock(LayerMask mask)
    {
        var ray = Camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hitInfo, _raycastDistance, mask))
            return false;
        return true;
    }
}
