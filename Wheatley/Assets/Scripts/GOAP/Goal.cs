using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapManager;

public class Goal : MonoBehaviour
{
    public virtual float Contentment()
    {
        return 0;
    }
}
