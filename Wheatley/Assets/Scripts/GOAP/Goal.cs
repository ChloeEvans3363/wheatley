using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapManager;

public interface IGoal
{
    public float Contentment();
}

public class Goal : MonoBehaviour, IGoal
{
    public virtual float Contentment()
    {
        return 0;
    }
}
