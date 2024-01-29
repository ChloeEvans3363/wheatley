using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputBase : MonoBehaviour
{
    Vector3 mousePosition;

    public virtual Vector3 MousePosition => Input.mousePosition;

    public virtual bool GetMouseButtonDown(int button) => Input.GetMouseButtonDown(button);
    public virtual bool GetMouseButtonUp(int button) => Input.GetMouseButtonUp(button);

}
