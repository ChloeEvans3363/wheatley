using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapManager;

public interface IGoal
{
    public int OnCalculatePriority();
    public bool CanRun();
    public void OnTickGoal();
    public void OnGoalActivated();
    public void OnGoalDeactivated();
}

public class Goal : MonoBehaviour, IGoal
{
    public virtual int OnCalculatePriority()
    {
        return -1;
    }

    public virtual bool CanRun()
    {
        return false;
    }

    public virtual void OnTickGoal()
    {

    }

    public virtual void OnGoalActivated()
    {

    }

    public virtual void OnGoalDeactivated()
    {

    }
}
