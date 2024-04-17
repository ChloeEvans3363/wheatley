using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    Vector3 newPosition;
    public Tuple<int, int> newMapPosition;

    protected Camera Camera { get; private set; }
    bool placed;
    public bool isFillingHole = false;

    [SerializeField] protected LayerMask _raycastLayerMask;
    [SerializeField] protected float _raycastDistance = 100f;
    [SerializeField] LayerMask _dropTargetLayerMask;
    [SerializeField] LayerMask _ignoreLayerMasks;

    TextMeshPro textMeshPro;

    public Vector3 deselectedLocation;
    public int mapIdentity;
    InteractibleObject interactibleObject;

    private void Start()
    {
        textMeshPro = gameObject.GetComponentInChildren<TextMeshPro>();
        interactibleObject = gameObject.GetComponent<InteractibleObject>();
    }

    protected virtual void Awake()
    {
        Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (placed || ManageScenes.Instance.needsToRestart) return;
        if (MapManager.Instance.selectedBlock == mapIdentity && DetectDropTarget())
        {
            gameObject.GetComponent<Renderer>().enabled = true;
            textMeshPro.GetComponent<Renderer>().enabled = true;
            transform.position = new Vector3(newPosition.x, newPosition.y + 1, newPosition.z);
        }
        else
        {
            transform.position = deselectedLocation;
        }

    }

    private void OnMouseDown()
    {
        if (DetectBlock(LayerMask.GetMask("Moveable Block")) && placed)
        {
            if (newMapPosition.Item1 == MapManager.Instance.playerLocation.Item1 && newMapPosition.Item2 == MapManager.Instance.playerLocation.Item2)
                return;

            MapManager.Instance.currentMap.objectsOnMap.Remove(newMapPosition);
            placed = false;

            if (isFillingHole)
            {
                MapManager.Instance.currentMap.mapHeights[newMapPosition.Item1, newMapPosition.Item2]--;
                MapManager.Instance.currentMap.floorElements.Remove(newMapPosition);
            }

            if (interactibleObject.canPush)
                ManageScenes.Instance.UpdateNumPushBlocks(1);
            else
                ManageScenes.Instance.UpdateNumImmovableBlocks(1);
        }
        else if (DetectBlock(LayerMask.GetMask("Moveable Block")) && !placed && mapIdentity != MapManager.Instance.selectedBlock)
        {
            MapManager.Instance.selectedBlock = mapIdentity;
        } 
        else if (DetectDropTarget())
        {
            MapManager.Instance.currentMap.objectsOnMap.Add(newMapPosition, this.gameObject);
            MapManager.Instance.selectedBlock = -1;
            placed = true;

            if (interactibleObject.canPush)
                ManageScenes.Instance.UpdateNumPushBlocks(-1);
            else
                ManageScenes.Instance.UpdateNumImmovableBlocks(-1);
        }
    }

    bool DetectDropTarget()
    {
        var ray = Camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hitInfo, _raycastDistance, _dropTargetLayerMask) || !hitInfo.collider.gameObject.GetComponent<GroundObject>())
            return false;
        else
        {
            newMapPosition = hitInfo.collider.gameObject.GetComponent<GroundObject>().location;
        }

        Tuple<int, int> endloc = MapManager.Instance.currentMap.endLocation;
        if (MapManager.Instance.currentMap.objectsOnMap.ContainsKey(newMapPosition) 
            || MapManager.Instance.currentMap.mapHeights[newMapPosition.Item1, newMapPosition.Item2] != 1
            || (newMapPosition.Item1 == endloc.Item1 && newMapPosition.Item2 == endloc.Item2)
            || (newMapPosition.Item1 == MapManager.Instance.playerLocation.Item1 && newMapPosition.Item2 == MapManager.Instance.playerLocation.Item2))
        {
            return false;
        }

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
