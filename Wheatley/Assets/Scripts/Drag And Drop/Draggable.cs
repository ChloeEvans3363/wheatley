using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MouseInputBase))]
public class Draggable : MonoBehaviour
{
    public Action<GameObject> OnMouseDown, OnMouseUp, OnMouseEntered, OnMouseExited;

    [SerializeField] protected LayerMask _raycastLayerMask;
    [SerializeField] protected float _raycastDistance = 100f;

    protected MouseInputBase MouseInput { get; private set; }

    protected Camera Camera { get; private set; }
    bool IsMouseOver { get; set; }

    // Map stuff (taken from player controller)
    MapManager mapManager;
    Tuple<int, int> position;

    protected virtual void Awake()
    {
        Camera = Camera.main;
        MouseInput = GetComponent<MouseInputBase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectMouseOver();
        DetectMouseDown();
        DetectMouseUp();
    }

    void DetectMouseOver()
    {
        var ray = Camera.ScreenPointToRay(MouseInput.MousePosition);
        if (!Physics.Raycast(ray, out var hitInfo, _raycastDistance, _raycastLayerMask))
        {
            if (!IsMouseOver) return;
            HandleMouseExit();
            return;
        }

        if (hitInfo.collider.gameObject != gameObject)
        {
            if (!IsMouseOver) return;
            HandleMouseExit();
            return;
        }

        if (IsMouseOver) return;
        HandleMouseEntered();
    }

    void HandleMouseEntered()
    {
        IsMouseOver = true;
        OnMouseEntered?.Invoke(gameObject);
    }

    void HandleMouseExit()
    {
        IsMouseOver = false;
        OnMouseExited?.Invoke(gameObject);
    }

    void DetectMouseDown()
    {
        if (!MouseInput.GetMouseButtonDown(0)) return;
        if (!IsMouseOver) return;
        OnMouseDown?.Invoke(gameObject);
    }

    void DetectMouseUp()
    {
        if (!MouseInput.GetMouseButtonUp(0)) return;
        if (!IsMouseOver) return;
        OnMouseUp?.Invoke(gameObject);
    }
}
