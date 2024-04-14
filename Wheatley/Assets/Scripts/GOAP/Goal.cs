using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapManager;

public interface IGoal
{
    public int CalculatePriority();
    public bool CanRun();
    public void OnGoalActivated();
    public void OnGoalDeactivated();
}

public class Goal : MonoBehaviour, IGoal
{
    protected Action LinkedAction;

    public virtual int CalculatePriority()
    {
        return -1;
    }

    public virtual bool CanRun()
    {
        return false;
    }

    // Consider linking an action to the goal here
    public virtual void OnGoalActivated()
    {

    }

    // Consider unlinking an action to the goal here
    public virtual void OnGoalDeactivated()
    {

    }
}
