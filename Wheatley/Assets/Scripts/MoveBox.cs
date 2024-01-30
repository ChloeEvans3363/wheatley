using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    Vector3 newPosition;
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
        if (DetectDropTarget())
            placed = true;
    }

    bool DetectDropTarget()
    {
        var ray = Camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hitInfo, _raycastDistance, _dropTargetLayerMask))
            return false;
        newPosition = hitInfo.collider.gameObject.transform.position;
        return true;
    }
}
